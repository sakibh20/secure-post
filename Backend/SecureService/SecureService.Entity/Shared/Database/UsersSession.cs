using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Database
{
    public class UserSession
    {
        [Key]
        public string SessionID { get; set; }  // UUID as the primary key

        [Required]
        public string UserId { get; set; }  // Foreign key reference

        [Required]
        public bool IsActiveSessionFlag { get; set; } = false;

        [Required]
        public DateTime SessionStartTime { get; set; } = DateTime.UtcNow;

        public DateTime? SessionEndTime { get; set; }

        // Navigation property for the foreign key relationship
        [ForeignKey("UserId")]
        public virtual UserDetail User { get; set; }
    }
}
