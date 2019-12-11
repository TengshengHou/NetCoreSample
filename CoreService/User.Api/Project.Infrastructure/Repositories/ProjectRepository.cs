using Project.Domain.AggergatesModel;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class ProjectRepository : IProjectRepository
    {
        public ProjectRepository()
        {
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task<Domain.AggergatesModel.Project> Add(Domain.AggergatesModel.Project project)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.AggergatesModel.Project> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.AggergatesModel.Project> Update(Domain.AggergatesModel.Project project)
        {
            throw new NotImplementedException();
        }
    }
}
