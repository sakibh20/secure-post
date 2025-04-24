using System;
using NativeWebSocket;
using UnityEngine;

public class WebSocketHandler : MonoBehaviour
{
    //public static WebSocketHandler Instance;
    protected WebSocket _websocket;
    
    public delegate void OnMessageReceived(string message);
    public event OnMessageReceived MessageReceived;

    public event Action OnClosedCallback;
    
    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        Subscribe();
    }

    protected virtual void OnDestroy()
    {
        UnSubscribe();
    }
    
    protected virtual void Subscribe()
    {
        
    }

    protected virtual void UnSubscribe()
    {
        
    }

    protected async void StartSocketConnection(string url)
    {
        OnClosedCallback = null;
        
        _websocket = new WebSocket(url);

        _websocket.OnOpen += () => Debug.Log("WebSocket opened.");
        _websocket.OnError += (e) => Debug.LogError($"WebSocket error: {e}");
        _websocket.OnClose += (e) =>
        {
            OnClosedCallback?.Invoke();
            Debug.Log("WebSocket closed.");
        };
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

    public async void CloseConnection(Action onClosed = null)
    {
        OnClosedCallback = onClosed;
        await _websocket.Close();
    }

    private async void OnApplicationQuit()
    {
        if (_websocket != null)
        {
            CloseConnection();   
        }
    }
}
