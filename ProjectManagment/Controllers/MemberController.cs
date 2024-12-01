using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ProjectManagment.Attributes;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.EmailService;
using ProjectManagment.Models;
using ProjectManagment.Repositories;
using ProjectManagment.Singleton;
using System.Security.Claims;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private ProjectRepository projectRepository;
        private IEmailSender EmailSender;

        public MemberController(ProjectRepository projectRepository, IEmailSender emailSender)
        {
            this.projectRepository = projectRepository;
            this.EmailSender = emailSender;
        }

        [HttpGet("Project/{projectId}/member/all")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> All(Guid projectId, string searchTerm, int page = 1)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var members = await this.projectRepository.GetAllProjectMembers(projectId);

            var filterdMembers = members.ToList().Where(x => string.IsNullOrEmpty(searchTerm) || x.Name.ToLower().Contains(searchTerm?.ToLower()));
            var totalItems = filterdMembers.Count();
            var itemsPerPage = 10;
            var totalPages = (int)Math.Ceiling(totalItems / (double)itemsPerPage);

            var membersForPage = filterdMembers.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            var model = new ProjectMembersViewModel
            {
                ProjectId = projectId,
                SearchTerm = searchTerm,
                PageNumber = page,
                isOwner = this.projectRepository.isUserProjectOwner(projectId, currentUserId),
                TotalPages = totalPages,
                Members = membersForPage
            };

            return View(model);
        }

        [HttpGet("Project/{projectId}/member/invite")]
        [ServiceFilter(typeof(ProjectOwnerAttribute))]
        public async Task<IActionResult> Invite(Guid projectId)
        {
            return View(new InviteUserModel { ProjectId = projectId });
        }

        [HttpPost("Project/{projectId}/member/invite")]
        [ServiceFilter(typeof(ProjectOwnerAttribute))]
        public async Task<IActionResult> Invite(Guid projectId, InviteDTO inviteReq)
        {
            var inviteId = await this.projectRepository.InviteUser(inviteReq, projectId);
            string errorMessage = null;
            if (inviteId == Guid.Empty)
            {
                errorMessage = "User does not exist";
            }
            else
            {
                var body = $"You have been invited to join a project. Invite code: {inviteId}";
                EmailSender.SendEmailAsync(inviteReq.Email, "Project Invite", body);
            }


            return View(new InviteUserModel { ProjectId = projectId, InviteId = inviteId, isInvited = true, ErrorMessage = errorMessage });
        }

        [HttpGet("Project/join")]
        public async Task<IActionResult> Join()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(new JoinProjectModel { UserId = userId });
        }

        [HttpPost("Project/join")]
        public async Task<IActionResult> Join(JoinDTO joinDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!Guid.TryParse(joinDTO.InviteId, out Guid inviteGuid))
            {
                return View(new JoinProjectModel { UserId = userId, ErrorMessage = "Invalid invite" });
            }

            var invite = await this.projectRepository.GetInvite(inviteGuid, userId);

            if (invite == null)
            {
                return View(new JoinProjectModel { UserId = userId, ErrorMessage = "Invalid invite" });
            }

            if(this.projectRepository.isUserInProject(invite.ProjectId, userId))
            {
                return View(new JoinProjectModel { UserId = userId, ErrorMessage = "User is already memeber of the project" });
            }

            await this.projectRepository.JoinProject(invite, userId);

            return View(new JoinProjectModel { UserId = userId, ErrorMessage = "Joined project successfully" });
        }

        public async Task<IActionResult> Kick(string userId, Guid projectId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.projectRepository.isUserProjectOwner(projectId, currentUserId))
            {
                return new ForbidResult();
            }

            await this.projectRepository.KickUser(userId, projectId);
            string url = Url.Content($"~/project/{projectId}/member/all");
            return Redirect(url);
        }

        public async Task<IActionResult> Leave(Guid projectId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!this.projectRepository.isUserInProject(projectId, currentUserId))
            {
                return new ForbidResult();
            }

            await this.projectRepository.KickUser(currentUserId, projectId);
            string url = Url.Content($"~/project/all");
            return Redirect(url);
        }
    }
}
