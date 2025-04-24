using TMPro;
using UnityEngine;

public class ConnectionUIManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI popupPanelText;
    
    protected void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    protected void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }

    public void ShowPopupPanel(string message)
    {
        popupPanelText.text = message;
        popupPanel.SetActive(true);
    }

    public void HidePopupPanel()
    {
        popupPanel.SetActive(false);
    }
}
