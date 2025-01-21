using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SonaeTestSol.Domain.Entities.Base
{
    public class EntityBase
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }

        public EntityBase()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.Now;
        }
    }
}
