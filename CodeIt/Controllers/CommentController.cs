using CodeIt.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CodeIt.Controllers
{
    //Handle the Comment Functionality
    public class CommentController : Controller
    {

        //Helper method. Check if User is Author of comment or Admin
        private bool IsAuthorised(Comment comment)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isAuthor = comment.IsAuthor(this.User.Identity.GetUserId());

            return isAdmin || isAuthor;
        }

        //Helper method. Check if User is Author of a comment on guest paste or Admin
        private bool IsAuthorisedOnGuest(CommentOnGuest comment)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isAuthor = comment.IsAuthor(this.User.Identity.GetUserId());

            return isAdmin || isAuthor;
        }



        [Authorize]
        [HttpGet]
        public ActionResult EditOnGuest(int id)
        {
            var db = new CodeItDbContext();

            var comment = db.CommentsOnGuest.Find(id);

            if (comment == null || !IsAuthorisedOnGuest(comment))
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditOnGuest(Comment model)
        {
            if (ModelState.IsValid)
            {
                var db = new CodeItDbContext();

                var comment = db.CommentsOnGuest.Find(model.Id);

                if (comment == null || !IsAuthorisedOnGuest(comment))
                {
                    return HttpNotFound();
                }

                comment.Content = model.Content;
                comment.TimeCreated = DateTime.Now;

                db.SaveChanges();

                return RedirectToAction("GuestCodeDetails", "Code", new { id = comment.CodeId });
            }
            return View(model);
        }





        [Authorize]
        [HttpGet]
        public ActionResult DeleteOnGuest(int id)
        {
            var db = new CodeItDbContext();
            var comment = db.CommentsOnGuest.Find(id);
            if (comment == null || !IsAuthorisedOnGuest(comment))
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [Authorize]
        [HttpPost]
        [ActionName("DeleteOnGuest")]
        public ActionResult DeleteOnGuestConf(int id)
        {
            var db = new CodeItDbContext();

            var comment = db.CommentsOnGuest.Find(id);

            if (comment == null || !IsAuthorisedOnGuest(comment))
            {
                return HttpNotFound();
            }

            var codeId = comment.CodeId;

            db.CommentsOnGuest.Remove(comment);

            db.SaveChanges();

            return RedirectToAction("GuestCodeDetails", "Code", new { id = codeId });

        }



        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new CodeItDbContext();

            var comment = db.Comments.Find(id);

            if(comment == null || !IsAuthorised(comment))
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(Comment model)
        {
            if (ModelState.IsValid)
            {
                var db = new CodeItDbContext();

                var comment = db.Comments.Find(model.Id);

                if(comment == null || !IsAuthorised(comment))
                {
                    return HttpNotFound();
                }

                comment.Content = model.Content;
                comment.TimeCreated = DateTime.Now;

                db.SaveChanges();

                return RedirectToAction("Details", "Code", new { id = comment.CodeId });
            }
            return View(model);
        }




        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new CodeItDbContext();
            var comment = db.Comments.Find(id);
            if(comment == null || !IsAuthorised(comment))
            {
                return HttpNotFound();
            }

            return View(comment);
        }

        [Authorize]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConf(int id)
        {
            var db = new CodeItDbContext();

            var comment = db.Comments.Find(id);

            if(comment == null || !IsAuthorised(comment))
            {
                return HttpNotFound();
            }

            var codeId = comment.CodeId;

            db.Comments.Remove(comment);

            db.SaveChanges();

            return RedirectToAction("Details" , "Code" , new { id = codeId });

        }





        [HttpGet]
        [Authorize]
        public ActionResult Create(int id)
        {
            var db = new CodeItDbContext();
            var code = db.Codes.Find(id).CodeContent
                    .Split(new string[] { "\r\n", "\n" }
                    , StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

            var comment = new CommentViewModel
            {
                CodeId = id,
                Code = code,
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
                var db = new CodeItDbContext();

                var authorId = User.Identity.GetUserId();

                db.Comments.Add(new Comment
                {
                    CodeId = model.Id,
                    AuthorId = authorId,
                    Content = model.Content
                });

                db.SaveChanges();

                return RedirectToAction("Details", "Code", new { id = model.Id });
            }

            return View(model);
        }



        [HttpGet]
        [Authorize]
        public ActionResult CreateOnGuest(int id)
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

            };
            return View(commentG);


        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult CreateOnGuest(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {

                var dbG = new CodeItDbContext();

                var authorIdG = User.Identity.GetUserId();

                dbG.CommentsOnGuest.Add(new CommentOnGuest
                {
                    CodeId = model.Id,
                    Content = model.Content,
                    AuthorId = authorIdG
                });

                dbG.SaveChanges();


                return RedirectToAction("GuestCodeDetails", "Code", new { model.Id });

            }

            return View(model);
        }
    }
}