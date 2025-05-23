public class JoinWebSocketClient : WebSocketHandler
{
    public static JoinWebSocketClient Instance;
    
    protected override void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
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
        StartSocketConnection(ServerDataManager.Instance.GetJoinUrl());
    }
}