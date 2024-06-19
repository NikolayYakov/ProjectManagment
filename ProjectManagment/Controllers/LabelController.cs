using Microsoft.AspNetCore.Mvc;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.DTOs.Responses;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    public class LabelController : Controller
    {
        public LabelController()
        {
            
        }

        [HttpGet("project/{id}/label/all")]
        public IActionResult All(Guid id, int page = 1, string searchTerm = "")
        {
            // Simulate data for demonstration purposes
            var labels = new List<ProjectLabel>
            {
                new ProjectLabel { Number = 1, Name = "Bug", Description = "Indicates a bug", LabelId = new Guid() },
                new ProjectLabel { Number = 2, Name = "Feature", Description = "Indicates a new feature", LabelId = new Guid() },
                new ProjectLabel { Number = 3, Name = "Enhancement", Description = "Indicates an enhancement", LabelId = new Guid() }
            };

            var model = new ProjectLabelViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Labels = labels,
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
        public IActionResult Create(Guid id, CreateIssueElementReq req)
        {
            string url = Url.Content($"~/project/{id}/label/all");
            return Redirect(url);
        }

        [HttpGet("project/{id}/label/{labelId}/edit")]
        public IActionResult Edit(Guid id, Guid labelId)
        {
            //var label = labels.FirstOrDefault(l => l.Number == id);
            //if (label == null)
            //{
            //    return NotFound();
            //}

            return View(new ProjectLabel { LabelId = new Guid(), Number = 0, Name="Test", Description = "Test"});
        }

        [HttpPost("project/{id}/label/{labelId}/edit")]
        public IActionResult EditLabel(Guid id, Guid labelId, CreateIssueElementReq req)
        {
            string url = Url.Content($"~/project/{id}/label/all");
            return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var project = await this.projectRepository.GetProject(projectReq.Id);

            //if (project != null)
            //{
            //    await this.projectRepository.DeleteProject(project);
            //}


            string url = Url.Content($"~/project/{id}/label/all");
            return Redirect(url);
        }
    }
}
