namespace ProjectManagment.Models
{
    public class ProjectLabelViewModel
    {
        public Guid ProjectId { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public List<ProjectLabel> Labels { get; set; }
    }

    public class ProjectLabel
    {
        public Guid ProjectId { get; set; }
        public Guid LabelId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
