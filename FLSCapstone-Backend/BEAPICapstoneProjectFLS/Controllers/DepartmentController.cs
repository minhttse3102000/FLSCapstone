using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.DepartmentRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _IDepartmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _IDepartmentService = departmentService;
        }

        [HttpGet("{id}", Name = "GetDepartmentById")]
        public async Task<IActionResult> GetDepartmentById(string id)
        {
            var DepartmentModel = await _IDepartmentService.GetDepartmentById(id);
            if (DepartmentModel == null)
                return NotFound();
            return Ok(DepartmentModel);
        }

        [HttpGet]
        public IActionResult GetAllDepartment([FromQuery] DepartmentViewModel flitter, DepartmentSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listDepartmentModel = _IDepartmentService.GetAllDepartment(flitter, pageIndex, pageSize,sortBy, order);
            return Ok(listDepartmentModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentRequest request)
        {
            var CheckIDDuplicate = await _IDepartmentService.GetDepartmentById(request.Id);
            if (CheckIDDuplicate != null)
            {
                return BadRequest("Duplicate Id");
            }
            else
            {
                var departmentVM = await _IDepartmentService.CreateDepartment(request);
                if (departmentVM == null)
                {
                    return BadRequest();
                }
                else
                {
                    return CreatedAtRoute(nameof(GetDepartmentById), new { id = departmentVM.Id }, departmentVM);
                }
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(string id, [FromBody] UpdateDepartmentRequest request)
        {
                var departmentVM = await _IDepartmentService.UpdateDepartment(id, request);
                if (departmentVM == null)
                    return BadRequest();
                return Ok(departmentVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(string id)
        {
            var rs = await _IDepartmentService.DeleteDepartment(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
