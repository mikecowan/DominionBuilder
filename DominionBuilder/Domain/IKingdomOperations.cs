using DominionBuilder.Data;
using DominionBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Domain
{
    public interface IKingdomOperations
    {
        void FillKingdom();
        void AddSpecials();
        void AddSplitPiles();
        void AddPeripherals();
        void AddExtraCards();
        void AddPortraits();
        void RemoveRestrictedCards();
        void AddRequiredCards();
        void AddAllies();
    }
}
