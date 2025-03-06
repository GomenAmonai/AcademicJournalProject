namespace AcademicJournal.Server.Models
{
    public class UpdateAttendanceDto
    {
        public int Id { get; set; }
        public bool IsPresent { get; set; }
        public string Comment { get; set; }
    }
}