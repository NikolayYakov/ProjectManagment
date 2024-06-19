using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Models;

namespace ProjectManagment.Controllers
{
    public class MilestoneController : Controller
    {
        public MilestoneController()
        {
                
        }

        [HttpGet("project/{id}/milestone/all")]
        public IActionResult All(Guid id, int page = 1, string searchTerm = "")
        {
            // Simulate data for demonstration purposes
            var milestones = new List<ProjectMilestone>
            {
                new ProjectMilestone { Number = 1, Name = "Bug", Description = "Indicates a bug" },
                new ProjectMilestone { Number = 2, Name = "Feature", Description = "Indicates a new feature" },
                new ProjectMilestone { Number = 3, Name = "Enhancement", Description = "Indicates an enhancement" }
            };

            var model = new ProjectMilestoneViewModel
            {
                PageNumber = page,
                TotalPages = 1, // Assuming there's only one page for demonstration
                SearchTerm = searchTerm,
                Milestones = milestones
            };

            return View(model);
        }
    }
}
