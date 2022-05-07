using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class CardCategoryLinks
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int CategoryId { get; set; }

        public virtual Cards Card { get; set; }
        public virtual CardCategories Category { get; set; }
    }
}
