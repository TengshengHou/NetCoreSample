using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Events
{
    public class ProjectCreatedEvent : INotification
    {
        public AggergatesModel.Project Project { get; set; }
    }
}
