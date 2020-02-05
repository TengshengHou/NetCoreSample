using Project.Domain.AggergatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applications.IntegrationEvents
{
    public class ProjectCreatedintegrationEvent
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        
        //public int FromUserId { get; set; }
        /// <summary>
        /// 项目logo
        /// </summary>
        public string PrjectAvatart { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 项目介绍
        /// </summary>
        public string Introduction { get; set; }
        /// <summary>
        /// 项目标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }

      


        public DateTime CreateTime { get; set; }
    }
}
