using ProjectManagment.Areas.Identity.Data;
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

        public Issue GetIssueByTitle(string issueTitle)
        {
            return dbContext.Issues.FirstOrDefault(x => x.Title == issueTitle && !x.IsDeleted);
        }

        public Issue GetIssued(Guid issueId)
        {
            return dbContext.Issues.FirstOrDefault(x => x.Id == issueId && !x.IsDeleted);
        }

        public async Task CreateIssue(CreateIssueReq issueReq, string userId)
        {
            Issue issue = new Issue()
            {
                Id = new Guid(),
                Title = issueReq.Title,
                Body = issueReq.Body,
                OwnerId = userId,
                ProjectId = issueReq.ProjectId,
                StatusId = issueReq.StatusId,
                AreaId = issueReq.AreaId,
                IsEpic = issueReq.isEpic
            };

            dbContext.Issues.Add(issue);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteIssue(Issue issue)
        {
            issue.IsDeleted = true;
            dbContext.SaveChanges();
        }

        public async Task UpdateIssue(Issue issue)
        {
            dbContext.Issues.Update(issue);
            dbContext.SaveChanges();
        }

        public async Task<IQueryable<Issue>> GetIssuesInProject(Guid projectId)
        {
            return dbContext.Issues.Where(x => x.ProjectId == projectId);
        }

        public async Task<IQueryable<Label>> GetIssueLabels(Guid issueId)
        {
            return dbContext.LabelsToIssues.Where(x => x.IssueId == issueId).Select(x=>x.Label);
        }

        public async Task<IQueryable<ApplicationUser>> GetIssueAssignees(Guid issueId)
        {
            return dbContext.UsersToIssues.Where(x => x.IssueId == issueId).Select(x => x.User);
        }

        public async Task AssignUser(Guid issueId, string userId)
        {
            UsersToIssues userToIssue = new UsersToIssues()
            {
                Id = new Guid(),
                UserId = userId,
                IssueId = issueId
            };

            dbContext.UsersToIssues.Add(userToIssue);
            await dbContext.SaveChangesAsync();
        }

        public async Task UnAssignUser(UsersToIssues userToIssue)
        {
            userToIssue.IsRemoved = true;
            dbContext.SaveChanges();
        }

        public async Task AddLabel(Guid issueId, Guid labelId)
        {
            LabelsToIssues labelToIssue = new LabelsToIssues()
            {
                Id = new Guid(),
                LabelId = labelId,
                IssueId = issueId
            };

            dbContext.LabelsToIssues.Add(labelToIssue);
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveLabel(LabelsToIssues labelToIssue)
        {
            labelToIssue.IsRemoved = true;
            dbContext.SaveChanges();
        }
    }
}
