using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Nest;
using PagedList;
using RabbitMQAssignment.Data;
using RabbitMQAssignment.Models;
using RabbitMQAssignment.Util;

namespace RabbitMQAssignment.Controllers
{
    public class ArticlesController : Controller
    {
        private RabbitContext db = new RabbitContext();

        // GET: Articles
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? categoryId)
        {
            ViewBag.ListCategory = db.Categories.ToList();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var articles = db.Articles.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                //var elasticSearchKeyWord = ElasticSearchQuery(searchString).ToArray();
                //if (elasticSearchKeyWord != null)
                //{    
                //    articles = articles.Where(s => elasticSearchKeyWord.Contains(s.Id));                
                //}
                articles = articles.Where(s => s.Title.Contains(searchString)
                                       || s.Tag.Contains(searchString));
            }
            if (Request.QueryString["categoryId"] == null)
            {
                categoryId = -1;
            }
            ViewBag.CurrentCategory = categoryId;

            if (categoryId != -1)
            {
                // tìm đến những Sources có CategoryId == ?
                var sources = db.Sources.Where(s => s.CategoryId == categoryId).ToList();

                if (sources != null || sources.Count != 0)
                {
                    foreach (var source in sources)
                    {
                        articles = articles.Where(s => s.SourceId.Equals(source.Id));
                    }
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    articles = articles.OrderByDescending(s => s.Title);
                    break;
                default:  // Name ascending 
                    articles = articles.OrderBy(s => s.Title);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            articles.Include(a => a.Source);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }

        List<int> ElasticSearchQuery(string searchString)
        {
            var list = new List<Article>();
            var listId = new List<int>();
            var searchRequest = new SearchRequest<Article>()
            {
                QueryOnQueryString = searchString,
            };
            var searchResult = ElasticSearchService.GetInstance().Search<Article>(searchRequest);
            list = searchResult.Documents.ToList();
            foreach(Article article in list)
            {
                listId.Add(article.Id);
            }
            return listId;
        }


        public ActionResult ClientIndex(string sortOrder, string currentFilter, string searchString, int? page, int? categoryId)
        {
            ViewBag.ListCategory = db.Categories.ToList();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var articles = db.Articles.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                var elasticSearchKeyWord = ElasticSearchQuery(searchString).ToArray();
                if (elasticSearchKeyWord != null)
                {
                    articles = articles.Where(s => elasticSearchKeyWord.Contains(s.Id));
                }
                //articles = articles.Where(s => s.Title.Contains(searchString)
                //                       || s.Tag.Contains(searchString));
            }
            if (Request.QueryString["categoryId"] == null)
            {
                categoryId = -1;
            }
            ViewBag.CurrentCategory = categoryId;

            if (categoryId != -1)
            {
                // tìm đến những Sources có CategoryId == ?
                var sources = db.Sources.Where(s => s.CategoryId == categoryId).ToList();

                if (sources.Count != 0)
                {
                    foreach (var source in sources)
                    {
                        articles = articles.Where(s => s.SourceId.Equals(source.Id));
                    }
                }
            }

            switch (sortOrder)
            {
                case "name_desc":
                    articles = articles.OrderByDescending(s => s.Title);
                    break;
                default:  // Name ascending 
                    articles = articles.OrderBy(s => s.Title);
                    break;
            }
            ViewBag.ListArticles = articles.OrderByDescending(s => s.Id).ToPagedList(1, 4);
            articles.Include(a => a.Source);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        public ActionResult ClientDetail(int? id)
        {
            ViewBag.ListCategory = db.Categories.ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        public ActionResult ClientCategory(string sortOrder, string searchString, string currentFilter, int? id, int? page)
        {
            ViewBag.ListCategory = db.Categories.ToList();
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.Category = db.Categories.Find(id);
            List<Article> articles = new List<Article>();
            var sources = db.Sources.Where(s => s.CategoryId == id).ToList();
            if (sources != null || sources.Count != 0)
            {
                foreach (var source in sources)
                {
                    articles = db.Articles.Where(s => s.SourceId == source.Id).ToList();
                }
            }
            ViewBag.ListArticles = articles.ToPagedList(1, 4);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(articles.ToPagedList(pageNumber, pageSize));
        }
        // GET: Articles/Create

        // GET: Articles/Create
        public ActionResult Create()
        {
            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Url");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Url,Title,Description,Content,Thumbnail,Author,SourceId,CreatedAt,Tag")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Url", article.SourceId);
            return View(article);
        }

        public ActionResult ElasticDeleteIndex()
        {
            ElasticSearchService.GetInstance().DeleteByQueryAsync<Article>(s =>
        s.Query(q => q.MatchAll()));
            return View("Success");
        }

        public ActionResult ElasticSearchLoad()
        {
            List<Article> listArticle = db.Articles.ToList();
            //    ElasticSearchService.GetInstance().DeleteByQueryAsync<Article>(s =>
            //s.Query(q => q.MatchAll()));
            
            
            foreach (Article article in listArticle)
            {
                var articleNew = new Article()
                {
                    Id = article.Id,
                    Url = article.Url,
                    Title = article.Title,
                    Description = article.Description,
                    Content = article.Content,
                    Thumbnail = article.Thumbnail,
                    Author = article.Author,
                    SourceId = article.SourceId,
                    CreatedAt = article.CreatedAt,
                    Tag = article.Tag,
                };
                ElasticSearchService.GetInstance().IndexDocument(articleNew);
            }
            
            return View("Success");
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Url", article.SourceId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Url,Title,Description,Content,Thumbnail,Author,SourceId,CreatedAt,Tag")] Article article)
        {
            if (ModelState.IsValid)
            {
                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SourceId = new SelectList(db.Sources, "Id", "Url", article.SourceId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
