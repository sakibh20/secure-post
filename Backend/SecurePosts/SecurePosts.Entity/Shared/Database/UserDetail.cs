using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurePosts.Entity.Shared.Database
{
    public class UserDetail
    {
        [Key]
        public string UserId { get; set; } 

        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }  // Store hashed passwords

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "User";

        public bool IsActiveFlag { get; set; } = true;

        public DateTime? LastLoginAt { get; set; }

        public int FailedLoginAttemptNo { get; set; } = 0;

        public bool ForcePasswordChangeFlag { get; set; } = false;

        // Navigation property for sessions
        public virtual ICollection<UserSession> Sessions { get; set; }

        [NotMapped]
        public bool Isupdated { get; set; }
    }
}
