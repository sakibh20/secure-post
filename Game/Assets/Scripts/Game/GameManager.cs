using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //WebSocketClient.Instance.MessageReceived += HandleServerMessage;
        
        //JoinWebSocketClient.Instance.MessageReceived += HandleServerMessage;
        //LobbyWebSocketClient.Instance.MessageReceived += HandleServerMessage;
    }

    private void OnDestroy()
    {
        //WebSocketClient.Instance.MessageReceived -= HandleServerMessage;
        
        //JoinWebSocketClient.Instance.MessageReceived -= HandleServerMessage;
        //LobbyWebSocketClient.Instance.MessageReceived -= HandleServerMessage;
    }

    private void HandleServerMessage(string message)
    {
        Debug.Log($"Server Message: {message}");

        if (message.StartsWith(NetworkConstants.RollPrefix))
        {
            string value = message.Substring(NetworkConstants.RollPrefix.Length);
            int realRoll = int.Parse(value);
            //PlayerController.Instance.ReceiveRoll(realRoll);
        }
        else if (message.StartsWith(NetworkConstants.ClaimPrefix))
        {
            // Format: ClaimPrefixA:4 or ClaimPrefixB:6
            string fullValue = message.Substring(NetworkConstants.ClaimPrefix.Length); // "A:4" or "B:6"
            string[] parts = fullValue.Split(':');
            if (parts.Length == 2)
            {
                string player = parts[0];
                string claimedValue = parts[1];
                UIManager.Instance.ShowClaim(player, claimedValue);
            }
        }
        else if (message.StartsWith(NetworkConstants.ResultPrefix))
        {
            string result = message.Substring(NetworkConstants.ResultPrefix.Length);
            UIManager.Instance.ShowResult(result);
        }
        else if (message.StartsWith(NetworkConstants.ScorePrefix))
        {
            string scoreString = message.Substring(NetworkConstants.ScorePrefix.Length); // A:2|B:1
            string[] parts = scoreString.Split('|');
            int scoreA = int.Parse(parts[0].Split(':')[1]);
            int scoreB = int.Parse(parts[1].Split(':')[1]);
            UIManager.Instance.UpdateScore(scoreA, scoreB);
        }
    }

    public void SendClaim(string player, int number)
    {
        WebSocketClient.Instance.Send($"{NetworkConstants.ClaimPrefix}{player}:{number}");
    }

    public void SendDecision(string player, string decision)
    {
        WebSocketClient.Instance.Send($"{NetworkConstants.DecisionPrefix}{player}:{decision}");
    }
}