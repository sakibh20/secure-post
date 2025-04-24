
from typing import Dict, List

from fastapi import WebSocket

from services.models import Game

active_websockets: Dict[str, WebSocket] = {}
lobby_users: List[str] = []
active_lobby_sockets: List[WebSocket] = []
game_data: Dict[str, Game] = {}
