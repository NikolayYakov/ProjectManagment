using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Areas.Identity.Data;
using ProjectManagment.Repositories;
using ProjectManagment.DTOs.Requests;
using System.Text.Json;

namespace ProjectManagment.Attributes
{
    public class ProjectOwnerAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ProjectRepository projectRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectOwnerAttribute(ProjectRepository projectRepository, UserManager<ApplicationUser> userManager)
        {
            this.projectRepository = projectRepository;
            this.userManager = userManager;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userId = userManager.GetUserId(context.HttpContext.User);
            if (userId == null)
            {
                context.Result = new ForbidResult();
                return;
            }
            var request = context.HttpContext.Request;

            var routeData = context.RouteData.Values;
            if (routeData.TryGetValue("projectId", out var projectIdValue) && Guid.TryParse(projectIdValue.ToString(), out var projectId))
            {
                if (!projectRepository.isUserProjectOwner(projectId, userId))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
