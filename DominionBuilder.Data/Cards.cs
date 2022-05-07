using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class Cards
    {
        public Cards()
        {
            CardCategoryLinks = new HashSet<CardCategoryLinks>();
            CardTypeLinks = new HashSet<CardTypeLinks>();
            ExtraCardsExtraCard = new HashSet<ExtraCards>();
            ExtraCardsKingdomCard = new HashSet<ExtraCards>();
            PeripheralLinks = new HashSet<PeripheralLinks>();
            SpecialCosts = new HashSet<SpecialCosts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SetId { get; set; }
        public int? Cost { get; set; }
        public bool Kingdom { get; set; }

        public virtual Sets Set { get; set; }
        public virtual ICollection<CardCategoryLinks> CardCategoryLinks { get; set; }
        public virtual ICollection<CardTypeLinks> CardTypeLinks { get; set; }
        public virtual ICollection<ExtraCards> ExtraCardsExtraCard { get; set; }
        public virtual ICollection<ExtraCards> ExtraCardsKingdomCard { get; set; }
        public virtual ICollection<PeripheralLinks> PeripheralLinks { get; set; }
        public virtual ICollection<SpecialCosts> SpecialCosts { get; set; }
    }
}
