using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Models
{
    //public class Requirements
    //{
    //    public bool? Village { get; set; }
    //    public bool? Treasure { get; set; }
    //    public bool? AltVp { get; set; }
    //    public bool? Buy { get; set; }
    //    public bool? Trasher { get; set; }
    //    public bool? Attack { get; set; }
    //    public bool? AtkCurse { get; set; }
    //    public bool? AtkHandsize { get; set; }
    //    public bool? AtkJunking { get; set; }

    //}

    public enum Table
    {
        category,
        type
    }

    public class Requirement
    {
        public string Name { get; set; }
        public bool? Value { get; set; }
        public Table Table { get; set; }
    }

}
