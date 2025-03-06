using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AcademicJournal.Server.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationHub : Hub
    {
        // Метод для присоединения пользователя к его персональной группе
        public async Task JoinUserGroup()
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            }
        }

        // Метод для присоединения преподавателя к группе предмета
        public async Task JoinSubjectGroup(string subjectId)
        {
            var isTeacher = Context.User.IsInRole("Teacher") || Context.User.IsInRole("Admin");
            if (isTeacher && !string.IsNullOrEmpty(subjectId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Subject_{subjectId}");
            }
        }

        // Вызывается при отключении клиента
        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            // Здесь можно добавить логику для отслеживания отключений
            await base.OnDisconnectedAsync(exception);
        }
    }
}