using UnityEngine;
using Random = UnityEngine.Random;

public class MockGameServer : MonoBehaviour
{
    public static MockGameServer Instance;

    private int _playerAScore = 0;
    private int _playerBScore = 0;
    private int _round = 0;
    private int _currentRealRollA = -1;
    private int _currentRealRollB = -1;
    private string _playerAClaim = "";
    private string _playerBClaim = "";
    private readonly int _maxScore = 3;

    public int MaxScore => _maxScore;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _playerAScore = 0;
        _playerBScore = 0;
        //UIManager.Instance.UpdateScore(0, 0);
        UIManager.Instance.UpdateRound(_round);
        
        NextRound();
    }
    
    public void NextRound()
    {
        _playerAClaim = "";
        _playerBClaim = "";

        _round += 1;

        UIManager.Instance.PrepUIForNextRound();
        UIManager.Instance.UpdateRound(_round);
    }

    public void PlayerARoll()
    {
        _currentRealRollA = Random.Range(1, 7);
        UIManager.Instance.UpdateRoll("A", _currentRealRollA);
        WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.RollPrefix}{_currentRealRollA}");
    }
    
    public void PlayerBRoll()
    {
        _currentRealRollB = Random.Range(1, 7);
        UIManager.Instance.UpdateRoll("B", _currentRealRollB);
    }

    public void ReceiveClaim(string player, string claimedValue)
    {
        if (player == "A")
        {
            _playerAClaim = claimedValue;
            WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ClaimPrefix}A:{claimedValue}");
        }
        else
        {
            _playerBClaim = claimedValue;
            WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ClaimPrefix}B:{claimedValue}");
        }
    }

    public void ReceiveDecision(string player, string decision)
    {
        string opponent = player == "A" ? "B" : "A";

        // Get claimed and actual roll of the opponent
        int opponentClaim = opponent == "A" ? int.Parse(_playerAClaim) : int.Parse(_playerBClaim);
        int opponentRoll = opponent == "A" ? _currentRealRollA : _currentRealRollB;

        bool wasHonest = opponentClaim == opponentRoll; // Was the claim true?

        string resultText = "";

        if (decision == NetworkConstants.Believe) // Opponent believed the claim
        {
            if (wasHonest) // The claim was truthful, claimer wins
            {
                resultText = $"{player} believed correctly. {player} +1";
                if (player == "A") _playerAScore++;
                else _playerBScore++;
            }
            else // The claim was false, but opponent believed it, so claimer wins
            {
                resultText = $"{player} believed incorrectly. {opponent} +1";
                if (opponent == "A") _playerAScore++;
                else _playerBScore++;
            }
        }
        else // Opponent called the bluff
        {
            if (wasHonest) // Opponent called bluff but the claim was truthful, so opponent loses
            {
                resultText = $"{player} called bluff incorrectly. {opponent} +1";
                if (opponent == "A") _playerAScore++;
                else _playerBScore++;
            }
            else // Opponent called bluff correctly, so opponent wins
            {
                resultText = $"{player} called bluff correctly. {player} +1";
                if (player == "A") _playerAScore++;
                else _playerBScore++;
            }
        }

        WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ResultPrefix}{resultText}");
        WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ScorePrefix}A:{_playerAScore}|B:{_playerBScore}");

        if (_playerAScore >= _maxScore || _playerBScore >= _maxScore)
        {
            string winText = _playerAScore >= _maxScore ? "Player A Wins!" : "Player B Wins!";
            WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ResultPrefix}{winText}");
            Debug.Log($"Game Over — {winText}");
            
            UIManager.Instance.GameOver();
        }
    }
}