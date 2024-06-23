using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    public class AreaController : Controller
    {
        private IssueElementRepository issueElementRepository;
        public AreaController(IssueElementRepository issueElementRepository)
        {
            this.issueElementRepository = issueElementRepository;
        }

        [HttpGet("project/{projectId}/area/all")]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var areas = await this.issueElementRepository.GetAllProjectAreas(projectId);
            var model = new ProjectAreaViewModel
            {
                PageNumber = page,
                TotalPages = 1,
                SearchTerm = searchTerm,
                ProjectId = projectId,
                Areas = areas.ToList()
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/area/create")]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/area/create")]
        public async Task<IActionResult> Create(Guid projectId, CreateIssueElementReq req)
        {
            var lastAreaNumber = await this.issueElementRepository.GetLastProjectAreaNumber(projectId);
            await this.issueElementRepository.AddAreaToProject(req, projectId, lastAreaNumber);

            string url = Url.Content($"~/project/{projectId}/area/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/area/{areaId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid areaId)
        {
            var area = await this.issueElementRepository.GetAreaFromProject(areaId);

            return View(new ProjectArea(projectId, area.Id, area.Name, area.Description, area.Number));
        }

        [HttpPost("project/{id}/area/{areaId}/edit")]
        public async Task<IActionResult> Edit(Guid id, Guid areaId, CreateIssueElementReq req)
        {
            var arae = await this.issueElementRepository.GetAreaFromProject(areaId);
            arae.Name = req.Name;
            arae.Description = req.Description;

            await this.issueElementRepository.UpdateArea(arae);

            string url = Url.Content($"~/project/{id}/area/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid areaId, Guid projectId)
        {
            var area = await this.issueElementRepository.GetAreaFromProject(areaId);
            await this.issueElementRepository.DeleteAreaFromProject(area);

            string url = Url.Content($"~/project/{projectId}/area/all");
            return Redirect(url);
        }
    }
}
