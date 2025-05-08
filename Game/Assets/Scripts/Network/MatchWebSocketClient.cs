using System;

public class MatchWebSocketClient : WebSocketHandler
{
    public static MatchWebSocketClient Instance;
    
    protected override void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void JoinMatch(Action onOpen, Action onClose)
    {
        StartSocketConnection(ServerDataManager.Instance.GetJoinMatchUrl(), onOpen, onClose);
    }
}