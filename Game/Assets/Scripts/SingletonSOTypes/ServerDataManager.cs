using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
[CreateAssetMenu(menuName = "SingletonSOs/ServerDataManager")]
public class ServerDataManager : SingletonSO<ServerDataManager>
{
    public string serverABaseUrl;
    public string registrationEndPoint;
    public string loginEndpoint;
    public string logoutEndpoint;
    public string leaderboardEndpoint;
    public string refreshAccessToken;
    public string requestMatch;
    public string acceptMatch;

    [Space]
    public string serverBBaseUrl = "http://localhost:8000";
    public string rollDice = "roll-dice";
    public string claimDice = "claim";
    public string decide = "decide";
    
    [Space]
    public string wsBaseUrl = "ws://localhost:8000/ws";
    public string lobbyEndpoint = "lobby";
    public string joinEndpoint = "user";
    public string joinMatch = "game";

    // [Space] 
    // public string matchId;
    // public string player1;
    // public string player2;

    [Space]
    public string FailedStatus = "failed";
    public string SuccessStatus = "ok";
    
    [Space]
    public string AcceptStatus = "ACCEPTED";
    public string DeclineStatus = "DECLINED";
    
    [Space]
    public string ReceivedChallengeCommand = "match_request";
    public string JoinMatchCommand = "join_match";
    public string RollDiceCommand = "roll_dice";
    public string ClaimCommand = "claim_dice";
    public string DecisionCommand = "decide";
    public string MatchStartCommand = "match_start";
    public string RoundOverCommand = "round_over";
    public string GameOverCommandCommand = "game_over";
    
    [Space]
    public ServerResponse serverResponse;
    public LeaderboardResponse leaderboardResponse;

    public WSLobbyMessage wsLobbyMessage;
    public WSLobbyMessage wsUserMessage;
    public WSLobbyMessage wsMatchMessage;

    public DiceRollResponse diceRollResponse;
    public DiceRollResponse decideResponse;
    public DiceRollResponse claimResponse;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoad()
    {
        ServerDataManager instance = Instance;
        if (instance != null)
        {
            Debug.Log("ServerDataManager loaded successfully.");
        }
        else
        {
            Debug.LogError("ServerDataManager instance is null.");
        }
    }
    
    public string GetLobbyUrl()
    {
        return $"{wsBaseUrl}/{lobbyEndpoint}";
    }
    
    public string GetJoinUrl()
    {
        return $"{wsBaseUrl}/{joinEndpoint}/{serverResponse.Result.userID}";
    }    
    
    public string GetJoinMatchUrl()
    {
        return $"{wsBaseUrl}/{joinMatch}/{serverResponse.Result.userID}/{wsUserMessage.payload.match_id}";
    }
    
    public string GetRollUrl()
    {
        return $"{serverBBaseUrl}/{rollDice}";
    }      
    
    public string GetClaimUrl()
    {
        return $"{serverBBaseUrl}/{claimDice}";
    }        
    
    public string GetDecisionUrl()
    {
        return $"{serverBBaseUrl}/{decide}";
    }   
    
    public string GetLoginUrl()
    {
        return $"{serverABaseUrl}/{loginEndpoint}";
    }    
    
    public string GetLogoutUrl()
    {
        return $"{serverABaseUrl}/{logoutEndpoint}";
    }
    
    public string GetRegistrationUrl()
    {
        return $"{serverABaseUrl}/{registrationEndPoint}";
    }    
    
    public string GetLeaderboardUrl()
    {
        return $"{serverABaseUrl}/{leaderboardEndpoint}";
    }
    
    public string GetRefreshUrl()
    {
        return $"{serverABaseUrl}/{refreshAccessToken}";
    }
    
    public string GetMatchRequestUrl(string targetUserID)
    {
        return $"{serverABaseUrl}/{requestMatch}?playerID={targetUserID}";
    }    
    
    public string AcceptDeclineMatchUrl()
    {
        return $"{serverABaseUrl}/{acceptMatch}";
    }
}

[Serializable]
public class MatchStatus
{
    public string command;
    public Payload payload = new Payload();
}

[Serializable]
public class DiceRollRequest
{
    public string match_id;
    public string user_id;
    public int claim_value;
    public bool decision;
}

[Serializable]
public class DiceRollResponse
{
    public string match_id;
    public string user_id;
    public int dice_roll;
}

[Serializable]
public class ServerResponse
{
    public string Status;
    public string Message;
    public Result Result = new Result();
}

[Serializable]
public class LeaderboardResponse
{
    public string Status;
    public string Message;
    public LeaderboardResult Result;
}

[Serializable]
public class LeaderboardResult
{
    public List<LeaderBoardItem> topUsers;
    public LeaderBoardItem user;
}

[Serializable]
public class LeaderBoardItem
{
    public string player;
    public string wins;
    public string position;
}

[Serializable]
public class Result
{
    public string userID;
    public string email;
    public string accessToken;
    public string refreshToken;
    public DateTime accessTokenExpiry;
    public DateTime refreshTokenExpiry;
}

[Serializable]
public class RegistrationRequestData
{
    public string UserId;
    public string UserName;
    public string Email;
    public string Password;
    public string ConfirmPassword;
}

[Serializable]
public class LoginRequestData
{
    public string UserId;
    public string Password;
}

[Serializable]
public class WSLobbyMessage
{
    public string command;
    public Payload payload = new Payload();
}

[Serializable]
public class Payload
{
    public string status;
    public string requested_by;
    public string match_id;
    public List<string> users = new List<string>();
    public int player1_score;
    public int player2_score;
    public string player1;
    public string player2;
    public string game_status;
    public int total_round;
    public int current_round;
    public int claim;
}

[Serializable]
public class ExceptMatchClass
{
    public string MatchId;
    public string Player1;
    public string Player2;
    public string MatchStatus;
}

