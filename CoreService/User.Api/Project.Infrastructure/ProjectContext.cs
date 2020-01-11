using MediatR;
using Microsoft.EntityFrameworkCore;
using Project.Domain.SeedWork;
using Project.Infrastructure.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class ProjectContext : DbContext, IUnitOfWork
    {
        private Mediator _mediator;
        public DbSet<Project.Domain.AggergatesModel.Project> projects { get; set; }
        public ProjectContext(DbContextOptions<ProjectContext> options, Mediator mediator) : base(options)
        {
            base.SaveChangesAsync();
            _mediator = mediator;
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {

            await base.SaveChangesAsync();
            await _mediator.DispatchDomainEventsAsync(this);
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectContrbutorEnirttyConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEnirttyConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectPropertyEnirttyConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectViewEnirttyConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVisibleRuleEnirttyConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
