using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class CardTypeLinks
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int TypeId { get; set; }

        public virtual Cards Card { get; set; }
        public virtual CardTypes Type { get; set; }
    }
}
