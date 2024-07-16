using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ProjectManagment.Attributes;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class AreaController : Controller
    {
        private IssueElementRepository issueElementRepository;
        private ProjectRepository projectRepository;

        public AreaController(IssueElementRepository issueElementRepository, ProjectRepository projectRepository)
        {
            this.issueElementRepository = issueElementRepository;
            this.projectRepository = projectRepository;
        }

        [HttpGet("project/{projectId}/area/all")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var areas = await this.issueElementRepository.GetAllProjectAreas(projectId);

            var filterdAreas = areas.ToList().Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) || searchTerm == string.Empty);

            var totalItems = filterdAreas.Count();
            var itemsPerPage = 10;
            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);

            var areasForPage = filterdAreas.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var model = new ProjectAreaViewModel
            {
                PageNumber = page,
                TotalPages = totalPages,
                SearchTerm = searchTerm,
                ProjectId = projectId,
                Areas = areasForPage
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/area/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/area/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Create(Guid projectId, CreateIssueElementReq req)
        {
            var lastAreaNumber = await this.issueElementRepository.GetLastProjectAreaNumber(projectId);
            await this.issueElementRepository.AddAreaToProject(req, projectId, lastAreaNumber);

            string url = Url.Content($"~/project/{projectId}/area/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/area/{areaId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid areaId)
        {
            var area = await this.issueElementRepository.GetAreaFromProject(areaId);

            return View(new ProjectArea(projectId, area.Id, area.Name, area.Description, area.Number));
        }

        [HttpPost("project/{projectId}/area/{areaId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid areaId, CreateIssueElementReq req)
        {
            var arae = await this.issueElementRepository.GetAreaFromProject(areaId);
            arae.Name = req.Name;
            arae.Description = req.Description;

            await this.issueElementRepository.UpdateArea(arae);

            string url = Url.Content($"~/project/{projectId}/area/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid areaId, Guid projectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.projectRepository.isUserInProject(projectId, userId))
            {
                return new ForbidResult();
            }

            var area = await this.issueElementRepository.GetAreaFromProject(areaId);
            await this.issueElementRepository.DeleteAreaFromProject(area);

            string url = Url.Content($"~/project/{projectId}/area/all");
            return Redirect(url);
        }
    }
}
