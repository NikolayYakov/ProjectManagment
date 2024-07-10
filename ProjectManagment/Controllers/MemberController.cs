using Microsoft.AspNetCore.Mvc;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.Models;
using ProjectManagment.Repositories;

namespace ProjectManagment.Controllers
{
    public class MemberController : Controller
    {
        private ProjectRepository projectRepository;

        public MemberController(ProjectRepository projectRepository)
        {
            this.projectRepository = projectRepository;
        }

        [HttpGet("Project/{projectId}/member/all")]
        public async Task<IActionResult> All(Guid projectId, string searchTerm, int page = 1)
        {
            const int PageSize = 10;
            var members = await this.projectRepository.GetAllProjectMembers(projectId);

            var model = new ProjectMembersViewModel
            {
                ProjectId = projectId,
                SearchTerm = searchTerm,
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(members.Count() / (double)PageSize),
                Members = members.Skip((page - 1) * PageSize).Take(PageSize).ToList()
            };

            return View(model);
        }

        [HttpGet("Project/{projectId}/member/invite")]
        public async Task<IActionResult> Invite(Guid projectId)
        {
            return View(new InviteUserModel { ProjectId = projectId });
        }

        [HttpPost("Project/{projectId}/member/invite")]
        public async Task<IActionResult> Invite(Guid projectId, InviteDTO inviteReq)
        {
            var inviteId = await this.projectRepository.InviteUser(inviteReq, projectId);
            string errorMessage = null;
            if (inviteId == Guid.Empty)
            {
                errorMessage = "User does not exist";
            }

            return View(new InviteUserModel { ProjectId = projectId, InviteId = inviteId, isInvited = true, ErrorMessage = errorMessage });
            //string url = Url.Content($"~/project/{projectId}/label/all");
            //return Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Kick(string userId, Guid projectId)
        {
            //var label = await this.issueElementRepository.GetLabelFromProject(labelId);
            //await this.issueElementRepository.DeleteLabelFromProject(label);

            string url = Url.Content($"~/project/{projectId}/label/all");
            return Redirect(url);
        }
        //[HttpPost]
        //public IActionResult Kick(int memberId, int projectId)
        //{
        //    var project = _projectService.GetProjectById(projectId);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    var member = _memberService.GetMemberById(memberId);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    if (member.Role == "Owner")
        //    {
        //        ModelState.AddModelError("", "Cannot kick the owner.");
        //        return RedirectToAction("All", new { projectId });
        //    }

        //    _memberService.RemoveMemberFromProject(memberId, projectId);

        //    return RedirectToAction("All", new { projectId });
        //}
    }
}
