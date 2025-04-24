from pydantic import BaseModel


class DiceRollRequest(BaseModel):
    match_id: str
    user_id: str


class ClaimRequest(BaseModel):
    match_id: str
    user_id: str
    claim_value: int


class MatchCreateRequest(BaseModel):
    MatchId: str
    Player1: str
    Player2: str
    MatchStatus: str


class MatchAcceptRequest(BaseModel):
    MatchId: str
    Player1: str
    Player2: str
    FirstTurn: str
    MatchToken: str
