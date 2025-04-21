using TMPro;
using UnityEngine;

public class HomeUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI initialInHomeText;
    [SerializeField] private TextMeshProUGUI userNameInHomeText;
    
    [SerializeField] private TextMeshProUGUI initialInProfileText;
    [SerializeField] private TextMeshProUGUI userNameInProfileText;
    [SerializeField] private TextMeshProUGUI emailInProfileText;

    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject profilePanel;
    
    public void ShowHomeUi()
    {
        homePanel.SetActive(true);
        InitHomeUi();
    }

    private void InitHomeUi()
    {
        HideProfilePanel();
        
        initialInHomeText.text = ServerDataManager.Instance.serverResponse.Result.useID[0].ToString().ToUpper();
        userNameInHomeText.text = ServerDataManager.Instance.serverResponse.Result.useID; 
        
        initialInProfileText.text = initialInHomeText.text;
        userNameInProfileText.text = $"<b>UserID:</b> {ServerDataManager.Instance.serverResponse.Result.useID}";
        emailInProfileText.text = $"<b>Email:</b> {ServerDataManager.Instance.serverResponse.Result.email}";
    }


    public void ShowProfilePanel()
    {
        profilePanel.SetActive(true);
    }
    
    public void HideProfilePanel()
    {
        profilePanel.SetActive(false);
    }
}
