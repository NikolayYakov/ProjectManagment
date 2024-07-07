using ProjectManagment.Data;

namespace ProjectManagment.Models
{
    public class IssueDetailsModel
    {
        public Guid ProjectId { get; set; }
        public Guid IssueId { get; set; }
        public int IssueNumber { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public List<string> Assignees { get; set; }
        public string Area { get; set; }
        public string Status { get; set; }
        public string Sprint { get; set; }
        public List<string> Labels { get; set; }
        public string Milestone { get; set; }
        public List<CommentModel> Comments { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CommentModel
    {
        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime PostedAt { get; set; }
    }
}
