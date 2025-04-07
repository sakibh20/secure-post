using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SecureService.BLL.Repositories;
using SecureService.DAL.Repositories;
using SecureService.Entity.Shared.Database;
using SecureService.Entity.Shared.Internal;
using SecureService.Entity.Shared.Status;

namespace SecureService.BLL.Services
{
    public class MatchService : IMatchRepository
    {
        private readonly IDalMatchRepository _IDalMatchRepository;

        public MatchService(IDalMatchRepository IDalMatchRepository)
        {
            this._IDalMatchRepository = IDalMatchRepository;
        }
        public StatusResult<object> CreateMatchID(PlayerValidationViewModel playerInfo)
        {
            return _IDalMatchRepository.CreateMatchID(playerInfo);
        }
        public StatusResult<object> UpdateMatchResult(MatchresultViewModel matchInfo, UserMatch match)
        {
            return _IDalMatchRepository.UpdateMatchResult(matchInfo, match);
        }
        public StatusResult<object> FetchLeaderBoard()
        {
            return _IDalMatchRepository.FetchLeaderBoard();
        }
        public StatusResult<object> FetchMatchHistory(UserDetail user)
        {
            return _IDalMatchRepository.FetchMatchHistory(user);
        }
    }
}
