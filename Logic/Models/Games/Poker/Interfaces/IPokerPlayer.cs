using EtherBetClientLib.Core.Game.Poker.Logic;

namespace EtherBetClientLib.Models.Games.Poker.Interfaces
{
    public interface IPokerPlayer
    {
        PokerTable CurrentTable { get; set; }
        PokerRound CurrentRound { get; set; }
        int CurrentChipAmount { get; set; }
        int LeftChipsAfterBet { get; set; }
        int CurrentBetAmount { get; set; }
        bool IsFold { get; set; }
        bool IsTurn { get; set; }
        bool IsAllIn { get; set; }
        bool CheckFoldState { get; set; }
        bool CallAnyState { get; set; }
    }
}
