using DotNetCore.CAP;
using Recommend.API.Data;
using Recommend.API.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.IntegrationEventHandels
{
    public class ProjectCreatedintegrationEventHandel : ICapSubscribe
    {
        private RecommendDbContext _dbContext;
        public ProjectCreatedintegrationEventHandel(RecommendDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task CreateRecommendFromProject(ProjectCreatedintegrationEvent @event)
        {
            //@event
            return Task.CompletedTask;
        }
    }
}
