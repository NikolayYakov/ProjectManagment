using ProjectManagment.Areas.Identity.Data;

namespace ProjectManagment.Data
{
    public class UsersToIssues
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid IssueId { get; set; }
        public ApplicationUser User { get; set; }
        public Issue Issue { get; set; }
        public bool IsRemoved { get; set; } = false;
    }
}
