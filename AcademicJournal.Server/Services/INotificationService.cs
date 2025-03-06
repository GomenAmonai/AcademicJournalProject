using System.Threading.Tasks;

namespace AcademicJournal.Server.Services
{
    public interface INotificationService
    {
        Task SendGradeNotificationAsync(string studentId, int gradeId);
        Task SendHomeworkNotificationAsync(string groupId, int homeworkId);
        Task SendPersonalNotificationAsync(string userId, string title, string message);
    }
}