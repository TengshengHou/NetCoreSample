using Microsoft.AspNetCore.Mvc;
using Project.Api.Applications.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggergatesModel;
using Project.Api.Applications.Service;
using Project.Api.Applications.Queries;

namespace Project.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class projectsController : BaseController
    {
        private IMediator _mediator;
        private IRecommendService _recommendService;
        private IProjectQueries _projectQueries;
        public projectsController(IMediator mediator, IRecommendService recommendService, IProjectQueries projectQueries)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }
        [HttpPost]
        public async Task<IActionResult> CreatProject([FromBody] Domain.AggergatesModel.Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));
            var command = new CreateCommand() { Project = project };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectQueries.GetMyProjectsByUserId(UserIdentity.UserId);
            return Ok(projects);
        }
        [Route("my/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetMyProjectDetail(int projectId)
        {
            var projects = await _projectQueries.GetMyProjectDetail(projectId);
            if (projects.UserId == UserIdentity.UserId)
            {
                return Ok(projects);
            }
            else
            {
                return BadRequest("无权限查看该项目");
            }
        }

        [Route("my/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetRecommendProjectDetail(int projectId)
        {

            if (await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                var projects = await _projectQueries.GetMyProjectDetail(projectId);
                return Ok(projects);
            }
            else
            {
                return BadRequest("无权限查看该项目");
            }
        }

        [HttpPut]
        [Route("view/{projectId}")]

        public async Task<IActionResult> ViewProject(int projectId)
        {

            if (await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("沒查看該項目的權限");
            }

            var command = new ViewProjectCommand()
            {
                UserId = UserIdentity.UserId,
                UserName = UserIdentity.Name,
                Avatar = UserIdentity.Avatar,
                ProjectId = projectId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<IActionResult> JoinProject([FromBody] ProjectContributor projectContributor)
        {
            var projectId = projectContributor.ProjectId;
            if (await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("沒查看該項目的權限");
            }
            var command = new JoinProjectCommand()
            {
                Contributor = projectContributor
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

