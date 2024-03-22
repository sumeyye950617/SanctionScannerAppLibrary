using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanctionScannerAppLibrary.Models.Entities;
using X.PagedList;

namespace SanctionScannerAppLibrary.Controllers
{
    public class UserController : Controller
    {
        private readonly DBLIBRARYContext _dbContext;
        private readonly ILogger _logger;


        public UserController(DBLIBRARYContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public ActionResult Index(int sayfa = 1)
        {
            var users = _dbContext.Users.ToList().ToPagedList(sayfa, 3); // Sayfalama Özelliği yapmış olduk 
            return View(users);
        }
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddUser(User user)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var exist = _dbContext.Users.FirstOrDefault(c => c.FirstName == user.FirstName && c.Mail == user.Mail);
                    if (exist != null)
                    {
                        ModelState.AddModelError("", "Bu isimde bir kullanıcı zaten mevcut.");
                        return View(user);
                    }
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                // Hata sayfasına yönlendirme veya başka bir işlem yapabilirsiniz
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                ModelState.AddModelError("", "Veritabanına kayıt işlemi sırasında bir hata oluştu.");
                return View(user);
            }

            // ModelState.IsValid false olduğunda buraya düşer
            return View(user);
        }
        public ActionResult DeleteUser(int? id)
        {
            try
            {
                var user = _dbContext.Users.Find(id);

                if (user == null)
                {
                    return NotFound(); // Kullanıcı bulunamadığında 404 hatası döndür
                }

                // İlişkili tablolarda silme işlemini gerçekleştirmek yerine IsActive false yapma daha sağlıklıdır.
                //user.IsActive = false;

                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View("Error");
            }
        }
        public ActionResult GetUser(int? id)
        {
            try
            {

                var user = _dbContext.Users.Find(id);
                if (user == null)
                {
                    return NotFound("Belirtilen ID ile kategori bulunamadı.");
                }

                return View("GetUser", user);
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
        public ActionResult UpdateUser(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var users = _dbContext.Users.Find(user.Id);
                    if (users != null)
                    {
                        users.FirstName = user.FirstName;
                        users.LastName = user.LastName;
                        users.UserName = user.UserName;
                        users.School = user.School;
                        users.Picture = user.Picture;
                        users.Mail = user.Mail;
                        users.Phone = user.Phone;

                        _dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                        return View(user);
                    }
                }
                else
                {
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata oluştu: {ErrorMessage}", ex.Message);

                ModelState.AddModelError("", "Kullanıcı güncellenirken bir hata oluştu: " + ex.Message);
                return View(user);
            }
        }
        public ActionResult UserBookHistory(int? id)
        {
            var bookHistory = _dbContext.Transactions.Where(x => x.UserNo == id).Include(y => y.BookNoNavigation).ToList();
            var uyekit = _dbContext.Users.Where(y => y.Id == id).Select(z => z.FirstName + " " + z.LastName).FirstOrDefault();
            ViewBag.u1 = uyekit;
            return View(bookHistory);
        }
    }
}
