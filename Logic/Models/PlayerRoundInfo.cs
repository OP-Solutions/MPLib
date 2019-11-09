using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPR.Models
{
    class PlayerRoundInfo
    {
        public Player Player { get; set; }
        public double CurrentBet { get; set; }
        public bool IsFold { get; set; }
        
    }
}
