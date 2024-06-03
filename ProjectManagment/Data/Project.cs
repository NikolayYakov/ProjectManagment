using ProjectManagment.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ProjectManagment.Data
{
    public class Project
    {
        public Project(Guid id, string title, string description)
        {
            this.Id = id;
            this.Name = title;
            this.Description = description;
        }
        public Project()
        {
            
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("OwnerId")]
        public ApplicationUser Owner { get; set; }
        public string OwnerId { get; set; }
        public ICollection<Issue> Issues { get; set; }
        public ICollection<Milestone> Milestones { get; set; }
        public ICollection<Label> Labels { get; set; }
        public ICollection<Status> Statuses { get; set; }
        public ICollection<Area> Areas { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
        public bool isDeleted { get; set; } = false;

    }
}
