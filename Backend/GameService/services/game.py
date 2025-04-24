from services.commands import request_match_command
from services.common import notify_user
from services.models import MatchCreateRequest


async def match_request_notifier(payload: MatchCreateRequest):
    await notify_user(payload.Player2, request_match_command({
        "status": payload.MatchStatus,
        "requested_by": payload.Player1,
        "match_id": payload.MatchId,
    }))
