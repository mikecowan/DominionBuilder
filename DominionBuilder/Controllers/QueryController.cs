using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DominionBuilder.Data;
using DominionBuilder.Models;
using Microsoft.AspNetCore.Mvc;

namespace DominionBuilder.Controllers
{
    public class QueryController : BaseController
    {

        public QueryController(DominionContext context) : base(context)
        {

        }

        private List<string> BasicTypes = new List<string>() { "Action", "Treasure", "Victory", "Attack", "Reaction", "Duration", "Event", "Landmark", "Night", "Project", "Reserve", "Way" };

        public IActionResult Index()
        {
            var topfive = new List<Item>();
            int idx = 0;
            foreach (var type in BasicTypes.Take(5))
            {
                idx++;
                topfive.Add(new Item()
                {
                    Id = idx,
                    Name = type,
                    Advanced = false
                });
            }

            var remainingTypes = (from t in _dbContext.CardTypes
                                  where !BasicTypes.Take(5).Contains(t.Name)
                                  select new Item()
                                  {
                                      Id = t.Id,
                                      Name = t.Name,
                                      Advanced = !BasicTypes.Contains(t.Name)
                                  }).OrderBy(t => t.Name);

            commonView.queryView.Types = topfive.Union(remainingTypes).ToList();

            commonView.queryView.Categories = (from c in _dbContext.CardCategories
                                               select new Item()
                                               {
                                                   Id = c.Id,
                                                   Name = c.Name
                                               }).ToList();
            return View(commonView);
        }

        public IActionResult Test(Query query)
        {
            if (!string.IsNullOrEmpty(query.SearchText))
            {
                queryView.QueryResults = (from x in _dbContext.Cards
                                          where x.Name.Contains(query.SearchText)
                                          select new CardModel()
                                          {
                                              Card = x
                                          }).Distinct().ToList();
            }
            else if (query.CostMax < query.CostMin || query.Sets == null /*|| query.Types == null*/)
            {
                return PartialView("_Results", queryView);
            }
            else
            {
                //queryView.QueryResults = (from x in _dbContext.Cards
                //                          join tl in _dbContext.CardTypeLinks on x.Id equals tl.CardId
                //                          join t in _dbContext.CardTypes on tl.TypeId equals t.Id
                //                          join s in _dbContext.Sets on x.SetId equals s.Id
                //                          join cl in _dbContext.CardCategoryLinks.DefaultIfEmpty() on x.Id equals cl.CardId
                //                          join c in _dbContext.CardCategories.DefaultIfEmpty() on cl.CategoryId equals c.Id
                //                          where
                //                          ((x.Cost >= query.CostMin && x.Cost <= query.CostMax) || !x.Cost.HasValue) &&
                //                          //(query.Types != null ? query.Types.Contains(t.Id) : true) &&
                //                          //(query.Categories != null ? query.Categories.Contains(c.Id) : true) &&
                //                          query.Sets.Contains(s.Id)
                //                          select new CardModel()
                //                          {
                //                              Card = x
                //                              //Card = new Cards()
                //                              //{
                //                              //    Name = x.Name,
                //                              //    Set = new Sets () { Name = x.Set.Name },
                //                              //    Cost = x.Cost
                //                              //},
                //                              //TypeString = string.Join(", ", t.Name),

                //                          }).Distinct().ToList();

                queryView.QueryResults = (from card in _dbContext.Cards
                                          from s in _dbContext.Sets.Where(map => map.Id == card.SetId)
                                          from tl in _dbContext.CardTypeLinks.Where(map => map.CardId == card.Id)
                                          from t in _dbContext.CardTypes.Where(map => map.Id == tl.TypeId)
                                          from cl in _dbContext.CardCategoryLinks.Where(map => map.CardId == card.Id).DefaultIfEmpty()
                                          from c in _dbContext.CardCategories.Where(map => map.Id == cl.CategoryId).DefaultIfEmpty()
                                          where
                                          ((card.Cost >= query.CostMin && card.Cost <= query.CostMax) || !card.Cost.HasValue) &&
                                          (query.Types != null ? query.Types.Contains(t.Id) : true) &&
                                          (query.Categories != null ? query.Categories.Contains(c.Id) : true) &&
                                          query.Sets.Contains(s.Id)
                                          select new CardModel()
                                          {
                                              Card = card
                                          }).Distinct().ToList();
            }

            if (query.AndSelect)
            {
                foreach (var type in query.Types)
                {
                    queryView.QueryResults.RemoveAll(x => x.Card.CardTypeLinks.Count(y => y.TypeId == type) == 0);
                }
            }

            foreach (var card in queryView.QueryResults)
            {
                var linkNames = new List<string>();
                foreach (var link in card.Card.CardTypeLinks)
                {
                    linkNames.Add(link.Type.Name);
                }

                card.TypeString = string.Join(", ", linkNames);

                var categoryNames = new List<string>();
                foreach (var category in card.Card.CardCategoryLinks)
                {
                    categoryNames.Add(category.Category.Name);
                }

                card.CategoryString = string.Join(", ", categoryNames);
            }

            return PartialView("_Results", queryView);
        }


    }
}
