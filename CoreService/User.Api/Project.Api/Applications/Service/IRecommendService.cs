using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applications.Service
{
    public interface IRecommendService
    {
        Task<bool> IsProjectInRecommend(int projectId,int userId);
    }
}
