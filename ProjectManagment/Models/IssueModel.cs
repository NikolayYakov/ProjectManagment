namespace ProjectManagment.Models
{
    public class IssueModel
    {
        public int IssueNumber { get; set; }
        public string Title { get; set; }
        public string Labels { get; set; }
        public string Assignees { get; set; }
        public string Milestone { get; set; }
        public string Area { get; set; }
        public string Status { get; set; }
    }

    public class IssueViewModel
    {
        public Guid ProjectId { get; set; }
        public List<IssueModel> Issues { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
