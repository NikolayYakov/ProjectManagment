using Microsoft.EntityFrameworkCore;
using ProjectManagment.Areas.Identity.Data;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;

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

        public async Task<Issue> GetIssue(Guid issueId)
        {
             return await dbContext.Issues.Where(x => x.Id == issueId && !x.IsDeleted)
                 .Include(x => x.Labels).Include(x => x.Milestone).Include(x => x.Assignees).Include(x => x.Area).Include(x => x.Status).Include(x=>x.Sprint).FirstOrDefaultAsync();
        }

        public async Task<int> GetLastIssueNumber(Guid projectId)
        {
            var issue = dbContext.Issues.OrderBy(x => x.CreatedAt).LastOrDefault(x => x.ProjectId == projectId);

            return issue == null ? 0: issue.Number;
        }


        public async Task<Guid> CreateIssue(IssueCreateModel issueReq, string userId, int lastNumber)
        {
            Issue issue = new Issue()
            {
                Id = new Guid(),
                Number = lastNumber + 1,
                Title = issueReq.Title,
                Body = issueReq.Body,
                OwnerId = userId,
                ProjectId = issueReq.ProjectId,
                StatusId = !string.IsNullOrEmpty(issueReq.Status) ? Guid.Parse(issueReq.Status) : null,
                AreaId = !string.IsNullOrEmpty(issueReq.Area) ? Guid.Parse(issueReq.Area) : null,
                MilestoneId = !string.IsNullOrEmpty(issueReq.Milestone) ? Guid.Parse(issueReq.Milestone) : null,
                SprintId = !string.IsNullOrEmpty(issueReq.Sprint) ? Guid.Parse(issueReq.Sprint) : null,
            };
            
            dbContext.Issues.Add(issue);
            await dbContext.SaveChangesAsync();
            return issue.Id;
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
            return dbContext.Issues
                .Where(x => x.ProjectId == projectId && !x.IsDeleted)
                .Include(x => x.Labels).Include(x => x.Milestone).Include(x=>x.Assignees).Include(x=>x.Area).Include(x=>x.Status).Include(x=>x.Sprint);
        }

        public async Task<IQueryable<Issue>> GetIssuesInProjectForSprint(Guid projectId, Guid sprintId)
        {
            return dbContext.Issues
                .Where(x => x.ProjectId == projectId && !x.IsDeleted && x.SprintId == sprintId);
                
        }

        public async Task<IQueryable<ReleaseNotesIssues>> GetIssuesInProjectForMilestone(Guid projectId, Guid? milestoneId)
        {
            return dbContext.Issues
                .Where(x => x.ProjectId == projectId && !x.IsDeleted && x.MilestoneId == milestoneId)
                .Include(x => x.Area)
                .Select(x => new ReleaseNotesIssues(x.Title, x.Area.Name));
        }

        public async Task<IQueryable<IssueLabelDTO>> GetIssueLabels(Guid issueId)
        {
            return dbContext.LabelsToIssues.Where(x => x.IssueId == issueId && !x.IsRemoved).Select(x => new IssueLabelDTO { Id = x.LabelId, Name = x.Label.Name }) ;
        }

        public async Task<IQueryable<IssueAssigneeDTO>> GetIssueAssignees(Guid issueId)
        {
            return dbContext.UsersToIssues.Where(x => x.IssueId == issueId && !x.IsRemoved).Select(x => new IssueAssigneeDTO { Id= x.UserId, Name = x.User.UserName});
        }

        public async Task AssignUser(Guid issueId, List<string> assigneesIds)
        {
            List<UsersToIssues> usersToIssues = new List<UsersToIssues>();

            var allAssignees = await this.dbContext.UsersToIssues.Where(x => x.IssueId == issueId).ToListAsync();
            foreach (var assignee in allAssignees)
            {
                assignee.IsRemoved = true;
            }

            if (assigneesIds == null)
            {
                return;
            }

            foreach (var assigneeId in assigneesIds)
            {
                if (await ReAssignUser(issueId, assigneeId))
                {
                    continue;
                }

                UsersToIssues userToIssue = new UsersToIssues()
                {
                    Id = new Guid(),
                    UserId = assigneeId,
                    IssueId = issueId
                };

                usersToIssues.Add(userToIssue);
            }

            dbContext.UsersToIssues.AddRange(usersToIssues);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ReAssignUser(Guid issueId, string userId)
        {
            var issueAssignee = dbContext.UsersToIssues.FirstOrDefault(x => x.IssueId == issueId && x.UserId == userId);
            if (issueAssignee != null)
            {
                issueAssignee.IsRemoved = false;
                return true;
            }

            return false;
        }

        public async Task AddLabels(Guid issueId, List<string> labelsIds)
        {
            List<LabelsToIssues> labelsToIssues = new List<LabelsToIssues>();

            var allLabels = await this.dbContext.LabelsToIssues.Where(x=>x.IssueId== issueId).ToListAsync();
            foreach (var label in allLabels)
            {
                label.IsRemoved = true;
            }

            if (labelsIds == null)
            {
                return;
            }

            foreach (var labelId in labelsIds)
            {
                var labelGuid = Guid.Parse(labelId);

                if(await ReAddIssueLable(issueId, labelGuid))
                {
                    continue;
                }

                LabelsToIssues labelToIssue = new LabelsToIssues()
                {
                    Id = new Guid(),
                    LabelId = labelGuid,
                    IssueId = issueId
                };
                labelsToIssues.Add(labelToIssue);
            }

            dbContext.LabelsToIssues.AddRange(labelsToIssues);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ReAddIssueLable(Guid issueId, Guid labelId)
        {
            var issueLabel = dbContext.LabelsToIssues.FirstOrDefault(x => x.IssueId == issueId && x.LabelId == labelId);
            if(issueLabel != null)
            {
                issueLabel.IsRemoved = false;
                return true;
            }
            return false;
        }
    }
}
