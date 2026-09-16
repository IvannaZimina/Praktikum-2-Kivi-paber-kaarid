namespace RpsTournament.Core
{
    // Core sisaldab loogikat; enum-tüübid Move ja RoundResult ning struct-tüüp GameRound
    public enum Move { Rock, Paper, Scissors }

    public enum RoundResult { Win, Loss, Draw }

    public struct GameRound
    {
        public int Number { get; set; }
        public Move PlayerMove { get; set; }
        public Move ComputerMove { get; set; }
        public RoundResult Result { get; set; }
    }
}
