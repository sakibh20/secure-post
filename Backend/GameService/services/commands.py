from typing import Dict


def create_command(command: str, payload: Dict[str, str]):
    """
    Create a command object with the specified command and payload.
    """
    return {
        "command": command,
        "payload": payload
    }


def create_lobby_command(payload: Dict):
    """
    Create a lobby command object with the specified payload.
    """
    return create_command("lobby_update", payload)

def request_match_command(payload: Dict):
    """
    Create a match command object with the specified payload.
    """
    return create_command("match_request", payload)