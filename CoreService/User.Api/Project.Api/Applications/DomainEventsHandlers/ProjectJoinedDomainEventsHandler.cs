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
    public class ProjectJoinedDomainEventsHandler : INotificationHandler<ProjectJoinedEvent>
    {
        private ICapPublisher _capPublisher;
        public ProjectJoinedDomainEventsHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinedintegrationEvent()
            {
                Company = notification.Company,
                Introduction = notification.Introduction,
                Contributor = notification.Contributor

            };
            _capPublisher.Publish("finbook.projectapi.projectjoined", @event);
            return Task.CompletedTask;
        }
    }
}
