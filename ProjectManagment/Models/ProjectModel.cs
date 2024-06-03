namespace ProjectManagment.Models
{
    public class ProjectModel
    {
        public ProjectModel(Guid id, string title, string Description)
        {
            this.ProjectId = id;
            this.Title = title;
            this.Description = Description;
        }
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
