using Dapper;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applications.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly string _connStr;
        public ProjectQueries(string connStr)
        {
            _connStr = connStr;
        }
        public async Task<dynamic> GetMyProjectDetail(int projectId)
        {
            var sql = @"SELECT  * FROM Projects 
inner join ProjectVisibleRules 
on Projects.Id=ProjectvisibleRules.ProjectId where
projects.id=@projectId";
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var result = await conn.QueryAsync<dynamic>("", new { projectId });
                return result;
            }
        }

        public async Task<dynamic> GetMyProjectsByUserId(int userId)
        {
            var sql = @"SELECT * FROM PROJECTS WHERE PROJECTS.USERID=@userId";
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var result = await conn.QueryAsync<dynamic>(sql, new { userId });
                return result;
            }

            throw new NotImplementedException();
        }
    }
}
