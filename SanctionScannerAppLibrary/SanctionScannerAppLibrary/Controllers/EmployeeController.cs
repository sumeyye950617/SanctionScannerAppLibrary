using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanctionScannerAppLibrary.Models.Entities;

namespace SanctionScannerAppLibrary.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly DBLIBRARYContext _dbContext;

        public EmployeeController(DBLIBRARYContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ActionResult Index()
        {
            var employee = _dbContext.Employees.ToList();
            return View(employee);
        }
        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddEmployee(Employee employee)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var exist = _dbContext.Employees.FirstOrDefault(c => c.Employee1 == employee.Employee1);
                    if (exist != null)
                    {
                        ModelState.AddModelError("", "Bu isimde bir personel zaten mevcut.");
                        return View(employee);
                    }
                    _dbContext.Employees.Add(employee);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {

                // Hata sayfasına yönlendirme veya başka bir işlem yapabilirsiniz
                ModelState.AddModelError("", "Veritabanına kayıt işlemi sırasında bir hata oluştu.");
                return View(employee);
            }

            // ModelState.IsValid false olduğunda buraya düşer
            return View(employee);
        }
        public ActionResult DeleteEmployee(int? id)
        {
            try
            {
                var employee = _dbContext.Employees.Find(id);

                if (employee == null)
                {
                    return NotFound(); // Personel bulunamadığında 404 hatası döndür
                }

                // İlişkili tablolarda silme işlemini gerçekleştirmek yerine IsActive false yapma
                // employee.IsActive = false;

                _dbContext.Employees.Remove(employee);
                _dbContext.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View("Error");
            }
        }
        public ActionResult GetEmployee(int? id)
        {
            try
            {

                var employee = _dbContext.Employees.Find(id);
                if (employee == null)
                {
                    return NotFound("Belirtilen ID ile kategori bulunamadı.");
                }

                return View("GetEmployee", employee);
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
        public ActionResult UpdateEmployee(Employee employee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var categories = _dbContext.Employees.Find(employee.Id);
                    if (categories != null)
                    {
                        categories.Employee1 = employee.Employee1;
                        _dbContext.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Personel bulunamadı.");
                        return View(employee);
                    }
                }
                else
                {
                    return View(employee);
                }
            }
            catch (Exception ex)
            {
                
                ModelState.AddModelError("", "Personel güncellenirken bir hata oluştu: " + ex.Message);
                return View(employee);
            }
        }
    }
}
