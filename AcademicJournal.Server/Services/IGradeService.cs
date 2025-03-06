using AcademicJournal.Server.Models;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Services
{
    public interface IGradeService
    {
        Task<PagedResult<GradeDto>> GetStudentGradesAsync(string studentId, GradeFilterDto filter);
        Task<GradeDto> GetGradeByIdAsync(int id);
        Task<GradeDto> AddGradeAsync(AddGradeDto gradeDto);
        Task UpdateGradeAsync(UpdateGradeDto gradeDto);
        Task DeleteGradeAsync(int id);
        Task<PagedResult<GradeDto>> GetGradesBySubjectAsync(int subjectId, GradeFilterDto filter);
    }
}