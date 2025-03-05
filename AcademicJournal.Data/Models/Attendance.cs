namespace AcademicJournal.Data.Models
{
    public enum AttendanceStatus
    {
        Present,
        Absent,
        Late
    }
    
    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
        
        // Навигационные свойства
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}