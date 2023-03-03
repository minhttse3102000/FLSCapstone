using BEAPICapstoneProjectFLS.IServices;
using BEAPICapstoneProjectFLS.Requests;
using BEAPICapstoneProjectFLS.Requests.ScheduleRequest;
using BEAPICapstoneProjectFLS.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BEAPICapstoneProjectFLS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _IScheduleService;

        public ScheduleController(IScheduleService ScheduleService)
        {
            _IScheduleService = ScheduleService;
        }

        [HttpGet("{id}", Name = "GetScheduleById")]
        public async Task<IActionResult> GetScheduleById(string id)
        {
            var scheduleModel = await _IScheduleService.GetScheduleById(id);
            if (scheduleModel == null)
                return NotFound();
            return Ok(scheduleModel);
        }

        [HttpGet]
        public IActionResult GetAllSchedule([FromQuery] ScheduleViewModel flitter, ScheduleSortBy sortBy, OrderBy order, int pageIndex = 1, int pageSize = 10)
        {
            var listScheduleModel = _IScheduleService.GetAllSchedule(flitter, pageIndex, pageSize, sortBy, order);
            return Ok(listScheduleModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule([FromBody] CreateScheduleRequest request)
        {
            var scheduleVM = await _IScheduleService.CreateSchedule(request);
            if (scheduleVM == null)
            {
                return BadRequest();
            }
            else
            {
                return CreatedAtRoute(nameof(GetScheduleById), new { id = scheduleVM.Id }, scheduleVM);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(string id, [FromBody] UpdateScheduleRequest request)
        {
            var scheduleVM = await _IScheduleService.UpdateSchedule(id, request);
            if (scheduleVM == null)
                return BadRequest();
            return Ok(scheduleVM);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(string id)
        {
            var rs = await _IScheduleService.DeleteSchedule(id);
            if (rs == false)
                return NotFound();
            return Ok();
        }
    }
}
