using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Attributes;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class SprintController : Controller
    {
        private IssueElementRepository issueElementRepository;
        private ProjectRepository projectRepository;
        public SprintController(IssueElementRepository issueElementRepository, ProjectRepository projectRepository)
        {
            this.issueElementRepository = issueElementRepository;
            this.projectRepository = projectRepository;
        }

        [HttpGet("project/{projectId}/sprint/all")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var sprints = await this.issueElementRepository.GetAllProjectSprints(projectId);

            var filterdSprints = sprints.ToList().Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) || searchTerm == string.Empty);

            var totalItems = filterdSprints.Count();
            var itemsPerPage = 10;
            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);

            var sprintForPage = filterdSprints.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var model = new ProjectSprintViewModel
            {
                PageNumber = page,
                TotalPages = totalPages,
                SearchTerm = searchTerm,
                Sprints = sprintForPage.ToList(),
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/sprint/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/sprint/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Create(Guid projectId, CreateSprintReq req)
        {
            var lastSprintNumber = await this.issueElementRepository.GetLastProjectSprintNumber(projectId);
            await this.issueElementRepository.AddSprintToProject(req, projectId, lastSprintNumber);

            string url = Url.Content($"~/project/{projectId}/sprint/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/sprint/{sprintId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid sprintId)
        {
            var sprint = await this.issueElementRepository.GetSprintFromProject(sprintId);
            return View(new ProjectSprint(projectId, sprint.Id, sprint.Name, sprint.Description, sprint.Number, sprint.StartDate, sprint.EndDate));
        }

        [HttpPost("project/{projectId}/sprint/{sprintId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.projectRepository.isUserInProject(projectId, userId))
            {
                return new ForbidResult();
            }

            var sprint = await this.issueElementRepository.GetSprintFromProject(statusId);
            await this.issueElementRepository.DeleteSprintFromProject(sprint);

            string url = Url.Content($"~/project/{projectId}/sprint/all");
            return Redirect(url);
        }
    }
}
