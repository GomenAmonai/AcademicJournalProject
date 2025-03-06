using AcademicJournal.Server.Models;
using AcademicJournal.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // GET: api/Attendances
        [HttpGet]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<PagedResult<AttendanceDto>>> GetAttendances([FromQuery] AttendanceFilterDto filter)
        {
            var result = await _attendanceService.GetAttendancesAsync(filter);
            return Ok(result);
        }

        // GET: api/Attendances/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AttendanceDto>> GetAttendance(int id)
        {
            var attendance = await _attendanceService.GetAttendanceByIdAsync(id);
            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        // GET: api/Attendances/lessons/{lessonId}
        [HttpGet("lessons/{lessonId}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<PagedResult<AttendanceDto>>> GetAttendancesByLesson(int lessonId, [FromQuery] AttendanceFilterDto filter)
        {
            var result = await _attendanceService.GetAttendancesByLessonAsync(lessonId, filter);
            return Ok(result);
        }

        // GET: api/Attendances/students/{studentId}
        [HttpGet("students/{studentId}")]
        public async Task<ActionResult<PagedResult<AttendanceDto>>> GetAttendancesByStudent(int studentId, [FromQuery] AttendanceFilterDto filter)
        {
            var result = await _attendanceService.GetAttendancesByStudentAsync(studentId, filter);
            return Ok(result);
        }

        // GET: api/Attendances/subjects/{subjectId}
        [HttpGet("subjects/{subjectId}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<PagedResult<AttendanceDto>>> GetAttendancesBySubject(int subjectId, [FromQuery] AttendanceFilterDto filter)
        {
            var result = await _attendanceService.GetAttendancesBySubjectAsync(subjectId, filter);
            return Ok(result);
        }

        // POST: api/Attendances
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<AttendanceDto>> AddAttendance(AddAttendanceDto attendanceDto)
        {
            var result = await _attendanceService.AddAttendanceAsync(attendanceDto);
            return CreatedAtAction(nameof(GetAttendance), new { id = result.Id }, result);
        }

        // PUT: api/Attendances/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateAttendance(int id, UpdateAttendanceDto attendanceDto)
        {
            if (id != attendanceDto.Id)
                return BadRequest();

            await _attendanceService.UpdateAttendanceAsync(attendanceDto);
            return NoContent();
        }

        // DELETE: api/Attendances/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            await _attendanceService.DeleteAttendanceAsync(id);
            return NoContent();
        }
    }
}