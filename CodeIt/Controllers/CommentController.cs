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
        public ActionResult Create(int id, string user = "")
        {
            if(user == "Guest")
            {
                var dbG = new CodeItDbContext();
                var codeG = dbG.GuestCodes.Find(id).CodeContent
                    .Split(new string[] { "\r\n", "\n" }
                    , StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                var commentG = new CommentViewModel
                {
                    CodeId = id,
                    Code = codeG,
                    TypeOfCode = user
                };
                return View(commentG);
            }
          
            
            var db = new CodeItDbContext();
            var code = db.Codes.Find(id).CodeContent
                    .Split(new string[] { "\r\n", "\n" }
                    , StringSplitOptions.RemoveEmptyEntries)
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
        [ValidateInput(false)]
        public ActionResult Create(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {

                if(model.TypeOfCode == "")
                {
                    var db = new CodeItDbContext();

                    var authorId = User.Identity.GetUserId();

                    db.Comments.Add(new Comment
                    {
                        CodeId = model.Id,
                        AuthorId = authorId,
                        Content = model.Content
                    });

                    db.SaveChanges();


                    return RedirectToAction("Details", "Code", new { model.Id });
                }


                var dbG = new CodeItDbContext();

                var authorIdG = User.Identity.GetUserId();

                dbG.CommentsOnGuest.Add(new CommentOnGuest
                {
                    CodeId = model.Id,
                    AuthorId = authorIdG,
                    Content = model.Content
                });

                dbG.SaveChanges();


                return RedirectToAction("GuestCodeDetails", "Code", new { model.Id });

            }

            return View(model);
        }
    }
}