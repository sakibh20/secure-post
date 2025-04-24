from services.commands import request_match_command
from services.common import notify_user
from services.data import game_data
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
            currentTurn=payload.Player1,
            currentProcess=GameProcess.ROLLING,
        )
    )
    game_data[payload.MatchId] = game
