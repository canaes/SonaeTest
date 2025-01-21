using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SonaeTestSol.Domain.Enumerators.Enumerators;

namespace SonaeTestSol.Domain.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiresOn { get; set; }
        public StatusOrder Status { get; set; }
    }
}
