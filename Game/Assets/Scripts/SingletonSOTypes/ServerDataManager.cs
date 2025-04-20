using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SingletonSOs/ServerDataManager")]
public class ServerDataManager : SingletonSO<ServerDataManager>
{
    public string baseUrl;
    public string registrationEndPoint;
    public string loginEndpoint;

    public ServerResponse serverResponse;

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
        string url = $"{baseUrl}/{loginEndpoint}";
        return url;
    }
    
    public string GetRegistrationUrl()
    {
        string url = $"{baseUrl}/{registrationEndPoint}";
        return url;
    }
}

[Serializable]
public class ServerResponse
{
    public string Status;
    public string Message;
    public string Result;
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

