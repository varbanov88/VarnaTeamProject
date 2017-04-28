using CodeIt.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CodeIt.Controllers
{
    //Handle operations with pasted Codes
    public class CodeController : Controller
    {
       
        //Helper method. Check if User is an Author of a Paste or Admin
        private bool IsAuthorised(CodeModel code)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isAuthor = code.IsAuthor(this.User.Identity.GetUserId());

            return isAdmin || isAuthor;
        }



        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var db = new CodeItDbContext();
            var code = db.Codes.Find(id);
            if(code == null || !IsAuthorised(code))
            {
                return HttpNotFound();
            }

            return View(code);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CodeModel model)
        {
            if (ModelState.IsValid )
            {
                using (var db = new CodeItDbContext())
                {
                    var code = db.Codes.Find(model.Id);

                    if(code == null || !IsAuthorised(code))
                    {
                        return HttpNotFound();
                    }

                    code.CodeTitle = model.CodeTitle;
                    code.CodeContent = model.CodeContent;

                    db.SaveChanges();
                }

                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }

        public ActionResult All(string search, int page = 1, string user = null)
        {
            var pageSize = 10;

            var db = new CodeItDbContext();

            var pasteQuery = db.Codes.AsQueryable();

            if (search != null)
            {
                pasteQuery = pasteQuery
                    .Where(p => p.CodeTitle.ToLower().Contains(search.ToLower())
                    || p.CodeTitle.ToLower().Contains(search.ToLower()));
            }

            if (user != null)
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
                    TimeCreated = c.TimeCreated
                }).ToList();

            ViewBag.CurrentPage = page;

            return View(pastes);
        }

        public ActionResult Details(int id, int pPage = 1, string myUser = null)
        {
            var db = new CodeItDbContext();
            var code = db.Codes.Where(c => c.Id == id).FirstOrDefault();
            var lines = code.CodeContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var comments = db.Comments.Where(c => c.CodeId == id).ToList();
            var viewCode = new CodeDetails
            {
                Id = id,
                Author = code.Author.Nickname,
                CodeTitle = code.CodeTitle,
                CodeContent = lines,
                PrevPage = pPage,
                ContactInfo = code.Author.Email,
                Coments = comments,
                AuthorId = code.AuthorId,
                MyUser = myUser,
                TimeCreated = code.TimeCreated
            };


            if (code == null)
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

                var code = db.Codes.Add(new CodeModel
                {
                    CodeTitle = model.CodeTitle,
                    CodeContent = model.CodeContent,
                    AuthorId = User.Identity.GetUserId()
                });

                db.SaveChanges();

                return RedirectToAction("Details", new { code.Id });

            }
            return View(model);
        }

        public ActionResult GuestCodeDetails(int id, int pPage = 1)
        {
            var db = new CodeItDbContext();
            var code = db.GuestCodes.Where(c => c.Id == id).FirstOrDefault();
            if(code == null)
            {
                return HttpNotFound();
            }
            var lines = code.CodeContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var comments = db.CommentsOnGuest.Where(c => c.CodeId == id).ToList();
            var viewCode = new GuestCodeDetailsModel
            {
                Id = id,
                Author = code.Author,
                CodeTitle = code.CodeTitle,
                CodeContent = lines,
                PrevPage = pPage,
                Coments = comments,
                TimeCreated = code.TimeCreated
            };


            if (code == null)
            {
                return HttpNotFound();
            }

            return View(viewCode);
        }

        //Action for GuestPosts

        [HttpGet]
        [ValidateInput(false)]
        public ActionResult CreateAsGuest()
        {
            return View();
        }

        //Action for GuestPosts

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateAsGuest(GuestCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var db = new CodeItDbContext();

                db.GuestCodes.Add(model);

                db.SaveChanges();

                return RedirectToAction("GuestCodeDetails", new { id = model.Id});

            }
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = new CodeItDbContext();

            var code = db.Codes
                .Where(a => a.Id == id)
                .FirstOrDefault();

            if(code == null || !IsAuthorised(code))
            {
                return HttpNotFound();
            }

            return View(code);
               
        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {
            var db = new CodeItDbContext();

            var code = db.Codes
                .Where(a => a.Id == id)
                .FirstOrDefault();

            if (code == null || !IsAuthorised(code))
            {
                return HttpNotFound();
            }

            db.Codes.Remove(code);

            var comments = db.Comments.Where(c => c.CodeId == id).ToList();

            foreach(var c in comments)
            {
                db.Comments.Remove(c);
            }

            db.SaveChanges();
            return RedirectToAction("All");

        }

        

        public ActionResult AllGuest(string search, int page = 1)
        {
            var pageSize = 10;

            var db = new CodeItDbContext();

            var pasteQuery = db.GuestCodes.AsQueryable();

            if (search != null)
            {
                pasteQuery = pasteQuery
                    .Where(p => p.CodeTitle.ToLower().Contains(search.ToLower())
                    || p.CodeTitle.ToLower().Contains(search.ToLower()));
            }

            var pastes = pasteQuery
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new AllGuestCodesModel
                {
                    Id = c.Id,
                    CodeTitle = c.CodeTitle,
                    Author = c.Author,
                    TimeCreated = c.TimeCreated 
                }).ToList();

            ViewBag.CurrentPage = page;

            return View(pastes);
        }

        [Authorize]
        [HttpGet]
        public ActionResult GuestEdit(int id)
        {
            var db = new CodeItDbContext();
            var code = db.GuestCodes.Find(id);
            if (code == null || !this.User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }

            return View(code);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GuestEdit(GuestCodeModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new CodeItDbContext())
                {
                    var code = db.GuestCodes.Find(model.Id);

                    if (code == null || !this.User.IsInRole("Admin"))
                    {
                        return HttpNotFound();
                    }

                    code.CodeTitle = model.CodeTitle;
                    code.CodeContent = model.CodeContent;

                    db.SaveChanges();
                }

                return RedirectToAction("GuestCodeDetails", new { id = model.Id });
            }
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public ActionResult GuestDelete(int id)
        {
            var db = new CodeItDbContext();

            var code = db.GuestCodes
                .Where(a => a.Id == id)
                .FirstOrDefault();

            if (code == null || !this.User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }

            return View(code);

        }

        [Authorize]
        [ActionName("GuestDelete")]
        [HttpPost]
        public ActionResult GuestConfirmDelete(int id)
        {
            var db = new CodeItDbContext();

            var code = db.GuestCodes
                .Where(a => a.Id == id)
                .FirstOrDefault();

            if (code == null || !this.User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }

            db.GuestCodes.Remove(code);

            var comments = db.CommentsOnGuest.Where(c => c.CodeId == id).ToList();

            foreach (var c in comments)
            {
                db.CommentsOnGuest.Remove(c);
            }

            db.SaveChanges();
            return RedirectToAction("AllGuest");
        }


    }


}