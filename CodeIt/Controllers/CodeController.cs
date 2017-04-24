﻿using CodeIt.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CodeIt.Controllers
{
    public class CodeController : Controller
    {

        public ActionResult All(int page = 1, string user = null)
        {
            var pageSize = 10;

            var db = new CodeItDbContext();

            var pasteQuery = db.Codes.AsQueryable();

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

                }).ToList();

            ViewBag.CurrentPage = page;

            return View(pastes);
        }

        public ActionResult Details(int id, int pPage = 1)
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
                Coments = comments
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
            var lines = code.CodeContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var comments = db.CommentsOnGuest.Where(c => c.CodeId == id).ToList();
            var viewCode = new GuestCodeDetailsModel
            {
                Id = id,
                Author = code.Author,
                CodeTitle = code.CodeTitle,
                CodeContent = lines,
                PrevPage = pPage,
                Coments = comments
            };


            if (code == null)
            {
                return HttpNotFound();
            }

            return View(viewCode);
        }

        //Action for GuestPosts

        [HttpGet]
        [AllowAnonymous]
        public ActionResult CreateAsGuest()
        {
            return View();
        }

        //Action for GuestPosts

        [HttpPost]
        [AllowAnonymous]
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

            if(code == null)
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

            if (code == null)
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
    }


}