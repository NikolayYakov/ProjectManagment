using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using ProjectManagment.Attributes;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class StatusController : Controller
    {
        private IssueElementRepository issueElementRepository;
        private ProjectRepository projectRepository;
        public StatusController(IssueElementRepository issueElementRepository, ProjectRepository projectRepository)
        {
            this.issueElementRepository = issueElementRepository;
            this.projectRepository = projectRepository;

        }

        [HttpGet("project/{projectId}/status/all")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            
            var statuses = await this.issueElementRepository.GetAllProjectStatuses(projectId);

            var filterdStatuses = statuses.ToList().Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) || searchTerm == string.Empty);

            var totalItems = filterdStatuses.Count();
            var itemsPerPage = 10;
            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);

            var statusForPage = filterdStatuses.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var model = new ProjectStatusViewModel
            {
                PageNumber = page,
                TotalPages = totalPages,
                SearchTerm = searchTerm,
                Statuses = statusForPage.ToList(),
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/status/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/status/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Create(Guid projectId, CreateIssueElementReq req)
        {
            var lastStatusNumber = await this.issueElementRepository.GetLastProjectStatusNumber(projectId);
            await this.issueElementRepository.AddStatusToProject(req, projectId, lastStatusNumber);

            string url = Url.Content($"~/project/{projectId}/status/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/status/{statusId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid statusId)
        {
            var status = await this.issueElementRepository.GetStatusFromProject(statusId);
            return View(new ProjectStatus(projectId, status.Id, status.Name, status.Description, status.Number, status.Order));
        }

        [HttpPost("project/{projectId}/status/{statusId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid statusId, CreateIssueElementReq req)
        {
            var status = await this.issueElementRepository.GetStatusFromProject(statusId);
            status.Name = req.Name;
            status.Description = req.Description;
            status.Order = req.Order;

            await this.issueElementRepository.UpdateStatus(status);

            string url = Url.Content($"~/project/{projectId}/status/all");
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

            var status = await this.issueElementRepository.GetStatusFromProject(statusId);
            await this.issueElementRepository.DeleteStatusFromProject(status);

            string url = Url.Content($"~/project/{projectId}/status/all");
            return Redirect(url);
        }
    }
}
