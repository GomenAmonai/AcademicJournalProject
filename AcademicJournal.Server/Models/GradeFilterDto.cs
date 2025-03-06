using System;

namespace AcademicJournal.Server.Models
{
    public class GradeFilterDto
    {
        public int? SubjectId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public SortDirection SortByDate { get; set; } = SortDirection.Descending;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}