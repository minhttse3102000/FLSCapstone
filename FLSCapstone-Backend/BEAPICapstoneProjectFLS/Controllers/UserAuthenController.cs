using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.UserRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenController : ControllerBase
    {
        private readonly IUserService _IUserService;

        public UserAuthenController(IUserService UserService)
        {
            _IUserService = UserService;
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetUserAuthenById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userVM = await _IUserService.GetUserById(id);
            if (userVM == null)
                return NotFound();
            return Ok(userVM);
        }

        [Authorize]
        [HttpGet("email/{email}", Name = "GetUserAuthenByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var userVM = await _IUserService.GetUserByEmail(email);
            if (userVM == null)
                return NotFound();
            return Ok(userVM);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUser([FromQuery] UserViewModel flitter, UserSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listUserModel = _IUserService.GetAllUser(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listUserModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var userVM = await _IUserService.CreateUser(request);
            if (userVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetUserById), new { id = userVM.Id }, userVM);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            var userVM = await _IUserService.UpdateUser(id, request);
            if (userVM == null)
                return BadRequest();
            return Ok(userVM);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var rs = await _IUserService.DeleteUser(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
