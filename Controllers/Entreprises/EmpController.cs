using Microsoft.AspNetCore.Mvc;
using Npgsql;
using SIProject.Models.Cnx;
using SIProject.Models.Entreprises;
using SIProject.Models.ViewModel;

namespace SIProject.Controllers.Entreprises
{
    public class EmpController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public EmpController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SaveHome()
        {

            return View();
        }

        public async Task<IActionResult> SaveEmp(AjoutEmpViewModel model)
        {
            if (model.nom==null || model.adresse==null)
            {
                ViewData["EmptyFieldError"] = true;
                Console.Error.WriteLine("ERROR: \n MODEL.NOM ET/OU MODEL.ADRESSE SONT NULL");
                return View("SaveHome");
            }
            int affectedRow = 0;
            NpgsqlConnection cnx = Connex.getConnection();
            cnx.Open();

            var imgName = "";
            string imgType = "";
            if (model.photo != null)
            {
                imgName = Guid.NewGuid().ToString() + Path.GetExtension(model.photo.FileName);
                imgType = model.photo.ContentType;
            }
            Emp newEmp = new Emp(model.nom,model.prenoms,model.datenaissance,model.genre,model.adresse,
                model.email,model.contact,imgType,imgName,model.idPoste);

            try
            {
                affectedRow = newEmp.CreateEmp(cnx);

                // ----------- UPLOAD FICHIER IMAGE(LOGO) -------------
                if (model.photo != null)
                {
                    var wwwRootPath = _hostEnvironment.WebRootPath;
                    var imgUploadFolder = Path.Combine(wwwRootPath, "img/uploads/employe");
                    if (!Directory.Exists(imgUploadFolder))
                    {
                        Directory.CreateDirectory(imgUploadFolder);
                    }
                    var imgPath = Path.Combine(imgUploadFolder, imgName);
                    using (var fileStream = new FileStream(imgPath, FileMode.Create))
                    {
                        await model.photo.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                cnx.Close();
                Console.Error.WriteLine("ERROR: \n"+e);

                ViewData["CreateEmpError"] = e.Message;
                return View("SaveHome");
            }

            cnx.Close();

            return View("SaveHome");
        }
    }
}
