using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class Sets
    {
        public Sets()
        {
            Cards = new HashSet<Cards>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Cards> Cards { get; set; }
    }
}
