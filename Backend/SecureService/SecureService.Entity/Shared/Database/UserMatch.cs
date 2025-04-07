using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Database
{
    [Table("user_match")]
    public class UserMatch
    {
        [Key]
        [Column("match_id")]
        public string MatchId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("player1")]
        public string Player1 { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("player2")]
        public string Player2 { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; } = DateTime.Now;

        [Column("end_time")]
        public DateTime? EndTime { get; set; }

        [Column("player1_moves")]
        public int Player1Moves { get; set; } = 0;

        [Column("player2_moves")]
        public int Player2Moves { get; set; } = 0;

        [Column("winner")]
        [MaxLength(100)]
        public string Winner { get; set; }
    }
}
