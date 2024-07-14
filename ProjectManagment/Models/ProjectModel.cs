namespace ProjectManagment.Models
{
    public class ProjectModel
    {
        public ProjectModel(Guid id, string title, string Description, bool isOwner = false)
        {
            this.ProjectId = id;
            this.Title = title;
            this.Description = Description;
            this.isOwner = isOwner;

        }
        public Guid ProjectId { get; set; }
        public bool isOwner {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
