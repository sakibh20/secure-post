from enum import Enum
from typing import List

from pydantic import BaseModel


class DiceRollRequest(BaseModel):
    match_id: str
    user_id: str


class ClaimRequest(BaseModel):
    match_id: str
    user_id: str
    claim_value: int

class DecideRequest(BaseModel):
    match_id: str
    user_id: str
    decision: bool

class MatchCreateRequest(BaseModel):
    MatchId: str
    Player1: str
    Player2: str
    MatchStatus: str


class MatchAcceptRequest(BaseModel):
    MatchId: str
    Player1: str
    Player2: str
    Player1SecretKey: str
    Player2SecretKey: str
    FirstTurn: str
    MatchToken: str



######### GAME #########

class GameStatus(Enum):
    WAITING = "waiting"
    IN_PROGRESS = "in_progress"
    FINISHED = "finished"

class GameProcess(Enum):
    ROLLING = "rolling"
    CLAIMING = "claiming"
    DECIDE = "decide"
    ROUND_OVER = "round_over"

class GameState(BaseModel):
    currentTurn: str
    currentProcess: GameProcess
    roll: int | None = None
    claim: int | None = None
    decide: bool | None = None
    round_winner: str | None = None

class Game(BaseModel):
    matchId: str
    player1: str
    player2: str
    player1SecretKey: str
    player2SecretKey: str
    status: GameStatus
    player1Score: int = 0
    player2Score: int = 0
    totalRound: int = 7
    currentRound: int = 1
    matchToken: str
    currentState: GameState
    oldStates : List[GameState] = []
