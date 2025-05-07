using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUIManager : ConnectionUIManager
{
    [SerializeField] private HomeUiManager homeUiManager;

    [SerializeField] private TMP_InputField userIdRegField;
    [SerializeField] private TMP_InputField userNameRegField;
    [SerializeField] private TMP_InputField emailRegField;
    [SerializeField] private TMP_InputField passwordRegField;
    [SerializeField] private TMP_InputField passwordConfirmRegField;
    [SerializeField] private Button signUpButton;
    
    [Space]
    [SerializeField] private TMP_InputField userIdLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;
    [SerializeField] private Button signInButton;
    
    [Space]
    [SerializeField] private GameObject logInPanel;
    [SerializeField] private GameObject signUpPanel;

    private const string UserIdKey = "userId";
    private const string PasswordKey = "pass";
    
    public event Action OnLoginSuccess;

    public static AuthUIManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        InitAuthUI();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(RefreshAccessToken));
    }

    private void InitAuthUI()
    {
        HidePopupPanel();
        if (PlayerPrefs.HasKey(UserIdKey))
        {
            passwordLoginField.text = PlayerPrefs.GetString(PasswordKey);
            userIdLoginField.text = PlayerPrefs.GetString(UserIdKey);
            ShowLoginPanel();
        }
        else
        {
            ShowSignUpPanel();
        }
    }

    private void UpdateUserId(string userId, string password)
    {
        PlayerPrefs.SetString(UserIdKey, userId);
        PlayerPrefs.SetString(PasswordKey, password);
    }
    
    public void OnClickLogOut()
    {
        ShowLoadingPanel();
        
        RESTAPIManager.Instance.LogOut(OnSuccessLogOut, OnFailLogOut);
    }
    
    private void OnSuccessLogOut(string message)
    {
        InitAuthUI();
        homeUiManager.HideProfilePanel();
        ShowPopupPanel(message);
        HideLoadingPanel();
    }
    
    private void OnFailLogOut(string message)
    {
        HideLoadingPanel();
        ShowPopupPanel(message);
    }

    public void OnClickLogIn()
    {
        (bool isValid, string message) = ValidateLogInFields();
        if(!isValid) return;
        
        ShowLoadingPanel();
        
        RESTAPIManager.Instance.LogIn(userIdLoginField.text, passwordLoginField.text, OnSuccessLogIn, OnFailLogIn);
    }

    private void ClearLogInFields()
    {
        userIdLoginField.text = "";
        passwordLoginField.text = "";
    }
    
    private (bool, string) ValidateLogInFields()
    {
        (bool, string) validationStatus = (true, "");
        return validationStatus;
    }

    private void OnSuccessLogIn(string message)
    {
        HideLoginPanel();
        HideSignUpPanel();

        HideLoadingPanel();
        UpdateUserId(userIdLoginField.text, passwordLoginField.text);
        
        homeUiManager.ShowHomeUi();
        ClearLogInFields();
        
        Invoke(nameof(RefreshAccessToken), GetInSec());
        
        OnLoginSuccess?.Invoke();
    }


    [SerializeField] private float early = 880;
    private float GetInSec()
    {
        DateTime tokenExpiry = DateTime.Parse(ServerDataManager.Instance.serverResponse.Result.accessTokenExpiry);
        TimeSpan remaining = tokenExpiry.ToUniversalTime() - DateTime.UtcNow;
        double secondsRemaining = remaining.TotalSeconds - early;
        if (secondsRemaining <= 0) secondsRemaining = 0;
        
        return (float)secondsRemaining;
    }

    [ContextMenu("RefreshAccessToken")]
    private void RefreshAccessToken()
    {
        RESTAPIManager.Instance.RefreshAccessToken(OnSuccessRefresh, OnFailRefresh);
    }

    private void OnSuccessRefresh()
    {
        Invoke(nameof(RefreshAccessToken), GetInSec());
    }

    [ContextMenu("OnFailRefresh")]
    private void OnFailRefresh()
    {
        ShowLoginPanel();
    }
    
    private void OnFailLogIn(string message)
    {
        HideLoadingPanel();
        ShowPopupPanel(message);
    }
    
    public void OnClickSignUp()
    {
        (bool isValid, string message) = ValidateSignUpFields();
        if(!isValid) return;
        
        ShowLoadingPanel();
        
        RESTAPIManager.Instance.Register(userIdRegField.text, userNameRegField.text, passwordRegField.text, passwordConfirmRegField.text, emailRegField.text, OnSuccessSignUp, OnFailSignUp);
    }
    
    private void ClearSignUpFields()
    {
        userIdRegField.text = "";
        userNameRegField.text = "";
        passwordRegField.text = "";
        passwordConfirmRegField.text = "";
        emailRegField.text = "";
    }
    
    private void OnSuccessSignUp(string message)
    {
        HideLoadingPanel();
        ShowPopupPanel(message);
        UpdateUserId(userIdRegField.text, passwordRegField.text);
        ClearSignUpFields();
        InitAuthUI();
    }
    
    private void OnFailSignUp(string message)
    {
        HideLoadingPanel();
        ShowPopupPanel(message);
    }
    
    private (bool, string) ValidateSignUpFields()
    {
        (bool, string) validationStatus = (true, "");
        return validationStatus;
    }

    public void ShowLoginPanel()
    {
        logInPanel.SetActive(true);
        HideSignUpPanel();
    }
    
    public void ShowSignUpPanel()
    {
        signUpPanel.SetActive(true);
        HideLoginPanel();
    }
    
    private void HideLoginPanel()
    {
        logInPanel.SetActive(false);
    }
    
    private void HideSignUpPanel()
    {
        signUpPanel.SetActive(false);
    }
}
