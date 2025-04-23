using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUIManager : ConnectionUIManager
{
    [SerializeField] private RESTAPIManager restapiManager;
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

    private void Awake()
    {
        InitAuthUI();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void InitAuthUI()
    {
        if (PlayerPrefs.HasKey(UserIdKey))
        {
            passwordLoginField.text = "";
            userIdLoginField.text = PlayerPrefs.GetString(UserIdKey);
            ShowLoginPanel();
        }
        else
        {
            ShowSignUpPanel();
        }
    }

    private void UpdateUserId(string userId)
    {
        PlayerPrefs.SetString(UserIdKey, userId);
    }
    
    public void OnClickLogOut()
    {
        ShowLoadingPanel();
        
        restapiManager.LogOut(OnSuccessLogOut, OnFailLogOut);
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
        
        restapiManager.LogIn(userIdLoginField.text, passwordLoginField.text, OnSuccessLogIn, OnFailLogIn);
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
        UpdateUserId(userIdLoginField.text);
        
        homeUiManager.ShowHomeUi();
        ClearLogInFields();
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
        
        restapiManager.Register(userIdRegField.text, userNameRegField.text, passwordRegField.text, passwordConfirmRegField.text, emailRegField.text, OnSuccessSignUp, OnFailSignUp);
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
        UpdateUserId(userIdRegField.text);
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
