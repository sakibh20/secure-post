using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;

namespace SecureService.BLL.Repositories
{
    public interface IMatchRepository
    {
        public StatusResult<object> InitializeMatchRequest(string playerID, UserDetail user);
        public StatusResult<object> ResponseMatchRequest(MatchRequestViewModel match, UserDetail user);
        public StatusResult<object> UpdateMatchResult(MatchresultViewModel matchInfo, UserMatch match);
        public StatusResult<object> FetchLeaderBoard(UserDetail user);
        public StatusResult<object> FetchMatchHistory(UserDetail user);
    }
}
