using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class ExtraCards
    {
        public int Id { get; set; }
        public int KingdomCardId { get; set; }
        public int ExtraCardId { get; set; }

        public virtual Cards ExtraCard { get; set; }
        public virtual Cards KingdomCard { get; set; }
    }
}
