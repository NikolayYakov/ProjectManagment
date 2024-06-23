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
        public ProjectLabel()
        {
        }

        public ProjectLabel(Guid projectId, Guid labelId, string name, string description, int number)
        {
            this.ProjectId = projectId;
            this.LabelId = labelId;
            this.Name = name;
            this.Description = description;
            this.Number = number;
        }


        public Guid ProjectId { get; set; }
        public Guid LabelId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
