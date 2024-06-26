﻿namespace ProjectManagment.Models
{
    public class IssueEditModel
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
        public List<string> AvailableAreas { get; set; }
        public List<string> AvailableLabels { get; set; }
        public List<string> AvailableMilestones { get; set; }
        public List<string> AvailableStatuses { get; set; }
    }
}
