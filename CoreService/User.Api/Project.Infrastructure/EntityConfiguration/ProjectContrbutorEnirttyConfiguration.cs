using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{

    public class ProjectContrbutorEnirttyConfiguration : IEntityTypeConfiguration<Domain.AggergatesModel.ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<Domain.AggergatesModel.ProjectContributor> builder)
        {
            builder.ToTable("ProjectContributors").HasKey(p => p.Id);
        }
    }
}
