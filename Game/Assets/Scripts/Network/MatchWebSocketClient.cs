public class MatchWebSocketClient : WebSocketHandler
{
    public static MatchWebSocketClient Instance;
    
    protected override void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // protected override void Start()
    // {
    //     base.Start();
    // }
    //
    // protected override void OnDestroy()
    // {
    //     base.OnDestroy();
    // }
    
    // protected override void Subscribe()
    // {
    //     AuthUIManager.Instance.OnLoginSuccess += OnLoginSuccess;
    // }
    //
    // protected override void UnSubscribe()
    // {
    //     AuthUIManager.Instance.OnLoginSuccess -= OnLoginSuccess;
    // }

    public void JoinMatch()
    {
        StartSocketConnection(ServerDataManager.Instance.GetJoinMatchUrl());
    }
}