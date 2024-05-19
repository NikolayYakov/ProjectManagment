using ProjectManagment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagment.Data
{
    public class ProjectsToMembers
    {
        public Guid Id { get; set; }
        [ForeignKey("ProjectId")]
        public Guid ProjectId { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public Project Project { get; set; }
        public ApplicationUser User { get; set; }
    }
}
