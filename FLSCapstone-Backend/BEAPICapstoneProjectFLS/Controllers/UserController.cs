using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.UserRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _IUserService;

        public UserController(IUserService UserService)
        {
            _IUserService = UserService;
        }


        
        [HttpGet("{id}", Name = "GetUserById")]
        //[Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var userVM = await _IUserService.GetUserById(id);
            if (userVM == null)
                return NotFound();
            return Ok(userVM);
        }
        [HttpGet("email/{email}", Name = "GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var userVM = await _IUserService.GetUserByEmail(email);
            if (userVM == null)
                return NotFound();
            return Ok(userVM);
        }

        [HttpGet]
        public IActionResult GetAllUser([FromQuery] UserViewModel flitter, UserSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listUserModel = _IUserService.GetAllUser(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listUserModel);
        }

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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            var userVM = await _IUserService.UpdateUser(id, request);
            if (userVM == null)
                return BadRequest();
            return Ok(userVM);
        }

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
