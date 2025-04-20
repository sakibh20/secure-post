using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Database
{
    [Table("users_details")]
    public class UserDetail
    {
        [Key]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("user_name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }  // Store hashed passwords

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        [Column("role")]
        public string Role { get; set; } = "User";

        [Column("is_active_flag")]
        public bool IsActiveFlag { get; set; } = true;

        [Column("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

        [Column("failed_login_attempt_no")]
        public int FailedLoginAttemptNo { get; set; } = 0;

        [Column("force_password_change_flag")]
        public bool ForcePasswordChangeFlag { get; set; } = false;

        // Navigation property for sessions
        public virtual ICollection<UserSession> Sessions { get; set; }

        [NotMapped]
        public bool Isupdated { get; set; }
    }
}
