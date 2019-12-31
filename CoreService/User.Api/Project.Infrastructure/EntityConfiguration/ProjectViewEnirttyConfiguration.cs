using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectViewEnirttyConfiguration : IEntityTypeConfiguration<Domain.AggergatesModel.ProjectViewer>
    {
        public void Configure(EntityTypeBuilder<Domain.AggergatesModel.ProjectViewer> builder)
        {
            builder.ToTable("ProjectViewers").HasKey(p => p.Id);
        }
    }
}
