using Microsoft.EntityFrameworkCore;
using SecureService.Entity.Shared.Database;

namespace SecureService.Context.Shared
{
    public static class DBContext
    {
        public static void ConfigureEntity(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDetail>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("USERS_DETAILS");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserId");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("UserName");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("Email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("Password");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnName("CreatedAt");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("Role");

                entity.Property(e => e.IsActiveFlag)
                    .HasColumnName("IsActiveFlag");

                entity.Property(e => e.LastLoginAt)
                    .HasColumnName("LastLoginAt");

                entity.Property(e => e.FailedLoginAttemptNo)
                    .HasColumnName("FailedLoginAttemptNo");

                entity.Property(e => e.ForcePasswordChangeFlag)
                    .HasColumnName("ForcePasswordChangeFlag");
            });

            modelBuilder.Entity<UserSession>(entity =>
            {
                entity.HasKey(e => e.SessionID);

                entity.ToTable("USER_SESSIONS");

                entity.Property(e => e.SessionID)
                    .HasColumnName("SessionID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("UserId");

                entity.Property(e => e.IsActiveSessionFlag)
                    .HasColumnName("IsActiveSessionFlag");

                entity.Property(e => e.SessionStartTime)
                    .IsRequired()
                    .HasColumnName("SessionStartTime");

                entity.Property(e => e.SessionEndTime)
                    .HasColumnName("SessionEndTime");

                // Foreign key relationship
                entity.HasOne(e => e.User)
                    .WithMany(u => u.Sessions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
