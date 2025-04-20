using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class AuthAPIManager : MonoBehaviour
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

        StartCoroutine(Hit(JsonUtility.ToJson(data), ServerDataManager.Instance.GetRegistrationUrl()));
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

        StartCoroutine(Hit(JsonUtility.ToJson(data), ServerDataManager.Instance.GetLoginUrl()));
    }

    private IEnumerator  Hit(string data, string url)
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
                OnFail?.Invoke(ServerDataManager.Instance.serverResponse.Result);
            }
            else
            {
                OnSuccess?.Invoke(ServerDataManager.Instance.serverResponse.Result);
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            OnFail?.Invoke(request.result.ToString());
        }
    }
}
