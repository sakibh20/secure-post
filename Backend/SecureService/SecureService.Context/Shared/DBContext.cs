using System.Text.RegularExpressions;
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

                entity.ToTable("users_details");

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

                entity.ToTable("user_sessions");

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

            modelBuilder.Entity<UserRefreshToken>(entity =>
            {
                entity.HasKey(e => e.TokenId);

                entity.ToTable("user_refresh_token");

                entity.Property(e => e.TokenId)
                    .HasColumnName("token_id")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ExpiryDate)
                    .HasColumnName("expiry_date")
                    .IsRequired();

                entity.Property(e => e.IsActive)
                    .HasColumnName("is_active")
                    .HasDefaultValue(false);

            });

            modelBuilder.Entity<UserMatch>(entity =>
            {
                entity.HasKey(e => e.MatchId);

                entity.ToTable("user_match");

                entity.Property(e => e.MatchId)
                    .HasColumnName("match_id")
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Player1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("player1");

                entity.Property(e => e.Player2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("player2");

                entity.Property(e => e.StartTime)
                    .HasColumnName("start_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EndTime)
                    .HasColumnName("end_time");

                entity.Property(e => e.Player1Moves)
                    .HasColumnName("player1_moves");

                entity.Property(e => e.Player2Moves)
                    .HasColumnName("player2_moves");

                entity.Property(e => e.Winner)
                    .HasMaxLength(100)
                    .HasColumnName("winner");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status")
                    .HasDefaultValue("PENDING");

                entity.HasIndex(e => e.Winner).HasDatabaseName("idx_winner");
            });

        }
    }
}
