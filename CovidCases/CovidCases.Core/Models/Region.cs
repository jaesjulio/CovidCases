using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidCases.Core.Models
{
    public class Region
    {
        public string Iso { get; set; }
        public string Name { get; set; }
        public string Province { get; set; }
        public List<object> Cities { get; set; }
    }
}
