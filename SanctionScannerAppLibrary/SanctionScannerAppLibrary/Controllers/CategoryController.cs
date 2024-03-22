using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanctionScannerAppLibrary.Models.Entities;

namespace SanctionScannerAppLibrary.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DBLIBRARYContext _dbContext;
        private readonly ILogger _logger;

        public CategoryController(DBLIBRARYContext dbContext, ILogger<CategoryController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            return View(categories);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

      
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var exist = _dbContext.Categories.FirstOrDefault(c => c.Name == category.Name);
                    if (exist != null)
                    {
                        ModelState.AddModelError("", "Bu isimde bir kategori zaten mevcut.");
                        return View(category);
                    }
                    _dbContext.Categories.Add(category);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                // Hata sayfasına yönlendirme veya başka bir işlem yapabilirsiniz
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                ModelState.AddModelError("", "Veritabanına kayıt işlemi sırasında bir hata oluştu.");
                return View(category);
            }

            // ModelState.IsValid false olduğunda buraya düşer
            return View(category);
        }
        public ActionResult DeleteCategory(byte? id)
        {
            try
            {
                var category = _dbContext.Categories.Find(id);

                if (category == null)
                {
                    return NotFound(); // Kategori bulunamadığında 404 hatası döndür
                }

                // İlişkili tablolarda silme işlemini gerçekleştirmek yerine IsActive false yapma
                // category.IsActive = false;

                _dbContext.Categories.Remove(category);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);
                return View("Error");
            }
        }
        public ActionResult GetCategory(byte? id)
        {
            try
            {

                var category = _dbContext.Categories.Find(id);
                if (category == null)
                {
                    return NotFound("Belirtilen ID ile kategori bulunamadı.");
                }

                return View("GetCategory", category);
            }
            catch (DbUpdateException ex)
            {
                // Veritabanı işlemi sırasında bir hata oluştuğunda buraya düşer
                // Hatanın loglanması veya özel bir mesaj döndürülmesi yapılabilir
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {

                // Hatanın loglanması veya özel bir mesaj döndürülmesi yapılabilir

                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateCategory(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _dbContext.Categories.Find(category.Id);
                    if (categories != null)
                    {
                        categories.Name = category.Name;
                        _dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kategori bulunamadı.");
                        return View(category);
                    }
                }
                else
                {
                    return View(category);
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                ModelState.AddModelError("", "Kategori güncellenirken bir hata oluştu: " + ex.Message);
                return View(category);
            }
        }
    }
}