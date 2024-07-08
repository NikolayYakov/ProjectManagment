using ProjectManagment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagment.Data
{
    public class Invite
    {
        public Guid InviteId { get; set; }
        public Guid ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsExpired { get; set; } = false;
    }
}
