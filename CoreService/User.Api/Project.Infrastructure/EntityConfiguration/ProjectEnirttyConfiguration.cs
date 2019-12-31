using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggergatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectEnirttyConfiguration : IEntityTypeConfiguration<Domain.AggergatesModel.Project>
    {
        public void Configure(EntityTypeBuilder<Domain.AggergatesModel.Project> builder)
        {
            builder.ToTable("Projects").HasKey(p => p.Id);
        }
    }
}
