using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applications.Queries
{
    public interface IProjectQueries
    {
        Task<dynamic> GetMyProjectsByUserId(int userId);
        Task<dynamic> GetMyProjectDetail( int projectId);
    }
}
