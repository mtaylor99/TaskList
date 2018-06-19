using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskList.DAL.Models;

namespace TaskList.DAL.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<AspNetUser, AspNetRole, string>
    {
        public DbSet<AspNetGroup> AspNetGroup { get; set; }

        public DbSet<AspNetRoleGroup> AspNetRoleGroup { get; set; }

        public DbSet<AspNetUserGroup> AspNetUserGroup { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Task> Task { get; set; }

        public DbSet<Image> Image { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Groups
            builder.Entity<AspNetGroup>()
                .ToTable("AspNetGroups")
                .HasKey(g => g.GroupId);

            //Map Users to Groups
            builder.Entity<AspNetGroup>()
                .ToTable("AspNetGroups")
                .HasMany<AspNetUserGroup>((AspNetGroup g) => g.Users);

            builder.Entity<AspNetUserGroup>()
                .ToTable("AspNetUserGroups")
                .HasKey((AspNetUserGroup r) =>
                new
                {
                    ApplicationUserId = r.UserId,
                    ApplicationGroupId = r.GroupId
                });

            //Map Roles to Groups
            builder.Entity<AspNetGroup>()
                .ToTable("AspNetGroups")
                .HasMany<AspNetRoleGroup>((AspNetGroup g) => g.Roles);

            builder.Entity<AspNetRoleGroup>()
                .ToTable("AspNetRoleGroups")
                .HasKey((AspNetRoleGroup gr) =>
                new
                {
                    ApplicationRoleId = gr.RoleId,
                    ApplicationGroupId = gr.GroupId
                });
        }
    }
}
