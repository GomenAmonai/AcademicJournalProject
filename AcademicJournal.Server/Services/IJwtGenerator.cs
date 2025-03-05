using AcademicJournal.Data.Models;

namespace AcademicJournal.Server.Services
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}