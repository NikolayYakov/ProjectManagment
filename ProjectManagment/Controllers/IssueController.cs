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
            // Replace this with actual data fetching logic
            return new List<IssueModel>
            {
                new IssueModel { IssueNumber = 1, Title = "Issue A", Labels = "Bug", Assignees = "John Doe", Milestone = "Milestone 1", Area = "Backend", Status = "Open" },
                new IssueModel { IssueNumber = 2, Title = "Issue B", Labels = "Feature", Assignees = "Jane Smith", Milestone = "Milestone 2", Area = "Frontend", Status = "Closed" },
                // Add more issues here
            };
        }

        public ActionResult AllIssues(string searchTerm, int page = 1, int pageSize = 10)
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
    }
}
