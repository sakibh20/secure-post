using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUiManager : MonoBehaviour
{
    [SerializeField] private LobbyPlayerItem lobbyPlayerItemPrefab;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject matchRequestPanel;
    [SerializeField] private TextMeshProUGUI matchRequestPanelText;
    [SerializeField] private Transform playerItemParent;
    [SerializeField] private int waitingTime = 8;
    
    private List<LobbyPlayerItem> _instantiatedItemsList = new List<LobbyPlayerItem>();
    
    public static LobbyUiManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    private void Start()
    {
        LobbyWebSocketClient.Instance.MessageReceived += HandleServerMessage;
    }

    private void OnDestroy()
    {
        LobbyWebSocketClient.Instance.MessageReceived -= HandleServerMessage;
    }

    private void HandleServerMessage(string message)
    {
        Debug.Log($"Server Message: {message}");
        
        if(string.IsNullOrWhiteSpace(message)) return;
        
        ServerDataManager.Instance.wsLobbyMessage = JsonUtility.FromJson<WSLobbyMessage>(message);

        GenerateItems();
    }

    public void ShowLobby()
    {
        GenerateItems();
        lobbyPanel.SetActive(true);
    }
    
    public void HideLobby()
    {
        lobbyPanel.SetActive(false);
    }
    
    private void InitLobbyPanel()
    {
        foreach (Transform child in playerItemParent)
        {
            Destroy(child.gameObject);
        }

        _instantiatedItemsList = new List<LobbyPlayerItem>();
    }

    private void GenerateItems()
    {
        InitLobbyPanel();
        
        foreach (string user in ServerDataManager.Instance.wsLobbyMessage.users)
        {
            if(user == ServerDataManager.Instance.serverResponse.Result.useID) continue;
            
            LobbyPlayerItem instantiatedItem = Instantiate(lobbyPlayerItemPrefab, playerItemParent);
            instantiatedItem.InitInfo(user);
            _instantiatedItemsList.Add(instantiatedItem);
        }
    }

    [ContextMenu("ShowNewMatchRequestView")]
    public void ShowNewMatchRequestView()
    {
        matchRequestPanel.SetActive(true);
        
        matchRequestPanelText.text = $"Received challenge from {ServerDataManager.Instance.player1}";
    }

    private void HideMatchRequestView()
    {
        matchRequestPanel.SetActive(false);
    }

    public void AcceptMatch()
    {
        RESTAPIManager.Instance.AcceptDeclineMatch(ServerDataManager.Instance.matchId, ServerDataManager.Instance.player1, 
            ServerDataManager.Instance.player2, ServerDataManager.Instance.AcceptStatus, OnSuccessAcceptDecline, OnFailAcceptDecline);
    }

    public void DeclineMatch()
    {
        RESTAPIManager.Instance.AcceptDeclineMatch(ServerDataManager.Instance.matchId, ServerDataManager.Instance.player1, ServerDataManager.Instance.player2, 
            ServerDataManager.Instance.DeclineStatus, OnSuccessAcceptDecline, OnFailAcceptDecline);
    }

    private void OnSuccessAcceptDecline(string message)
    {
        HideMatchRequestView();
    }    
    
    private void OnFailAcceptDecline(string message)
    {
        AuthUIManager.Instance.ShowPopupPanel(message);
    }

    public void Requested()
    {
        foreach (LobbyPlayerItem lobbyPlayerItem in _instantiatedItemsList)
        {
            lobbyPlayerItem.ActivateButtons(false);
        }
        
        Invoke(nameof(RequestTimeout), waitingTime);
    }

    public void RequestTimeout()
    {
        foreach (LobbyPlayerItem lobbyPlayerItem in _instantiatedItemsList)
        {
            lobbyPlayerItem.ActivateButtons(true);
        }
    }
}
