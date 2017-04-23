using CodeIt.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CodeIt.Controllers
{
    public class CodeController : Controller
    {

        public ActionResult All(int page = 1, string user=null)
        {
            var pageSize = 10;

            var db = new CodeItDbContext();

            var pasteQuery = db.Codes.AsQueryable();

            if(user != null)
            {
                pasteQuery = pasteQuery
                    .Where(c => c.Author.Email == user);
            }

            var pastes = pasteQuery
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new AllCodesModel
                {
                    Id = c.Id,
                    CodeTitle = c.CodeTitle,
                    Author = c.Author.Nickname,

                }).ToList();

            ViewBag.CurrentPage = page;

            return View(pastes);
        }

        public ActionResult Details(int id, int pPage=1)
        {
            var db = new CodeItDbContext();
            var code = db.Codes.Where(c => c.Id == id).FirstOrDefault();
            var lines = code.CodeContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var viewCode = new CodeDetails
            {

                Author = code.Author.Nickname,
                CodeTitle = code.CodeTitle,
                CodeContent = lines,           
                PrevPage = pPage
            };
            

            if(code == null)
            {
                return HttpNotFound();
            }

            return View(viewCode);
        }


       [HttpGet]
       [Authorize]
       public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult Create(CreateCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new CodeItDbContext();

                var code = db.Codes.Add(new CodeModel {
                    CodeTitle = model.CodeTitle,
                    CodeContent = model.CodeContent,
                    AuthorId = User.Identity.GetUserId()      
                });

                

                db.SaveChanges();

                return RedirectToAction("Details", new { code.Id });

            }
            return View(model);
        }
    }
}