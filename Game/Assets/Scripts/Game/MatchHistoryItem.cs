using System;
using TMPro;
using UnityEngine;

public class MatchHistoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI vsText;
    [SerializeField] private TextMeshProUGUI startTimeText;
    [SerializeField] private TextMeshProUGUI endTimeText;
    [SerializeField] private TextMeshProUGUI totalMoveText;

    private HistoryItem _historyItem;

    public void InitInfo(HistoryItem historyItem, int index)
    {
        _historyItem = historyItem;

        string result = _historyItem.winner == ServerDataManager.Instance.serverResponse.Result.userID ? "Won" : "Lost";
        string opponent = _historyItem.player1 == ServerDataManager.Instance.serverResponse.Result.userID ? _historyItem.player2 : _historyItem.player1;
        
        resultText.text = $"{index+1}. {result}";
        vsText.text =  $"vs <b>{opponent}</b>";
        startTimeText.text = $"Started At: {_historyItem.startTime}";
        endTimeText.text = $"Ended At: {_historyItem.endTime}";
        totalMoveText.text = $"Total Move: {Int32.Parse(_historyItem.player1Moves)+int.Parse(_historyItem.player2Moves)}";
    }
}
