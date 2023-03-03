using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.SubjectRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _ISubjectService;

        public SubjectController(ISubjectService SubjectService)
        {
            _ISubjectService = SubjectService;
        }

        [HttpGet("{id}", Name = "GetSubjectById")]
        public async Task<IActionResult> GetSubjectById(string id)
        {
            var SubjectModel = await _ISubjectService.GetSubjectById(id);
            if (SubjectModel == null)
                return NotFound();
            return Ok(SubjectModel);
        }

        [HttpGet]
        public IActionResult GetAllSubject([FromQuery] SubjectViewModel flitter, SubjectSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listSubjectModel = _ISubjectService.GetAllSubject(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listSubjectModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            var CheckIDDuplicate = await _ISubjectService.GetSubjectById(request.Id);
            if (CheckIDDuplicate != null)
            {
                return BadRequest("Duplicate Id");
            }
            else
            {
                var SubjectVM = await _ISubjectService.CreateSubject(request);
                if (SubjectVM == null)
                {
                    return BadRequest();
                }
                else
                {
                    return CreatedAtRoute(nameof(GetSubjectById), new { id = SubjectVM.Id }, SubjectVM);
                }
            }

        }

        /*[HttpPost("CreateListSubject/")]
        public async Task<IActionResult> CreateListSubject([FromBody] List<CreateSubjectRequest> requests)
        {
            var SubjectVM = await _ISubjectService.CreateListSubject(requests);
            if (SubjectVM.Success == true)
            {
                return Ok(SubjectVM);
            }
            else
            {
                return BadRequest(SubjectVM);
            }
        }*/

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(string id, [FromBody] UpdateSubjectRequest request)
        {
            var SubjectVM = await _ISubjectService.UpdateSubject(id, request);
            if (SubjectVM == null)
                return BadRequest();
            return Ok(SubjectVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            var rs = await _ISubjectService.DeleteSubject(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }

    }
}
