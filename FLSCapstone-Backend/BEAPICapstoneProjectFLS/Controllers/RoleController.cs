using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.RoleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _IRoleService;

        public RoleController(IRoleService RoleService)
        {
            _IRoleService = RoleService;
        }

        [HttpGet("{id}", Name = "GetRoleById")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var roleModel = await _IRoleService.GetRoleById(id);
            if (roleModel == null)
                return NotFound();
            return Ok(roleModel);
        }

        [HttpGet]
        public IActionResult GetAllRole([FromQuery] RoleViewModel flitter, RoleSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listRoleModel = _IRoleService.GetAllRole(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listRoleModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            var roleVM = await _IRoleService.CreateRole(request);
            if (roleVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetRoleById), new { id = roleVM.Id }, roleVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleRequest request)
        {
            var roleVM = await _IRoleService.UpdateRole(id, request);
            if (roleVM == null)
                return BadRequest();
            return Ok(roleVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var rs = await _IRoleService.DeleteRole(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
