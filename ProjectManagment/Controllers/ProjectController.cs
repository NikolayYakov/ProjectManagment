﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectManagment.Data;
using ProjectManagment.DTOs.Requests;
using ProjectManagment.DTOs.Responses;
using ProjectManagment.Repositories;
using System.Security.Claims;
using ProjectManagment.Models;
using System.Security;
using Microsoft.CodeAnalysis;
using Microsoft.Build.Evaluation;
using Microsoft.AspNetCore.Authorization;
using ProjectManagment.Attributes;
using ProjectManagment.ReleaseNotesWriters;
using System.Xml;
using System.Xml.Schema;
using System.Net;
using System.IO;

namespace ProjectManagment.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private ProjectRepository projectRepository;
        private IssueElementRepository issueElementRepository;
        private IssueRepository issueRepository;
        private DocumentWriter documentWriter;


        public ProjectController(ProjectRepository projectRepository, IssueElementRepository issueElementRepository, IssueRepository issueRepository, XmlDocumentWriter documentWriter)
        {
            this.projectRepository = projectRepository;
            this.issueElementRepository = issueElementRepository;
            this.issueRepository = issueRepository;
            this.documentWriter = documentWriter;
        }

        public async Task<IActionResult> All()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userProjects = await this.projectRepository.GetAllUserProjects(userId);

            return View(userProjects);
        }

        [HttpGet("Project/Details/{projectId}")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Details(Guid projectId)
        {
            var projectDetailsModel = new ProjectDetailsModel()
            {
                ProjectId = projectId,
            };

            return View(projectDetailsModel);
        }

        [HttpGet("Project/{projectId}/Board/{spritnId}")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Board(Guid projectId, Guid spritnId)
        {
            var model = new Board()
            {
                ProjectId = projectId,
                SelectedSprintId = spritnId
            };
            var sprints = await this.issueElementRepository.GetAllProjectSprints(projectId);

            model.Sprints = sprints.ToList();

            var columns = new List<Column>();
            
            var projectStatuses = await this.issueElementRepository.GetAllProjectStatuses(projectId);

            var issueForSprint = await this.issueRepository.GetIssuesInProjectForSprint(projectId, spritnId);

            foreach (var status in projectStatuses)
            {
                var column = new Column()
                {
                    Name = status.Name,
                };
                var cards = new List<Card>();

                foreach (var item in issueForSprint.Where(x=>x.StatusId == status.StatusId))
                {
                    var card = new Card(item.Number, item.Id, item.Title);
                    cards.Add(card);
                }

                column.Cards = cards;
                columns.Add(column);
            }

            model.Columns = columns;

            return View(model);
        }

        [HttpGet("Project/{projectId}/ReleaseNotes")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> ReleaseNotes(Guid projectId)
        {
            var milestones = await this.issueElementRepository.GetAllProjectMilestones(projectId);
            var releaseNotes = new GenerateReleaseNotesModel(projectId, milestones.ToList());

            return View(releaseNotes);
        }

        [HttpPost("Project/{projectId}/ReleaseNotes")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> ReleaseNotes(Guid projectId, GenerateReleaseNotesModel model)
        {
            var issues = await this.issueRepository.GetIssuesInProjectForMilestone(projectId, model.SelectedMilestoneId);
            var xmlMemoryStream = documentWriter.Write(issues);

            var file = new FileStreamResult(xmlMemoryStream, "application/xml")
            {
                FileDownloadName = "ReleaseNotes.xml"
            };
            return file;
        }

        [HttpGet("Project/{projectId}/Board")]
        [ServiceFilter(typeof(ProjectMemberAttribute))]
        public async Task<IActionResult> Board(Guid projectId)
        {
            var sprintId = await this.issueElementRepository.GetLastSprintFromProject(projectId);
            string url = Url.Content($"~/project/{projectId}/board/{sprintId}/");
            return Redirect(url);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateProjectReq project)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!await this.projectRepository.DoesUserContainProjectWithName(project.Title, userId))
            {
                await this.projectRepository.CreateProject(project, userId);
            }

            return RedirectToAction("all");
        }

        [HttpPut]
        public async Task<CommanRes> Delete([FromBody ]DeleteProjectReq projectReq)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var project = await this.projectRepository.GetProject(projectReq.Id);

            if (project != null && this.projectRepository.isUserProjectOwner(projectReq.Id, userId))
            {
                await this.projectRepository.DeleteProject(project);
            }
            
            return new CommanRes();
        }

        [HttpGet("project/{projectId}/edit")]
        [ServiceFilter(typeof(ProjectOwnerAttribute))]
        public async Task<IActionResult> Edit(Guid projectId)
        {
            var project = await projectRepository.GetProject(projectId);
            if (project == null)
            {
                return NotFound();
            }

            return View(new ProjectModel(project.Id, project.Name, project.Description));
        }

        [HttpPost("project/{projectId}/edit")]
        [ServiceFilter(typeof(ProjectOwnerAttribute))]
        public async Task<IActionResult> Edit(Guid projectId, UpdateProjectReq projectReq)
        {

            var project = await projectRepository.GetProject(projectId);
            project.Name = projectReq.Title;
            project.Description = projectReq.Description;

            await projectRepository.UpdateProject(project);

            return RedirectToAction("all");
        }
    }
}
