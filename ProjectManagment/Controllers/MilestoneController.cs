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
    public class MilestoneController : Controller
    {
        private IssueElementRepository issueElementRepository;
        private ProjectRepository projectRepository;
        public MilestoneController(IssueElementRepository issueElementRepository, ProjectRepository projectRepository)
        {
            this.issueElementRepository = issueElementRepository;
            this.projectRepository = projectRepository;
        }

        [HttpGet("project/{projectId}/milestone/all")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> All(Guid projectId, int page = 1, string searchTerm = "")
        {
            var milestones = await this.issueElementRepository.GetAllProjectMilestones(projectId);

            var filterdMilestone = milestones.ToList().Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) || searchTerm == string.Empty);

            var totalItems = milestones.Count();
            var itemsPerPage = 10;
            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);

            var milestoneForPage = milestones.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var model = new ProjectMilestoneViewModel
            {
                PageNumber = page,
                TotalPages = totalPages,
                SearchTerm = searchTerm,
                Milestones = milestoneForPage.ToList(),
                ProjectId = projectId
            };

            return View(model);
        }

        [HttpGet("project/{projectId}/milestone/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public IActionResult Create(Guid projectId)
        {
            return View(new CreateProjectElementModel { ProjectId = projectId });
        }

        [HttpPost("project/{projectId}/milestone/create")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Create(Guid projectId, CreateIssueElementReq req)
        {
            var lastMilestoneNumber = await this.issueElementRepository.GetLastProjectMilestoneNumber(projectId);
            await this.issueElementRepository.AddMilestoneToProject(req, projectId, lastMilestoneNumber);

            string url = Url.Content($"~/project/{projectId}/milestone/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/milestone/{milestoneId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid milestoneId)
        {
            var milestone = await this.issueElementRepository.GetMilestoneFromProject(milestoneId);
            return View(new ProjectMilestone(projectId, milestone.Id, milestone.Name, milestone.Description, milestone.Number));
        }

        [HttpPost("project/{projectId}/milestone/{milestoneId}/edit")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, Guid milestoneId, CreateIssueElementReq req)
        {
            var milestone = await this.issueElementRepository.GetMilestoneFromProject(milestoneId);
            milestone.Name = req.Name;
            milestone.Description = req.Description;

            await this.issueElementRepository.UpdateMilestone(milestone);

            string url = Url.Content($"~/project/{projectId}/milestone/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid milestoneId, Guid projectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.projectRepository.isUserInProject(projectId, userId))
            {
                return new ForbidResult();
            }

            var milestone = await this.issueElementRepository.GetMilestoneFromProject(milestoneId);
            await this.issueElementRepository.DeleteeMilestoneFromProject(milestone);

            string url = Url.Content($"~/project/{projectId}/milestone/all");
            return Redirect(url);
        }
    }
}
