using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI playerARollText;
    public TextMeshProUGUI playerBRollText;
    public TextMeshProUGUI playerAClaimText;
    public TextMeshProUGUI playerBClaimText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI playerAScoreText;
    public TextMeshProUGUI playerBScoreText;

    [Header("Buttons")]
    public Button playerARollBtn;
    public Button playerAClaimBtn;
    public Button playerABelieveBtn;
    public Button playerABluffBtn;

    public Button playerBRollBtn;
    public Button playerBClaimBtn;
    public Button playerBBelieveBtn;
    public Button playerBBluffBtn;
    
    public Button nextRoundBtn;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        BindButtons();
    }

    private void BindButtons()
    {
        playerARollBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerARoll());
        playerAClaimBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerAClaim());
        playerABelieveBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerABelieves());
        playerABluffBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerACallsBluff());

        playerBRollBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerBRoll());
        playerBClaimBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerBClaim());
        playerBBelieveBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerBBelieves());
        playerBBluffBtn.onClick.AddListener(() => WebSocketClient.Instance.Mock_PlayerBBluff());
        
        nextRoundBtn.onClick.AddListener(OnNextRoundButtonPressed);
    }

    private void OnNextRoundButtonPressed()
    {
        MockGameServer.Instance.NextRound();

        ClearTexts();
    }

    private void ClearTexts()
    {
        //playerARollText.SetText("");
        //playerBRollText.SetText("");
        playerAClaimText.SetText("");
        playerBClaimText.SetText("");
    }
    
    public void UpdateResult(string msg)
    {
        resultText.text = $"{msg}";
    }
    
    public void UpdateRoll(string player, int value)
    {
        if (player == "A")
            playerARollText.text = $"Player A Roll: {value}";
        else if (player == "B")
            playerBRollText.text = $"Player B Roll: {value}";
    }

    public void ShowClaim(string player, string claimedValue)
    {
        if (player == "A")
            playerAClaimText.text = $"Player A Claim: {claimedValue}";
        else if (player == "B")
            playerBClaimText.text = $"Player B Claim: {claimedValue}";
    }

    public void ShowResult(string result)
    {
        resultText.text = $"Result: {result}";
    }

    public void UpdateScore(int scoreA, int scoreB)
    {
        playerAScoreText.text = $"Score: {scoreA}";
        playerBScoreText.text = $"Score: {scoreB}";
    }

    public void UpdateRound(int round)
    {
        roundText.text = $"Round: {round}";
    }
}