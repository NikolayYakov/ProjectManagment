using Microsoft.AspNetCore.Mvc;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.DTOs.Responses;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    public class LabelController : Controller
    {
        private IssueElementRepository issueElementRepository;
        public LabelController(IssueElementRepository issueElementRepository)
        {

            this.issueElementRepository = issueElementRepository;

        }

        [HttpGet("project/{id}/label/all")]
        public async Task<IActionResult> All(Guid id, int page = 1, string searchTerm = "")
        {
            var labels = await this.issueElementRepository.GetAllProjectLabels(id);

            var model = new ProjectLabelViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Labels = labels.ToList(),
                ProjectId = id
            };

            return View(model);
        }

        [HttpGet("project/{id}/label/create")]
        public IActionResult Create(Guid id)
        {
            return View(new CreateProjectElementModel { ProjectId = id});
        }

        [HttpPost("project/{id}/label/create")]
        public async Task<IActionResult> Create(Guid id, CreateIssueElementReq req)
        {
            var lastLabelNumber = await issueElementRepository.GetLastProjectLabelNumber(id);
            await this.issueElementRepository.AddLabelToProject(req, id, lastLabelNumber);

            string url = Url.Content($"~/project/{id}/label/all");
            return Redirect(url);
        }

        [HttpGet("project/{projectId}/label/{labelId}/edit")]
        public async Task<IActionResult> Edit(Guid projectId, Guid labelId)
        {
            var label = await this.issueElementRepository.GetLabelFromProject(labelId);

            return View(new ProjectLabel(projectId, label.Id, label.Name, label.Description, label.Number));
        }

        [HttpPost("project/{id}/label/{labelId}/edit")]
        public async Task<IActionResult> Edit(Guid id, Guid labelId, CreateIssueElementReq req)
        {
            var label = await this.issueElementRepository.GetLabelFromProject(labelId);
            label.Name = req.Name;
            label.Description = req.Description;

            await this.issueElementRepository.UpdateLabel(label);

            string url = Url.Content($"~/project/{id}/label/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid labelId, Guid projectId)
        {
            var label = await this.issueElementRepository.GetLabelFromProject(labelId);
            await this.issueElementRepository.DeleteLabelFromProject(label);

            string url = Url.Content($"~/project/{projectId}/label/all");
            return Redirect(url);
        }
    }
}
