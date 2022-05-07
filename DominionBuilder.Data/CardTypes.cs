using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class CardTypes
    {
        public CardTypes()
        {
            CardTypeLinks = new HashSet<CardTypeLinks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CardTypeLinks> CardTypeLinks { get; set; }
    }
}
