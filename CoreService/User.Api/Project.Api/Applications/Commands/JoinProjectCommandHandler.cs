using MediatR;
using Project.Domain.AggergatesModel;
using Project.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Api.Applications.Commands
{
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand>//传入CreateOrderCommand，返回bool
    {
        private IProjectRepository _projectRepository;
        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);
            if (project == null)
                throw new Domain.Exceptions.ProjectDomainException($"project ot  found:{request.Contributor.ProjectId}");

            if (project.UserId == request.Contributor.UserId)
                throw new ProjectDomainException("you cannot  join your own project");

            project.AddContributor(request.Contributor);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return await Unit.Task;
        }
    }
}
