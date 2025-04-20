using TMPro;
using UnityEngine;

public class AuthUIManager : MonoBehaviour
{
    [SerializeField] private AuthAPIManager authAPIManager;

    [SerializeField] private TMP_InputField userIdRegField;
    [SerializeField] private TMP_InputField userNameRegField;
    [SerializeField] private TMP_InputField emailRegField;
    [SerializeField] private TMP_InputField passwordRegField;
    [SerializeField] private TMP_InputField passwordConfirmRegField;
    
    [SerializeField] private TMP_InputField userIdLoginField;
    [SerializeField] private TMP_InputField passwordLoginField;

    [SerializeField] private GameObject logInPanel;
    [SerializeField] private GameObject signUpPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI popupPanelText;
    
    public void OnClickLogIn()
    {
        (bool isValid, string message) = ValidateLogInFields();
        if(!isValid) return;
        
        ShowLoadingPanel();
        
        authAPIManager.LogIn(userIdLoginField.text, passwordLoginField.text, OnSuccessLogIn, OnFailLogIn);
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
        
        authAPIManager.Register(userIdRegField.text, userNameRegField.text, passwordRegField.text, passwordConfirmRegField.text, emailRegField.text, OnSuccessSignUp, OnFailSignUp);
    }
    
    private void OnSuccessSignUp(string message)
    {
        ShowLoginPanel();
        HideSignUpPanel();
        HideLoadingPanel();
        ShowPopupPanel(message);
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
    
    private void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    private void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    private void ShowPopupPanel(string message)
    {
        popupPanelText.text = message;
        popupPanel.SetActive(true);
    }

    public void HidePopupPanel()
    {
        popupPanel.SetActive(false);
    }
}
