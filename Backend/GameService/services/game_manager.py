from services.models import Game, GameStatus, GameProcess, GameState


# noinspection PyMethodMayBeStatic
class GameManager:
    match_players = {}

    def __init__(self, game: Game):
        """Initialize a GameManager instance."""
        self.game: Game = game

    def is_game_over(self):
        """Check if the game is over."""
        if self.game.currentRound == self.game.totalRound and self.game.status == GameStatus.FINISHED:
            return True
        return False

    async def next_turn(self):
        """Advance to the next turn."""

        if self.is_game_over():
            raise ValueError("Game is already over.")

        game = self.game
        state = game.currentState

        if state.currentProcess == GameProcess.ROLLING:
            await self.handle_rolling(
                player=state.currentTurn,
            )
        elif state.currentProcess == GameProcess.CLAIMING:
            await self.handle_claiming(
                player=state.currentTurn,
            )
        elif state.currentProcess == GameProcess.DECIDE:
            await self.handle_decide(
                player=state.currentTurn,
            )
        elif state.currentProcess == GameProcess.ROUND_OVER:
            round_winner = None
            if bool(state.claim == state.roll) == state.decide:
                if state.currentTurn == self.game.player1:
                    self.game.player1Score += 1
                    round_winner = self.game.player1
                else:
                    self.game.player2Score += 1
                    round_winner = self.game.player2
            else:
                if state.currentTurn == self.game.player1:
                    self.game.player2Score += 1
                    round_winner = self.game.player2
                else:
                    self.game.player1Score += 1
                    round_winner = self.game.player1
            self.game.currentState.round_winner = round_winner
        else:
            raise ValueError("Invalid game process.")

        self.update_game_state()
        print(self.game.model_dump_json(indent=2))

    async def handle_rolling(self, player):
        """Handle the rolling process."""
        print(f"Handling rolling for player: {player}")
        await self.send_command(
            player,
            "roll_dice",
            {}
        )

    async def handle_claiming(self, player):
        print(f"Handling claiming for player: {player}")
        await self.send_command(
            player,
            "claim_dice",
            {}
        )

    async def handle_decide(self, player):
        await self.send_command(
            player,
            "decide",
            {}
        )

    async def send_command(self, player, command, payload):
        """Send a command to the game."""
        print(f"Sending command: {command}")
        await self.match_players[player].send_json({
            "command": command,
            "payload": payload
        })

    def notify_game_status(self, current_state):
        for player in [self.game.player1, self.game.player2]:
            self.send_command(player, "round_over", {
                "round_winner": current_state.round_winner,
                "round_status": "win" if current_state.round_winner == player else "lose",
                "game_status": self.game.status,
                "player1_score": self.game.player1Score,
                "player2_score": self.game.player2Score,
                "current_round": self.game.currentRound,
                "total_round": self.game.totalRound,
            })
    def update_game_state(self):
        """Update the game state based on the current process."""
        current_state = self.game.currentState.currentProcess

        if current_state == GameProcess.ROLLING:
            self.game.currentState.currentProcess = GameProcess.CLAIMING
        elif current_state == GameProcess.CLAIMING:
            self.game.currentState.currentProcess = GameProcess.DECIDE
            self.game.currentState.currentTurn = self.get_opponent()
        elif current_state == GameProcess.DECIDE:
            self.game.currentState.currentProcess = GameProcess.ROUND_OVER
        elif current_state == GameProcess.ROUND_OVER:
            current_state = self.game.currentState
            self.game.oldStates.append(current_state)
            self.game.currentState = GameState(
                currentTurn=self.get_opponent(),
                currentProcess=GameProcess.ROLLING,
            )
            self.game.currentRound += 1
            self.notify_game_status(current_state)


    def get_opponent(self):
        """Get the opponent of the current player."""
        if self.game.player1 == self.game.currentState.currentTurn:
            return self.game.player2
        else:
            return self.game.player1
