import requests

def update_match_result(token: str,  match_id: str, player1_moves: int, player2_moves: int, winner: str):
    url = "http://localhost:8080/api/UpdateMatchResult"
    token = "xxxx"  # Replace with your actual token

    payload = {
        "MatchId": "0f915ef7-8feb-4ba4-95de-976b41e67b6f",
        "Player1Moves": 5,
        "Player2Moves": 6,
        "Winner": "samith"
    }

    headers = {
        "Authorization": f"Bearer {token}",
        "Content-Type": "application/json"
    }
    try:

        response = requests.post(url, json=payload, headers=headers)

        print("Status Code:", response.status_code)
        print("Response Body:", response.text)
    except (Exception,):
        print("Error occurred while making the request")
