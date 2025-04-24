
from typing import Dict, List

from fastapi import WebSocket

from services.game_manager import GameManager

active_websockets: Dict[str, WebSocket] = {}
lobby_users: List[str] = []
active_lobby_sockets: List[WebSocket] = []
game_data: Dict[str, GameManager] = {}
match_players: Dict[str, Dict] = {}