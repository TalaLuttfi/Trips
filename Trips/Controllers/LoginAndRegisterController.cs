using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Trips.Models;

namespace Trips.Controllers
{
    public class LoginAndRegisterController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        private readonly DbTripsContext _context;
        private readonly IWebHostEnvironment
        _hostEnvironment;
        public LoginAndRegisterController(DbTripsContext
        context, IWebHostEnvironment _hostEnvironment)
        {
            this._hostEnvironment = _hostEnvironment;
            _context = context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Fullname,Username,UPassword,PhoneNum,Gender,Imagepath,ImageFile,RoleId")] User user)
        {
            if (ModelState.IsValid)
            {
                //Add Customer Details
                string wwwRootPath = _hostEnvironment.WebRootPath;
               string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await user.ImageFile.CopyToAsync(filestream);

                }
                user.Imagepath = fileName;

                user.RoleId = 2;
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "LoginAndRegister");
            }
            return View(user);
        }




        [HttpPost]
        public IActionResult Login([Bind("Username ,UPassword ")] User user)
             {var auth = _context.Users.Where(x =>  x.Username == user.Username && x.UPassword == user.UPassword).SingleOrDefault();
            if (auth != null)
            {
                HttpContext.Session.SetInt32("CustomerId", (int)auth.Id);
                HttpContext.Session.SetString("AdminName", auth.Username);


                // 1 > customer 
                // 2 > admin 
                // 3 > employee 
                switch (auth.RoleId)
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                        
                        
                     case 2:
                        return RedirectToAction("User", "Home");
                  }
            }
            return View();
        }








    }
            }

		