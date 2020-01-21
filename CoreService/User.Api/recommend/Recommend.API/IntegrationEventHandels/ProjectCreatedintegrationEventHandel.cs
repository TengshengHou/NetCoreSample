using DotNetCore.CAP;
using Recommend.API.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.IntegrationEventHandels
{
    public class ProjectCreatedintegrationEventHandel : ICapSubscribe
    {
        public ProjectCreatedintegrationEventHandel()
        {

        }
        public Task CreateRecommendFromProject(ProjectCreatedintegrationEvent @event)
        {
            //@event
        }
    }
}
