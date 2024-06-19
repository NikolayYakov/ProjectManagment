using System.Collections.Generic;

namespace ProjectManagment.Models
{
    public class ProjectAreaViewModel
    {
        public Guid ProjectId { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public List<ProjectArea> Areas { get; set; }
    }

    public class ProjectArea
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
