﻿using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggergatesModel
{
    public class ProjectContributor:Entity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 关闭者
        /// </summary>
        public bool IsCloser { get; set; }
        /// <summary>
        ///1 财务顾问 2投资机构
        /// </summary>
        public int ContributorType { get; set; }
    }
}
