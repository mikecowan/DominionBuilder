using DominionBuilder.Data;
using DominionBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Domain
{
    public class MultiSetOps : SetOpsBase, IKingdomOperations
    {
        public MultiSetOps(List<Cards> allCards, DominionViewModel vm, List<int> sets, List<Requirement> requirements)
            : base(allCards, vm, sets, requirements)
        {

        }

        public void AddRequiredCards()
        {
            foreach (var req in _Requirements)
            {
                AddThisRequirement(req);
            }

            if (_vm.Kingdom.Count(x => x.Card.SetId == 1) == 6)
            {
                var test = 1;
            }
        }

        protected void AddThisRequirement(Requirement req)
        {
            if (req.Value.HasValue && req.Value.Value)
            {
                var cardSublist = _availableKingdomCards;
                if (_vm.Kingdom.Count(x => x.Card.SetId == _sets[0]) == 5)
                {
                    cardSublist = _availableKingdomCards.FindAll(x => x.SetId == _sets[1]);
                }
                else if (_vm.Kingdom.Count(x => x.Card.SetId == _sets[1]) == 5)
                {
                    cardSublist = _availableKingdomCards.FindAll(x => x.SetId == _sets[0]);
                }

                if (req.Table == Table.type)
                {
                    AddRandomCardFromList(cardSublist.Where(x => x.CardTypeLinks.Any(y => y.Type.Name == req.Name)).ToList());
                }
                else if (req.Table == Table.category)
                {
                    AddRandomCardFromList(cardSublist.Where(x => x.CardCategoryLinks.Any(y => y.Category.Name == req.Name)).ToList());
                }
            }
        }

        public void FillKingdom()
        {
            Random rand = new Random();
            var dist = GetCostDistributions();

            var sublistA = _availableKingdomCards.FindAll(x => x.SetId == _sets[0]);
            var sublistB = _availableKingdomCards.FindAll(x => x.SetId == _sets[1]);

            int modA = 0;
            int modB = 0;

            if (_vm.MouseCard != null && _vm.BaneCard != null)
            {
                if (_vm.MouseCard.SetId == _sets[0] && _vm.BaneCard.SetId == _sets[0])
                {
                    modA--;
                    modB++;
                }
                else if (_vm.MouseCard.SetId == _sets[1] && _vm.BaneCard.SetId == _sets[1])
                {
                    modA++;
                    modB--;
                }
            }

            while (_vm.Kingdom.Where(x => x.Card.SetId == _sets[0]).Count() < 5 + modA)
            {
                var next = rand.Next(sublistA.Count);
                var cardCost = _availableKingdomCards[next].Cost.Value;
                if (_vm.Kingdom.Count(x => x.Card.Cost == cardCost) < dist[cardCost])
                {
                    _vm.Kingdom.Add(new CardModel() { Card = sublistA[next] });
                    _availableKingdomCards.Remove(sublistA[next]);
                    sublistA.RemoveAt(next);
                }
                else
                {
                    var test = 1;
                }
            }

            while (_vm.Kingdom.Where(x => x.Card.SetId == _sets[1]).Count() < 5 + modB)
            {
                if (_vm.Kingdom.Count == 10)
                {
                    var test2 = 2;
                }

                var next = rand.Next(sublistB.Count);
                var cardCost = _availableKingdomCards[next].Cost.Value;
                if (_vm.Kingdom.Count(x => x.Card.Cost == cardCost) < dist[cardCost])
                {
                    _vm.Kingdom.Add(new CardModel() { Card = sublistB[next] });
                    _availableKingdomCards.Remove(sublistB[next]);
                    sublistB.RemoveAt(next);
                }
                else
                {
                    var test = 1;
                }
            }

        }

    }
}
