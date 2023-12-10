using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TechTree.Data;
using TechTree.Entities;
using TechTree.Models;

namespace TechTree.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;


        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager; 
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels = null;
            IEnumerable<GroupedCategoryItemsByCategoryModel> groupedCategoryItemsByCategoryModels = null;
            CategoryDetailsModel categoryDetailsModel = new CategoryDetailsModel();

            if(_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if(user != null)
                {
                    categoryItemDetailsModels = await GetCategoryItemsDetailsForUser(user.Id);
                    groupedCategoryItemsByCategoryModels = GetGroupedCategoryItemsByCategory(categoryItemDetailsModels);
                    categoryDetailsModel.GroupedCategoryItemsByCategoryModels = groupedCategoryItemsByCategoryModels;

                }
            }
            return View(categoryDetailsModel);
        }

        private IEnumerable<GroupedCategoryItemsByCategoryModel> GetGroupedCategoryItemsByCategory(IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels)
        {
            return (from item in categoryItemDetailsModels
                    group item by item.CategoryId into g
                    select new GroupedCategoryItemsByCategoryModel
                    {
                        Id = g.Key,
                        Title = g.Select(c => c.CategoryTitle).FirstOrDefault(),
                        Items = g
                    }
                    );
        }

        private async Task<IEnumerable<CategoryItemDetailsModel>> GetCategoryItemsDetailsForUser(string userId)
        {
            return await (
                            from cateItem in _context.CategoryItem
                            join category in _context.Category
                            on cateItem.CategoryId equals category.Id
                            join content in _context.Content
                            on cateItem.Id equals content.CategoryItem.Id
                            join userCat in _context.UserCategory
                            on category.Id equals userCat.CategoryId
                            join mediatype in _context.MediaType
                            on cateItem.MediaTypeId equals mediatype.Id
                            where userCat.UserId == userId

                            select new CategoryItemDetailsModel
                            {
                                CategoryId = category.Id,
                                CategoryTitle = category.Title,
                                CategoryItemId = cateItem.Id,
                                CategoryItemTitle = cateItem.Title,
                                Description = cateItem.Description,
                                ImagePath = mediatype.ThumbnailImagePath


                            }).ToListAsync();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}