using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;

namespace ProjectManagment.Repositories
{
    public class IssueRepository
    {
        private ApplicationDbContext dbContext;
        public IssueRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Issue GetIssue(string issueTitle)
        {
            return dbContext.Issues.FirstOrDefault(x => x.Title == issueTitle);
        }

        public async Task CreateIssue(CreateProjectReq projectReq, string userId)
        {
            Project project = new Project()
            {
                Id = new Guid(),
                Name = projectReq.Name,
                Description = projectReq.Description,
                OwnerId = userId
            };

            dbContext.Projects.Add(project);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Project> GetUserProjectByName(string projectName, string userId)
        {
            return dbContext.Projects.FirstOrDefault(x => x.OwnerId == userId && x.Name == projectName);
        }

        public async Task<Project> GetProject(Guid ProjectId)
        {
            return dbContext.Projects.FirstOrDefault(x => x.Id == ProjectId);
        }

        public async Task DeleteProject(Project Project)
        {
            Project.isDeleted = true;
            dbContext.SaveChanges();
        }

        public async Task UpdateProject(Project Project)
        {
            dbContext.Projects.Update(Project);
            dbContext.SaveChanges();
        }

        public async Task<bool> DoesUserContainProjectWithName(string projectName, string userId)
        {
            return dbContext.Projects.Any(x => x.OwnerId == userId && x.Name == projectName && !x.isDeleted);
        }

        public async Task<List<Project>> GetAllUserProjects(string userId)
        {
            var allProjects = new List<Project>();
            allProjects.AddRange(dbContext.Projects.Where(x => x.OwnerId == userId && !x.isDeleted));
            allProjects.AddRange(dbContext.ProjectsToMembers.Where(x => x.UserId == userId && x.Project.isDeleted).Select(x => x.Project));
            return allProjects;
        }
    }
}
