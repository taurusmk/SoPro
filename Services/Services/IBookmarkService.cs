using System.Collections.Generic;
using ReadLater.Entities;

namespace ReadLater.Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark);
        Bookmark UpdateBookmark(Bookmark bookmark);
        List<Bookmark> GetBookmarks(string userId, string category);
        Bookmark GetBookmark(int id);
        void DeleteBookmark(int id);
        void UrlClicked(int id);
    }
}