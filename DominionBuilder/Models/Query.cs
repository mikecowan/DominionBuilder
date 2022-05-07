using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Models
{
    public class Query
    {
        public int CostMin { get; set; }
        public int CostMax { get; set; }
        public string SearchText { get; set; }
        public bool AndSelect { get; set; }
        public List<int> Types { get; set; }
        public List<int> Categories { get; set; }
        public List<int> Sets { get; set; }
    }
}
