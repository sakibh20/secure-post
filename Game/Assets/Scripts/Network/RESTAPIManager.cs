using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RESTAPIManager : MonoBehaviour
{
    private event Action<string> OnSuccess; 
    private event Action<string> OnFail; 
    
    public void Register(string userId, string userName, string password, string confirmPassword, string email, Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;
        
        RegistrationRequestData data = new RegistrationRequestData
        {
            UserId = userId,
            UserName = userName,
            Password = password,
            ConfirmPassword = confirmPassword,
            Email = email
        };

        StartCoroutine(HitPost(JsonUtility.ToJson(data), ServerDataManager.Instance.GetRegistrationUrl()));
    }
    
    public void LogIn(string userId, string password, Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;
        
        LoginRequestData data = new LoginRequestData
        {
            UserId = userId,
            Password = password
        };

        StartCoroutine(HitPost(JsonUtility.ToJson(data), ServerDataManager.Instance.GetLoginUrl()));
    }    
    
    public void LogOut(Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;

        StartCoroutine(HitLogout());
    }
    public void GetLeaderboard(Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;

        StartCoroutine(HitGetLeaderboard());
    }

    private IEnumerator HitPost(string data, string url)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success: " + request.downloadHandler.text);
            ServerDataManager.Instance.serverResponse = new ServerResponse();
            ServerDataManager.Instance.serverResponse =
                JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            if (ServerDataManager.Instance.serverResponse.Status.ToLower() == ServerDataManager.Instance.FailedStatus)
            {
                OnFail?.Invoke(ServerDataManager.Instance.serverResponse.Message);
            }
            else
            {
                OnSuccess?.Invoke(ServerDataManager.Instance.serverResponse.Message);
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            OnFail?.Invoke(request.result.ToString());
        }
    }
    
    private IEnumerator HitLogout()
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerDataManager.Instance.GetLogoutUrl());

        request.SetRequestHeader("Authorization", "Bearer " + ServerDataManager.Instance.serverResponse.Result.accessToken);

        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            
            ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            
            if (serverResponse.Status.ToLower() == ServerDataManager.Instance.FailedStatus)
            {
                OnFail?.Invoke(serverResponse.Message);
            }
            else
            {
                OnSuccess?.Invoke(serverResponse.Message);
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
            OnFail?.Invoke(request.result.ToString());
        }
    }
    
    private IEnumerator HitGetLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerDataManager.Instance.GetLeaderboardUrl());

        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ServerDataManager.Instance.serverResponse.Result.accessToken);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            
            ServerDataManager.Instance.leaderboardResponse = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);
            
            if (ServerDataManager.Instance.leaderboardResponse.Status.ToLower() == ServerDataManager.Instance.FailedStatus)
            {
                OnFail?.Invoke(ServerDataManager.Instance.leaderboardResponse.Message);
            }
            else
            {
                OnSuccess?.Invoke(ServerDataManager.Instance.leaderboardResponse.Message);
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
            OnFail?.Invoke(request.result.ToString());
        }
    }
}
