using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Identity.Dto
{
    public class ServiceDisvoveryOptions
    {
        public string UserServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
