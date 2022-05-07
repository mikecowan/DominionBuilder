using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class PromoCards
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Cost { get; set; }
        public bool Kingdom { get; set; }
    }
}
