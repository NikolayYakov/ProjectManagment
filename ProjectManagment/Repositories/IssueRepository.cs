﻿using ProjectManagment.Areas.Identity.Data;
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

        public Issue GetIssued(Guid issueId)
        {
            return dbContext.Issues.FirstOrDefault(x => x.Id == issueId && !x.IsDeleted);
        }

        public async Task<Guid> CreateIssue(IssueCreateModel issueReq, string userId)
        {
            Issue issue = new Issue()
            {
                Id = new Guid(),
                Title = issueReq.Title,
                Body = issueReq.Body,
                OwnerId = userId,
                ProjectId = issueReq.ProjectId,
                StatusId = Guid.Parse(issueReq.Status),
                AreaId = Guid.Parse(issueReq.Area),
                MilestoneId = Guid.Parse(issueReq.Milestone),
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

        public async Task AddLabels(Guid issueId, List<string> labelsIds)
        {
            List<LabelsToIssues> labelsToIssues = new List<LabelsToIssues>();
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
            var issueLabel = dbContext.LabelsToIssues.FirstOrDefault(x => x.IssueId == issueId && x.IssueId == issueId);
            if(issueLabel != null)
            {
                issueLabel.IsRemoved = false;
                return true;
            }
            return false;
        }

        public async Task RemoveLabel(LabelsToIssues labelToIssue)
        {
            labelToIssue.IsRemoved = true;
            dbContext.SaveChanges();
        }
    }
}
