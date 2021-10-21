using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingOnBoard.MODELS
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public List<Street> Streets { get; set; }
        public State State { get; set; } //Inverse Navigation
        
    }
}
