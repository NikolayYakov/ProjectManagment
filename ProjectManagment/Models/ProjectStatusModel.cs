using System.Collections.Generic;

namespace ProjectManagment.Models
{
    public class ProjectStatusViewModel
    {
        public Guid ProjectId { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public List<ProjectStatus> Statuses { get; set; }
    }

    public class ProjectStatus
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
