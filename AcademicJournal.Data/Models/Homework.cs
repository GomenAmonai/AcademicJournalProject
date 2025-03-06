using System;
using System.ComponentModel.DataAnnotations;

namespace AcademicJournal.Data.Models
{
    public class Homework
    {
        public int Id { get; set; }
        
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public DateTime Deadline { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public int? FileId { get; set; }
        public virtual FileEntity File { get; set; }
    }
}