
from typing import Dict, List
from fastapi import WebSocket



active_websockets: Dict[str, WebSocket] = {}
lobby_users: List[str] = []
active_lobby_sockets: List[WebSocket] = []

