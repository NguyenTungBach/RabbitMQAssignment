using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using PagedList;
using RabbitMQAssignment.Data;
using RabbitMQAssignment.Models;

namespace RabbitMQAssignment.Controllers
{
    public class SourcesController : Controller
    {
        private RabbitContext db = new RabbitContext();

        // GET: Sources
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

            var sources = db.Sources.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                sources = sources.Where(s => s.Url.Contains(searchString));
            }
            if (Request.QueryString["categoryId"] == null)
            {
                categoryId = -1;
            }
            ViewBag.CurrentCategory = categoryId;

            if (categoryId != -1)
            {
                // tìm đến những Sources có CategoryId == ?
                sources = db.Sources.Where(s => s.CategoryId == categoryId);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    sources = sources.OrderByDescending(s => s.Url);
                    break;
                default:  // Name ascending 
                    sources = sources.OrderBy(s => s.Url);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            sources.Include(s => s.Category);

            //var sources = db.Sources.Include(s => s.Category);
            return View(sources.ToPagedList(pageNumber, pageSize));
        }

        // GET: Sources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = db.Sources.Find(id);
            if (source == null)
            {
                return HttpNotFound();
            }
            return View(source);
        }

        // GET: Sources/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        public ActionResult CheckSource()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckLink(SourceCheck sourceCheck)
        {
            if (sourceCheck.Url != "" && sourceCheck.SelectorUrl != "")
            {
                try
                {
                    var web = new HtmlWeb();
                    HtmlDocument doc = web.Load(sourceCheck.Url);
                    var nodeList = doc.QuerySelectorAll(sourceCheck.SelectorUrl); // tìm đến những thẻ a nằm trong h3 có class= title-news
                    var setLinkSource = new HashSet<SourceCheck>(); // Đảm bảo link không giống nhau, nếu có sẽ bị ghi đè ở phần tử trước

                    foreach (var node in nodeList)
                    {
                        var link = node.Attributes["href"]?.Value;
                        if (string.IsNullOrEmpty(link)) // nếu link này null
                        {
                            continue;
                        }
                        var sourceCheck1 = new SourceCheck()
                        {
                            Url = link
                        };

                        setLinkSource.Add(sourceCheck1);
                    }
  
                    return PartialView("GetListSource", setLinkSource);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error: " + e.Message);
                    return PartialView("Fail");
                }
            }
            return PartialView("Fail");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Preview(SourceCheck sourceCheck)
        {
            if (sourceCheck.SubUrl != "" && sourceCheck.SelectorTitle != "" && sourceCheck.SelectorThumbnail != "" && sourceCheck.SelectorContent != "" && sourceCheck.SelectorDescription != "" && sourceCheck.SelectorAuthor != "")
            {
                try
                {
                    Console.OutputEncoding = System.Text.Encoding.UTF8;
                    var web = new HtmlWeb();
                    HtmlDocument doc = web.Load(sourceCheck.SubUrl);
                    var title = doc.QuerySelector(sourceCheck.SelectorTitle).InnerText?? "";
                    var description = doc.QuerySelector(sourceCheck.SelectorDescription).InnerText;
                    var imageNode = doc.QuerySelector(sourceCheck.SelectorThumbnail)?.Attributes["data-src"].Value;
                    var author = doc.QuerySelector(sourceCheck.SelectorAuthor)?.InnerText;
                    string thumbnail = "";
                    if (imageNode != null)
                    {
                        thumbnail = imageNode;
                    }
                    else
                    {
                        thumbnail = "";
                    }
                    var contentNode = doc.QuerySelectorAll(sourceCheck.SelectorContent);
                    StringBuilder contentBuilder = new StringBuilder();
                    foreach (var content in contentNode)
                    {
                        contentBuilder.Append(content.InnerText);
                    }

                    Article article = new Article()
                    {
                        Title = title,
                        Description = description,
                        Content = contentBuilder.ToString(),
                        Thumbnail = thumbnail,
                        Author = author,
                    };

                    return PartialView("Preview", article);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error: " + e.Message);
                    return PartialView("Fail");
                }
            }
            return PartialView("Fail");
        }

        // POST: Sources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Url,SelectorUrl,SelectorTitle,SelectorDescription,SelectorContent,SelectorThumbnail,SelectorAuthor,CategoryId")] Source source)
        {
            if (ModelState.IsValid)
            {
                db.Sources.Add(source);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", source.CategoryId);
            return View(source);
        }

        // GET: Sources/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = db.Sources.Find(id);
            if (source == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", source.CategoryId);
            return View(source);
        }

        // POST: Sources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Url,SelectorUrl,SelectorTitle,SelectorDescription,SelectorContent,SelectorThumbnail,SelectorAuthor,CategoryId")] Source source)
        {
            if (ModelState.IsValid)
            {
                db.Entry(source).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", source.CategoryId);
            return View(source);
        }

        // GET: Sources/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Source source = db.Sources.Find(id);
            if (source == null)
            {
                return HttpNotFound();
            }
            return View(source);
        }

        // POST: Sources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Source source = db.Sources.Find(id);
            db.Sources.Remove(source);
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
