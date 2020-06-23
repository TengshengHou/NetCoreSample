using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Data
{
    public class ServiceDisvoveryOptions

    {
        /// <summary>
        /// 当前站点IP
        /// </summary>
        public string ServiceIP { get; set; }
        /// <summary>
        /// 当前站点端口
        /// </summary>
        public int ServicePort { get; set; }
        /// <summary>
        /// 当前站点在 服务发现中名字
        /// </summary>

        public string ServiceName { get; set; }

        public string UserServiceName { get; set; }
        public string ContactServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
