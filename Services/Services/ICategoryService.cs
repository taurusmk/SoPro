using System;
using System.Collections.Generic;
using ReadLater.Entities;
namespace ReadLater.Services
{
    public interface ICategoryService
    {
        Category CreateCategory(Category category);
        List<Category> GetCategoriesForUser(string userId);
        Category GetCategory(int Id);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
