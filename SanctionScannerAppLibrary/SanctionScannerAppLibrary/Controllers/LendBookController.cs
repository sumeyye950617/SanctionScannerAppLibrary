using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanctionScannerAppLibrary.Models.Entities;

namespace SanctionScannerAppLibrary.Controllers
{
    public class LendBookController : Controller
    {

        private readonly DBLIBRARYContext _dbContext;
        private readonly ILogger _logger;

        public LendBookController(DBLIBRARYContext dbContext, ILogger<LendBookController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public ActionResult Index()
        {
            IQueryable<Transaction> transactionQuery = _dbContext.Transactions.Where(y=>y.IsTransaction==false)
               .Include(b => b.EmployeeNoNavigation)
               .Include(b => b.UserNoNavigation)
               .Include(b => b.BookNoNavigation);
            



            var transaction = transactionQuery
                .OrderBy(x => x.InsertTime)
                .ToList();

            return View(transaction);
        }

        [HttpGet]
        public ActionResult LendBook()
        {
            // Kullanıcıların listesi
            var userList = _dbContext.Users.Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.LastName,
                Value = x.Id.ToString()
            }).ToList();

            // IsActive True olanlar sadece kütüphane de olanları seçebilmeli 
            var bookList = _dbContext.Books
                .Where(x => x.IsActive == true)
                .Select(y => new SelectListItem
                {
                    Text = y.Name,
                    Value = y.Id.ToString()
                }).ToList();

            // Personellerin listesi
            var employeeList = _dbContext.Employees.Select(z => new SelectListItem
            {
                Text = z.Employee1,
                Value = z.Id.ToString()
            }).ToList();

            // ViewBag'de taşıma işlemi
            ViewBag.UserList = userList;
            ViewBag.BookList = bookList;
            ViewBag.EmployeeList = employeeList;

            return View();
        }
        [HttpPost]
        public ActionResult LendBook(Transaction transaction)
        {
            try
            {
                // Kullanıcı, kitap ve personel nesnelerini veritabanından alıyoruz
                var users = _dbContext.Users.Find(transaction.UserNo);
                var books = _dbContext.Books.Find(transaction.BookNo);
                var employee = _dbContext.Employees.Find(transaction.EmployeeNo);

                // Eğer herhangi bir nesne bulunamazsa uygun bir hata mesajıyla birlikte View'e yönlendirilir
                if (users == null || books == null || employee == null)
                {
                    throw new Exception("Kullanıcı, kitap veya personel bulunamadı.");
                }


                transaction.UserNoNavigation = users;
                transaction.BookNoNavigation = books;
                transaction.EmployeeNoNavigation = employee;
                transaction.PurchaseDate=DateTime.Now; 
                _dbContext.Transactions.Add(transaction);
                _dbContext.SaveChanges();


                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);
                return View(transaction);
            }
        }
        public ActionResult ReturnBook(int? id)
        {
            // var transaction = _dbContext.Transactions.Find(id);

            var transaction = _dbContext.Transactions
                .Where(t => t.Id == id) // ID'ye göre işlemi filtrele
                .Include(t => t.EmployeeNoNavigation)
                .Include(t => t.UserNoNavigation)
                .Include(t => t.BookNoNavigation)
                .FirstOrDefault();

            if (transaction == null)
            {
                ViewBag.Error = "İşlem bulunamadı.";
                return View("ReturnBook");
            }

            DateTime iadeTarihi = transaction.ReturnDate ?? DateTime.Now;


            TimeSpan fark = DateTime.Now.Date - iadeTarihi.Date;

            ViewBag.dgr = fark.TotalDays; 

            return View("ReturnBook", transaction);
        }
        public ActionResult UpdateLendBook(int id, DateTime userReturnDate)
        {
            try
            {
          
                var lend = _dbContext.Transactions.Find(id);
                if (lend == null)
                {
                    return NotFound();
                }

                
                lend.UserReturnBook = DateTime.Now;
                lend.IsTransaction = true;

       
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                return View("Error");
            }
        }
    }
}
