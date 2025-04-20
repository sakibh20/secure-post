using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Database
{
    [Table("user_refresh_token")]
    public class UserRefreshToken
    {
        [Key]
        [Column("token_id")]
        [Required]
        [MaxLength(255)]
        public string TokenId { get; set; }  // Typically a GUID or secure token string

        [Column("user_id")]
        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("expiry_date")]
        [Required]
        public DateTime ExpiryDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = false;

    }
}
