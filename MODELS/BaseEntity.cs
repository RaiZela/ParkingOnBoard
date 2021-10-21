using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingOnBoard.MODELS
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            IsAvailable = true;
        }
        public int ID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime DeletionDate { get; set; }
    }
}
