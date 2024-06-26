using System.Collections.Generic;

namespace ProjectManagment.Models
{
    public class ProjectAreaViewModel
    {
        public Guid ProjectId { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public List<ProjectArea> Areas { get; set; }
    }

    public class ProjectArea
    {
        public ProjectArea()
        {
        }

        public ProjectArea(Guid projectId, Guid areaId, string name, string description, int number)
        {
            this.ProjectId = projectId;
            this.AreaId = areaId;
            this.Name = name;
            this.Description = description;
            this.Number = number;
        }

        public Guid ProjectId { get; set; }
        public Guid AreaId  { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
