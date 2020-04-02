﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Recommend.API.Data;

namespace Recommend.API.Migrations
{
    [DbContext(typeof(RecommendDbContext))]
    [Migration("20200201032225_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Recommend.API.Mdoels.ProjectRecommend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Company");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("FinStage");

                    b.Property<string>("FromUserAvatar");

                    b.Property<int>("FromUserId");

                    b.Property<string>("FromUserName");

                    b.Property<string>("Introduction");

                    b.Property<string>("PrjectAvatart");

                    b.Property<int>("ProjectId");

                    b.Property<DateTime>("RecommendTime");

                    b.Property<int>("RecommendType");

                    b.Property<string>("Tags");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ProjectRecommends");
                });
#pragma warning restore 612, 618
        }
    }
}