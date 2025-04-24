using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userIdText;
    [SerializeField] private Button challengeButton;

    private string _userID;

    private void Start()
    {
        challengeButton.onClick.AddListener(OnClickChallenge);
    }

    private void OnDestroy()
    {
        challengeButton.onClick.RemoveListener(OnClickChallenge);
    }

    public void InitInfo(string userId)
    {
        _userID = userId;
        userIdText.text = userId;
    }

    private void OnClickChallenge()
    {
        RESTAPIManager.Instance.RequestMatch(_userID, OnSuccess, OnFail);
    }
    
    private void OnSuccess(string message)
    {
        LobbyUiManager.Instance.Requested();
    }
    
    private void OnFail(string message)
    {
        
    }

    public void ActivateButtons(bool value)
    {
        challengeButton.interactable = value;
    }
}
