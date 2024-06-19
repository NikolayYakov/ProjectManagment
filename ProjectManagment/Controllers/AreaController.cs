using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Models;

namespace ProjectManagment.Controllers
{
    public class AreaController : Controller
    {
        public AreaController()
        {
            
        }

        [HttpGet("project/{id}/area/all")]
        public IActionResult All(Guid id,int page = 1, string searchTerm = "")
        {

            var areas = new List<ProjectArea>
            {
                new ProjectArea { Number = 1, Name = "Server", Description = "This area is used for issues ralated to the backend" },
                new ProjectArea { Number = 2, Name = "Frontend", Description = "This area is used for issues ralated to the frontend" },
                new ProjectArea { Number = 3, Name = "Infrastructure", Description = "This area is used for issues ralated to the Infrastructure" }
            };

            var model = new ProjectAreaViewModel
            {
                PageNumber = page,
                TotalPages = 1,
                SearchTerm = searchTerm,
                ProjectId = id,
                Areas = areas
            };

            return View(model);
        }
    }
}
