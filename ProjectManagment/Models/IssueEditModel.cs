namespace ProjectManagment.Models
{
    public class IssueEditModel
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> Assignees { get; set; }
        public string Area { get; set; }
        public List<string> Labels { get; set; }
        public string Milestone { get; set; }
        public string Status { get; set; }

        public List<UserModel> AvailableAssignees { get; set; }
        public List<ProjectArea> AvailableAreas { get; set; }
        public List<ProjectLabel> AvailableLabels { get; set; }
        public List<ProjectMilestone> AvailableMilestones { get; set; }
        public List<ProjectStatus> AvailableStatuses { get; set; }
    }
}
