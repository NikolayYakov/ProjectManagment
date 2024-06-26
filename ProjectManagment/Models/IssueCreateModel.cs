using System.ComponentModel.DataAnnotations;

namespace ProjectManagment.Models
{
    public class IssueCreateModel
    {
        public Guid ProjectId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> Assignees { get; set; }
        public string Area { get; set; }
        public List<string> Labels { get; set; }
        public string Milestone { get; set; }
        public string Status { get; set; }

        public List<string> AvailableAssignees { get; set; }
        public List<ProjectArea> AvailableAreas { get; set; }
        public List<ProjectLabel> AvailableLabels { get; set; }
        public List<ProjectMilestone> AvailableMilestones { get; set; }
        public List<ProjectStatus> AvailableStatuses { get; set; }
    }
}
