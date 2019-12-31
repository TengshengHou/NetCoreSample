using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applications.Commands
{
    public class ViewProjectCommand : IRequest
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public String Avatar { get; set; }
    }
}
