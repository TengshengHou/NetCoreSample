using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applications.Commands
{
    public class JoinProjectCommand : IRequest
    {
        public Domain.AggergatesModel.ProjectContributor Contributor { get; set; }

    }
}
