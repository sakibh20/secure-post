using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;

namespace SecureService.DAL.Repositories
{
    public interface IDalMatchRepository
    {
        public StatusResult<object> CreateMatchID(PlayerValidationViewModel playerInfo);
        public StatusResult<object> UpdateMatchResult(MatchresultViewModel matchInfo, UserMatch match);
        public StatusResult<object> FetchLeaderBoard();
        public StatusResult<object> FetchMatchHistory(UserDetail user);
    }
}
