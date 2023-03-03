using BEAPICapstoneProjectFLS.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.ViewModel;
using BEAPICapstoneProjectFLS.RandomKey;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.Enum;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly AppSetting _appSettings;

        private readonly FLSCapstoneDatabaseContext _context;

        private readonly IUserService _IUserService;
        private readonly ITokenService _ITokenService;

        private readonly IGenericRepository<User> _res;

        private readonly IGenericRepository<RefreshToken> _resRefresh;

        public TokenController(FLSCapstoneDatabaseContext context, IOptionsMonitor<AppSetting> optionsMonitor, 
            IUserService UserService, ITokenService TokenService,
            IGenericRepository<User> repository, IGenericRepository<RefreshToken> refreshtokenRepository)
        {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
            _IUserService = UserService;
            _ITokenService = TokenService;
            _res = repository;
            _resRefresh = refreshtokenRepository;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Validate(string email)
        {

            var user = await _res.GetAllByIQueryable()
                .Where(x => x.Email == email && x.Status == (int)UserStatus.Active)
                .Include(x => x.Department)
                .Include(x => x.UserAndRoles)
                .FirstOrDefaultAsync();
            if (user == null) //không đúng
            {
                return NotFound();  
            }

            //cấp token
            var token = await GenerateToken(user);

            return Ok(new ApiResponseToken
            {
                Success = true,
                Message = "Authenticate success",
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken
            });
        }

        private async Task<TokenViewModel> GenerateToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, RandomPKKey.NewRamDomPKKey()),
                    new Claim("UserName", user.Name),
                    new Claim("Id", user.Id),

                    //roles
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Lưu database
            var refreshTokenEntity = new RefreshToken
            {
                Id = RandomPKKey.NewRamDomPKKey(),
                JwtId = token.Id,
                UserId = user.Id,
                Token = refreshToken,
                IsUsed = 0,
                IsRevoked = 0,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(2)
            };

            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenViewModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            /*var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }*/

            string token = RandomPKKey.NewRamDomToken();
            return token;

        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenViewModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed == 1)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked == 1 )
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsRevoked = 1;
                storedToken.IsUsed = 1;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var user = await _context.Users.SingleOrDefaultAsync(nd => nd.Id == storedToken.UserId);


                var token = await GenerateToken(user);

                return Ok(new ApiResponseToken
                {
                    Success = true,
                    Message = "Renew token success",
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong",
                    Data = ex.Message
                    
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }



        [HttpPost("CheckTokenValue")]
        public IActionResult checkToken(TokenViewModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return BadRequest(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed == 1)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked == 1)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Token is still valid"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong",
                    Data = ex.Message
                });
            }
        }


        [HttpGet("GetUserByRefreshToken/{refreshToken}", Name = "GetUserByRefreshToken")]
        public async Task<IActionResult> GetUserByRefreshToken(string refreshToken)
        {
            //var storedRefreshToken = await _context.RefreshTokens.Where(x => x.Token == refreshToken).FirstOrDefaultAsync();
            var storedRefreshToken = await _ITokenService.GetRefreshTokenByToken(refreshToken);
            ApiResponse response = new ApiResponse();
            if (storedRefreshToken == null)
            {
                response.Success = false;
                response.Message = "Not found RefreshToken";
                return NotFound(response);
            }


            if (storedRefreshToken.IsRevoked == 1)
            {
                response.Success = false;
                response.Message = "RefreshToken have been revoked";
                return NotFound(response);
            }
            if (DateTime.Now > storedRefreshToken.ExpiredAt)
            {
                response.Success = false;
                response.Message = "RefreshToken has expired";
                return NotFound(response);
            }


            var userVM = await _IUserService.GetUserById(storedRefreshToken.UserId);
            if (userVM == null)
            {
                response.Success = false;
                response.Message = "Not found user";
                return NotFound(response);
            }
            return Ok(userVM);
        }


        [HttpDelete("DeleteRefreshToken/{refreshToken}", Name = "DeleteRefreshToken")]
        public async Task<IActionResult> DeleteRefreshToken(string refreshToken)
        {
            var rs = await _ITokenService.DeleteRefreshToken(refreshToken);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
