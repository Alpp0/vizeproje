using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using ÜniversiteProje.Models.Entity; // Kullanıcı modelinizin olduğu namespace




namespace ÜniversiteProje.Controllers
{
    public class HomeController : Controller
    {
        ProjeEntities1 db = new ProjeEntities1(); // Veritabanı bağlantısını buraya ekleyin

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(string ad, string soyad, string bolum)
        {
            try
            {
                // Bağlantı dizesi
                string connectionString = "Data Source=LAPTOP-L4NM2DDD\\SQLEXPRESS; Initial Catalog=Proje; Integrated Security=True";

                // SQL sorgusu
                string query = "INSERT INTO Ogretmn (Ad, Soyad, Bolum) VALUES (@ad, @soyad, @bolum)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametrelerin eklenmesi
                        command.Parameters.AddWithValue("@ad", ad);
                        command.Parameters.AddWithValue("@soyad", soyad);
                        command.Parameters.AddWithValue("@bolum", bolum);

                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Başarılı ise ana sayfaya yönlendir
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // Başarısız ise hata sayfasına yönlendir
                            return RedirectToAction("Error", "Home");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda, hata mesajını model hata olarak ekle ve öğretmen ekleme sayfasını tekrar göster
                ModelState.AddModelError("", "An error occurred while adding the teacher: " + ex.Message);
                return View();
            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Ad == username && u.Soyad == password);

            if (user != null)
            {
                // Kullanıcı doğrulandı, oturum başlat
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("About", "Home"); // Başarılı girişte About sayfasına yönlendir
            }
            else
            {
                // Kullanıcı doğrulanamadı, hata mesajı göster
                ModelState.AddModelError("", "Invalid username or password");
                return View();
            }
        }



        public ActionResult Ekle(int id, string ad, string soyad)
        {
            // Veritabanı bağlantı dizesi
            string connectionString = "Data Source = LAPTOP-L4NM2DDD\\SQLEXPRESS; Initial Catalog = Proje; Integrated Security = True";


            // SQL sorgusu
            string query = "INSERT INTO Ogrenciler (id, Ad, Soyad) VALUES (@id, @ad, @soyad)";

            // Bağlantı ve komut oluşturma
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Parametreleri ekleyerek SQL sorgusunu hazırla
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@ad", ad);
                command.Parameters.AddWithValue("@soyad", soyad);

                // Bağlantıyı aç
                connection.Open();

                // Komutu çalıştır ve etkilenen satır sayısını al
                int rowsAffected = command.ExecuteNonQuery();

                // Etkilenen satır sayısına göre işlem başarılı mı kontrol et
                if (rowsAffected > 0)
                {
                    // Başarılı bir şekilde öğrenci eklendiğinde, kullanıcıyı başka bir sayfaya yönlendir
                    return RedirectToAction("Ogrenciler", "Home");
                }
                else
                {
                    // Hata durumunda gerekli işlemleri yapabilirsiniz
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        public ActionResult Ogrenciler()
        {
            var ogrenciler = db.Ogrenciler.ToList();
            return View(ogrenciler);
        }
        public ActionResult Delete(int id)
        {
            var ogrenci = db.Ogrenciler.Find(id);
            db.Ogrenciler.Remove(ogrenci);
            db.SaveChanges();
            return RedirectToAction("Ogrenciler");
        }


        public ActionResult Ogretmn()
       
        {
            var Ogretmn = db.Ogretmn.ToList();
            return View(Ogretmn);
        }
        public ActionResult Sil(int id)
        {
            var Ogretmn = db.Ogretmn.Find(id);
            db.Ogretmn.Remove(Ogretmn);
            db.SaveChanges();
            return RedirectToAction("Ogretmn");
        }
    }
}



    
    

