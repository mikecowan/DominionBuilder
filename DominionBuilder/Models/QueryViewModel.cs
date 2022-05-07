using DominionBuilder.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Models
{
    public class QueryViewModel
    {
        public List<Item> Categories { get; set; }
        public List<Item> Types { get; set; }
        public List<CardModel> QueryResults { get; set; }
    }
}
