using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Recommend.API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectRecommends",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    FromUserId = table.Column<int>(nullable: false),
                    FromUserName = table.Column<string>(nullable: true),
                    FromUserAvatar = table.Column<string>(nullable: true),
                    RecommendType = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    PrjectAvatart = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Introduction = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    FinStage = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    RecommendTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRecommends", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRecommends");
        }
    }
}
