namespace ProjectManagment.Models
{
    public class GenerateReleaseNotesModel
    {
        public GenerateReleaseNotesModel()
        {
            
        }
        public GenerateReleaseNotesModel(Guid projectId, List<ProjectMilestone> milestones)
        {
            this.ProjectId = projectId;
            this.Milestones = milestones;
        }
        public Guid ProjectId { get; set; }
        public List<ProjectMilestone> Milestones { get; set; }
        public Guid? SelectedMilestoneId { get; set; }
    }
}
