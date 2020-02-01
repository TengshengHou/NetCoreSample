using Microsoft.EntityFrameworkCore;
using Recommend.API.Mdoels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Data
{
    public class RecommendDbContext : DbContext
    {
        public RecommendDbContext(DbContextOptions<RecommendDbContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectRecommend>().ToTable("ProjectRecommends").HasKey(p => p.Id);
            //modelBuilder.Entity<ProjectReferenceUser>().ToTable("ProjectReferenceUser").HasKey(t => new { t.ProjectRecommendId, t.UserId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
