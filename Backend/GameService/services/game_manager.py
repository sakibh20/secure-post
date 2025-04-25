from services.external_requests import update_match_result
from services.models import Game, GameStatus, GameProcess, GameState


# noinspection PyMethodMayBeStatic
class GameManager:
    match_players = {}

    def __init__(self, game: Game):
        """Initialize a GameManager instance."""
        self.game: Game = game

    def is_game_over(self):
        """Check if the game is over."""
        if (
                (self.game.currentRound > self.game.totalRound)
                or (max(self.game.player1Score, self.game.player2Score) > self.game.totalRound // 2)
        ):
            self.game.status = GameStatus.FINISHED
            return True
        return False

    async def next_turn(self):
        """Advance to the next turn."""

        if self.is_game_over():
            raise ValueError("Game is already over.")

        if self.game.status == GameStatus.WAITING:
            self.game.status = GameStatus.IN_PROGRESS
            await self.notify_match_start()

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

        await self.update_game_state()
        if self.is_game_over():
            await self.notify_game_over()
            update_match_result(self.game.matchToken, self.game.matchId, self.game.player1Score, self.game.player2Score,
                                self.get_winner())

        print(self.game.model_dump_json(indent=2))

        if self.game.CurrentProcess == GameProcess.ROLLING:
            await self.handle_rolling(self.game.currentState.currentTurn)

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
            {
                "claim": self.game.currentState.claim,
            }
        )

    async def send_command(self, player, command, payload):
        """Send a command to the game."""
        print(f"Sending command: {command}")
        await self.match_players[player].send_json({
            "command": command,
            "payload": payload
        })

    def get_winner(self):
        """Determine the winner of the game."""
        if self.game.player1Score > self.game.player2Score:
            winner = self.game.player1
        elif self.game.player1Score < self.game.player2Score:
            winner = self.game.player2
        else:
            winner = None
        return winner

    async def notify_game_over(self):
        for player in [self.game.player1, self.game.player2]:
            winner = self.get_winner()
            if winner is None:
                win_status = "draw"
            else:
                win_status = "win" if winner == player else "lose"
            await self.send_command(player, "game_over", {
                "status": str(self.game.status),
                "player1_score": self.game.player1Score,
                "player2_score": self.game.player2Score,
                "player1": self.game.player1,
                "player2": self.game.player2,
                "game_status": win_status,
                "current_round": self.game.currentRound,
                "total_round": self.game.totalRound,
            })

    async def notify_match_start(self):
        for player in [self.game.player1, self.game.player2]:
            await self.send_command(player, "match_start", {
                "status": str(self.game.status),
                "player1_score": self.game.player1Score,
                "player2_score": self.game.player2Score,
                "player1": self.game.player1,
                "player2": self.game.player2,
                "current_round": self.game.currentRound,
                "total_round": self.game.totalRound,
            })

    async def notify_round_over(self, current_state):
        for player in [self.game.player1, self.game.player2]:
            await self.send_command(player, "round_over", {
                "round_winner": current_state.round_winner,
                "round_status": "win" if current_state.round_winner == player else "lose",
                "status": str(self.game.status),
                "player1_score": self.game.player1Score,
                "player2_score": self.game.player2Score,
                "player1": self.game.player1,
                "player2": self.game.player2,
                "current_round": self.game.currentRound,
                "total_round": self.game.totalRound,
            })

    async def update_game_state(self):
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
            await self.notify_round_over(current_state)

    def get_opponent(self):
        """Get the opponent of the current player."""
        if self.game.player1 == self.game.currentState.currentTurn:
            return self.game.player2
        else:
            return self.game.player1
