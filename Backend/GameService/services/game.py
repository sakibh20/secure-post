from starlette.websockets import WebSocket, WebSocketDisconnect

from services.commands import request_match_command
from services.common import notify_user
from services.data import game_data, match_players
from services.game_manager import GameManager
from services.models import MatchCreateRequest, MatchAcceptRequest, Game, GameStatus, GameState, GameProcess


async def match_request_notifier(payload: MatchCreateRequest):
    await notify_user(payload.Player2, request_match_command({
        "status": payload.MatchStatus,
        "requested_by": payload.Player1,
        "match_id": payload.MatchId,
    }))


async def create_game(payload: MatchAcceptRequest):
    game = Game(
        matchId=payload.MatchId,
        player1=payload.Player1,
        player2=payload.Player2,
        status=GameStatus.WAITING,
        matchToken=payload.MatchToken,
        currentState=GameState(
            currentTurn=payload.FirstTurn,
            currentProcess=GameProcess.ROLLING,
        )
    )

    game_manager = GameManager(
        game=game,
    )
    game_data[payload.MatchId] = game_manager


async def handle_match_join(websocket: WebSocket, user_id: str, match_id):
    await websocket.accept()
    players = match_players.get(match_id, {})

    if user_id in players:
        return
    players.update({
        f"{user_id}": websocket
    })
    match_players[match_id] = players


    print(f"{user_id} connected to game socket and joined lobby.")
    if len(players.keys()) == 2:
        game = game_data.get(match_id)
        if not game:
            raise ValueError("Game not found")
        game.match_players = players
        await game.next_turn()
        game_data[match_id] = game
    try:
        while True:
            data = await websocket.receive_json()
            print(f"{user_id} says: {data}")
    except WebSocketDisconnect:
        print(f"{user_id} disconnected.")
        #TODO: handle disconnection

        # active_websockets.pop(user_id, None)
        # if user_id in lobby_users:
        #     lobby_users.remove(user_id)
        #     await broadcast_lobby_update()

async def handle_game_roll_dice(user_id: str, match_id: str, roll: int):
    game_manager = game_data.get(match_id)
    if not game_manager:
        raise ValueError("Game not found")
    if user_id != game_manager.game.currentState.currentTurn:
        raise ValueError("Not your turn")

    game_manager.game.currentState.roll= roll
    await game_manager.next_turn()
    game_data[match_id] = game_manager