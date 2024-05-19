using Microsoft.AspNetCore.Mvc;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.DTOs.Responses;
using ProjectManagment.Repositories;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private ProjectRepository projectRepository;
        public ProjectController(ProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        [HttpPost]
        public async Task<CommanRes> CreateProject(CreateProjectReq projectReq)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if(!await this.projectRepository.DoesUserContainProjectWithName(projectReq.Name, userId))
            {
                await this.projectRepository.CreateProject(projectReq, userId);
            }
            else
            {
                return new CommanRes(Singleton.ErrorCodes.ProjectAlreadyExist);
            }

            return new CommanRes();
        }


        [HttpPut]
        public async Task<CommanRes> UpdateProjectName(UpdateProjectReq projectReq)
        {
            var project = await this.projectRepository.GetProject(projectReq.Id);
            project.Name = projectReq.Name;
            project.Description = projectReq.Description;

            await this.projectRepository.UpdateProject(project);

            return new CommanRes();
        }

        [HttpDelete]
        public async Task<CommanRes> DeleteProject(UpdateProjectReq projectReq)
        {
            var project = await this.projectRepository.GetProject(projectReq.Id);
            await this.projectRepository.DeleteProject(project);

            return new CommanRes();
        }
    }
}
