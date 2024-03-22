using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanctionScannerAppLibrary.Models.Entities;
using X.PagedList;

namespace SanctionScannerAppLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly DBLIBRARYContext _dbContext;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(DBLIBRARYContext dbContext, ILogger<BookController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        public ActionResult Index(string b, int page = 1)
        {
            IQueryable<Book> booksQuery = _dbContext.Books
     .Include(b => b.CategoryNoNavigation)
     .Include(b => b.WriterNoNavigation);

            // Arama butonu için 
            if (!string.IsNullOrEmpty(b))
            {
                booksQuery = booksQuery.Where(m => m.Name.Contains(b));
            }

            var books = booksQuery
                .OrderBy(x => x.Name)
                .ToList().ToPagedList(page, 4);

            return View(books);

        }

        [HttpGet]
        public ActionResult AddBook()
        {
            var categories = _dbContext.Categories.ToList();
            var writers = _dbContext.Writers
    .Select(w => new { Id = w.Id, FullName = w.FirstName + " " + w.LastName })
    .ToList();
            ViewBag.Category = new SelectList(categories, "Id", "Name");
            ViewBag.Writer = new SelectList(writers, "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBook(Book book, IFormFile file)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "resimler");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    book.Picture = uniqueFileName;
                }

                if (!ModelState.IsValid)
                {
                    return View(book);
                }

                var existingBook = _dbContext.Books.FirstOrDefault(b => b.Name == book.Name);
                if (existingBook != null)
                {
                    ModelState.AddModelError("", "Bu isimde bir kitap zaten mevcut.");
                    return View(book);
                }

                var category = _dbContext.Categories.FirstOrDefault(k => k.Id == book.CategoryNo);
                var writer = _dbContext.Writers.FirstOrDefault(k => k.Id == book.WriterNo);

                if (category == null)
                {
                    ModelState.AddModelError("", "Seçilen kategori geçersiz.");
                    return View(book);
                }

                if (writer == null)
                {
                    ModelState.AddModelError("", "Seçilen yazar geçersiz.");
                    return View(book);
                }

                book.CategoryNoNavigation = category;
                book.WriterNoNavigation = writer;

                _dbContext.Books.Add(book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);
                ModelState.AddModelError("", "Veritabanına kayıt işlemi sırasında bir hata oluştu: " + ex.Message);
                return View(book);
            }
        }
        public ActionResult DeleteBook(int? id)
        {
            try
            {
                if(id == null)
                {
                    return BadRequest();
                }
                var book = _dbContext.Books.Find(id);

                if (book == null)
                {
                    return NotFound(); // Kitap bulunamadığında 404 hatası döndür
                }

                // İlişkili tablolarda silme işlemini gerçekleştirmek yerine IsActive false yapma daha sağlıklı bir yaklaşımdır verilerimizi Kurumsal Hayatta özellikle silmek istemeyiz
                book.IsActive = false;

                //_dbContext.Books.Remove(book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                return View("Error");
            }
        }
    
        public ActionResult GetTransaction(int id)
        {
            var bookHistory = _dbContext.Transactions.Where(x => x.BookNo == id).Include(y=>y.UserNoNavigation).ToList(); // Kitap Hareketlerimizi takip Etmek İçin oluşturduk
            var userBook= _dbContext.Books.Where(y => y.Id == id).Select(z => z.Name).FirstOrDefault();
            ViewBag.UserBook = userBook;
            return View(bookHistory);
        }
    }
}
