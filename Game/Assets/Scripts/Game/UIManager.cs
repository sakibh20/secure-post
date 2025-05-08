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
    [SerializeField] private GameObject rolledAnimatedImage;

    [Header("Buttons")]
    public Button playerARollBtn;
    public List<Button> playerAClaimBtns;
    public Button playerABelieveBtn;
    public Button playerABluffBtn;

    [Space] 
    public GameObject gameView;
    public GameObject winnerView;
    public GameObject waiting;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
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
    }

    private void InitButtons()
    {
        ActivePlayerARollBtn(false);
        ActivePlayerAClaimBtn(false);
        ActivePlayerABelieveBtn(false);
        ActivePlayerABluffBtn(false);
    }

    private void InitTexts()
    {
        playerARollText.text = "";
        
        rolledAnimatedImage.gameObject.SetActive(false);
        rolledImage.sprite = emptyDices;
        rolledImage.gameObject.SetActive(true);
        
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
        rolledAnimatedImage.gameObject.SetActive(false);
        rolledImage.sprite = emptyDices;
        rolledImage.gameObject.SetActive(true);
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
        ActivePlayerARollBtn(false);
        ActivePlayerAClaimBtn(true);
        
        rolledAnimatedImage.gameObject.SetActive(false);
        rolledImage.gameObject.SetActive(true);
    }
    private void OnPlayerAClaim()
    {
        ActivePlayerAClaimBtn(false);
    }
    private void OnPlayerABelieve()
    {
        ActivePlayerABelieveBtn(false);
        ActivePlayerABluffBtn(false);

        OnNextRoundButtonPressed();
    }
    private void OnPlayerACallBluff()
    {
        ActivePlayerABelieveBtn(false);
        ActivePlayerABluffBtn(false);

        OnNextRoundButtonPressed();
    }
    
    public void OnPlayerBClaim(int claim)
    {
        resultText.text = $"Opponent claimed {claim}. Now you decide !";
        ActivePlayerABelieveBtn(true);
        ActivePlayerABluffBtn(true);
    }

    private void ActivePlayerARollBtn(bool value)
    {
        playerARollBtn.interactable = value;
        
        rolledAnimatedImage.gameObject.SetActive(true);
        rolledImage.gameObject.SetActive(false);
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
    
    private void OnNextRoundButtonPressed()
    {
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
        
        rolledAnimatedImage.gameObject.SetActive(false);
    }
}