namespace AcademicJournal.Server.Models
{
    public class AddAttendanceDto
    {
        public int LessonId { get; set; }
        public int StudentId { get; set; }
        public bool IsPresent { get; set; }
        public string Comment { get; set; }
    }
}