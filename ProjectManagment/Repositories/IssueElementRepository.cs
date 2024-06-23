using Microsoft.EntityFrameworkCore;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;

namespace ProjectManagment.Repositories
{
    public class IssueElementRepository
    {
        private ApplicationDbContext dbContext;
        public IssueElementRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddLabelToProject(CreateIssueElementReq issueElementReq, Guid projectId, int lastLabelNumber)
        {
            Label label = new Label()
            {
                Id = new Guid(),
                Name = issueElementReq.Name,
                Description = issueElementReq.Description,
                Number = lastLabelNumber + 1,
                ProjectId = projectId,
            };

            dbContext.Labels.Add(label);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddStatusToProject(CreateIssueElementReq issueElementReq, Guid projectId, int lastStatusNumber)
        {
            Status status = new Status()
            {
                Id = new Guid(),
                Name = issueElementReq.Name,
                Description = issueElementReq.Description,
                Number = lastStatusNumber + 1,
                ProjectId = projectId,
            };

            dbContext.Status.Add(status);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddMilestoneToProject(CreateIssueElementReq issueElementReq, Guid projectId, int lastMilestoneNumber)
        {
            Milestone milestone = new Milestone()
            {
                Id = new Guid(),
                Name = issueElementReq.Name,
                Description = issueElementReq.Description,
                Number = lastMilestoneNumber + 1,
                ProjectId = projectId,
            };

            dbContext.Milestones.Add(milestone);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddAreaToProject(CreateIssueElementReq issueElementReq, Guid projectId, int lastAreaNumber)
        {
            Area area = new Area()
            {
                Id = new Guid(),
                Name = issueElementReq.Name,
                Description = issueElementReq.Description,
                Number = lastAreaNumber + 1,
                ProjectId = projectId,
            };

            dbContext.Areas.Add(area);
            await dbContext.SaveChangesAsync();
        }


        public async Task DeleteLabelFromProject(Label label)
        {
            label.isDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteStatusFromProject(Status status)
        {
            status.isDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteeMilestoneFromProject(Milestone milestone)
        {
            milestone.isDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAreaFromProject(Area area)
        {
            area.isDeleted = true;
            await dbContext.SaveChangesAsync();
        }

        public async Task<Label> GetLabelFromProject(Guid labelId)
        {
            return await this.dbContext.Labels.FirstOrDefaultAsync(x => x.Id == labelId && !x.isDeleted);
        }

        public async Task<Status> GetStatusFromProject(Guid statusId)
        {
            return await this.dbContext.Status.FirstOrDefaultAsync(x => x.Id == statusId);
        }

        public async Task<Milestone> GetMilestoneFromProject(Guid milestoneId)
        {
            return await this.dbContext.Milestones.FirstOrDefaultAsync(x => x.Id == milestoneId);
        }

        public async Task<Area> GetAreaFromProject(Guid areaId)
        {
            return await this.dbContext.Areas.FirstOrDefaultAsync(x => x.Id == areaId);
        }

        public async Task UpdateLabel(Label label)
        {
            dbContext.Labels.Update(label);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateStatus(Status status)
        {
            dbContext.Status.Update(status);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateMilestone(Milestone milestone)
        {
            dbContext.Milestones.Update(milestone);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateArea(Area area)
        {
            dbContext.Areas.Update(area);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DoesProjectContainLabelWithName(string labelName, Guid projectId)
        {
            return await dbContext.Labels.AnyAsync(x => x.ProjectId == projectId && x.Name == labelName && !x.isDeleted);
        }

        public async Task<bool> DoesProjectContainStatusWithName(string statusName, Guid projectId)
        {
            return await dbContext.Status.AnyAsync(x => x.ProjectId == projectId && x.Name == statusName && !x.isDeleted);
        }

        public async Task<bool> DoesProjectContainMilestoneWithName(string milestoneName, Guid projectId)
        {
            return await dbContext.Milestones.AnyAsync(x => x.ProjectId == projectId && x.Name == milestoneName && !x.isDeleted);
        }

        public async Task<bool> DoesProjectContainAreaWithName(string areaName, Guid projectId)
        {
            return await dbContext.Areas.AnyAsync(x => x.ProjectId == projectId && x.Name == areaName && !x.isDeleted);
        }

        public async Task<IQueryable<ProjectLabel>> GetAllProjectLabels(Guid projectId)
        {
            return this.dbContext.Labels.Where(x=>x.ProjectId == projectId && !x.isDeleted)
                                        .Select(x=>new ProjectLabel(projectId, x.Id, x.Name, x.Description, x.Number));
        }

        public async Task<IQueryable<ProjectStatus>> GetAllProjectStatuses(Guid projectId)
        {
            return this.dbContext.Status.Where(x => x.ProjectId == projectId && !x.isDeleted)
                                        .Select(x => new ProjectStatus(projectId, x.Id, x.Name, x.Description, x.Number));
        }

        public async Task<IQueryable<ProjectMilestone>> GetAllProjectMilestones(Guid projectId)
        {
            return this.dbContext.Milestones.Where(x => x.ProjectId == projectId && !x.isDeleted)
                                            .Select(x => new ProjectMilestone(projectId, x.Id, x.Name, x.Description, x.Number));
        }

        public async Task<IQueryable<ProjectArea>> GetAllProjectAreas(Guid projectId)
        {
            return this.dbContext.Areas.Where(x => x.ProjectId == projectId && !x.isDeleted)
                                        .Select(x => new ProjectArea(projectId, x.Id, x.Name, x.Description, x.Number));
        }

        public async Task<int> GetLastProjectLabelNumber(Guid projectId)
        {
            var lastLabel = await this.dbContext.Labels.Where(x => x.ProjectId == projectId).OrderBy(x => x.Number).LastOrDefaultAsync();

            return lastLabel == null ? 0 : lastLabel.Number;
        }

        public async Task<int> GetLastProjectAreaNumber(Guid projectId)
        {
            var lastArea = await this.dbContext.Areas.Where(x=>x.ProjectId == projectId).OrderBy(x => x.Number).LastOrDefaultAsync();

            return lastArea == null ? 0 : lastArea.Number;
        }

        public async Task<int> GetLastProjectMilestoneNumber(Guid projectId)
        {
            var lastMilestone = await this.dbContext.Milestones.Where(x => x.ProjectId == projectId).OrderBy(x => x.Number).LastOrDefaultAsync();

            return lastMilestone == null ? 0 : lastMilestone.Number;
        }

        public async Task<int> GetLastProjectStatusNumber(Guid projectId)
        {
            var lastStatus = await this.dbContext.Status.Where(x => x.ProjectId == projectId).OrderBy(x => x.Number).LastOrDefaultAsync();

            return lastStatus == null ? 0 : lastStatus.Number;
        }
    }
}
