using ProjectManagment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagment.Data
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        [ForeignKey("AuthorId")]
        public ApplicationUser Author { get; set; }
        public string AuthorId { get; set; }

        [ForeignKey("IssueId")]
        public Issue Issue { get; set; }
        public Guid IssueId { get; set; }

        public bool isDeleted { get; set; } = false;
    }
}
