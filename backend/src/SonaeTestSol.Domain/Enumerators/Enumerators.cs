using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SonaeTestSol.Domain.Enumerators
{
    public class Enumerators
    {
        public enum StatusOrder
        {
            [Display(Name = "Active")]
            Active = 0,

            [Display(Name = "Expired")]
            Expired = 1,

            [Display(Name = "Completed")]
            Completed = 2
        }

    }
}
