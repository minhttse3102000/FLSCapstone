using AutoMapper;
using BEAPICapstoneProjectFLS.Entities;
using BEAPICapstoneProjectFLS.IRepositories;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Services
{
    public class TokenService : ITokenService
    {
        private readonly IGenericRepository<RefreshToken> _res;
        private readonly IMapper _mapper;

        public TokenService(IGenericRepository<RefreshToken> repository, IMapper mapper)
        {
            _res = repository;
            _mapper = mapper;
        }
        public async Task<RefreshToken> GetRefreshTokenByToken(string Token)
        {
            var refreshTokens = await _res.GetAllByIQueryable()
                            .Where(x => x.Token == Token)
                            .Include(x => x.User)
                            .ToListAsync();

            var refreshToken = refreshTokens.FirstOrDefault();
            if (refreshToken == null)
                return null;
            return refreshToken;
        }

        public async Task<bool> DeleteRefreshToken(string Token)
        {
            var refreshToken = (await _res.FindByAsync(x => x.Token == Token)).FirstOrDefault();
            if (refreshToken == null)
                return false;
            refreshToken.IsUsed = 1;
            refreshToken.IsRevoked = 1;       
            await _res.UpdateAsync(refreshToken);
            await _res.SaveAsync();
            return true;
        }
    }
}
