using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DominionBuilder.Models;
using DominionBuilder.Data;
using DominionBuilder.Domain;

namespace DominionBuilder.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DominionContext context) : base(context)
        {

        }

        public IActionResult Index()
        {
            return View(commonView);
        }

        public IActionResult GenerateKingdom(List<int> Sets, /*Requirements Requirements,*/ List<Requirement> reqs)
        {
            IKingdomOperations kingdom;

            var allCards = (from x in _dbContext.Cards
                            select x).ToList();

            if (Sets.Count > 1)
                kingdom = new MultiSetOps(allCards, dominionView, Sets, reqs);
            else
                kingdom = new SingleSetOps(allCards, dominionView, Sets, reqs);

            kingdom.RemoveRestrictedCards();
            kingdom.AddRequiredCards();

            // use RNG to determine/add youngwitch & mouse first, then you never have to swap out a kingdom card for one
            kingdom.AddPortraits();
            kingdom.AddSpecials();

            kingdom.FillKingdom();
            kingdom.AddAllies();

            kingdom.AddPeripherals();
            kingdom.AddExtraCards();

            kingdom.AddSplitPiles();

            dominionView.Kingdom.Sort((x, y) =>
            {
                return x.Card.Cost.Value.CompareTo(y.Card.Cost.Value);
            });

            dominionView.ExtraCards.Sort((x,y) =>
            {
                if (!x.Cost.HasValue || !y.Cost.HasValue)
                {
                    if (!x.Cost.HasValue && !y.Cost.HasValue)
                    {
                        return x.Name.CompareTo(y.Name);
                    }
                    else
                    {
                        return x.Cost.HasValue ? -1 : 1;
                    }
                }

                bool XisHeirloom = x.CardTypeLinks.Any(z => z.Type.Name == "Heirloom");
                bool YisHeirloom = y.CardTypeLinks.Any(z => z.Type.Name == "Heirloom");

                if (XisHeirloom || YisHeirloom)
                {
                    if (XisHeirloom && YisHeirloom)
                    {
                        return x.Name.CompareTo(y.Name);
                    }
                    else
                    {
                        return XisHeirloom ? 1 : -1;
                    }
                }
                else
                {
                    return x.Cost.Value.CompareTo(y.Cost.Value);
                }
            });

            return PartialView("_Kingdom", dominionView);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
