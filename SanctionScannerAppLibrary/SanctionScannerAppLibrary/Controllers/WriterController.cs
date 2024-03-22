using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanctionScannerAppLibrary.Models.Entities;

namespace SanctionScannerAppLibrary.Controllers
{
    public class WriterController : Controller
    {
        private readonly DBLIBRARYContext _dbContext;
        private readonly ILogger _logger;


        public WriterController(DBLIBRARYContext dbContext, ILogger<WriterController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public ActionResult Index()
        {
            var writer = _dbContext.Writers.ToList();
            return View(writer);
        }

        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddWriter(Writer writer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Writers.Add(writer);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                // Hata sayfasına yönlendirme veya başka bir işlem yapabilirsiniz
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                ModelState.AddModelError("", "Veritabanına kayıt işlemi sırasında bir hata oluştu.");
                return View(writer);
            }

            // ModelState.IsValid false olduğunda buraya düşer
            return View(writer);
        }
        public ActionResult DeleteWriter(int? id)
        {
            try
            {
                var writer = _dbContext.Writers.Find(id);

                if (writer == null)
                {
                    return NotFound(); // Yazar bulunamadığında 404 hatası döndür
                }

                // İlişkili tablolarda silme işlemini gerçekleştirmek yerine IsActive false yapma
                // writer.IsActive = false;

                _dbContext.Writers.Remove(writer);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                return View("Error");
            }
        }
        public ActionResult GetWriter(int? id)
        {
            try
            {

                var writer = _dbContext.Writers.Find(id);
                if (writer == null)
                {
                    return NotFound("Belirtilen ID ile kategori bulunamadı.");
                }

                return View("GetWriter", writer);
            }
            catch (DbUpdateException ex)
            {
                // Veritabanı işlemi sırasında bir hata oluştuğunda buraya düşer
                // Hatanın loglanması veya özel bir mesaj döndürülmesi yapılabilir
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                // Beklenmeyen diğer hatalar için buraya düşer
                // Hatanın loglanması veya özel bir mesaj döndürülmesi yapılabilir
                return RedirectToAction("Error", "Home");
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult UpdateWriter(Writer writer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var writers = _dbContext.Writers.Find(writer.Id);
                    if (writers != null)
                    {
                        writers.FirstName = writer.FirstName;
                        writers.LastName = writer.LastName;
                        writers.Detail = writer.Detail;
                        _dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", " Yazar bulunamadı.");
                        return View(writer);
                    }
                }
                else
                {
                    return View(writer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                ModelState.AddModelError("", "Yazar güncellenirken bir hata oluştu: " + ex.Message);
                return View(writer);
            }
        }
        public ActionResult WriterBook(int id)
        {
            var writer = _dbContext.Books.Where(x => x.WriterNo == id).Include(y=>y.CategoryNoNavigation).ToList();
            var writerName = _dbContext.Writers.Where(y => y.Id == id).Select(z => z.FirstName + " " + z.LastName).FirstOrDefault();
            ViewBag.WriterName = writerName;
            return View(writer);
        }
    }
}
