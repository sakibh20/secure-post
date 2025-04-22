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

    public ServerResponse serverResponse;
    public LeaderboardResponse leaderboardResponse;

    public string FailedStatus = "failed";
    public string SuccessStatus = "ok";
    
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
}

[Serializable]
public class ServerResponse
{
    public string Status;
    public string Message;
    public Result Result;
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

