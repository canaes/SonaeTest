using SonaeTestSol.Domain.Entities.Base;
using SonaeTestSol.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SonaeTestSol.Domain.Enumerators.Enumerators;

namespace SonaeTestSol.Domain.Entities
{
    public class Order: EntityBase, IEntity
    {
        public int Quantity { get; set; }
        public DateTime ExpiresOn { get; set; }
        public StatusOrder Status { get; set; }
    }
}
