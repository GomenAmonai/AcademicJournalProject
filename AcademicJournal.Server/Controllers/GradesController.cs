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
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // GET: api/Grades
        // Получение собственных оценок для студента
        [HttpGet]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<PagedResult<GradeDto>>> GetMyGrades([FromQuery] GradeFilterDto filter)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _gradeService.GetStudentGradesAsync(userId, filter);
            return Ok(result);
        }

        // GET: api/Grades/students/{studentId}
        // Получение оценок конкретного студента (для преподавателей и администраторов)
        [HttpGet("students/{studentId}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<PagedResult<GradeDto>>> GetStudentGrades(string studentId, [FromQuery] GradeFilterDto filter)
        {
            var result = await _gradeService.GetStudentGradesAsync(studentId, filter);
            return Ok(result);
        }

        // GET: api/Grades/subjects/{subjectId}
        // Получение оценок по конкретному предмету (для преподавателей и администраторов)
        [HttpGet("subjects/{subjectId}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<PagedResult<GradeDto>>> GetGradesBySubject(int subjectId, [FromQuery] GradeFilterDto filter)
        {
            var result = await _gradeService.GetGradesBySubjectAsync(subjectId, filter);
            return Ok(result);
        }

        // GET: api/Grades/{id}
        // Получение информации о конкретной оценке
        [HttpGet("{id}")]
        public async Task<ActionResult<GradeDto>> GetGrade(int id)
        {
            var grade = await _gradeService.GetGradeByIdAsync(id);
            if (grade == null)
                return NotFound();

            return Ok(grade);
        }

        // POST: api/Grades
        // Добавление новой оценки
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<ActionResult<GradeDto>> AddGrade(AddGradeDto gradeDto)
        {
            var result = await _gradeService.AddGradeAsync(gradeDto);
            return CreatedAtAction(nameof(GetGrade), new { id = result.Id }, result);
        }

        // PUT: api/Grades/{id}
        // Обновление существующей оценки
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateGrade(int id, UpdateGradeDto gradeDto)
        {
            if (id != gradeDto.Id)
                return BadRequest();

            await _gradeService.UpdateGradeAsync(gradeDto);
            return NoContent();
        }

        // DELETE: api/Grades/{id}
        // Удаление оценки
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            await _gradeService.DeleteGradeAsync(id);
            return NoContent();
        }
    }
}