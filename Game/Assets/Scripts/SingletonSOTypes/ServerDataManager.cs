using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SingletonSOs/ServerDataManager")]
public class ServerDataManager : SingletonSO<ServerDataManager>
{
    public string baseUrl;
    public string registrationEndPoint;
    public string loginEndpoint;
    public string logoutEndpoint;
    public string leaderboardEndpoint;
    public string refreshAccessToken;
    public string requestMatch;
    public string acceptMatch;
    
    [Space]
    public string wsBaseUrl = "ws://localhost:8000/ws";
    public string lobbyEndpoint = "lobby";
    public string joinEndpoint = "user";

    [Space] 
    public string matchId;
    public string player1;
    public string player2;
    
    public ServerResponse serverResponse;
    public LeaderboardResponse leaderboardResponse;

    public WSLobbyMessage wsLobbyMessage;

    public string FailedStatus = "failed";
    public string SuccessStatus = "ok";
    
    public string AcceptStatus = "ACCEPTED";
    public string DeclineStatus = "DECLINED";
    
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
    
    public string GetJoinUrl(string userId)
    {
        return $"{wsBaseUrl}/{joinEndpoint}/{userId}";
    }

    public string GetLoginUrl()
    {
        return $"{baseUrl}/{loginEndpoint}";
    }    
    
    public string GetLogoutUrl()
    {
        return $"{baseUrl}/{logoutEndpoint}";
    }
    
    public string GetRegistrationUrl()
    {
        return $"{baseUrl}/{registrationEndPoint}";
    }    
    
    public string GetLeaderboardUrl()
    {
        return $"{baseUrl}/{leaderboardEndpoint}";
    }
    
    public string GetRefreshUrl()
    {
        return $"{baseUrl}/{refreshAccessToken}";
    }
    
    public string GetMatchRequestUrl(string targetUserID)
    {
        return $"{baseUrl}/{requestMatch}?playerID={targetUserID}";
    }    
    
    public string AcceptDeclineMatchUrl()
    {
        return $"{baseUrl}/{acceptMatch}";
    }
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
    public string useID;
    public string email;
    public string accessToken;
    public string refreshToken;
    public string accessTokenExpiry;
    public string refreshTokenExpiry;
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
    //public string eventType;
    public List<string> users = new List<string>();
}

[Serializable]
public class ExceptMatchClass
{
    public string MatchId;
    public string Player1;
    public string Player2;
    public string MatchStatus;
}

