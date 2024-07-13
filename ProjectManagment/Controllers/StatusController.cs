using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class StatusController : Controller
    {
        private IssueElementRepository issueElementRepository;
        public StatusController(IssueElementRepository issueElementRepository)
        {
            this.issueElementRepository = issueElementRepository;
        }

        [HttpGet("project/{projectId}/status/all")]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var statuses = await this.issueElementRepository.GetAllProjectStatuses(projectId);

            var model = new ProjectStatusViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Statuses = statuses.ToList(),
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/status/create")]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/status/create")]
        public async Task<IActionResult> Create(Guid projectId, CreateIssueElementReq req)
        {
            var lastStatusNumber = await this.issueElementRepository.GetLastProjectStatusNumber(projectId);
            await this.issueElementRepository.AddStatusToProject(req, projectId, lastStatusNumber);

            string url = Url.Content($"~/project/{projectId}/status/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/status/{statusId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid statusId)
        {
            var status = await this.issueElementRepository.GetStatusFromProject(statusId);
            return View(new ProjectStatus(projectId, status.Id, status.Name, status.Description, status.Number));
        }

        [HttpPost("project/{projectId}/status/{statusId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid statusId, CreateIssueElementReq req)
        {
            var status = await this.issueElementRepository.GetStatusFromProject(statusId);
            status.Name = req.Name;
            status.Description = req.Description;

            await this.issueElementRepository.UpdateStatus(status);

            string url = Url.Content($"~/project/{projectId}/status/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid statusId, Guid projectId)
        {
            var status = await this.issueElementRepository.GetStatusFromProject(statusId);
            await this.issueElementRepository.DeleteStatusFromProject(status);

            string url = Url.Content($"~/project/{projectId}/status/all");
            return Redirect(url);
        }
    }
}
