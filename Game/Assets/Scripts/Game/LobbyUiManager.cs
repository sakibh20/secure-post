using UnityEngine;

public class LobbyUiManager : MonoBehaviour
{
    [SerializeField] private LobbyPlayerItem lobbyPlayerItemPrefab;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private Transform playerItemParent;
    
    public static LobbyUiManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowLobby()
    {
        InitLeaderboardPanel();
        GenerateItems();
        lobbyPanel.SetActive(true);
    }
    
    public void HideLobby()
    {
        lobbyPanel.SetActive(false);
    }
    
    private void InitLeaderboardPanel()
    {
        foreach (Transform child in playerItemParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateItems()
    {
        foreach (LeaderBoardItem item in ServerDataManager.Instance.leaderboardResponse.Result.topUsers)
        {
            LobbyPlayerItem instantiatedItem = Instantiate(lobbyPlayerItemPrefab, playerItemParent);
            instantiatedItem.InitInfo(item.player);
            
        }
    }
}
