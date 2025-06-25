using Microsoft.EntityFrameworkCore;
using ShoppingApi.Data;
using ShoppingApi.Models;

namespace ShoppingApi.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _db;

        public CategoryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<CategoryDto>> GetCategoriesDtoAsync()
        {
            var categories = await _db.Categories.Include(c => c.Products).ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Products = c.Products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList()
            }).ToList();

        }


        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _db.Categories.Include(c => c.Products).ToListAsync();
        }

        public async Task AddSampleDataAsync()
        {
            if (!_db.Categories.Any())
            {
                var dairy = new Category { Name = "חלב וגבינות" };
                dairy.Products.Add(new Product { Name = "קוטג'" });
                dairy.Products.Add(new Product { Name = "שמנת" });
                dairy.Products.Add(new Product { Name = "חמוצה" });

                var meat = new Category { Name = "בשר ומוצריו" };
                meat.Products.Add(new Product { Name = "נקנקיות" });
                meat.Products.Add(new Product { Name = "שוקיים" });

                _db.Categories.AddRange(dairy, meat);
                await _db.SaveChangesAsync();
            }
        }
    }
}
