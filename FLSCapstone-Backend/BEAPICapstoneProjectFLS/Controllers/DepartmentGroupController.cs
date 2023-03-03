using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentGroupRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentGroupController : ControllerBase
    {
        private readonly IDepartmentGroupService _IDepartmentGroupService;

        public DepartmentGroupController(IDepartmentGroupService departmentGroupService)
        {
            _IDepartmentGroupService = departmentGroupService;
        }

        [HttpGet("{id}", Name = "GetDepartmentGroupById")]
        public async Task<IActionResult> GetDepartmentGroupById(string id)
        {
            var DepartmentGroupModel = await _IDepartmentGroupService.GetDepartmentGroupById(id);
            if (DepartmentGroupModel == null)
                return NotFound();
            return Ok(DepartmentGroupModel);
        }

        [HttpGet]
        public IActionResult GetAllDepartmentGroup([FromQuery] DepartmentGroupViewModel flitter, DepartmentGroupSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listDepartmentGroupModel = _IDepartmentGroupService.GetAllDepartmentGroup(flitter, pageIndex, pageSize,
                sortBy, order);
            return Ok(listDepartmentGroupModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartmentGroup([FromBody] CreateDepartmentGroupRequest request)
        {
            var departmentGroupVM = await _IDepartmentGroupService.CreateDepartmentGroup(request);
            if(departmentGroupVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetDepartmentGroupById), new { id = departmentGroupVM.Id }, departmentGroupVM);
            }
            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartmentGroup(string id, [FromBody] UpdateDepartmentGroupRequest request)
        {
            var DepartmentGroupModelFindId = await _IDepartmentGroupService.GetDepartmentGroupById(id);
            if (DepartmentGroupModelFindId == null)
                return NotFound();
            else
            {
                var departmentGroupVM = await _IDepartmentGroupService.UpdateDepartmentGroup(id, request);
                if (departmentGroupVM == null)
                    return BadRequest();
                return Ok(departmentGroupVM);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartmentGroup(string id)
        {
            var rs = await _IDepartmentGroupService.DeleteDepartmentGroup(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
