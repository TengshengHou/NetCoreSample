using Project.Domain.AggergatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applications.IntegrationEvents
{
    public class ProjectJoinedintegrationEvent
    {
        public string Company { get; set; }
        public string Introduction { get; set; }
        public ProjectContributor Contributor { get; set; }
    }
}
