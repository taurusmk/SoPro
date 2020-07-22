using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC.Models;
using ReadLater.Data;
using ReadLater.Entities;
using ReadLater.Services;

namespace MVC.Controllers
{
    [Authorize]
    public class BookmarksController : Controller
    {
        private IBookmarkService _bookmarkService;
        private ICategoryService _categoryService;

        public BookmarksController(IBookmarkService bookmarkService, ICategoryService categoryService)
        {
            _bookmarkService = bookmarkService;
            _categoryService = categoryService;
        }

        // GET: Bookmarks
        public ActionResult Index()
        {
            var bookmarks = _bookmarkService.GetBookmarks(User.Identity.GetUserId(), null);
            return View(bookmarks.ToList());
        }

        public ActionResult UrlClicked(int id)
        {

            _bookmarkService.UrlClicked(id);
            return new EmptyResult();
        }

        // GET: Bookmarks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bookmark bookmark = _bookmarkService.GetBookmark(id.Value);
            if (bookmark == null)
            {
                return HttpNotFound();
            }
            return View(bookmark);
        }

        // GET: Bookmarks/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryService.GetCategoriesForUser(User.Identity.GetUserId()), "ID", "Name");
            return View();
        }

        // POST: Bookmarks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,URL,ShortDescription,CategoryId,CategoryName")] BookmarksViewModel bookmarkViewModel)
        {
            if (ModelState.IsValid)
            {
                Bookmark bookmark = new Bookmark
                {
                    CategoryId = bookmarkViewModel.CategoryId,
                    CreateDate = DateTime.Now,
                    ShortDescription = bookmarkViewModel.ShortDescription,
                    URL = bookmarkViewModel.URL
                };

                if (bookmarkViewModel.CategoryId == 0)
                {
                    bookmark.Category = _categoryService.CreateCategory(new Category { Name = bookmarkViewModel.CategoryName, UserId = User.Identity.GetUserId() });
                    bookmark.CategoryId = bookmark.Category.ID;
                    bookmark.Category.ObjectState = ObjectState.Unchanged;
                }

                _bookmarkService.CreateBookmark(bookmark);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_categoryService.GetCategoriesForUser(User.Identity.GetUserId()), "ID", "Name", bookmarkViewModel.CategoryId);
            return View(bookmarkViewModel);
        }

        // GET: Bookmarks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bookmark bookmark = _bookmarkService.GetBookmark(id.Value);

            if (bookmark == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(_categoryService.GetCategoriesForUser(User.Identity.GetUserId()), "ID", "Name", bookmark.CategoryId);
            return View(bookmark);
        }

        // POST: Bookmarks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,URL,ShortDescription,CategoryId,CreateDate")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                _bookmarkService.UpdateBookmark(bookmark);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryService.GetCategoriesForUser(User.Identity.GetUserId()), "ID", "Name", bookmark.CategoryId);
            return View(bookmark);
        }

        // GET: Bookmarks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(_bookmarkService.GetBookmark(id.Value));
        }

        // POST: Bookmarks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _bookmarkService.DeleteBookmark(id);
            return RedirectToAction("Index");
        }
    }
}
