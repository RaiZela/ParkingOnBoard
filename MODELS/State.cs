using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingOnBoard.MODELS
{
    public class State : BaseEntity
    {
        public string Name { get; set; }
        public List<City> Cities { get; set; }
    }
}
