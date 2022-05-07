using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominionBuilder.Data;
using DominionBuilder.Domain;
using DominionBuilder.Models;
using Microsoft.AspNetCore.Mvc;

namespace DominionBuilder.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DominionContext _dbContext;

        protected CommonViewModel commonView;
        protected DominionViewModel dominionView;
        protected QueryViewModel queryView;

        protected Random rand;

        public BaseController(DominionContext context)
        {
            _dbContext = context;
            Initialize();
        }

        private void Initialize()
        {
            var allCards = (from x in _dbContext.Cards
                            select x).ToList();

            var allSets = (from x in _dbContext.Sets
                           select x).ToList();

            var types = (from t in _dbContext.CardTypes
                         select t).ToList();

            var typelinks = (from tl in _dbContext.CardTypeLinks
                             select tl).ToList();

            var categories = (from c in _dbContext.CardCategories
                              select c).ToList();

            var categorylinks = (from cl in _dbContext.CardCategoryLinks
                                 select cl).ToList();

            var peripherals = (from p in _dbContext.Peripherals
                               select p).ToList();

            var peripherallinks = (from pl in _dbContext.PeripheralLinks
                                   select pl).ToList();

            var specialcosts = (from s in _dbContext.SpecialCosts
                                select s).ToList();

            var specialcostTypes = (from s in _dbContext.SpecialCostTypes
                                    select s).ToList();

            var extracards = (from e in _dbContext.ExtraCards
                              select e).ToList();

            var typeDictionary = new Dictionary<string, int>();
            foreach (var t in types)
            {
                typeDictionary.Add(t.Name, t.Id);
            }

            var categoryDictionary = new Dictionary<string, int>();
            foreach (var c in categories)
            {
                categoryDictionary.Add(c.Name, c.Id);
            }

            Mapping.SetTypeIds(typeDictionary);
            Mapping.SetCategoryIds(categoryDictionary);

            dominionView = new DominionViewModel()
            {
                Kingdom = new List<CardModel>(),
                Peripherals = new List<Peripherals>(),
                PortraitCards = new List<Cards>(),
                ExtraCards = new List<Cards>(),
                DruidCards = new List<Cards>(),
                //KingdomCards = new List<Cards>(),
                IncludedSetNames = new List<string>()
            };

            queryView = new QueryViewModel();

            commonView = new CommonViewModel()
            {
                dominionView = dominionView,
                queryView = queryView
            };

            commonView.ExpNames = (from x in _dbContext.Sets
                                       //where x.Id != 0
                                   select x.Name).ToList();

            rand = new Random();
        }

    }
}