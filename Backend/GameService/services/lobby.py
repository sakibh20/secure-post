from typing import Dict, List
from fastapi import WebSocket, WebSocketDisconnect

active_websockets: Dict[str, WebSocket] = {}
lobby_users: List[str] = []
active_lobby_sockets: List[WebSocket] = []


async def handle_lobby_socket(websocket: WebSocket):
    await websocket.accept()
    active_lobby_sockets.append(websocket)

    try:
        await websocket.send_json({
            "event": "lobby_snapshot",
            "users": lobby_users
        })

        while True:
            await websocket.receive_text()
    except WebSocketDisconnect:
        if websocket in active_lobby_sockets:
            active_lobby_sockets.remove(websocket)
        print("Lobby WebSocket client disconnected.")


async def handle_user_socket(websocket: WebSocket, user_id: str):
    await websocket.accept()
    active_websockets[user_id] = websocket

    if user_id not in lobby_users:
        lobby_users.append(user_id)
        await broadcast_lobby_update()

    print(f"{user_id} connected to game socket and joined lobby.")

    try:
        while True:
            data = await websocket.receive_json()
            print(f"{user_id} says: {data}")
    except WebSocketDisconnect:
        print(f"{user_id} disconnected.")
        active_websockets.pop(user_id, None)
        if user_id in lobby_users:
            lobby_users.remove(user_id)
            await broadcast_lobby_update()


async def broadcast_lobby_update():
    message = {
        "event": "lobby_update",
        "users": lobby_users
    }

    disconnected = []
    for ws in active_lobby_sockets:
        try:
            await ws.send_json(message)
        except Exception as e:
            print(f"Failed to broadcast: {e}")
            disconnected.append(ws)

    for ws in disconnected:
        if ws in active_lobby_sockets:
            active_lobby_sockets.remove(ws)
