using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class MockGameServer : MonoBehaviour
{
    public static MockGameServer Instance;

    private int playerAScore = 0;
    private int playerBScore = 0;
    private int round = 0;
    private int currentRealRollA = -1;
    private int currentRealRollB = -1;
    private string playerAClaim = "";
    private string playerBClaim = "";
    private int maxScore = 3;

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
        playerAScore = 0;
        playerBScore = 0;
        round = 1;
        UIManager.Instance.UpdateScore(0, 0);
        UIManager.Instance.UpdateRound(round);
        NextRound();
    }

    public void NextRound()
    {
        Debug.Log($"Starting Round {round}");
        currentRealRollA = Random.Range(1, 7);
        currentRealRollB = Random.Range(1, 7);
        playerAClaim = "";
        playerBClaim = "";

        UIManager.Instance.UpdateRound(round);
        UIManager.Instance.UpdateRoll("A", currentRealRollA);
        UIManager.Instance.UpdateRoll("B", currentRealRollB);
        UIManager.Instance.UpdateResult("Waiting for both players to claim...");
    }

    public void ReceiveClaim(string player, string claimedValue)
    {
        if (player == "A")
        {
            playerAClaim = claimedValue;
            WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ClaimPrefix}A:{claimedValue}");
        }
        else
        {
            playerBClaim = claimedValue;
            WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ClaimPrefix}B:{claimedValue}");
        }

        if (!string.IsNullOrEmpty(playerAClaim) && !string.IsNullOrEmpty(playerBClaim))
        {
            UIManager.Instance.UpdateResult("Both players have claimed. Make your decisions.");
        }
    }

    public void ReceiveDecision(string player, string decision)
    {
        string opponent = player == "A" ? "B" : "A";

        // Get claimed and actual roll of the opponent
        int opponentClaim = opponent == "A" ? int.Parse(playerAClaim) : int.Parse(playerBClaim);
        int opponentRoll = opponent == "A" ? currentRealRollA : currentRealRollB;

        bool wasHonest = opponentClaim == opponentRoll; // Was the claim true?

        string resultText = "";
        string roundWinner = "";

        if (decision == NetworkConstants.Believe) // Opponent believed the claim
        {
            if (wasHonest) // The claim was truthful, claimer wins
            {
                resultText = $"{opponent} believed correctly. {opponent} +1";
                if (opponent == "A") playerAScore++;
                else playerBScore++;
                roundWinner = opponent;
            }
            else // The claim was false, but opponent believed it, so claimer wins
            {
                resultText = $"{opponent} believed incorrectly. {player} +1";
                if (player == "A") playerAScore++;
                else playerBScore++;
                roundWinner = player;
            }
        }
        else // Opponent called the bluff
        {
            if (wasHonest) // Opponent called bluff but the claim was truthful, so opponent loses
            {
                resultText = $"{opponent} called bluff incorrectly. {opponent} +1";
                if (opponent == "A") playerAScore++;
                else playerBScore++;
                roundWinner = opponent;
            }
            else // Opponent called bluff correctly, so opponent wins
            {
                resultText = $"{opponent} called bluff correctly. {opponent} +1";
                if (player == "A") playerAScore++;
                else playerBScore++;
                roundWinner = player;
            }
        }

        WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ResultPrefix}{resultText}");
        WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ScorePrefix}A:{playerAScore}|B:{playerBScore}");

        // Check for game win condition
        if (playerAScore >= maxScore || playerBScore >= maxScore)
        {
            string winText = playerAScore >= maxScore ? "Player A Wins!" : "Player B Wins!";
            WebSocketClient.Instance.Mock_SendToClient($"{NetworkConstants.ResultPrefix}{winText}");
            Debug.Log($"Game Over — {winText}");
        }
    }

    private IEnumerator DelayedNextRound()
    {
        yield return new WaitForSeconds(2f);
        NextRound();
    }

    public void PrintScore()
    {
        Debug.Log($"Score — A: {playerAScore} | B: {playerBScore}");
    }
}