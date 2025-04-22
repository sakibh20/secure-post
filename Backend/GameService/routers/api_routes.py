import random

from fastapi import APIRouter, Body
from starlette.responses import JSONResponse

api_router = APIRouter()


@api_router.post("/roll-dice")
async def roll_dice(payload: dict = Body(...)):
    match_id = payload.get("match_id")
    user_id = payload.get("user_id")

    if not match_id or not user_id:
        return JSONResponse(status_code=400, content={
            "error": "match_id and user_id are required"
        })

    value = random.randint(1, 6)
    print(f"User {user_id} rolled a {value} in match {match_id}")

    return {
        "match_id": match_id,
        "user_id": user_id,
        "dice_roll": value
    }
