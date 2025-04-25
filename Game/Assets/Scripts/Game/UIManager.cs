using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public TextMeshProUGUI opponentScoreText;
    public TextMeshProUGUI playerBScoreText;
    public TextMeshProUGUI finalResultText;

    [SerializeField] private List<Sprite> dices;
    [SerializeField] private Sprite emptyDices;
    [SerializeField] private Image rolledImage;

    [Header("Buttons")]
    public Button playerARollBtn;
    public List<Button> playerAClaimBtns;
    public Button playerABelieveBtn;
    public Button playerABluffBtn;
    //
    // public Button playerBRollBtn;
    // public Button playerBClaimBtn;
    // public Button playerBBelieveBtn;
    // public Button playerBBluffBtn;

    [Space] 
    public GameObject gameView;
    public GameObject winnerView;
    public GameObject waiting;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // BindButtons();
        // InitButtons();
        // InitTexts();
        
        InitNewGame();
    }

    private void InitNewGame()
    {
        BindButtons();
        InitButtons();
        InitTexts();
    }

    public void ShowGameView()
    {
        InitNewGame();
        gameView.SetActive(true);
        ShowWaitingPanel();
    }

    public void ShowWaitingPanel()
    {
        waiting.SetActive(true);
    }

    public void HideWaitingPanel()
    {
        roundText.text = $"Round: {ServerDataManager.Instance.wsUserMessage.payload.current_round}/{ServerDataManager.Instance.wsUserMessage.payload.total_round}";
        
        waiting.SetActive(false);
    }
    
    public void HideGameView()
    {
        gameView.SetActive(false);
    }

    private void BindButtons()
    {
        playerARollBtn.onClick.AddListener(OnPlayerARoll);
        foreach (Button claimBtn in playerAClaimBtns)
        {
            claimBtn.onClick.AddListener(OnPlayerAClaim);
        }
        playerABelieveBtn.onClick.AddListener(OnPlayerABelieve);
        playerABluffBtn.onClick.AddListener(OnPlayerACallBluff);
        //
        // playerBRollBtn.onClick.AddListener(OnPlayerBRoll);
        // playerBClaimBtn.onClick.AddListener(OnPlayerBClaim);
        // playerBBelieveBtn.onClick.AddListener(OnPlayerBBelieve);
        // playerBBluffBtn.onClick.AddListener(OnPlayerBCallBluff);
    }

    private void InitButtons()
    {
        ActivePlayerARollBtn(false);
        ActivePlayerAClaimBtn(false);
        ActivePlayerABelieveBtn(false);
        ActivePlayerABluffBtn(false);
        
        // ActivePlayerBRollBtn(false);
        // ActivePlayerBClaimBtn(false);
        // ActivePlayerBBelieveBtn(false);
        // ActivePlayerBBluffBtn(false);
    }

    private void InitTexts()
    {
        playerARollText.text = "";
        rolledImage.sprite = emptyDices;
        playerAClaimText.text = "";

        playerBRollText.text = "";
        playerBClaimText.text = "";

        resultText.text = "";
    }

    public void PlayerARoll()
    {
        ActivePlayerARollBtn(true);
    }

    public void OnRoundOver()
    {
        int yourScore = 0;
        int opponentScore = 0;
        yourScore = ServerDataManager.Instance.wsMatchMessage.payload.player1 ==
                ServerDataManager.Instance.serverResponse.Result.userID ? ServerDataManager.Instance.wsMatchMessage.payload.player1_score : ServerDataManager.Instance.wsMatchMessage.payload.player2_score;        
        
        opponentScore = ServerDataManager.Instance.wsMatchMessage.payload.player1 ==
                        ServerDataManager.Instance.serverResponse.Result.userID ? ServerDataManager.Instance.wsMatchMessage.payload.player2_score : ServerDataManager.Instance.wsMatchMessage.payload.player1_score;
        
        roundText.text = $"Round: {ServerDataManager.Instance.wsMatchMessage.payload.current_round}/{ServerDataManager.Instance.wsMatchMessage.payload.total_round}";
        playerAScoreText.text = $"Your Score: {yourScore}";
        opponentScoreText.text = $"Opponent's Score: {opponentScore}";

        resultText.text = "";
        playerARollText.text = "";
        rolledImage.sprite = emptyDices;
    }

    public void OnGameOver(string winner)
    {
        HideGameView();
        winnerView.SetActive(true);
        finalResultText.text = $"{winner} own the match!";
    }    
    
    public void GameOver()
    {
        InitButtons();
        
        ActivePlayerARollBtn(false);
        
        Invoke(nameof(RestartGame), 2);
    }

    private void RestartGame()
    {
        UpdateResult("Restarting in 3 Seconds ..");
        Invoke(nameof(ReLoadScene), 3f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReLoadScene();
        }
    }

    private void ReLoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnPlayerARoll()
    {
        //MockGameServer.Instance.PlayerARoll();
        ActivePlayerARollBtn(false);
        ActivePlayerAClaimBtn(true);
    }
    private void OnPlayerAClaim()
    {
        //WebSocketClient.Instance.Mock_PlayerAClaim();
        ActivePlayerAClaimBtn(false);
        // ActivePlayerBBelieveBtn(true);
        // ActivePlayerBBluffBtn(true);
    }
    private void OnPlayerABelieve()
    {
        //WebSocketClient.Instance.Mock_PlayerABelieves();
        ActivePlayerABelieveBtn(false);
        ActivePlayerABluffBtn(false);

        OnNextRoundButtonPressed();
    }
    private void OnPlayerACallBluff()
    {
        //WebSocketClient.Instance.Mock_PlayerACallsBluff();
        ActivePlayerABelieveBtn(false);
        ActivePlayerABluffBtn(false);

        OnNextRoundButtonPressed();
    }
    
    // private void OnPlayerBRoll()
    // {
    //     MockGameServer.Instance.PlayerBRoll();
    //     ActivePlayerBClaimBtn(true);
    //     ActivePlayerBRollBtn(false);
    // }
    public void OnPlayerBClaim(int claim)
    {
        //WebSocketClient.Instance.Mock_PlayerBClaim();
        //ActivePlayerBClaimBtn(false);
        resultText.text = $"Opponent claimed {claim}. Now you decide !";
        ActivePlayerABelieveBtn(true);
        ActivePlayerABluffBtn(true);
    }
    // private void OnPlayerBBelieve()
    // {
    //     WebSocketClient.Instance.Mock_PlayerBBelieves();
    //     ActivePlayerBRollBtn(true);
    //     ActivePlayerBBelieveBtn(false);
    //     ActivePlayerBBluffBtn(false);
    // }
    // private void OnPlayerBCallBluff()
    // {
    //     WebSocketClient.Instance.Mock_PlayerBBluff();
    //     ActivePlayerBRollBtn(true);
    //     ActivePlayerBBelieveBtn(false);
    //     ActivePlayerBBluffBtn(false);
    // }

    private void ActivePlayerARollBtn(bool value)
    {
        playerARollBtn.interactable = value;
    }
    
    private void ActivePlayerAClaimBtn(bool value)
    {
        foreach (Button claimBtn in playerAClaimBtns)
        {
            claimBtn.interactable = value;
        }
    }
    
    private void ActivePlayerABelieveBtn(bool value)
    {
        playerABelieveBtn.interactable = value;
    }
    
    private void ActivePlayerABluffBtn(bool value)
    {
        playerABluffBtn.interactable = value;
    }
    
    // private void ActivePlayerBRollBtn(bool value)
    // {
    //     playerBRollBtn.interactable = value;
    // }
    //
    // private void ActivePlayerBClaimBtn(bool value)
    // {
    //     playerBClaimBtn.interactable = value;
    // }
    //
    // private void ActivePlayerBBelieveBtn(bool value)
    // {
    //     playerBBelieveBtn.interactable = value;
    // }
    //
    // private void ActivePlayerBBluffBtn(bool value)
    // {
    //     playerBBluffBtn.interactable = value;
    // }    
    
    private void OnNextRoundButtonPressed()
    {
        //MockGameServer.Instance.NextRound();

        ClearTexts();
    }

    private void ClearTexts()
    {
        playerAClaimText.SetText("");
        playerBClaimText.SetText("");
    }
    
    private void UpdateResult(string msg)
    {
        resultText.text = $"{msg}";
    }
    
    public void UpdateRoll(int value)
    {
        playerARollText.text = value.ToString();
        rolledImage.sprite = dices[value - 1];
    }    
    
    public void UpdateRoll(string player, int value)
    {
        if (player == "A")
            playerARollText.text = value.ToString();
        else if (player == "B")
            playerBRollText.text = value.ToString();
    }

    public void ShowClaim(string player, string claimedValue)
    {
        if (player == "A")
            playerAClaimText.text = claimedValue;
        else if (player == "B")
            playerBClaimText.text = claimedValue;
    }

    public void ShowResult(string result)
    {
        resultText.text = $"Result: {result}";
    }

    // public void UpdateScore(int scoreA, int scoreB)
    // {
    //     int maxScore = MockGameServer.Instance.MaxScore;
    //     playerAScoreText.text = $"Score: {scoreA}/{maxScore}";
    //     playerBScoreText.text = $"Score: {scoreB}/{maxScore}";
    // }

    public void UpdateRound(int round)
    {
        roundText.text = $"Round: {round}";
    }

    public void PrepUIForNextRound()
    {
        InitButtons();
        InitTexts();
    }
}