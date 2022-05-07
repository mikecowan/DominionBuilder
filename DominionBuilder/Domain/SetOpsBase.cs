using DominionBuilder.Data;
using DominionBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DominionBuilder.Domain
{
    public abstract class SetOpsBase
    {
        protected List<Cards> _allAvailableCards;
        protected List<Cards> _availableKingdomCards;
        protected List<Cards> _availablePortraitCards;
        //protected List<CardModel> _currentKingdom;
        protected List<int> _sets;
        protected List<Requirement> _Requirements;

        protected DominionViewModel _vm;

        private Dictionary<int, List<int>> _splitPiles;
        private Dictionary<int, int> _cardCount;

        public SetOpsBase(List<Cards> allCards, DominionViewModel vm, List<int> sets, List<Requirement> Requirements)
        {
            _cardCount = new Dictionary<int, int>()
            {
                { 0, 26 }, // 25
                { 1, 26 }, // 51
                { 2, 26 },
                { 3, 12 },
                { 4, 25 },
                { 5, 13 },
                { 6, 26 },
                { 7, 35 },
                { 8, 13 },
                { 9, 30 },
                { 10, 24 },
                { 11, 33 },
                { 12, 25 },
                { 13, 30 },
                { 14, 31 },
            };

            _sets = DetermineKingdomSets(sets);

            _allAvailableCards = allCards.Where(x => _sets.Contains(x.SetId)).ToList();
            _availableKingdomCards = _allAvailableCards.FindAll(x => x.Kingdom);

            var portraitTypeNames = new List<string> { "event", "landmark", "project", "way" };
            _availablePortraitCards = _allAvailableCards.FindAll(x => x.CardTypeLinks.Any(y => portraitTypeNames.Contains(y.Type.Name.ToLower())));

            _vm = vm;
            _Requirements = Requirements;

            _splitPiles = new Dictionary<int, List<int>>()
            {
                { 198, new List<int>() { 205, 206, 207, 208, 209, 210, 211, 212, 213, 214 } }, // knights
                { 215, new List<int>() { 216, 217, 218, 219 } }, // ruins

                { 301, new List<int>() { 302 } }, // encampment / plunder
                { 303, new List<int>() { 304 } }, // patrician / emporium
                { 305, new List<int>() { 306 } }, // settlers / bustling village
                { 308, new List<int>() { 309 } }, // catapult / rocks
                { 313, new List<int>() { 314 } }, // gladiator / fortune

                { 307, new List<int>() { 326, 327, 328, 329, 330, 331, 332, 333 } }, // castles

                { 591, new List<int>() { 592, 593, 594 } }, // augur
                { 595, new List<int>() { 596, 597, 598 } }, // clash
                { 599, new List<int>() { 600, 601, 602 } }, // fort
                { 603, new List<int>() { 604, 605, 606 } }, // odyssey
                { 607, new List<int>() { 608, 609, 610 } }, // townsfolk
                { 611, new List<int>() { 612, 613, 614 } }, // wizard
            };

        }

        public void RemoveRestrictedCards()
        {
            foreach (var req in _Requirements)
            {
                RemoveThisRestriction(req);
            }
        }

        private void RemoveThisRestriction(Requirement req)
        {
            if (req.Value.HasValue && !req.Value.Value)
            {
                if (req.Table == Table.type)
                {
                    _availableKingdomCards.RemoveAll(x => x.CardTypeLinks.Any(y => y.Type.Name == req.Name));
                }
                else if (req.Table == Table.category)
                {
                    _availableKingdomCards.RemoveAll(x => x.CardCategoryLinks.Any(y => y.Category.Name == req.Name));
                }
            }
        }

        protected void AddRandomCardFromList(List<Cards> list)
        {
            Random rand = new Random();

            if (list.Count > 0)
            {
                var next = rand.Next(list.Count);
                _vm.Kingdom.Add(new CardModel() { Card = list[next] });
                _availableKingdomCards.Remove(list[next]);
            }
        }

        public void AddSpecials()
        {
            Random rand = new Random();

            if (_vm.PortraitCards.Any(x => x.Name == "Way of the Mouse"))
            {
                var mousepossibles = _availableKingdomCards.FindAll(x => (x.Cost == 2 || x.Cost == 3) && !x.SpecialCosts.Any() && x.CardTypeLinks.Any(y => y.Type.Name == "Action"));
                var next = rand.Next(mousepossibles.Count);
                _vm.MouseCard = mousepossibles[next];
                _availableKingdomCards.Remove(_vm.MouseCard);
            }

            if (_availableKingdomCards.Any(x => x.Name == "Young Witch"))
            {
                var next = rand.Next(_availableKingdomCards.Count);
                if (next < 10)  // "10 out of N" chance of getting Young Witch
                {
                    var card = _availableKingdomCards.Find(x => x.Name == "Young Witch");
                    _vm.Kingdom.Add(new CardModel() { Card = card });
                    _availableKingdomCards.Remove(card);

                    var banepossibles = _availableKingdomCards.FindAll(x => (x.Cost == 2 || x.Cost == 3) && !x.SpecialCosts.Any() && x.Name != "Castles");
                    next = rand.Next(banepossibles.Count);
                    _vm.BaneCard = banepossibles[next];
                    _availableKingdomCards.Remove(_vm.BaneCard);
                }
                else
                {
                    _availableKingdomCards.RemoveAll(x => x.Name == "Young Witch");
                }
            }

        }

        public void AddPortraits()
        {
            Random rand = new Random();

            if (_availablePortraitCards.Any())
            {
                for (int i = 0; i < 2; i++)
                {
                    var next = rand.Next(_availablePortraitCards.Count());
                    _vm.PortraitCards.Add(_availablePortraitCards[next]);
                    _availablePortraitCards.RemoveAt(next);

                    if (_vm.PortraitCards[0].CardTypeLinks.Any(x => x.TypeId == Mapping.GetTypeId("Way")))
                    {
                        _availablePortraitCards.RemoveAll(x => x.CardTypeLinks.Any(y => y.TypeId == Mapping.GetTypeId("Way")));
                    }
                }
            }
        }

        public void AddAllies()
        {
            Random rand = new Random();

            if (_vm.Kingdom.Any(x => x.Card.CardTypeLinks.Any(y => y.Type.Name == "Liaison")))
            {
                var allies = _allAvailableCards.FindAll(x => x.CardTypeLinks.Any(y => y.Type.Name == "Ally"));
                var next = rand.Next(allies.Count);

                _vm.PortraitCards.Add(allies[next]);
            }
        }

        public void AddExtraCards()
        {
            var cardsToCheck = _vm.Kingdom.Select(x => x.Card).Union(_vm.PortraitCards).ToList();
            if (_vm.MouseCard != null)
            {
                cardsToCheck.Add(_vm.MouseCard);
            }
            if (_vm.BaneCard != null)
            {
                cardsToCheck.Add(_vm.BaneCard);
            }

            foreach (var c in cardsToCheck)
            {
                foreach (var extra in c.ExtraCardsKingdomCard)
                {
                    _vm.ExtraCards.Add(extra.ExtraCard);
                }
            }

            var extraRuins = new int[] { 215, 216, 217, 219 };
            _vm.ExtraCards.RemoveAll(x => extraRuins.Contains(x.Id));

            if (cardsToCheck.Exists(x => x.Name == "Druid"))
            {
                Random rand = new Random();
                var boons = _allAvailableCards.FindAll(x => x.CardTypeLinks.Any(y => y.TypeId == Mapping.GetTypeId("Boon")));

                while (_vm.DruidCards.Count < 3)
                {
                    var next = rand.Next(boons.Count);
                    _vm.DruidCards.Add(boons[next]);
                }
                // add 3 boons
            }

            _vm.Boons = cardsToCheck.Any(x => x.CardTypeLinks.Any(y => y.TypeId == Mapping.GetTypeId("Fate")));
            _vm.Hexes = cardsToCheck.Any(x => x.CardTypeLinks.Any(y => y.TypeId == Mapping.GetTypeId("Doom")));

            _vm.ExtraCards = _vm.ExtraCards.Distinct().ToList();
        }

        public void AddPeripherals()
        {
            var cardsToCheck = _vm.Kingdom.Select(x => x.Card).Union(_vm.PortraitCards).ToList();
            if (_vm.MouseCard != null)
            {
                cardsToCheck.Add(_vm.MouseCard);
            }
            if (_vm.BaneCard != null)
            {
                cardsToCheck.Add(_vm.BaneCard);
            }

            foreach (var c in cardsToCheck)
            {
                foreach (var pl in c.PeripheralLinks)
                {
                    _vm.Peripherals.Add(pl.Peripheral);
                }
            }

            _vm.Peripherals = _vm.Peripherals.Distinct().ToList();
        }

        public void AddSplitPiles()
        {
            foreach (var card in _vm.Kingdom)
            {
                if (_splitPiles.ContainsKey(card.Card.Id))
                {
                    var test = _splitPiles.GetValueOrDefault(card.Card.Id);
                    card.Split = new List<Cards>();
                    foreach (var id in _splitPiles.GetValueOrDefault(card.Card.Id))
                    {
                        card.Split.Add(_allAvailableCards.Find(x => x.Id == id));
                    }
                }
            }

        }

        private List<int> DetermineKingdomSets(List<int> Sets)
        {
            Random rand = new Random();
            var returnlist = new List<int>();
            SetTest();
            switch (Sets.Count)
            {
                case 0:
                    break;
                case 1:
                    returnlist.Add(Sets[0]);
                    break;
                default:

                    var largeIntList = new List<int>();
                    foreach (var integer in Sets)
                    {
                        for (int i = 0; i < _cardCount[integer]; i++)
                        {
                            largeIntList.Add(integer);
                        }
                    }

                    var next = rand.Next(largeIntList.Count);
                    var item = largeIntList[next];
                    returnlist.Add(item);
                    largeIntList.RemoveAll(x => x == item);
                    while (returnlist.Count == 1)
                    {
                        next = rand.Next(largeIntList.Count);
                        item = largeIntList[next];
                        returnlist.Add(item);
                        largeIntList.RemoveAll(x => x == item);
                    }
                    //var next = rand.Next(Sets.Count);
                    //returnlist.Add(Sets[next]);

                    //while (returnlist.Count == 1)
                    //{
                    //    next = rand.Next(Sets.Count);
                    //    if (!returnlist.Contains(Sets[next]))
                    //    {
                    //        returnlist.Add(Sets[next]);
                    //    }
                    //}

                    break;
            }

            return returnlist;
        }

        private int[] upperBounds = new int[15];

        private void SetTest()
        {
            upperBounds[0] = _cardCount[0] - 1;
            for (int i = 1; i < 15; i++)
            {
                upperBounds[i] = upperBounds[i - 1] + _cardCount[i];
            }
            Random rand = new Random();

            int[] tally = new int[15];

            for (int j = 0; j < 375000; j++)
            {

                // large list method
                //var largeIntList = new List<int>();
                //for (int integer = 0; integer < 15; integer++)
                //{
                //    for (int i = 0; i < _cardCount[integer]; i++)
                //    {
                //        largeIntList.Add(integer);
                //    }
                //}


                //var next = rand.Next(largeIntList.Count);
                //var item = largeIntList[next];
                //tally[item]++;
                //largeIntList.RemoveAll(x => x == item);

                //Random rand2 = new Random();

                //var next2 = rand2.Next(largeIntList.Count);
                //var item2 = largeIntList[next2];
                //tally[item2]++;
                //largeIntList.RemoveAll(x => x == item);

                // [0, 1] partition method
                var next = rand.Next(375);
                //e.g. 53-> 2
                // 25-> 0
                var index = 0;
                while (next > upperBounds[index])
                {
                    index++;
                }

                //var setnumA = GetSetFromMapping(next);
                tally[index]++;

                //next = rand.Next(375);
                //var setnumB = GetSetFromMapping(next);
                //while (setnumB == setnumA)
                //{
                //    next = rand.Next(375);
                //    setnumB = GetSetFromMapping(next);
                //}
                //tally[setnumB]++;
            }


        }

        private int GetSetFromMapping(int input)
        {
            int output = 0;

            for (int i = 0; i < 15; i++)
            {
                if (input > upperBounds[i])
                    output++;
                else
                    break;
            }

            return output;
        }

        protected int[] GetCostDistributions()
        {
            var distributions = new int[9]; // cards can cost 0, 1, 2, 3, ...
            var total = _availableKingdomCards.Count() + _vm.Kingdom.Count();
            var POTIONVALUE = 2;

            for (int i = 0; i < 9; i++)
            {
                // factor in potions & debt
                // 1 potion = $2
                // 1 debt = $1

                var nonspecials = _availableKingdomCards.Count(x => x.Cost == i && !x.SpecialCosts.Any()) + _vm.Kingdom.Count(x => x.Card.Cost == i && !x.Card.SpecialCosts.Any());
                var potions = _availableKingdomCards.Count(x => x.Cost == i - POTIONVALUE && x.SpecialCosts.Any(z => z.Type.Name == "Potion")) + _vm.Kingdom.Count(x => x.Card.Cost == i - POTIONVALUE && x.Card.SpecialCosts.Any(z => z.Type.Name == "Potion"));
                var debts = _availableKingdomCards.Count(x => x.Cost == 0 && x.SpecialCosts != null && x.SpecialCosts.Any(z => z.Type.Name == "Debt") && x.SpecialCosts.FirstOrDefault().Amount == i) + _vm.Kingdom.Count(x => x.Card.Cost == 0 && x.Card.SpecialCosts != null && x.Card.SpecialCosts.Any(z => z.Type.Name == "Debt") && x.Card.SpecialCosts.FirstOrDefault().Amount == i);

                //var dub = (double)_availableKingdomCards.Count(x => x.Cost == i) + _vm.Kingdom.Count(x => x.Card.Cost == i);
                var dub = (double) nonspecials + potions + debts;
                distributions[i] = (int) Math.Ceiling(10 * dub / total);
            }

            return distributions;
        }



    }
}
