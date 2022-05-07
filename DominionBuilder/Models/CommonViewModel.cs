using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Models
{
    public class CommonViewModel
    {
        public DominionViewModel dominionView { get; set; }
        public QueryViewModel queryView { get; set; }

        public List<string> ExpNames { get; set; }

    }
}
