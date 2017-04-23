using CodeIt.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeIt.Controllers
{
    public class CommentController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Create(int id)
        {
            var db = new CodeItDbContext();
            var code = db.Codes.Find(id).CodeContent
                .Split(new string[] { "\r\n", "\n" }
                ,StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var comment = new CommentViewModel
            {
                CodeId = id,
                Code = code
            };
            return View(comment);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new CodeItDbContext();

                var authorId = User.Identity.GetUserId();

                var comment = db.Comments.Add(new Comment
                {
                    CodeId = model.Id,
                    AuthorId = authorId,
                    Content = model.Content
                });

                db.SaveChanges();

                return RedirectToAction("Details", "Code", new { model.Id });
            }

            return View(model);
        }
    }
}