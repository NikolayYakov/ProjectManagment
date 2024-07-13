using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class IssueController : Controller
    {
        private IssueRepository issueRepository;
        private IssueElementRepository issueElementRepository;
        private ProjectRepository projectRepository;
        private CommentRepositorie commentRepository;

        public IssueController(IssueRepository issuesRepository, IssueElementRepository issueElementRepository, ProjectRepository projectRepository, CommentRepositorie commentRepository)
        {
            this.issueRepository = issuesRepository;
            this.issueElementRepository = issueElementRepository;
            this.projectRepository = projectRepository;
            this.commentRepository = commentRepository;
        }


        [HttpGet("project/{projectId}/issue/all")]
        public async Task<ActionResult> All(Guid projectId, string searchTerm, int page = 1, int pageSize = 10)
        {
            var allIssues = await this.issueRepository.GetIssuesInProject(projectId);

            var issueModels = new List<IssueModel>();
            foreach (var issue in allIssues)
            {
                var issueAssignees = await this.issueRepository.GetIssueAssignees(issue.Id);
                var issueLabels = await this.issueRepository.GetIssueLabels(issue.Id);

                var list =  issueLabels.ToList();

                var issueModel = new IssueModel
                {
                    IssueNumber = issue.Number,
                    IssueId = issue.Id,
                    Title = issue.Title,
                    Assignees = string.Join(", ", issueAssignees.Select(x=>x.Name)),
                    Labels = string.Join(", ", issueLabels.Select(x=>x.Name)),
                    Milestone = issue.Milestone.Name,
                    Status = issue.Status.Name,
                    Area = issue.Area.Name,
                    ProjectId = issue.ProjectId,
                    Sprint = issue.Sprint.Name
                };

                issueModels.Add(issueModel);
            }

            

            if (!string.IsNullOrEmpty(searchTerm))
            {
                issueModels = issueModels.Where(i => i.Title.Contains(searchTerm) ||
                                                 i.Labels.Contains(searchTerm) ||
                                                 i.Assignees.Contains(searchTerm) ||
                                                 i.Milestone.Contains(searchTerm) ||
                                                 i.Area.Contains(searchTerm) ||
                                                 i.Status.Contains(searchTerm)).ToList();
            }

            var totalIssues = issueModels.Count;
            var totalPages = (int)Math.Ceiling((double)totalIssues / pageSize);
            var issues = allIssues.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new IssueViewModel
            {
                ProjectId = projectId,
                Issues = issueModels,
                SearchTerm = searchTerm,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        [HttpGet("project/{projectId}/issue/{issueId}/details")]
        public async Task<IActionResult> Details(Guid projectId, Guid issueId)
        {
            var issue = await this.issueRepository.GetIssue(issueId);

            var issueAssignees = await this.issueRepository.GetIssueAssignees(issueId);
            var issueLabels = await this.issueRepository.GetIssueLabels(issueId);
            var issueComment = await this.commentRepository.GetIssueComments(issueId);
            var list = issueLabels.ToList();

            var issueModel = new IssueDetailsModel
            {
                IssueNumber = 0,
                IssueId = issue.Id,
                Title = issue.Title,
                Body = issue.Body,
                Assignees = issueAssignees.Select(x => x.Name).ToList(),
                Labels = issueLabels.Select(x => x.Name).ToList(),
                Milestone = issue.Milestone.Name,
                Status = issue.Status.Name,
                Sprint = issue.Sprint.Name,
                Area = issue.Area.Name,
                ProjectId = issue.ProjectId,
                CreatedAt = DateTime.UtcNow,
                Comments = issueComment?.Select(x=>new CommentModel { PostedAt = x.PostedAt, Author = x.Author.Email, Text = x.Content }).ToList() ?? new List<CommentModel>()
            };

            return View(issueModel);
        }

        [HttpGet("project/{projectId}/issue/{issueId}/edit")]
        public  async Task<IActionResult> Edit(Guid projectId, Guid issueId)
        {
            var availalbeAreas = await this.issueElementRepository.GetAllProjectAreas(projectId);
            var availableMilestone = await this.issueElementRepository.GetAllProjectMilestones(projectId);
            var availalbeStatuses = await this.issueElementRepository.GetAllProjectStatuses(projectId);
            var availalbeLabels = await this.issueElementRepository.GetAllProjectLabels(projectId);
            var availableAssignees = await this.projectRepository.GetAllProjectMembers(projectId);
            var availalbeSprints = await this.issueElementRepository.GetAllProjectSprints(projectId);

            var issue = await this.issueRepository.GetIssue(issueId);

            var issueAssignees = await this.issueRepository.GetIssueAssignees(issue.Id);
            var issueLabels = await this.issueRepository.GetIssueLabels(issue.Id);

            var model = new IssueEditModel
            {
                Id = issue.Id,
                ProjectId = projectId,
                Title = issue.Title,
                Body = issue.Body,
                Assignees = issueAssignees.Select(x=> x.Id).ToList(),
                Area = issue.Area.Id.ToString(),
                Labels = issueLabels.Select(x => x.Id.ToString()).ToList(),
                Milestone = issue.Milestone.Id.ToString(),
                Sprint = issue.Sprint.Id.ToString(),
                Status = issue.Status.Id.ToString(),

                AvailableAssignees = availableAssignees.ToList(),
                AvailableAreas = availalbeAreas.ToList(),
                AvailableLabels = availalbeLabels.ToList(),
                AvailableMilestones = availableMilestone.ToList(),
                AvailableStatuses = availalbeStatuses.ToList(),
                AvailableSprints = availalbeSprints.ToList(),
            };

            return View(model);
        }

        [HttpPost("project/{projectId}/issue/{issueId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid issueId,IssueEditModel issueEditModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.issueRepository.AddLabels(issueId, issueEditModel.Labels);
            await this.issueRepository.AssignUser(issueId, issueEditModel.Assignees);
            var issue = await this.issueRepository.GetIssue(issueId);

            issue.Title = issueEditModel.Title;
            issue.Body = issueEditModel.Body;

            issue.AreaId = Guid.Parse(issueEditModel.Area);
            issue.MilestoneId = Guid.Parse(issueEditModel.Milestone);
            issue.StatusId = Guid.Parse(issueEditModel.Status);
            issue.SprintId = Guid.Parse(issueEditModel.Sprint);

            await this.issueRepository.UpdateIssue(issue);

            string url = Url.Content($"~/project/{projectId}/issue/{issueId}/details");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/issue/create")]
        public async Task<IActionResult> Create(Guid projectId)
        {
            var availalbeAreas = await this.issueElementRepository.GetAllProjectAreas(projectId);
            var availableMilestone = await this.issueElementRepository.GetAllProjectMilestones(projectId);
            var availalbeStatuses = await this.issueElementRepository.GetAllProjectStatuses(projectId);
            var availalbeLabels = await this.issueElementRepository.GetAllProjectLabels(projectId);
            var availableAssignees = await this.projectRepository.GetAllProjectMembers(projectId);
            var availalbeSprints = await this.issueElementRepository.GetAllProjectSprints(projectId);


            var model = new IssueCreateModel
            {
                ProjectId = projectId,
                AvailableAssignees = availableAssignees.ToList(),
                AvailableAreas = availalbeAreas.ToList(),
                AvailableLabels = availalbeLabels.ToList(),
                AvailableStatuses = availalbeStatuses.ToList(),
                AvailableMilestones = availableMilestone.ToList(),
                AvailableSprints = availalbeSprints.ToList(),
            };

            return View(model);
        }

        // POST: /Issue/Create
        [HttpPost("project/{projectId}/issue/create")]
        public async Task<IActionResult> Create(Guid projectId, IssueCreateModel model)
        {
            //if (ModelState.IsValid)
            //{
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var lastNumber = await this.issueRepository.GetLastIssueNumber(projectId);
            var issueId = await this.issueRepository.CreateIssue(model, userId, lastNumber);
            await this.issueRepository.AddLabels(issueId, model.Labels);
            await this.issueRepository.AssignUser(issueId, model.Assignees);

            string url = Url.Content($"~/project/{projectId}/issue/all");
            return Redirect(url);
            //}
            // If ModelState is not valid, return back to the create page with the model
            return View(model);
        }

        [HttpPost("project/{projectId}/issue/{issueId}/comment")]
        public async Task<IActionResult> Comment(Guid projectId, Guid issueId, CreateCommentReq model)
        {
            //if (ModelState.IsValid)
            //{
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var commnet = new Comment()
            {
                Id = Guid.NewGuid(),
                AuthorId = userId,
                IssueId = issueId,
                PostedAt = DateTime.UtcNow,
                Content = model.Content,
            };

            this.commentRepository.CreateComment(commnet);
            string url = Url.Content($"~/project/{projectId}/issue/{issueId}/details");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid issueId, Guid projectId)
        {
            var issue = await this.issueRepository.GetIssue(issueId);
            await this.issueRepository.DeleteIssue(issue);

            string url = Url.Content($"~/project/{projectId}/issue/all");
            return Redirect(url);
        }
    }
}
