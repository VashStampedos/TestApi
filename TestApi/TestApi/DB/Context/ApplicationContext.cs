using Microsoft.EntityFrameworkCore;
using TestApi.DB.Entities;

namespace TestApi.DB.Context
{
    public class ApplicationContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles{ get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.Age).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasData(
                        new User { Id = 1, Age = 18, Email = "wp@mail.com", Name = "Vlad" },
                        new User { Id = 2, Age = 28, Email = "asddsa@mail.com", Name = "Gena" },
                        new User { Id = 3, Age = 12, Email = "repp@mail.com", Name = "Fena" }
                    );

            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired();
                entity.HasData(
                    new Role { Id = 1, Name = "User" },
                    new Role { Id = 2, Name = "Admin" },
                    new Role { Id = 3, Name = "Support" },
                    new Role { Id = 4, Name = "SuperAdmin" }

                    );

            });
            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.ToTable("UserRoles");
                entity.Property(x => x.UserId).IsRequired();
                entity.Property(x => x.RoleId).IsRequired();
                entity.HasKey(x => new { x.UserId, x.RoleId });
                entity.HasOne(x => x.User).WithMany(x => x.UserRoles).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Role).WithMany(x => x.UserRoles).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
                entity.HasData(
                   new UserRoles { UserId = 1, RoleId = 1 },
                   new UserRoles { UserId = 2, RoleId = 1 },
                   new UserRoles { UserId = 2, RoleId = 3 },
                   new UserRoles { UserId = 3, RoleId = 2 }
                   );
            });
        }
    }
}
