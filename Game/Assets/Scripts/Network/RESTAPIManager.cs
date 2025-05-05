using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RESTAPIManager : MonoBehaviour
{
    public static RESTAPIManager Instance;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }
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
        AesEncryptionHelper encryptionHelper = new AesEncryptionHelper();
        string encryptedData = encryptionHelper.Encrypt(JsonUtility.ToJson(data));

        StartCoroutine(HitPost(encryptedData, ServerDataManager.Instance.GetRegistrationUrl()));
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
        AesEncryptionHelper encryptionHelper = new AesEncryptionHelper();
        string encryptedData = encryptionHelper.Encrypt(JsonUtility.ToJson(data));

        StartCoroutine(HitPost(encryptedData, ServerDataManager.Instance.GetLoginUrl()));
    }    
    
    public void AcceptDeclineMatch(string matchId, string player1, string player2, string status, Action<string> onSuccess = null, Action<string> onFailed = null)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;
        
        ExceptMatchClass data = new ExceptMatchClass
        {
            MatchId = matchId,
            Player1 = player1,
            Player2 = player2,
            MatchStatus = status
        };

        StartCoroutine(HitAcceptDeclineMatch(JsonUtility.ToJson(data), ServerDataManager.Instance.AcceptDeclineMatchUrl()));
    }   
    
    public void RollDice(string userId, string matchId, Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;
        
        DiceRollRequest data = new DiceRollRequest
        {
            match_id = matchId,
            user_id = userId
        };

        StartCoroutine(HitRollDice(JsonUtility.ToJson(data), ServerDataManager.Instance.GetRollUrl()));
    }
    
    public void ClaimDice(string userId, string matchId, int claimValue, Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;
        
        DiceRollRequest data = new DiceRollRequest
        {
            match_id = matchId,
            user_id = userId,
            claim_value = claimValue
        };

        StartCoroutine(HitClaimDecideDice(JsonUtility.ToJson(data), ServerDataManager.Instance.GetClaimUrl()));
    }
    
    public void Decide(string userId, string matchId, bool decision, Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;
        
        DiceRollRequest data = new DiceRollRequest
        {
            match_id = matchId,
            user_id = userId,
            decision = decision
        };

        StartCoroutine(HitClaimDecideDice(JsonUtility.ToJson(data), ServerDataManager.Instance.GetDecisionUrl()));
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
    
    public void GetHistory(Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;

        StartCoroutine(HitGetHistory());
    }
    
    public void RequestMatch(string userId, Action<string> onSuccess, Action<string> onFailed)
    {
        OnSuccess = onSuccess;
        OnFail = onFailed;

        StartCoroutine(HitRequestMatch(userId));
    }

    private IEnumerator HitPost(string data, string url)
    {
        Debug.Log($"Url: {url}");
        Debug.Log($"data: {data}");
        
        WWWForm form = new WWWForm();
        form.AddField("encryptData", data);

        using UnityWebRequest request = UnityWebRequest.Post(url, form);
        yield return request.SendWebRequest();
        
        // byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        //
        // UnityWebRequest request = new UnityWebRequest(url, "POST");
        // request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        // request.downloadHandler = new DownloadHandlerBuffer();
        // request.SetRequestHeader("Content-Type", "application/json");
        //
        // yield return request.SendWebRequest();
        
        ServerDataManager.Instance.serverResponse = new ServerResponse();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success: " + request.downloadHandler.text);
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
    
    private IEnumerator HitAcceptDeclineMatch(string data, string url)
    {
        Debug.Log($"Url: {url}");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + ServerDataManager.Instance.serverResponse.Result.accessToken);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        
        ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success: " + request.downloadHandler.text);
            
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
            Debug.LogError("Error: " + request.error);
            OnFail?.Invoke(serverResponse.Message);
        }
    }
    
    private IEnumerator HitLogout()
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerDataManager.Instance.GetLogoutUrl());

        request.SetRequestHeader("Authorization", "Bearer " + ServerDataManager.Instance.serverResponse.Result.accessToken);

        request.SetRequestHeader("Accept", "application/json");

        yield return request.SendWebRequest();
        
        ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            
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
        
        ServerDataManager.Instance.leaderboardResponse = JsonUtility.FromJson<LeaderboardResponse>(request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            
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
    
    private IEnumerator HitGetHistory()
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerDataManager.Instance.GetHistoryUrl());

        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ServerDataManager.Instance.serverResponse.Result.accessToken);

        yield return request.SendWebRequest();
        
        ServerDataManager.Instance.historyResponse = JsonUtility.FromJson<HistoryResponse>(request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            
            if (ServerDataManager.Instance.historyResponse.Status.ToLower() == ServerDataManager.Instance.FailedStatus)
            {
                OnFail?.Invoke(ServerDataManager.Instance.historyResponse.Message);
            }
            else
            {
                OnSuccess?.Invoke(ServerDataManager.Instance.historyResponse.Message);
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
            OnFail?.Invoke(request.result.ToString());
        }
    }
    
    private IEnumerator HitRequestMatch(string userId)
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerDataManager.Instance.GetMatchRequestUrl(userId));
        
        Debug.Log($"{ServerDataManager.Instance.GetMatchRequestUrl(userId)}");

        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ServerDataManager.Instance.serverResponse.Result.accessToken);

        yield return request.SendWebRequest();
        
        ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            
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
            OnFail?.Invoke(serverResponse.Message);
        }
    }
    
    private IEnumerator HitRollDice(string data, string url)
    {
        Debug.Log($"Url: {url}");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        
        ServerDataManager.Instance.diceRollResponse = JsonUtility.FromJson<DiceRollResponse>(request.downloadHandler.text);
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success: " + request.downloadHandler.text);
            
            OnSuccess?.Invoke(request.result.ToString());
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            OnFail?.Invoke(request.error);
        }
    }
    
    private IEnumerator HitClaimDecideDice(string data, string url)
    {
        Debug.Log($"Url: {url}");
        Debug.Log($"data: {data}");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        
        DiceRollResponse response = JsonUtility.FromJson<DiceRollResponse>(request.downloadHandler.text);
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Success: " + request.downloadHandler.text);
            
            OnSuccess?.Invoke(request.result.ToString());
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            OnFail?.Invoke(request.error);
        }
    }

    private Action OnSuccessRefresh;
    private Action OnFailRefresh;
    public void RefreshAccessToken(Action onSuccess, Action onFailed)
    {
        OnSuccessRefresh = onSuccess;
        OnFailRefresh = onFailed;
        StartCoroutine(HandleAccessTokenTimeout());
    }

    private IEnumerator HandleAccessTokenTimeout()
    {
        UnityWebRequest request = UnityWebRequest.Get(ServerDataManager.Instance.GetRefreshUrl());

        request.SetRequestHeader("Accept", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ServerDataManager.Instance.serverResponse.Result.refreshToken);

        yield return request.SendWebRequest();
        
        ServerResponse serverResponse = JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
            ServerDataManager.Instance.serverResponse =
                JsonUtility.FromJson<ServerResponse>(request.downloadHandler.text);
            if (serverResponse.Status.ToLower() == ServerDataManager.Instance.FailedStatus)
            {
                OnFailRefresh?.Invoke();
                //OnFail?.Invoke(ServerDataManager.Instance.leaderboardResponse.Message);
            }
            else
            {
                OnSuccessRefresh?.Invoke();
                //OnSuccess?.Invoke(ServerDataManager.Instance.leaderboardResponse.Message);
            }
        }
        else
        {
            Debug.LogError("Request failed: " + request.error);
            //OnFail?.Invoke(request.result.ToString());
            OnFailRefresh?.Invoke();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
