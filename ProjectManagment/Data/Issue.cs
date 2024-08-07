﻿using ProjectManagment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagment.Data
{
    public class Issue
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Number { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public Guid ProjectId { get; set; }
        [ForeignKey("OwnerId")]
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Milestone Milestone { get; set; }
        [ForeignKey("MilestoneId")]
        public Guid? MilestoneId { get; set; }
        public Status Status { get; set; }
        [ForeignKey("StatusId")]
        public Guid? StatusId{ get; set; }
        public Sprint Sprint { get; set; }
        [ForeignKey("SprintId")]
        public Guid? SprintId { get; set; }
        public Area Area { get; set; }
        [ForeignKey("AreaId")]
        public Guid? AreaId { get; set; }
        public List<Label> Labels { get; set; }
        public bool IsEpic { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<ApplicationUser> Assignees { get; set; }
    }
}
