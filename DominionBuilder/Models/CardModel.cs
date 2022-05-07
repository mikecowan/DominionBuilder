using DominionBuilder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Models
{
    public class CardModel
    {
        public Cards Card { get; set; }
        public Cards SplitPile { get; set; }

        public List<Cards> Split { get; set; }
        public string TypeString { get; set; }
        public string CategoryString { get; set; }
    }
}
