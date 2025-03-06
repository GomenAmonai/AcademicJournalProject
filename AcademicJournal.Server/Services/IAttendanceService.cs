using AcademicJournal.Server.Models;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Services
{
    public interface IAttendanceService
    {
        Task<PagedResult<AttendanceDto>> GetAttendancesAsync(AttendanceFilterDto filter);
        Task<AttendanceDto> GetAttendanceByIdAsync(int id);
        Task<AttendanceDto> AddAttendanceAsync(AddAttendanceDto attendanceDto);
        Task UpdateAttendanceAsync(UpdateAttendanceDto attendanceDto);
        Task DeleteAttendanceAsync(int id);
        Task<PagedResult<AttendanceDto>> GetAttendancesByLessonAsync(int lessonId, AttendanceFilterDto filter);
        Task<PagedResult<AttendanceDto>> GetAttendancesByStudentAsync(int studentId, AttendanceFilterDto filter);
        Task<PagedResult<AttendanceDto>> GetAttendancesBySubjectAsync(int subjectId, AttendanceFilterDto filter);
    }
}