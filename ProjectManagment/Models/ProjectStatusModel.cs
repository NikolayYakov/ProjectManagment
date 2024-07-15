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
        public ProjectStatus()
        {
        }

        public ProjectStatus(Guid projectId, Guid statusId, string name, string description, int number, decimal? order)
        {
            this.ProjectId = projectId;
            this.StatusId = statusId;
            this.Name = name;
            this.Description = description;
            this.Number = number;
            this.Order = order;
        }
        public Guid StatusId { get; set; }
        public Guid ProjectId { get; set; }
        public decimal? Order { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
