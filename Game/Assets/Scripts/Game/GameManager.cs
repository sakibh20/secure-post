using UnityEngine;
using Random = UnityEngine.Random;

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
        
        MatchWebSocketClient.Instance.MessageReceived += OnMatchMessageReceived;
    }

    private void OnDestroy()
    {
        //WebSocketClient.Instance.MessageReceived -= HandleServerMessage;
        
        MatchWebSocketClient.Instance.MessageReceived -= OnMatchMessageReceived;
    }

    private void OnMatchMessageReceived(string message)
    {
        Debug.Log($"[MatchWebSocketClient] :: Server Message: {message}");
        
        if(string.IsNullOrWhiteSpace(message)) return;
        
        WSLobbyMessage data = JsonUtility.FromJson<WSLobbyMessage>(message);

        if (data.command == ServerDataManager.Instance.RollDiceCommand)
        {
            UIManager.Instance.PlayerARoll();
            
            // RESTAPIManager.Instance.RollDice(ServerDataManager.Instance.serverResponse.Result.userID, 
            //     ServerDataManager.Instance.wsUserMessage.payload.match_id, OnSuccessRoll, OnFailRoll);
        }
        else if (data.command == ServerDataManager.Instance.ClaimCommand)
        {
            UIManager.Instance.OnPlayerARoll();
            // RESTAPIManager.Instance.ClaimDice(ServerDataManager.Instance.serverResponse.Result.userID, 
            //     ServerDataManager.Instance.wsUserMessage.payload.match_id, Random.Range(1,7), OnSuccessClaim, OnFailClaim);
        }
        else if (data.command == ServerDataManager.Instance.DecisionCommand)
        {
            UIManager.Instance.OnPlayerBClaim();
            // RESTAPIManager.Instance.Decide(ServerDataManager.Instance.serverResponse.Result.userID, 
            //     ServerDataManager.Instance.wsUserMessage.payload.match_id, true, OnSuccessDecide, OnFailDecide);
        }
    }

    [ContextMenu("RollDice")]
    public void RollDice()
    {
        RESTAPIManager.Instance.RollDice(ServerDataManager.Instance.serverResponse.Result.userID, 
            ServerDataManager.Instance.wsUserMessage.payload.match_id, OnSuccessRoll, OnFailRoll);
    }

    [ContextMenu("Claim")]
    public void Claim(int value)
    {
        RESTAPIManager.Instance.ClaimDice(ServerDataManager.Instance.serverResponse.Result.userID, 
            ServerDataManager.Instance.wsUserMessage.payload.match_id, value, OnSuccessClaim, OnFailClaim);
    }    
    
    [ContextMenu("Decide")]
    public void Decide(bool value)
    {
        RESTAPIManager.Instance.Decide(ServerDataManager.Instance.serverResponse.Result.userID, 
            ServerDataManager.Instance.wsUserMessage.payload.match_id, value, OnSuccessDecide, OnFailDecide);
    }

    private void OnSuccessRoll(string message)
    {
        UIManager.Instance.UpdateRoll(ServerDataManager.Instance.diceRollResponse.dice_roll);
    }

    private void OnFailRoll(string message)
    {
        AuthUIManager.Instance.ShowPopupPanel(message);
    }
    
    private void OnSuccessClaim(string message)
    {
        
    }

    private void OnFailClaim(string message)
    {
        AuthUIManager.Instance.ShowPopupPanel(message);
    }
    
    private void OnSuccessDecide(string message)
    {
        
    }

    private void OnFailDecide(string message)
    {
        AuthUIManager.Instance.ShowPopupPanel(message);
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