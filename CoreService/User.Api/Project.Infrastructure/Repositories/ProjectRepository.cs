using Microsoft.EntityFrameworkCore;
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
        private readonly ProjectContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public ProjectRepository(ProjectContext context)
        {
            _context = context;
        }



        public Domain.AggergatesModel.Project Add(Domain.AggergatesModel.Project project)
        {
            //判断是否ID为0
            if (project.IsTransient())
            {
                return _context.Add(project).Entity;
            }
            return project;
        }

        public async Task<Domain.AggergatesModel.Project> GetAsync(int id)
        {
            var proejct = await _context.projects
                .Include(p => p.Properties)
                .Include(p => p.Viewers)
                .Include(p => p.Contributors)
                .Include(p => p.VisibleRule).SingleOrDefaultAsync(a => a.Id == id);
            return proejct;
        }

        public void Update(Domain.AggergatesModel.Project project)
        {
            _context.Update(project);
        }

       
    }
}
