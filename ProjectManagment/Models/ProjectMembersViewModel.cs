namespace ProjectManagment.Models
{
    public class ProjectMembersViewModel
    {
        public Guid ProjectId { get; set; }
        public List<UserModel> Members { get; set; }
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
    }
}
