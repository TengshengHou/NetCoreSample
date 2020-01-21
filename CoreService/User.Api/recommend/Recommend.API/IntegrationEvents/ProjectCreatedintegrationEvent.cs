using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.IntegrationEvents
{
    public class ProjectCreatedintegrationEvent
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
