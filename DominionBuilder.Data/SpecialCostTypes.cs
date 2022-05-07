using System;
using System.Collections.Generic;

namespace DominionBuilder.Data
{
    public partial class SpecialCostTypes
    {
        public SpecialCostTypes()
        {
            SpecialCosts = new HashSet<SpecialCosts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SpecialCosts> SpecialCosts { get; set; }
    }
}
