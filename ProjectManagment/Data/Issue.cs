using ProjectManagment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagment.Data
{
    public class Issue
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public Guid ProjectId { get; set; }
        [ForeignKey("OwnerId")]
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Milestone Milestone { get; set; }
        public int MilestoneId { get; set; }
        public List<Label> Labels { get; set; }
    }
}
