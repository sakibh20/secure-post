using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Internal
{
    public class MatchAcceptedViewModel
    {
        public string MatchId { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string FirstTurn { get; set; }
        public string MatchToken { get; set; }
    }
}
