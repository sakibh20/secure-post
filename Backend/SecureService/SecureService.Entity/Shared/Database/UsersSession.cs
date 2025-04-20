using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Database
{
    [Table("user_sessions")]
    public class UserSession
    {
        [Key]
        [Column("session_id")]
        public string SessionID { get; set; }  // UUID as the primary key

        [Required]
        [Column("user_id")]
        public string UserId { get; set; }  // Foreign key reference

        [Required]
        [Column("is_active_session_flag")]
        public bool IsActiveSessionFlag { get; set; } = false;

        [Required]
        [Column("session_start_time")]
        public DateTime SessionStartTime { get; set; } = DateTime.UtcNow;

        [Column("session_end_time")]
        public DateTime? SessionEndTime { get; set; }

        // Navigation property for the foreign key relationship
        [ForeignKey("UserId")]
        public virtual UserDetail User { get; set; }
    }
}
