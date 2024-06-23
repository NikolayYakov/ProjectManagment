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
        public ProjectMilestone()
        {
        }

        public ProjectMilestone(Guid projectId, Guid milestoneId, string name, string description, int number)
        {
            this.ProjectId = projectId;
            this.MilestoneId = milestoneId;
            this.Name = name;
            this.Description = description;
            this.Number = number;
        }

        public Guid MilestoneId { get; set; }
        public Guid ProjectId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
