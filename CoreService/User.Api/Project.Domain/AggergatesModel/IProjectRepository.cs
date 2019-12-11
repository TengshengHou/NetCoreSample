using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.AggergatesModel
{
    public interface IProjectRepository : IRepository<Project> 
    {
        Task<Project> GetAsync(int id);
        Task<Project> Add(Project project);
        Task<Project> Update(Project project);
    }
}
