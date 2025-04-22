using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureService.Entity.Shared.Internal
{
    public class LeaderBoardDataViewModel
    {
        public List<LeaderBoardUserData> TopUsers = new List<LeaderBoardUserData>();
        public LeaderBoardUserData User { get; set; }
    }
    public class LeaderBoardUserData
    {
        public string Position { get; set; }
        public string Player { get; set; }
        public int Wins { get; set; }
    }
}
