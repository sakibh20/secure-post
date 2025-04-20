using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Internal
{
    public class MatchresultViewModel
    {
        public string MatchId { get; set; }
        public int Player1Moves { get; set; } 
        public int Player2Moves { get; set; }
        public string Winner { get; set; }
    }
}
