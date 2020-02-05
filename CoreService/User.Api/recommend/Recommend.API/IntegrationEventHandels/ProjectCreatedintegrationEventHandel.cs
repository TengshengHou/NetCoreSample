using DotNetCore.CAP;
using Recommend.API.Data;
using Recommend.API.IntegrationEvents;
using Recommend.API.Mdoels;
using Recommend.API.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.IntegrationEventHandels
{
    public class ProjectCreatedintegrationEventHandel : ICapSubscribe
    {
        private RecommendDbContext _dbContext;
        private IUserService _userService;
        private IContactService _contactService;
        public ProjectCreatedintegrationEventHandel(RecommendDbContext dbContext, IUserService userService, IContactService contactService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _contactService = contactService;
        }

        /// <summary>
        /// 一度项目推荐
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task CreateRecommendFromProject(ProjectCreatedintegrationEvent @event)
        {
            var fromUser = await _userService.GetBaseUserInfoAsync(@event.UserId);
            var contacts = await _contactService.GetContactsByUserId(@event.UserId);
            foreach (var contact in contacts)
            {
                var recommend = new ProjectRecommend()
                {
                    FromUserId = @event.UserId,
                    Company = @event.Company,
                    Tags = @event.Tags,
                    ProjectId = @event.ProjectId,
                    PrjectAvatar = @event.PrjectAvatar,
                    FinStage = @event.FinStage,
                    RecommendTime = @event.CreateTime,
                    CreatedTime = @event.CreateTime,
                    Introduction = @event.Introduction,
                    RecommendType = EnumRecommendType.Friend,
                    FromUserAvatar = fromUser.Avatar,
                    FromUserName = fromUser.Name,

                    UserId = contact.UserId
                };
                _dbContext.Recommends.Add(recommend);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
