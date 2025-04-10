using System.Collections;
using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    public static WebSocketClient Instance;
    private WebSocket websocket;

    public delegate void OnMessageReceived(string message);
    public event OnMessageReceived MessageReceived;

    private async void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        websocket = new WebSocket(NetworkConstants.ServerUrl);

        websocket.OnOpen += () => Debug.Log("WebSocket opened.");
        websocket.OnError += (e) => Debug.LogError($"WebSocket error: {e}");
        websocket.OnClose += (e) => Debug.Log("WebSocket closed.");
        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            MessageReceived?.Invoke(message);
        };

        await websocket.Connect();
    }

    private void Update()
    {
        websocket?.DispatchMessageQueue();
    }

    public async void Send(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(message);
            Debug.Log($"[WS Sent]: {message}");
        }
        else
        {
            Debug.LogWarning("WebSocket not connected. Cannot send.");
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    // -------------------------------
    // 🔧 MOCK TOOLS (Accessible via Inspector)
    // -------------------------------

    [ContextMenu("Mock: Start Full Game Loop")]
    public void Mock_StartGame()
    {
        MockGameServer.Instance.StartGame();
    }

    // [ContextMenu("Mock: Player A Rolls")]
    // public void Mock_PlayerARoll()
    // {
    //     MockGameServer.Instance.PlayerARoll();
    // }

    [ContextMenu("Mock: Player A Claims")]
    public void Mock_PlayerAClaim()
    {
        int claim = Random.Range(1, 7); // Claiming a random number
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

    // [ContextMenu("Mock: Player B Rolls")]
    // public void Mock_PlayerBRoll()
    // {
    //     MockGameServer.Instance.PlayerBRoll();
    // }

    [ContextMenu("Mock: Player B Claims")]
    public void Mock_PlayerBClaim()
    {
        int claim = Random.Range(1, 7); // Claiming a random number
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