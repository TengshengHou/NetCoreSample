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
    public class ProjectViewedDomainEventsHandler : INotificationHandler<ProjectViewedEvent>
    {
        private ICapPublisher _capPublisher;
        public ProjectViewedDomainEventsHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public Task Handle(ProjectViewedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectViewedintegrationEvent()
            {
                Company = notification.Company,
                Introduction = notification.Introduction,
                Viewer= notification.Viewer
            };
            _capPublisher.Publish("finbook.projectapi.priojectviewed", @event);
            return Task.CompletedTask;
        }
    }
}
