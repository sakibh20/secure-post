using UnityEngine;

public class LeaderboardUIManager : ConnectionUIManager
{
    [SerializeField] private RESTAPIManager restapiManager;
    
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private LeaderboardPlayerItem leaderboardPlayerItem;
    [SerializeField] private Transform leaderboardItemParent;
    
    public void ShowLeaderboardPanel()
    {
        InitLeaderboardPanel();
        GetLeaderboardData();
        
        leaderboardPanel.SetActive(true);
    }
    
    public void HideLeaderboardPanel()
    {
        leaderboardPanel.SetActive(false);
    }
    
    private void InitLeaderboardPanel()
    {
        foreach (Transform child in leaderboardItemParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateLeaderboardItems()
    {
        int count = 0;
        foreach (LeaderBoardItem item in ServerDataManager.Instance.leaderboardResponse.Result)
        {
            LeaderboardPlayerItem instantiatedItem = Instantiate(leaderboardPlayerItem, leaderboardItemParent);
            instantiatedItem.InitInfo((count+1).ToString(), item.player, item.wins);
            
            count += 1;
            if(count == 10) return;
        }
    }

    private void GetLeaderboardData()
    {
        ShowLoadingPanel();
        restapiManager.GetLeaderboard(OnSuccess, OnFail);
    }
    
    private void OnSuccess(string message)
    {
        HideLoadingPanel();

        GenerateLeaderboardItems();
    }
    
    private void OnFail(string message)
    {
        HideLoadingPanel();
        ShowPopupPanel(message);
    }
}
