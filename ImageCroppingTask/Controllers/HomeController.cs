using ImageCroppingTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ImageCroppingTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
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

        [HttpPost]
        public IActionResult SaveCroppedImage(string filename, IFormFile blob)
        {
            try{
                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    var uploadDir = @"images";
                    filename = Guid.NewGuid().ToString() + "-" + filename;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, uploadDir, filename);
                    image.Mutate(x => x.Resize(300, 300));
                    image.Save(path);
                }
                return Json(new { Message = "OK" });
            }
            catch (Exception)
            {
                return Json(new { Message = "ERROR" });
            }
        }
    }
}