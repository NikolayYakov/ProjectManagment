using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Data;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    public class IssueController : Controller
    {
        private IssueRepository issueRepository;
        public IssueController(IssueRepository issuesRepository)
        {
            this.issueRepository = issueRepository;
        }

        private List<IssueModel> GetIssues()
        {
            //issueRepository.GetIssuesInProject();
            return new List<IssueModel>
            {
                new IssueModel { IssueNumber = 1, Title = "Issue A", Labels = "Bug", Assignees = "John Doe", Milestone = "Milestone 1", Area = "Backend", Status = "Open" },
                new IssueModel { IssueNumber = 2, Title = "Issue B", Labels = "Feature", Assignees = "Jane Smith", Milestone = "Milestone 2", Area = "Frontend", Status = "Closed" },
                // Add more issues here
            };
        }

        [HttpGet("project/{id}/issue/all")]
        public ActionResult All(Guid id, string searchTerm, int page = 1, int pageSize = 10)
        {
            var allIssues = GetIssues();

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
                Issues = issues,
                SearchTerm = searchTerm,
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            // Example issue data
            var issue = new IssueDetailsModel
            {
                Id = id,
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

        public IActionResult Edit(int id)
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
                AvailableMilestones = new List<string> { "Sprint 1", "Sprint 2", "Release 1.0" }
            };

            return View(model);
        }

        public IActionResult Create()
        {
            var model = new IssueCreateModel
            {
                AvailableAssignees = new List<string>(), // Initialize with empty list
                AvailableAreas = new List<string>(), // Initialize with empty list
                AvailableLabels = new List<string>(), // Initialize with empty list
                AvailableMilestones = new List<string>() // Initialize with empty list
            };

            return View(model);
        }

        // POST: /Issue/Create
        [HttpPost]
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
