import random

from fastapi import APIRouter

from services.common import notify_user
from services.game import match_request_notifier
from services.models import DiceRollRequest, ClaimRequest, MatchCreateRequest, MatchAcceptRequest

api_router = APIRouter()


@api_router.post("/roll-dice")
async def roll_dice_api(payload: DiceRollRequest):
    value = random.randint(1, 6)
    print(f"User {payload.user_id} rolled a {value} in match {payload.match_id}")
    await notify_user(payload.user_id)
    return {
        "match_id": payload.match_id,
        "user_id": payload.user_id,
        "dice_roll": value
    }


@api_router.post("/claim")
async def claim_dice_api(payload: ClaimRequest):
    pass


@api_router.post("/match/request")
async def match_request_api(payload: MatchCreateRequest):
    await match_request_notifier(payload)
    return {
        "detail": "Match request sent",
    }


@api_router.post("/match/accept")
async def match_accept_api(payload: MatchAcceptRequest):
    # await create_match()
    pass