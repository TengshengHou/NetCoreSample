using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Domain;
using Project.Domain.AggergatesModel;
using System.Threading;

namespace Project.Api.Applications.Commands
{
    
    public class CreateCommandHandler : IRequestHandler<CreateCommand, Domain.AggergatesModel.Project>//传入CreateOrderCommand，返回Project
    {
        private IProjectRepository _projectRepository;
        public CreateCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Domain.AggergatesModel.Project> Handle(CreateCommand request, CancellationToken cancellationToken)
        {
             _projectRepository.Add(request.Project);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return request.Project;
        }
    }
}
