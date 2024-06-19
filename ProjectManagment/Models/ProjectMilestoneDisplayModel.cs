using System.Collections.Generic;

namespace ProjectManagment.Models
{
    public class ProjectMilestoneViewModel
    {
        public Guid ProjectId { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public List<ProjectMilestone> Milestones { get; set; }
    }

    public class ProjectMilestone
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
