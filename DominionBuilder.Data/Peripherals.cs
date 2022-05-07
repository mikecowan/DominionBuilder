using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class Peripherals
    {
        public Peripherals()
        {
            PeripheralLinks = new HashSet<PeripheralLinks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PeripheralLinks> PeripheralLinks { get; set; }
    }
}
