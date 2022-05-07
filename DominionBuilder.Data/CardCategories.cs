using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class CardCategories
    {
        public CardCategories()
        {
            CardCategoryLinks = new HashSet<CardCategoryLinks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CardCategoryLinks> CardCategoryLinks { get; set; }
    }
}
