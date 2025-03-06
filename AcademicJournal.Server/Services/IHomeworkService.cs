using AcademicJournal.Server.Models;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Services
{
    public interface IHomeworkService
    {
        Task<PagedResult<HomeworkDto>> GetHomeworksAsync(HomeworkFilterDto filter);
        Task<HomeworkDto> GetHomeworkByIdAsync(int id);
        Task<HomeworkDto> AddHomeworkAsync(AddHomeworkDto homeworkDto);
        Task UpdateHomeworkAsync(UpdateHomeworkDto homeworkDto);
        Task DeleteHomeworkAsync(int id);
        Task<PagedResult<HomeworkDto>> GetHomeworksBySubjectAsync(int subjectId, HomeworkFilterDto filter);
        Task<PagedResult<HomeworkDto>> GetHomeworksByGroupAsync(int groupId, HomeworkFilterDto filter);
    }
}