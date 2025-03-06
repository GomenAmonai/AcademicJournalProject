// AcademicJournal.Data/Models/HomeworkSubmission.cs
using System;

namespace AcademicJournal.Data.Models
{
    public class HomeworkSubmission
    {
        public int Id { get; set; }
        public int HomeworkId { get; set; }
        public int StudentId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Content { get; set; }
        public int? FileId { get; set; }
        public double? Grade { get; set; }
        public string TeacherComment { get; set; }
        public SubmissionStatus Status { get; set; }
        
        // Навигационные свойства
        public virtual Homework Homework { get; set; }
        public virtual Student Student { get; set; }
        public virtual FileEntity File { get; set; }
    }
    
    public enum SubmissionStatus
    {
        Submitted,
        UnderReview,
        NeedsRevision,
        Accepted,
        Rejected
    }
}