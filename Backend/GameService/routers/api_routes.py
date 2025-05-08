import os
import random

from fastapi import APIRouter, HTTPException, Header

from services.common import notify_user
from services.game import match_request_notifier, create_game, handle_game_roll_dice, handle_game_claim_dice, \
    handle_game_round_decide
from services.models import DiceRollRequest, ClaimRequest, MatchCreateRequest, MatchAcceptRequest, DecideRequest

api_router = APIRouter()
WEBSECRET_KEY = os.getenv("WEBSECRET_KEY", "default_websecret_key")


@api_router.post("/roll-dice")
async def roll_dice_api(payload: DiceRollRequest):
    value = random.randint(1, 6)
    await handle_game_roll_dice(payload.user_id, payload.match_id, value, payload.token)
    return {
        "match_id": payload.match_id,
        "user_id": payload.user_id,
        "dice_roll": value
    }


@api_router.post("/claim")
async def claim_dice_api(payload: ClaimRequest):
    await handle_game_claim_dice(payload.user_id, payload.match_id, payload.claim_value, payload.token)
    return {
        "detail": "Claim processed",
    }

@api_router.post("/decide")
async def decide_dice_api(payload: DecideRequest):
    await handle_game_round_decide(payload.user_id, payload.match_id, payload.decision, payload.token)
    return {
        "detail": "Decision processed",
    }

@api_router.post("/match/request")
async def match_request_api(payload: MatchCreateRequest, websecret: str = Header(...)):
    if websecret != WEBSECRET_KEY:
        raise HTTPException(status_code=403, detail="Invalid websecret key")
    await match_request_notifier(payload)
    return {
        "detail": "Match request sent",
    }


@api_router.post("/match/accept")
async def match_accept_api(payload: MatchAcceptRequest, websecret: str = Header(...)):
    if websecret != WEBSECRET_KEY:
        raise HTTPException(status_code=403, detail="Invalid websecret key")
    await create_game(payload)
    return {
        "detail": "Match accepted",
    }