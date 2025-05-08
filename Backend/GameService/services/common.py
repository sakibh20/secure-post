from fastapi import HTTPException

from services.data import active_websockets


async def notify_user(user_id, payload):
    disconnected = []
    ws = active_websockets.get(user_id)
    if ws is None:
        raise HTTPException(status_code=404, detail="User not found")
    try:
        await ws.send_json(payload)
    except Exception as e:
        disconnected.append(ws)

