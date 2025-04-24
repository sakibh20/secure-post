public class LobbyWebSocketClient : WebSocketHandler
{
    public static LobbyWebSocketClient Instance;
    
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
    
    protected override void Subscribe()
    {
        AuthUIManager.Instance.OnLoginSuccess += OnLoginSuccess;
    }

    protected override void UnSubscribe()
    {
        AuthUIManager.Instance.OnLoginSuccess -= OnLoginSuccess;
    }

    private void OnLoginSuccess()
    {
        StartSocketConnection(ServerDataManager.Instance.GetLobbyUrl());
    }
}