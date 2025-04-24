from services.models import Game, GameStatus, GameProcess


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
        else:
            raise ValueError("Invalid game process.")

    async def handle_rolling(self, player):
        """Handle the rolling process."""
        print(f"Handling rolling for player: {player}")
        await self.send_command(
            player,
            "roll_dice",
            {}
        )

    async def handle_claiming(self, player):
        pass

    async def handle_decide(self, player):
        pass

    async def send_command(self, player, command, payload):
        """Send a command to the game."""
        print(f"Sending command: {command}")
        await self.match_players[player].send_json({
            "command": command,
            "payload": payload
        })


