using AcademicJournal.Server.Models;
using AcademicJournal.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HomeworksController : ControllerBase
    {
        private readonly IHomeworkService _homeworkService;

        public HomeworksController(IHomeworkService homeworkService)
        {
            _homeworkService = homeworkService;
        }

        // GET: api/Homeworks
        [HttpGet]
        public async Task<ActionResult<PagedResult<HomeworkDto>>> GetHomeworks([FromQuery] HomeworkFilterDto filter)
        {
            var result = await _homeworkService.GetHomeworksAsync(filter);
            return Ok(result);
        }

        // GET: api/Homeworks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HomeworkDto>> GetHomework(int id)
        {
            var homework = await _homeworkService.GetHomeworkByIdAsync(id);
            if (homework == null)
                return NotFound();

            return Ok(homework);
        }

        // GET: api/Homeworks/subjects/{subjectId}
        [HttpGet("subjects/{subjectId}")]
        public async Task<ActionResult<PagedResult<HomeworkDto>>> GetHomeworksBySubject(int subjectId, [FromQuery] HomeworkFilterDto filter)
        {
            var result = await _homeworkService.GetHomeworksBySubjectAsync(subjectId, filter);
            return Ok(result);
        }

        // GET: api/Homeworks/groups/{groupId}
        [HttpGet("groups/{groupId}")]
        public async Task<ActionResult<PagedResult<HomeworkDto>>> GetHomeworksByGroup(int groupId, [FromQuery] HomeworkFilterDto filter)
        {
            var result = await _homeworkService.GetHomeworksByGroupAsync(groupId, filter);
            return Ok(result);
        }

        // POST: api/Homeworks
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<HomeworkDto>> AddHomework(AddHomeworkDto homeworkDto)
        {
            var result = await _homeworkService.AddHomeworkAsync(homeworkDto);
            return CreatedAtAction(nameof(GetHomework), new { id = result.Id }, result);
        }

        // PUT: api/Homeworks/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateHomework(int id, UpdateHomeworkDto homeworkDto)
        {
            if (id != homeworkDto.Id)
                return BadRequest();

            await _homeworkService.UpdateHomeworkAsync(homeworkDto);
            return NoContent();
        }

        // DELETE: api/Homeworks/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHomework(int id)
        {
            await _homeworkService.DeleteHomeworkAsync(id);
            return NoContent();
        }
    }
}