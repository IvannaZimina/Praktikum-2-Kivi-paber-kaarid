using System;

// organize namespace structure for business logic classes
namespace RpsTournament.Core
{
    // Contains the core business logic and rules for the Rock-Paper-Scissors game.
    public static class GameLogic
    {
        // Generates a random move for the computer.
        public static Move GetComputerMove()
        {
            // Get all available moves from the Move enum and pick one randomly
            Move[] moves = Enum.GetValues<Move>();
            return moves[Random.Shared.Next(moves.Length)];
        }

        // Determines the outcome of a single round by comparing player and computer moves.
        public static RoundResult GetResult(Move player, Move computer)
        {
            // If both choices are identical, the round is a draw
            if (player == computer) return RoundResult.Draw;

            // Use pattern matching to determine winning combinations for the player; everything else is a loss
            return (player, computer) switch
            {
                (Move.Rock, Move.Scissors) => RoundResult.Win,
                (Move.Paper, Move.Rock) => RoundResult.Win,
                (Move.Scissors, Move.Paper) => RoundResult.Win,
                _ => RoundResult.Loss
            };
        }
    }
}