using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadLater.Entities;
using ReadLater.Repository;

namespace ReadLater.Services
{
    public class BookmarkService : IBookmarkService
    {
        protected IUnitOfWork _unitOfWork;

        public BookmarkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            _unitOfWork.Repository<Bookmark>().Insert(bookmark);
            _unitOfWork.Save();
            return bookmark;
        }

        public Bookmark UpdateBookmark(Bookmark bookmark)
        {
            _unitOfWork.Repository<Bookmark>().Update(bookmark);
            _unitOfWork.Save();
            return bookmark;
        }

        public Bookmark GetBookmark(int id)
        {
            return _unitOfWork.Repository<Bookmark>().Query()
                .Filter(b => b.ID == id).Get().FirstOrDefault();
        }

        public void DeleteBookmark(int id)
        {
            _unitOfWork.Repository<Bookmark>().Delete(id);
            _unitOfWork.Save();
        }

        public void UrlClicked(int id)
        {
            Bookmark bookmark = GetBookmark(id);

            bookmark.ClickedCount++;

            UpdateBookmark(bookmark);

            _unitOfWork.Save();
        }

        public List<Bookmark> GetBookmarks(string userId, string category)
        {
            var userCategories = _unitOfWork.Repository<Category>().Query()
                .Filter(cat => cat.UserId == userId)
                .Get()
                .ToList();

            List<int> categoriesIds = userCategories.Select(c => c.ID).ToList();


            if (string.IsNullOrEmpty(category))
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                        .Filter(b => categoriesIds.Contains(b.CategoryId.Value))
                                                        .OrderBy(l => l.OrderByDescending(b => b.CreateDate))
                                                        .Get()
                                                        .ToList();
            }
            else
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                            .Filter(b => b.Category != null && b.Category.Name == category && categoriesIds.Contains(b.CategoryId.Value))
                                                            .Get()
                                                            .ToList();
            }
        }
    }
}
