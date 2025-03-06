using System;

namespace AcademicJournal.Server.Models
{
    public class HomeworkFilterDto
    {
        public int? SubjectId { get; set; }
        public int? GroupId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool OnlyActive { get; set; } = true;
        public SortDirection SortByDate { get; set; } = SortDirection.Descending;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}