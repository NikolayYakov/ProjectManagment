using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Models;

namespace ProjectManagment.Controllers
{
    public class StatusController : Controller
    {
        public StatusController()
        {

        }

        [HttpGet("project/{id}/status/all")]
        public IActionResult All(Guid id, int page = 1, string searchTerm = "")
        {
            // Simulate data for demonstration purposes
            var statuses = new List<ProjectStatus>
        {
            new ProjectStatus { Number = 1, Name = "Bug", Description = "Indicates a bug" },
            new ProjectStatus { Number = 2, Name = "Feature", Description = "Indicates a new feature" },
            new ProjectStatus { Number = 3, Name = "Enhancement", Description = "Indicates an enhancement" }
        };

            var model = new ProjectStatusViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Statuses = statuses
            };

            return View(model);
        }
    }
}
