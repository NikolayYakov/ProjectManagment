using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using ProjectManagment.Data;
using ProjectManagment.Models;
using ProjectManagment.Repositories;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    public class IssueController : Controller
    {
        private IssueRepository issueRepository;
        private IssueElementRepository issueElementRepository;
        private ProjectRepository projectRepository;
        public IssueController(IssueRepository issuesRepository, IssueElementRepository issueElementRepository, ProjectRepository projectRepository)
        {
            this.issueRepository = issuesRepository;
            this.issueElementRepository = issueElementRepository;
            this.projectRepository = projectRepository;
        }

        private List<IssueModel> GetIssues(Guid projectId)
        {
            //issueRepository.GetIssuesInProject();
            return new List<IssueModel>
            {
                new IssueModel { IssueNumber = 1, Title = "Issue A", Labels = "Bug", Assignees = "John Doe", Milestone = "Milestone 1", Area = "Backend", Status = "Open", ProjectId = projectId, IssueId = projectId },
                new IssueModel { IssueNumber = 2, Title = "Issue B", Labels = "Feature", Assignees = "Jane Smith", Milestone = "Milestone 2", Area = "Frontend", Status = "Closed", ProjectId = projectId, IssueId = projectId  },
                // Add more issues here
            };
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
                    IssueNumber = 0,
                    IssueId = issue.Id,
                    Title = issue.Title,
                    Assignees = string.Join(", ", issueAssignees.Select(x=>x.Name)),
                    Labels = string.Join(", ", issueLabels.Select(x=>x.Name)),
                    Milestone = issue.Milestone.Name,
                    Status = issue.Status.Name,
                    Area = issue.Area.Name,
                    ProjectId = issue.ProjectId,
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
        public IActionResult Details(Guid projectId, Guid issueId)
        {
            // Example issue data
            var issue = new IssueDetailsModel
            {
                IssueId = issueId,
                ProjectId = projectId,
                Title = "Example Issue",
                Body = "This is a sample issue description.",
                Assignee = "John Doe",
                Area = "Backend",
                Labels = new List<string> { "bug", "documentation" },
                Milestone = "Sprint 1",
                CreatedAt = DateTime.Now,
                Comments = new List<CommentModel>
                {
                    new CommentModel { Author = "Alice", Text = "Initial comment.", PostedAt = DateTime.Now },
                    new CommentModel { Author = "Bob", Text = "Follow-up comment.", PostedAt = DateTime.Now.AddMinutes(10) }
                }
            };

            return View(issue);
        }

        [HttpGet("project/{projectId}/issue/{issueId}/edit")]
        public IActionResult Edit(int projectId)
        {
            //var issue = Issues.FirstOrDefault(i => i.Id == id);
            //if (issue == null)
            //{
            //    return NotFound();
            //}

            var model = new IssueEditModel
            {
                Id = 1,
                Title = "Test Title",
                Body = "Test Body",
                Assignees = new List<string> { "Boris", "NikoalyY" },
                Area = "Constant Area",
                Labels = new List<string> { "constant-bug", "constant-documentation" },
                Milestone = "Constant Sprint",
                AvailableAssignees = new List<string> { "Constant Assignee", "John Doe", "Jane Smith" },
                AvailableAreas = new List<string> { "Backend", "Frontend", "UI/UX" },
                AvailableLabels = new List<string> { "bug", "documentation", "enhancement" },
                AvailableMilestones = new List<string> { "Sprint 1", "Sprint 2", "Release 1.0" },
                AvailableStatuses = new List<string> { "Sprint 1", "Sprint 2", "Release 1.0" }
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/issue/create")]
        public async Task<IActionResult> Create(Guid projectId)
        {
            var availableAssignees = await this.issueElementRepository.GetAllProjectAreas(projectId);
            var availableMilestone = await this.issueElementRepository.GetAllProjectMilestones(projectId);
            var availalbeStatuses = await this.issueElementRepository.GetAllProjectStatuses(projectId);
            var availalbeLabels = await this.issueElementRepository.GetAllProjectLabels(projectId);
            var availalbeAssignees = await this.projectRepository.GetAllProjectMembers(projectId);

            var model = new IssueCreateModel
            {
                ProjectId = projectId,
                AvailableAssignees = availalbeAssignees.ToList(),
                AvailableAreas = availableAssignees.ToList(),
                AvailableLabels = availalbeLabels.ToList(),
                AvailableStatuses = availalbeStatuses.ToList(),
                AvailableMilestones = availableMilestone.ToList(),
            };

            return View(model);
        }

        // POST: /Issue/Create
        [HttpPost("project/{projectId}/issue/create")]
        public async Task<IActionResult> Create(IssueCreateModel model)
        {
            //if (ModelState.IsValid)
            //{
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var issueId = await this.issueRepository.CreateIssue(model, userId);
                await this.issueRepository.AddLabels(issueId, model.Labels);
                await this.issueRepository.AssignUser(issueId, model.Assignees);

                string url = Url.Content($"~/project/{model.ProjectId}/issue/all");
                return Redirect(url);
            //}
            // If ModelState is not valid, return back to the create page with the model
            return View(model);
        }

        //[HttpPost]
        //public IActionResult EditIssue(IssueEditModel model)
        //{
        //    var issue = Issues.FirstOrDefault(i => i.Id == model.Id);
        //    if (issue == null)
        //    {
        //        return NotFound();
        //    }

        //    issue.Title = model.Title;
        //    issue.Body = model.Body;
        //    issue.Assignee = model.Assignee;
        //    issue.Area = model.Area;
        //    issue.Labels = model.Labels;
        //    issue.Milestone = model.Milestone;

        //    return RedirectToAction("Details", new { id = model.Id });
        //}
    }
}
