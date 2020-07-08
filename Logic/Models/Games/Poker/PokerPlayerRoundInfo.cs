namespace EtherBetClientLib.Models
{
    class PokerPlayerRoundInfo
    {
        public Player Player { get; set; }
        public double CurrentBet { get; set; }
        public bool IsFold { get; set; }
    }
}
