using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class PeripheralLinks
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int PeripheralId { get; set; }

        public virtual Cards Card { get; set; }
        public virtual Peripherals Peripheral { get; set; }
    }
}
