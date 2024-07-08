using Microsoft.EntityFrameworkCore;
using ProjectManagment.Areas.Identity.Data;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using Microsoft.Identity;
using ProjectManagment.Models;

namespace ProjectManagment.Repositories
{
    public class ProjectRepository
    {
        private ApplicationDbContext dbContext;
        public ProjectRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Project GetProject(string projectName)
        {
            return dbContext.Projects.FirstOrDefault(x=>x.Name == projectName);
        }

        public async Task CreateProject(CreateProjectReq projectReq, string userId)
        {
            Project project = new Project()
            {
                Id = new Guid(),
                Name = projectReq.Title,
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

        public async Task<List<ProjectModel>> GetAllUserProjects(string userId)
        {
            var allProjects = new List<ProjectModel>();
            allProjects.AddRange(dbContext.Projects.Where(x => x.OwnerId == userId && !x.isDeleted).Select(x=> new ProjectModel(x.Id, x.Name, x.Description)));
            allProjects.AddRange(dbContext.ProjectsToMembers.Where(x => x.UserId == userId && !x.Project.isDeleted).Select(x => x.Project).Select(x => new ProjectModel(x.Id, x.Name, x.Description)));
            return allProjects;
        }

        public async Task<List<UserModel>> GetAllProjectMembers(Guid projectId)
        {
            var allMembers = new List<UserModel>();
            var owner = await dbContext.Projects.Where(x => x.Id == projectId).Select(x=>x.Owner).Select(x=> new UserModel(x.Id, x.Email, true)).FirstOrDefaultAsync();
            allMembers.Add(owner);
            allMembers.AddRange(dbContext.ProjectsToMembers.Where(x => x.ProjectId == projectId && !x.Project.isDeleted).Select(x => x.User).Select(x => new UserModel(x.Id, x.Email, false)));
            return allMembers;
        }


        public async Task AddUser(Guid projectId, string userId)
        {
            ProjectsToMembers projectToMember = new ProjectsToMembers()
            {
                Id = new Guid(),
                UserId = userId,
                ProjectId = projectId
            };

            dbContext.ProjectsToMembers.Add(projectToMember);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveUser(ProjectsToMembers projectToMember)
        {
            projectToMember.IsRemoved = true;
            dbContext.SaveChanges();
        }


    }
}
