from services.models import Game, GameStatus


class GameManager:
    def __init__(self, game: Game):
        """Initialize a GameManager instance."""
        self.game: Game = game

    def is_game_over(self):
        """Check if the game is over."""
        if self.game.currentRound == self.game.totalRound and self.game.status == GameStatus.FINISHED:
            return True
        return False
