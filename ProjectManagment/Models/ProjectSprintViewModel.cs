namespace ProjectManagment.Models
{

    public class ProjectSprintViewModel
    {
        public Guid ProjectId { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public List<ProjectSprint> Sprints { get; set; }
    }

    public class ProjectSprint
    {
        public ProjectSprint()
        {
        }

        public ProjectSprint(Guid projectId, Guid statusId, string name, string description, int number, DateTime startDate, DateTime endDate)
        {
            this.ProjectId = projectId;
            this.SprintId = statusId;
            this.Name = name;
            this.Description = description;
            this.Number = number;
            StartDate = startDate;
            EndDate = endDate;
        }
        public Guid SprintId { get; set; }
        public Guid ProjectId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


    }
}
