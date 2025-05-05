using TMPro;
using UnityEngine;

public class HistoryUIManager : ConnectionUIManager
{
    [SerializeField] private RESTAPIManager restapiManager;

    [SerializeField] private GameObject historyPanel;
    [SerializeField] private MatchHistoryItem matchHistoryItem;
    [SerializeField] private Transform historyItemParent;

    [SerializeField] private TextMeshProUGUI totalMatchText;
    [SerializeField] private TextMeshProUGUI totalWinText;
    [SerializeField] private TextMeshProUGUI winPText;
    
    public void ShowHistoryPanel()
    {
        InitHistoryPanel();
        GetHistoryData();
        
        historyPanel.SetActive(true);
    }
    
    public void HideHistoryPanel()
    {
        historyPanel.SetActive(false);
    }
    
    private void InitHistoryPanel()
    {
        totalMatchText.text = "";
        totalWinText.text = "";
        winPText.text = "";
        
        foreach (Transform child in historyItemParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void GenerateHistoryItems()
    {
        int totalMatchCount = ServerDataManager.Instance.historyResponse.Result.Count;
        int totalWinCount = 0;
        for (int i = 0; i < ServerDataManager.Instance.historyResponse.Result.Count; i++)
        {
            if (ServerDataManager.Instance.historyResponse.Result[i].winner ==
                ServerDataManager.Instance.serverResponse.Result.userID) totalWinCount += 1;
            
            if(i >= 10) continue;
            
            MatchHistoryItem instantiatedItem = Instantiate(matchHistoryItem, historyItemParent);
            instantiatedItem.InitInfo(ServerDataManager.Instance.historyResponse.Result[i], i);
        }
        
        totalMatchText.text = $"Total Played: {totalMatchCount}";
        totalWinText.text = $"Won: {totalWinCount}";
        winPText.text = $"Win %: {(totalWinCount*100.0f/totalMatchCount):F2}%";
    }

    private void GetHistoryData()
    {
        ShowLoadingPanel();
        restapiManager.GetHistory(OnSuccess, OnFail);
    }
    
    private void OnSuccess(string message)
    {
        HideLoadingPanel();

        GenerateHistoryItems();
    }
    
    private void OnFail(string message)
    {
        HideLoadingPanel();
        ShowPopupPanel(message);
    }
}
