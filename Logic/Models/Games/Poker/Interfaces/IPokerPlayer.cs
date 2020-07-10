namespace EtherBetClientLib.Models.Games.Poker.Interfaces
{
    public interface IPokerPlayer
    {
        public PokerTable CurrentTable { get; set; }
        public PokerRound CurrentRound { get; set; }
        public int CurrentChipAmount { get; set; }
        public int LeftChipsAfterBet { get; set; }
        public int CurrentBetAmount { get; set; }
    }
}
