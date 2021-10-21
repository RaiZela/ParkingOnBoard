using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingOnBoard.MODELS
{
    public class ParkingSlot : BaseEntity 
    {
        public bool IsValid { get; set; }
        public Street Street { get; set; } //Inverse Navigation
        public int PositionNumber { get; set; }
        public ParkingSlot()
        {
            IsValid = true;
        }
    }
}
