using DominionBuilder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Models
{
    public class DominionViewModel
    {
        public CardModel DisplayCard { get; set; }
        public List<string> IncludedSetNames { get; set; }

        public List<CardModel> Kingdom { get; set; }
        public Cards BaneCard { get; set; }
        public Cards MouseCard { get; set; }

        public List<Cards> PortraitCards { get; set; }
        public List<Cards> ExtraCards { get; set; }
        public List<Cards> DruidCards { get; set; }

        public bool Boons { get; set; }
        public bool Hexes { get; set; }

        public List<Peripherals> Peripherals { get; set; }
        public List<Cards> Port { get; set; }


    }
}
