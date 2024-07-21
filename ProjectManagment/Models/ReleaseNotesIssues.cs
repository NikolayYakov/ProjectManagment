namespace ProjectManagment.Models
{
    public class ReleaseNotesIssues
    {
        public ReleaseNotesIssues()
        {
            
        }
        public ReleaseNotesIssues(string title, string areaName)
        {
            this.Title = title;
            this.AreaName = areaName;
        }

        public string Title { get; set; }
        public string AreaName { get; set; }
    }
}
