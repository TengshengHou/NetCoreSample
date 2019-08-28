
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Api.Model;

namespace User.Api.Data
{

    public class UserContext : DbContext
    {
        private DbContextOptionsBuilder<UserContext> options;

        public DbSet<AppUser> Users {get;set;}
        public DbSet<UserProperty> UserProperty { get; set; }

        public DbSet<UserTag> UserTags  { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AppUser>().ToTable("Users")
                  .HasKey(u => u.Id);

            modelBuilder.Entity<UserProperty>().Property(u => u.Key).HasMaxLength(100);
            modelBuilder.Entity<UserProperty>().Property(u => u.Value).HasMaxLength(100);


            modelBuilder.Entity<UserProperty>().ToTable("UserProperties")
                  .HasKey(u => new { u.Key,u.AppUserId,u.Value});

            modelBuilder.Entity<UserTag>().Property(u => u.Tag).HasMaxLength(100);//因mysql 主键最大长度约束为100 
            modelBuilder.Entity<UserTag>().ToTable("UserTags")
                  .HasKey(u => new { u.UserId,u.Tag });
            

            modelBuilder.Entity<BPfile>().ToTable("UserBPFiles")
               .HasKey(b => b.Id);

        }
    }
}
