using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using ProjectManagment.Data;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    public class IssueController : Controller
    {
        private IssueRepository issueRepository;
        private IssueElementRepository issueElementRepository;
        public IssueController(IssueRepository issuesRepository, IssueElementRepository issueElementRepository)
        {
            this.issueRepository = issueRepository;
            this.issueElementRepository = issueElementRepository;
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
        public ActionResult All(Guid projectId, string searchTerm, int page = 1, int pageSize = 10)
        {
            var allIssues = GetIssues(projectId);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allIssues = allIssues.Where(i => i.Title.Contains(searchTerm) ||
                                                 i.Labels.Contains(searchTerm) ||
                                                 i.Assignees.Contains(searchTerm) ||
                                                 i.Milestone.Contains(searchTerm) ||
                                                 i.Area.Contains(searchTerm) ||
                                                 i.Status.Contains(searchTerm)).ToList();
            }

            var totalIssues = allIssues.Count;
            var totalPages = (int)Math.Ceiling((double)totalIssues / pageSize);
            var issues = allIssues.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new IssueViewModel
            {
                ProjectId = projectId,
                Issues = issues,
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
            var model = new IssueCreateModel
            {
                ProjectId = projectId,
                AvailableAssignees = new List<string>(),
                AvailableAreas = availableAssignees.ToList(),
                AvailableLabels = new List<ProjectLabel>(),
                AvailableStatuses = new List<ProjectStatus>(),
                AvailableMilestones = new List<ProjectMilestone>()
            };

            return View(model);
        }

        // POST: /Issue/Create
        [HttpPost("project/{projectId}/issue/create")]
        public IActionResult Create(IssueCreateModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to save the new issue to the database
                // For example:
                // _issueRepository.Create(model);
                // Where _issueRepository is a service responsible for handling issue-related operations
                return RedirectToAction("Index", "Home"); // Redirect to home page or any other appropriate action
            }
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
