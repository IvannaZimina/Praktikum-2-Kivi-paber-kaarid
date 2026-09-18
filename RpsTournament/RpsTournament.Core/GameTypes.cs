namespace RpsTournament.Core
{
    // Represents the available moves in the game.
    public enum Move { Rock, Paper, Scissors }

    // Represents the possible outcomes of a single game round.
    public enum RoundResult { Win, Loss, Draw }

    // Stores data and statistics for a single round of the game.
    public struct GameRound
    {
        // The sequential number of the round
        public int Number { get; set; }

        // The move chosen by the player
        public Move PlayerMove { get; set; }

        // The move chosen by the computer
        public Move ComputerMove { get; set; }

        // The final outcome of this specific round
        public RoundResult Result { get; set; }
    }
}
