using DotNetCore.CAP;
using MediatR;
using Project.Api.Applications.IntegrationEvents;
using Project.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Api.Applications.DomainEventsHandlers
{
    public class ProjectCreateDomainEventsHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private ICapPublisher _capPublisher;
        public ProjectCreateDomainEventsHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreatedintegrationEvent()
            {
                UserId = notification.Project.UserId,
                CreateTime = DateTime.Now,
                ProjectId = notification.Project.Id,

            };
            _capPublisher.Publish("finbook.projectapi.projectcreated", @event);
            return Task.CompletedTask;
        }
    }
}
