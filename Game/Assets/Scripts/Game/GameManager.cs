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
        MatchWebSocketClient.Instance.MessageReceived += OnMatchMessageReceived;
    }

    private void OnDestroy()
    {
        MatchWebSocketClient.Instance.MessageReceived -= OnMatchMessageReceived;
    }

    private void OnMatchMessageReceived(string message)
    {
        Debug.Log($"[MatchWebSocketClient] :: Server Message: {message}");
        
        if(string.IsNullOrWhiteSpace(message)) return;
        
        ServerDataManager.Instance.wsMatchMessage = JsonUtility.FromJson<WSLobbyMessage>(message);

        if (ServerDataManager.Instance.wsMatchMessage.command == ServerDataManager.Instance.RollDiceCommand)
        {
            UIManager.Instance.PlayerARoll();
        }
        else if (ServerDataManager.Instance.wsMatchMessage.command == ServerDataManager.Instance.ClaimCommand)
        {
            UIManager.Instance.OnPlayerARoll();
        }
        else if (ServerDataManager.Instance.wsMatchMessage.command == ServerDataManager.Instance.DecisionCommand)
        {
            UIManager.Instance.OnPlayerBClaim(ServerDataManager.Instance.wsMatchMessage.payload.claim);
        }
        else if (ServerDataManager.Instance.wsMatchMessage.command == ServerDataManager.Instance.MatchStartCommand)
        {
            UIManager.Instance.HideWaitingPanel();
        }
        else if (ServerDataManager.Instance.wsMatchMessage.command == ServerDataManager.Instance.RoundOverCommand)
        {
            UIManager.Instance.OnRoundOver();
        }
        else if (ServerDataManager.Instance.wsMatchMessage.command == ServerDataManager.Instance.GameOverCommandCommand)
        {
            var winner = ServerDataManager.Instance.wsMatchMessage.payload.player1_score == 4 ? ServerDataManager.Instance.wsMatchMessage.payload.player1 : ServerDataManager.Instance.wsMatchMessage.payload.player2;   
            
            UIManager.Instance.OnGameOver(winner);
            MatchWebSocketClient.Instance.CloseConnection();
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
}