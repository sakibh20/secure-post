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
        bool generatedMine = false;
        foreach (LeaderBoardItem item in ServerDataManager.Instance.leaderboardResponse.Result.topUsers)
        {
            LeaderboardPlayerItem instantiatedItem = Instantiate(leaderboardPlayerItem, leaderboardItemParent);
            instantiatedItem.InitInfo((count+1).ToString(), item.player, item.wins);

            if (item.player == ServerDataManager.Instance.serverResponse.Result.useID)
            {
                generatedMine = true;
            }
            
            count += 1;
            if(count == 10) return;
        }

        if (!generatedMine)
        {
            LeaderBoardItem me= ServerDataManager.Instance.leaderboardResponse.Result.user;
            LeaderboardPlayerItem instantiatedItem = Instantiate(leaderboardPlayerItem, leaderboardItemParent);
            instantiatedItem.InitInfo(me.position, me.player, me.wins);
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
