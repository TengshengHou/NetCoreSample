using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Data
{
    public class ServiceDisvoveryOptions
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
