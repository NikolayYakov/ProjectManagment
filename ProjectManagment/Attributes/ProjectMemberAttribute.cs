using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ProjectManagment.Areas.Identity.Data;
using ProjectManagment.Repositories;

namespace ProjectManagment.Attributes
{
    public class ProjectMemberAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ProjectRepository projectRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public ProjectMemberAttribute(ProjectRepository projectRepository, UserManager<ApplicationUser> userManager)
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

            var routeData = context.RouteData.Values;
            if (routeData.TryGetValue("projectId", out var projectIdValue) && Guid.TryParse(projectIdValue.ToString(), out var projectId))
            {
                if (!projectRepository.isUserInProject(projectId, userId))
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
