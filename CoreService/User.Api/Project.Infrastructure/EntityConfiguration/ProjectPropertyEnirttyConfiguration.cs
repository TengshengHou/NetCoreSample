using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{

    public class ProjectPropertyEnirttyConfiguration : IEntityTypeConfiguration<Domain.AggergatesModel.ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<Domain.AggergatesModel.ProjectProperty> builder)
        {
            builder.ToTable("ProjectPropertys").Property(p => p.Key).HasMaxLength(100);
            builder.Property(p => p.Value).HasMaxLength(100);
            builder.HasKey(p => new
            {
                p.ProjectId,
                p.Key,
                p.Value
            });
        }
    }
}
