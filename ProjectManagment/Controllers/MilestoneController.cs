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
    public class MilestoneController : Controller
    {
        private IssueElementRepository issueElementRepository;
        public MilestoneController(IssueElementRepository issueElementRepository)
        {
            this.issueElementRepository = issueElementRepository;
        }

        [HttpGet("project/{projectId}/milestone/all")]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var milestones = await this.issueElementRepository.GetAllProjectMilestones(projectId);
            var model = new ProjectMilestoneViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Milestones = milestones.ToList(),
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/milestone/create")]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/milestone/create")]
        public async Task<IActionResult> Create(Guid projectId, CreateIssueElementReq req)
        {
            var lastMilestoneNumber = await this.issueElementRepository.GetLastProjectMilestoneNumber(projectId);
            await this.issueElementRepository.AddMilestoneToProject(req, projectId, lastMilestoneNumber);

            string url = Url.Content($"~/project/{projectId}/milestone/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/milestone/{milestoneId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid milestoneId)
        {
            var milestone = await this.issueElementRepository.GetMilestoneFromProject(milestoneId);
            return View(new ProjectMilestone(projectId, milestone.Id, milestone.Name, milestone.Description, milestone.Number));
        }

        [HttpPost("project/{id}/milestone/{milestoneId}/edit")]
        public async Task<IActionResult> Edit(Guid id, Guid milestoneId, CreateIssueElementReq req)
        {
            var milestone = await this.issueElementRepository.GetMilestoneFromProject(milestoneId);
            milestone.Name = req.Name;
            milestone.Description = req.Description;

            await this.issueElementRepository.UpdateMilestone(milestone);

            string url = Url.Content($"~/project/{id}/milestone/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid milestoneId, Guid projectId)
        {
            var milestone = await this.issueElementRepository.GetMilestoneFromProject(milestoneId);
            await this.issueElementRepository.DeleteeMilestoneFromProject(milestone);

            string url = Url.Content($"~/project/{projectId}/milestone/all");
            return Redirect(url);
        }
    }
}
