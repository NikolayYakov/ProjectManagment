using Microsoft.AspNetCore.Mvc;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    public class SprintController : Controller
    {
        private IssueElementRepository issueElementRepository;
        public SprintController(IssueElementRepository issueElementRepository)
        {
            this.issueElementRepository = issueElementRepository;
        }

        [HttpGet("project/{projectId}/sprint/all")]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var sprints = await this.issueElementRepository.GetAllProjectSprints(projectId);

            var model = new ProjectSprintViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Sprints = sprints.ToList(),
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/sprint/create")]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/sprint/create")]
        public async Task<IActionResult> Create(Guid projectId, CreateSprintReq req)
        {
            var lastSprintNumber = await this.issueElementRepository.GetLastProjectSprintNumber(projectId);
            await this.issueElementRepository.AddSprintToProject(req, projectId, lastSprintNumber);

            string url = Url.Content($"~/project/{projectId}/sprint/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/sprint/{sprintId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid sprintId)
        {
            var sprint = await this.issueElementRepository.GetSprintFromProject(sprintId);
            return View(new ProjectSprint(projectId, sprint.Id, sprint.Name, sprint.Description, sprint.Number, sprint.StartDate, sprint.EndDate));
        }

        [HttpPost("project/{projectId}/sprint/{sprintId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid sprintId, CreateSprintReq req)
        {
            var sprint = await this.issueElementRepository.GetSprintFromProject(sprintId);
            sprint.Name = req.Name;
            sprint.Description = req.Description;
            sprint.StartDate = req.StartDate;
            sprint.EndDate = req.EndDate;

            await this.issueElementRepository.UpdateSprint(sprint);

            string url = Url.Content($"~/project/{projectId}/sprint/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid statusId, Guid projectId)
        {
            var sprint = await this.issueElementRepository.GetSprintFromProject(statusId);
            await this.issueElementRepository.DeleteSprintFromProject(sprint);

            string url = Url.Content($"~/project/{projectId}/sprint/all");
            return Redirect(url);
        }
    }
}
