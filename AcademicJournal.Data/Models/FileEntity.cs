
using System;

namespace AcademicJournal.Data.Models
{
    public class FileEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
        public int UploadedById { get; set; }
        
        // Навигационное свойство
        public virtual User UploadedBy { get; set; }
    }
}