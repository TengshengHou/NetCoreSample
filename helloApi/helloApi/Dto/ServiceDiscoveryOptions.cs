using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Data
{
    public class ServiceDisvoveryOptions
                 
    {
        public string ServiceName { get; set; }
        public string ContactServiceName { get; set; }
        public string ServiceIP { get; set; }
        public int ServicePort { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
