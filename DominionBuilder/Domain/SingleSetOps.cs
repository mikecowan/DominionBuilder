using DominionBuilder.Data;
using DominionBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Domain
{
    public class SingleSetOps : SetOpsBase, IKingdomOperations
    {
        public SingleSetOps(List<Cards> allCards, DominionViewModel vm, List<int> sets, List<Requirement> requirements)
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
                if (req.Table == Table.type)
                {
                    AddRandomCardFromList(_availableKingdomCards.Where(x => x.CardTypeLinks.Any(y => y.Type.Name == req.Name)).ToList());
                }
                else if (req.Table == Table.category)
                {
                    AddRandomCardFromList(_availableKingdomCards.Where(x => x.CardCategoryLinks.Any(y => y.Category.Name == req.Name)).ToList());
                }
            }
        }

        public void FillKingdom()
        {
            var POTIONVALUE = 2;

            Random rand = new Random();
            var dist = GetCostDistributions();

            while (_vm.Kingdom.Where(x => x.Card.SetId == _sets[0]).Count() < 10)
            {
                var next = rand.Next(_availableKingdomCards.Count);
                var thisCard = _availableKingdomCards[next];
                var potioncost = thisCard.SpecialCosts.Any(x => x.Type.Name == "Potion") ? POTIONVALUE : 0;
                var debtcost = thisCard.SpecialCosts.FirstOrDefault(x => x.Type.Name == "Debt")?.Amount ?? 0;

                var cardCost = thisCard.Cost.Value + potioncost + debtcost;

                var numKingdomCardsWithCost = _vm.Kingdom.Count(x => x.Card.Cost == cardCost && !x.Card.SpecialCosts.Any())
                    + _vm.Kingdom.Count(x => x.Card.Cost == cardCost - POTIONVALUE && x.Card.SpecialCosts.Any(z => z.Type.Name == "Potion"))
                    + _vm.Kingdom.Count(x => x.Card.Cost == 0 && x.Card.SpecialCosts.Any(z => z.Type.Name == "Debt") && x.Card.SpecialCosts.FirstOrDefault().Amount == cardCost);

                if (numKingdomCardsWithCost < dist[cardCost])
                {
                    _vm.Kingdom.Add(new CardModel() { Card = _availableKingdomCards[next] });
                    _availableKingdomCards.RemoveAt(next);
                }
                else
                {
                    var test = 1;
                }
            }

        }



    }
}
