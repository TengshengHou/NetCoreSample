using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{

    public class ProjectVisibleRuleEnirttyConfiguration : IEntityTypeConfiguration<Domain.AggergatesModel.ProjectVisibleRule>
    {
        public void Configure(EntityTypeBuilder<Domain.AggergatesModel.ProjectVisibleRule> builder)
        {
            builder.ToTable("ProjectVisibleRules").HasKey(p => p.Id);
        }
    }
}
