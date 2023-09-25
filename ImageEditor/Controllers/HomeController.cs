using ImageEditor.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Security.Cryptography;

namespace ImageEditor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {

            FileBytesss = await Globals.ToByteArray(file.OpenReadStream());
            var img = Bitmap.FromStream(file.OpenReadStream());
            var mBytesTransparent = await Globals.MakeTransparent((Bitmap)img, Color.White, 0);
            FileBytes = mBytesTransparent;
            var response = new ResultResponse { Data = mBytesTransparent, StatusText = "Success" };
            return Json(response);
        }
        public byte[] FileBytes
        {
            get
            {
                var sessionVar = HttpContext.Session.GetString("FILEOUTPUT");
                var res = JsonConvert.DeserializeObject<byte[]>(sessionVar);

                return res;
            }
            set
            {
                HttpContext.Session.SetString("FILEOUTPUT", JsonConvert.SerializeObject(value));
            }
        }

        public byte[] FileBytesss
        {
            get
            {
                var sessionVar = HttpContext.Session.GetString("FILEZZ");
                var res = JsonConvert.DeserializeObject<byte[]>(sessionVar);

                return res;
            }
            set
            {
                HttpContext.Session.SetString("FILEZZ", JsonConvert.SerializeObject(value));
            }
        }

        public IActionResult Output()
        {
            ViewData["FILEDATA"] = FileBytes;
            return PartialView("_Output");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Editor()
        {
            // ViewData["FILEDATASS"] = FileBytesss;
            
                var request = HttpContext.Request;

                var host = request.Host.ToUriComponent();

                var pathBase = request.PathBase.ToUriComponent();

                ViewBag.BaseUrl =  $"{request.Scheme}://{host}{pathBase}";
            
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
    }
}