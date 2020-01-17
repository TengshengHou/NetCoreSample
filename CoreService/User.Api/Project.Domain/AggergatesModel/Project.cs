using Project.Domain.Events;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Domain.AggergatesModel
{
    public class Project : Entity, IAggregateRoot
    {

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }
        //項目Log
        public string Avatar { get; set; }
        //公司名
        public string Company { get; set; }
        /// <summary>
        /// 原BP文件地址
        /// </summary>
        public string OriginBPFile { get; set; }
        /// <summary>
        /// 转换后BP文件地址
        /// </summary>
        public string FormatBPFile { get; set; }
        /// <summary>
        /// 是否显示敏感信息
        /// </summary>
        public bool ShowSecurityInfo { get; set; }

        /// <summary>
        /// 公司所在省的ID
        /// </summary>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 公司所在省
        /// </summary>
        public string Province { get; set; }



        /// <summary>
        /// 公司所在省的ID
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区域Id
        /// </summary>
        public string AreaId { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 公司成立时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 项目基本信息
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 出让股份比例
        /// </summary>
        public string FinPercentage { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }

        /// <summary>
        /// 融资金额 单位：（万）
        /// </summary>
        public int FinMoney { get; set; }
        /// <summary>
        ///收入  单位：（万）
        /// </summary>
        public int Income { get; set; }
        /// <summary>
        ///利润   单位：（万）
        /// </summary>
        public int Revenue { get; set; }

        /// <summary>
        ///估值   单位：（万）
        /// </summary>
        public int Valuation { get; set; }
        /// <summary>
        /// 佣金分分配方式
        /// </summary>
        public int BrokerageOptions { get; set; }
        /// <summary>
        /// 是否委托给finbook
        /// </summary>
        public bool OnPlatForm { get; set; }

        public ProjectVisibleRule VisibleRule { get; set; }

        /// <summary>
        /// 根引用项目ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 上级引用项目ID
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// 项目标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 项目属性
        /// </summary>
        public List<ProjectProperty> Properties { get; set; }
        /// <summary>
        /// 贡献者列表
        /// </summary>

        public List<ProjectContributor> Contributors { get; set; }
        /// <summary>
        /// 查看者
        /// </summary>

        public List<ProjectViewer> Viewers { get; set; }
        public DateTime UpdateTime { get; set; }
        public DateTime CreatedTime { get; set; }

        private Project CloneProject(Project source = null)
        {
            if (source == null)
                source = this;
            var newProject = new Project()
            {
                AreaId = source.AreaId,
                BrokerageOptions = source.BrokerageOptions,
                Avatar = source.Avatar,
                City = source.City,
                CityId = source.CityId,
                Company = source.Company,
                Contributors = new List<ProjectContributor> { },
                Viewers = new List<ProjectViewer> { },
                CreatedTime = DateTime.Now,
                FinMoney = source.FinMoney,
                FinPercentage = source.FinPercentage,
                FinStage = source.FinStage,
                FormatBPFile = source.FormatBPFile,
                Income = source.Income,
                Introduction = source.Introduction,
                OnPlatForm = source.OnPlatForm,
                Province = source.Province,
                ProvinceId = source.ProvinceId,
                VisibleRule = source.VisibleRule == null ? null : new ProjectVisibleRule()
                {
                    Visible = source.VisibleRule.Visible,
                    Tags = source.VisibleRule.Tags
                },
                Tags = source.Tags,
                Valuation = source.Valuation,
                ShowSecurityInfo = source.ShowSecurityInfo,
                RegisterTime = source.RegisterTime,
                Revenue = source.Revenue
            };
            newProject.Properties = new List<ProjectProperty> { };
            foreach (var item in source.Properties)
            {
                newProject.Properties.Add(
                    new ProjectProperty
                    {
                        Key = item.Key,
                        Text = item.Text,
                        Value = item.Value
                    }
                    );
            }
            return newProject;
        }

        public Project ContributorFork(int contributorId, Project source = null)
        {
            if (source == null)
                source = this;
            var newProjcet = CloneProject(source);
            newProjcet.UserId = contributorId;
            newProjcet.SourceId = SourceId == 0 ? source.Id : source.SourceId;
            newProjcet.ReferenceId = source.ReferenceId == 0 ? source.Id : source.ReferenceId;
            newProjcet.UpdateTime = DateTime.Now;
            return newProjcet;
        }
        public void AddViewer(int userId, string userName, string avatar)
        {
            var viewer = new ProjectViewer
            {
                UserId = UserId,
                UserName = userName,
                Avatar = avatar,
                CreatedTime = DateTime.Now

            };
            if (!Viewers.Any(v => v.UserId == UserId))
                Viewers.Add(viewer);
            AddDomainEvent(new ProjectViewedEvent
            {
                Company = this.Company,
                Introduction = this.Introduction,
                Avatar = this.Avatar,
                Viewer = viewer
            });
        }

        public void AddContributor(ProjectContributor contributor)
        {
            if (!Contributors.Any(v => v.UserId == UserId))
            {
                Contributors.Add(contributor);
                AddDomainEvent(new ProjectJoinedEvent
                {
                    Company = this.Company,
                    Introduction = this.Introduction,
                    Avatar = this.Avatar,
                    Contributor = contributor
                });
            }
        }

        public Project()
        {
            Viewers = new List<ProjectViewer>();
            Contributors = new List<ProjectContributor>();
            AddDomainEvent(new ProjectCreatedEvent { Project = this });
        }
    }
}
