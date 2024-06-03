using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.DTOs.Responses;
using ProjectManagment.Repositories;
using System.Security.Claims;
using ProjectManagment.Models;

namespace ProjectManagment.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private ProjectRepository projectRepository;
        public ProjectController(ProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        //[HttpPost]
        //public async Task<CommanRes> CreateProject(CreateProjectReq projectReq)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

        //    if(!await this.projectRepository.DoesUserContainProjectWithName(projectReq.Name, userId))
        //    {
        //        await this.projectRepository.CreateProject(projectReq, userId);
        //    }
        //    else
        //    {
        //        return new CommanRes(Singleton.ErrorCodes.ProjectAlreadyExist);
        //    }

        //    return new CommanRes();
        //}

        public async Task<IActionResult> AllProjects()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userProjects = await this.projectRepository.GetAllUserProjects(userId);

            //var projects = new List<ProjectModel>
            //{
            //    new ProjectModel { ProjectId = new Guid(), Title = "Project A", Description = "Description for Project A" },
            //    new ProjectModel { ProjectId = new Guid(), Title = "Project B", Description = "Description for Project B" },
            //    // Add more projects here
            //};

            return View(userProjects);
        }


		public IActionResult Board()
		{
            var model = new Board
            {
                Columns = new List<Column>
                {
                    new Column
                    {
                        Name = "To Do",
                        Cards = new List<Card>
                        {
                            new Card { Id = 1, Title = "Task 1", Description = "Description of Task 1" },
                            new Card { Id = 2, Title = "Task 2", Description = "Description of Task 2" }
                        }
                    },
                    new Column
                    {
                        Name = "In Progress",
                        Cards = new List<Card>
                        {
                            new Card { Id = 3, Title = "Task 3", Description = "Description of Task 3" }
                        }
                    },
                    new Column
                    {
                        Name = "Done",
                        Cards = new List<Card>
                        {
                            new Card { Id = 4, Title = "Task 4", Description = "Description of Task 4" },
                            new Card { Id = 5, Title = "Task 5", Description = "Description of Task 5" }
                        }
                    }
                }
            };

            return View(model);
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateProjectReq project)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!await this.projectRepository.DoesUserContainProjectWithName(project.Title, userId))
                {
                    await this.projectRepository.CreateProject(project, userId);
                }
              
                return RedirectToAction("AllProjects"); // Redirect to the index or another appropriate action
            }

            return View(project);
        }

        public ActionResult MoveCard(int cardId, int newColumnId)
        {
            try
            {
                // Here, you would typically update your database or data source
                // to reflect the new position and column of the card based on the provided parameters.

                // For example, if you're using Entity Framework:
                /*
                using (var dbContext = new YourDbContext())
                {
                    var card = dbContext.Cards.Find(cardId);
                    if (card != null)
                    {
                        card.ColumnId = newColumnId; // Update column ID
                        dbContext.SaveChanges(); // Save changes to database
                    }
                }
                */

                // Simulating success response for demonstration purposes
                return Json(new { success = true, message = "Card moved successfully." });
            }
            catch (Exception ex)
            {
                // Log or handle the error as needed
                return Json(new { success = false, message = "An error occurred while moving the card." });
            }
        }

        //[HttpGet]
        //public async Task<List<Project>> GetAllUserProjects(CreateProjectReq projectReq)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    return await this.projectRepository.GetAllUserProjects(userId);
        //}

        [HttpGet]
        //public async Task<List<Project>> GetProject(CreateProjectReq projectReq)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    return await this.projectRepository.GetAllUserProjects(userId);
        //}


        [HttpPut]
        public async Task<CommanRes> UpdateProjectName(UpdateProjectReq projectReq)
        {
            var project = await this.projectRepository.GetProject(projectReq.Id);
            project.Name = projectReq.Title;
            project.Description = projectReq.Description;

            await this.projectRepository.UpdateProject(project);

            return new CommanRes();
        }

        [HttpPut]
        public async Task<CommanRes> Delete([FromBody ]DeleteProjectReq projectReq)
        {
            var project = await this.projectRepository.GetProject(projectReq.Id);

            if (project != null)
            {
                await this.projectRepository.DeleteProject(project);
            }


            return new CommanRes();
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await projectRepository.GetProject(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(new ProjectModel(project.Id, project.Name, project.Description));
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, UpdateProjectReq projectReq)
        {
            if (id != projectReq.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var project = await projectRepository.GetProject(id);
                project.Name = projectReq.Title;
                project.Description = projectReq.Description;

                await projectRepository.UpdateProject(project);
                return RedirectToAction("AllProjects");
            }

            return View(new ProjectModel(projectReq.Id, projectReq.Title, projectReq.Description));
        }
    }
}
