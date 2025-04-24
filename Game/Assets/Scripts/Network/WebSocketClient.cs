using System.Collections;
using UnityEngine;
using NativeWebSocket;
using Random = UnityEngine.Random;

public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient Instance;
    private WebSocket _websocket;

    public delegate void OnMessageReceived(string message);
    public event OnMessageReceived MessageReceived;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }
    
    private void Subscribe()
    {
        AuthUIManager.Instance.OnLoginSuccess += JoinPlayer;
    }

    private void UnSubscribe()
    {
        AuthUIManager.Instance.OnLoginSuccess -= JoinPlayer;
    }

    private async void JoinPlayer()
    {
        //_websocket = new WebSocket(NetworkConstants.GetJoinUrl(ServerDataManager.Instance.serverResponse.Result.useID));
        _websocket = new WebSocket(ServerDataManager.Instance.GetLobbyUrl());

        _websocket.OnOpen += () => Debug.Log("WebSocket opened.");
        _websocket.OnError += (e) => Debug.LogError($"WebSocket error: {e}");
        _websocket.OnClose += (e) => Debug.Log("WebSocket closed.");
        _websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            MessageReceived?.Invoke(message);
        };

        await _websocket.Connect();
    }
    
    private void Update()
    {
        _websocket?.DispatchMessageQueue();
    }

    public async void Send(string message)
    {
        if (_websocket.State == WebSocketState.Open)
        {
            await _websocket.SendText(message);
            Debug.Log($"[WS Sent]: {message}");
        }
        else
        {
            Debug.LogWarning("WebSocket not connected. Cannot send.");
        }
    }

    private async void OnApplicationQuit()
    {
        await _websocket.Close();
    }

    [ContextMenu("Mock: Start Full Game Loop")]
    public void Mock_StartGame()
    {
        MockGameServer.Instance.StartGame();
    }

    [ContextMenu("Mock: Player A Claims")]
    public void Mock_PlayerAClaim()
    {
        int claim = Random.Range(1, 7);
        MockGameServer.Instance.ReceiveClaim("A", claim.ToString());
    }
    
    [ContextMenu("Mock: Player A Believes")]
    public void Mock_PlayerABelieves()
    {
        MockGameServer.Instance.ReceiveDecision("A", NetworkConstants.Believe);
    }

    [ContextMenu("Mock: Player A Calls Bluff")]
    public void Mock_PlayerACallsBluff()
    {
        MockGameServer.Instance.ReceiveDecision("A", NetworkConstants.Bluff);
    }
    
    [ContextMenu("Mock: Player B Claims")]
    public void Mock_PlayerBClaim()
    {
        int claim = Random.Range(1, 7);
        MockGameServer.Instance.ReceiveClaim("B", claim.ToString());
    }

    [ContextMenu("Mock: Player B Believes")]
    public void Mock_PlayerBBelieves()
    {
        MockGameServer.Instance.ReceiveDecision("B", NetworkConstants.Believe);
    }

    [ContextMenu("Mock: Player B Calls Bluff")]
    public void Mock_PlayerBBluff()
    {
        MockGameServer.Instance.ReceiveDecision("B", NetworkConstants.Bluff);
    }

    private IEnumerator MockReceiveMessage(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log($"[Mock Received]: {message}");
        MessageReceived?.Invoke(message);
    }
    
    public void Mock_SendToClient(string message)
    {
        StartCoroutine(MockReceiveMessage(message, 0.5f));
    }
}