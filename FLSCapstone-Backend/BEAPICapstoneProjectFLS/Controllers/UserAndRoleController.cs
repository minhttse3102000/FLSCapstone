using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.UserAndRoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAndRoleController : ControllerBase
    {
        private readonly IUserAndRoleService _IUserAndRoleService;

        public UserAndRoleController(IUserAndRoleService UserAndRoleService)
        {
            _IUserAndRoleService = UserAndRoleService;
        }

        [HttpGet("{id}", Name = "GetUserAndRoleById")]
        public async Task<IActionResult> GetUserAndRoleById(string id)
        {
            var userAndRoleModel = await _IUserAndRoleService.GetUserAndRoleById(id);
            if (userAndRoleModel == null)
                return NotFound();
            return Ok(userAndRoleModel);
        }

        [HttpGet]
        public IActionResult GetAllUserAndRole([FromQuery] UserAndRoleViewModel flitter, UserAndRoleSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listUserAndRoleModel = _IUserAndRoleService.GetAllUserAndRole(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listUserAndRoleModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAndRole([FromBody] CreateUserAndRoleRequest request)
        {
            var userAndRoleVM = await _IUserAndRoleService.CreateUserAndRole(request);
            if (userAndRoleVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetUserAndRoleById), new { id = userAndRoleVM.Id }, userAndRoleVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAndRole(string id, [FromBody] UpdateUserAndRoleRequest request)
        {
            var userAndRoleVM = await _IUserAndRoleService.UpdateUserAndRole(id, request);
            if (userAndRoleVM == null)
                return BadRequest();
            return Ok(userAndRoleVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAndRole(string id)
        {
            var rs = await _IUserAndRoleService.DeleteUserAndRole(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
