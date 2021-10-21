using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingOnBoard.MODELS
{
    public class Street : BaseEntity 
    {
        public string Name { get; set; }
        public City City { get; set; } //Inverse Navigation
        public int SidesAvailable { get; set; }
        public List<ParkingSlot> ParkingSlots { get; set; }
        public Reasons Reason { get; set; }
    }
}
