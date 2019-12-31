using MediatR;
using Project.Domain.AggergatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Api.Applications.Commands
{
    public class CreateCommand : IRequest<Project.Domain.AggergatesModel.Project>
    {
        public Domain.AggergatesModel.Project Project { get; set; }
    }
}
