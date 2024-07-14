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
            allProjects.AddRange(dbContext.Projects.Where(x => x.OwnerId == userId && !x.isDeleted).Select(x=> new ProjectModel(x.Id, x.Name, x.Description, true)));
            allProjects.AddRange(dbContext.ProjectsToMembers.Where(x => x.UserId == userId && !x.Project.isDeleted && !x.IsRemoved).Select(x => x.Project).Select(x => new ProjectModel(x.Id, x.Name, x.Description, false)));
            return allProjects;
        }

        public async Task<List<UserModel>> GetAllProjectMembers(Guid projectId)
        {
            var allMembers = new List<UserModel>();
            var owner = await dbContext.Projects.Where(x => x.Id == projectId).Select(x=>x.Owner).Select(x=> new UserModel(x.Id, x.Email, true)).FirstOrDefaultAsync();
            allMembers.Add(owner);
            allMembers.AddRange(dbContext.ProjectsToMembers.Where(x => x.ProjectId == projectId && !x.Project.isDeleted && !x.IsRemoved).Select(x => x.User).Select(x => new UserModel(x.Id, x.Email, false)));
            return allMembers;
        }

        public bool isUserInProject(Guid projectId, string userId)
        {
            var allMembers = new List<UserModel>();
            var owner = dbContext.Projects.Any(x => x.Id == projectId && x.OwnerId == userId);
            if (owner)
            {
                return true;
            }

            return dbContext.ProjectsToMembers.Any(x => x.ProjectId == projectId && x.UserId == userId && !x.Project.isDeleted && !x.IsRemoved);
        }

        public bool isUserProjectOwner(Guid projectId, string userId)
        {
            var allMembers = new List<UserModel>();
            var owner = dbContext.Projects.Any(x => x.Id == projectId && x.OwnerId == userId);
            if (owner)
            {
                return true;
            }

            return false;
        }

        public async Task KickUser(string userId, Guid projectId)
        {
            var member = dbContext.ProjectsToMembers.FirstOrDefault(x => x.UserId == userId && x.ProjectId == projectId);
            member.IsRemoved = true;
            dbContext.SaveChanges();
        }

        public async Task<Guid> InviteUser(InviteDTO invireDto, Guid projectId)
        {
            var userId = this.dbContext.Users.FirstOrDefault(X => X.Email == invireDto.Email)?.Id;
            if (userId == null)
            {
                return Guid.Empty;
            }

            Invite invite = new Invite()
            {
                InviteId = Guid.NewGuid(),
                ProjectId = projectId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
            };

            dbContext.Invites.Add(invite);
            dbContext.SaveChanges();

            return invite.InviteId;
        }

        public async Task<Invite> GetInvite(Guid inviteId, string userId)
        {
            return await this.dbContext.Invites.FirstOrDefaultAsync(x=>x.InviteId == inviteId && x.UserId == userId && !x.IsExpired);
        }

        public async Task JoinProject(Invite invite, string userId)
        {
         
            var removedUser = dbContext.ProjectsToMembers.FirstOrDefault(x => x.UserId == userId && x.ProjectId == invite.ProjectId);
            if (removedUser != null)
            {
                removedUser.IsRemoved = false;
            }
            else
            {
                ProjectsToMembers projectToMember = new ProjectsToMembers()
                {
                    Id = Guid.NewGuid(),
                    ProjectId = invite.ProjectId,
                    UserId = userId,
                };
                dbContext.ProjectsToMembers.Add(projectToMember);
            }
            invite.IsExpired = true;

            dbContext.SaveChanges();
        }
    }
}
