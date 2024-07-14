using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ProjectManagment.Attributes;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.DTOs.Responses;
using ProjectManagment.Models;
using ProjectManagment.Repositories;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class LabelController : Controller
    {
        private IssueElementRepository issueElementRepository;
        private ProjectRepository projectRepository;

        public LabelController(IssueElementRepository issueElementRepository, ProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
            this.issueElementRepository = issueElementRepository;
        }

        [HttpGet("project/{projectId}/label/all")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var labels = await this.issueElementRepository.GetAllProjectLabels(projectId);

            var model = new ProjectLabelViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Labels = labels.ToList(),
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/label/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/label/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Create(Guid projectId, CreateIssueElementReq req)
        {
            var lastLabelNumber = await issueElementRepository.GetLastProjectLabelNumber(projectId);
            await this.issueElementRepository.AddLabelToProject(req, projectId, lastLabelNumber);

            string url = Url.Content($"~/project/{projectId}/label/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/label/{labelId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid labelId)
        {
            var label = await this.issueElementRepository.GetLabelFromProject(labelId);

            return View(new ProjectLabel(projectId, label.Id, label.Name, label.Description, label.Number));
        }

        [HttpPost("project/{projectId}/label/{labelId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid labelId, CreateIssueElementReq req)
        {
            var label = await this.issueElementRepository.GetLabelFromProject(labelId);
            label.Name = req.Name;
            label.Description = req.Description;

            await this.issueElementRepository.UpdateLabel(label);

            string url = Url.Content($"~/project/{projectId}/label/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid labelId, Guid projectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.projectRepository.isUserInProject(projectId, userId))
            {
                return new ForbidResult();
            }

            var label = await this.issueElementRepository.GetLabelFromProject(labelId);
            await this.issueElementRepository.DeleteLabelFromProject(label);

            string url = Url.Content($"~/project/{projectId}/label/all");
            return Redirect(url);
        }
    }
}
