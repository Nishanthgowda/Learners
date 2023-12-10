using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTree.Areas.Admin.Models;
using TechTree.Data;
using TechTree.Entities;

namespace TechTree.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UsersToCategory : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsersToCategory(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetusersForCategory(int categoryId)
        {
            UsersCategoryListModel usersCategoryListModel = new UsersCategoryListModel();
            var allUsers = await GetAllUsers();
            var SavedSelectedUsersForCategory = await GetSavedUsersForCategoryId(categoryId);
            usersCategoryListModel.Users = allUsers;
            usersCategoryListModel.UsersSelected = SavedSelectedUsersForCategory;

            return PartialView("_UsersListViewPartial", usersCategoryListModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelectedUsers([Bind("CategoryId,UsersSelected")] UsersCategoryListModel usersCategoryListModel)
        {
            List<UserCategory> usersSelectedForCategoryAdd = null;
            if (usersCategoryListModel.UsersSelected != null) 
            {
                usersSelectedForCategoryAdd = await GetUsersForCategoryToAdd(usersCategoryListModel);
            }
           
            var usersSelectedForCategoryDelete = await GetUsersForCategoryToDelete(usersCategoryListModel.CategoryId);

            using(var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try {
                    _context.RemoveRange(usersSelectedForCategoryDelete);
                    await _context.SaveChangesAsync();
                    if (usersSelectedForCategoryAdd != null)
                    {
                        _context.AddRange(usersSelectedForCategoryAdd);
                        await _context.SaveChangesAsync();
                    }
                    await dbContextTransaction.CommitAsync();
                }
                catch(Exception ex) 
                {
                    await dbContextTransaction.DisposeAsync();
                }
            }
            usersCategoryListModel.Users = await GetAllUsers();
            return PartialView("_UsersListViewPartial", usersCategoryListModel);
        }
        private async Task<List<UserViewModel>> GetAllUsers()
        {
            var allUsers = await (from user in _context.Users
                                  select new UserViewModel
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName
                                  }
                                  ).ToListAsync();
            return allUsers;
        }

        private async Task<List<UserViewModel>> GetSavedUsersForCategoryId(int categoryId)
        {
            var savedUsersForCategory = await (from user in _context.UserCategory
                                  where user.CategoryId == categoryId
                                  select new UserViewModel
                                  {
                                      Id = user.UserId
                                  }).ToListAsync();
            return savedUsersForCategory;
        }

        private async Task<List<UserCategory>> GetUsersForCategoryToDelete(int categoryId)
        {
            var userCategoryToDelete = await (from userCat in _context.UserCategory
                                        where userCat.CategoryId == categoryId
                                        select new UserCategory { CategoryId = categoryId,Id = userCat.Id,UserId = userCat.UserId}).ToListAsync();
            return userCategoryToDelete;
        }

        private async Task<List<UserCategory>> GetUsersForCategoryToAdd(UsersCategoryListModel usersCategoryListModel)
        {
            var userCategoryToAdd = (from userCat in usersCategoryListModel.UsersSelected
                                     select new UserCategory { CategoryId = usersCategoryListModel.CategoryId,
                                     UserId = userCat.Id
                                     }).ToList();
            return await Task.FromResult(userCategoryToAdd);
        }
    }
}
