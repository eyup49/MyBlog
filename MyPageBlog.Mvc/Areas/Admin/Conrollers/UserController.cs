using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPageBlog.Entities.Concreate;
using MyPageBlog.Entities.Dtos;
using MyPageBlog.Mvc.Areas.Admin.Models;
using MyPageBlog.Shared.Utilities.Extensions;
using MyPageBlog.Shared.Utilities.Results.Abstract.ComplexTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyPageBlog.Mvc.Areas.Admin.Conrollers
{
    [Area("Admin")]
    
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper, SignInManager<User> signInManager = null)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
            _signInManager = signInManager;
        }
        //[Authorize(Roles = "Admin,Editor,Eyup")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                 ResultStatus=ResultStatus.Success
            }) ;
        }

        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserAddDto userAddDto)
        {
            
            if (!ModelState.IsValid)
            {
                return View(userAddDto);
            }
      
            
            userAddDto.Picture = await ImageUpload(userAddDto.UserName, userAddDto.PictureFile);
            var user = _mapper.Map<User>(userAddDto);
            var result = await _userManager.CreateAsync(user, userAddDto.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("User", "Login");
            }
            return View(userAddDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
            
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
               

                if (user!=null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "E-Posta adresiniz veya şifreniz yanlıştır");
                        return View("UserLogin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-Posta adresiniz veya şifreniz yanlıştır");
                    return View("UserLogin");
                }
            }
            else
            {
                return View("UserLogin");
            }
            
        }

        
        [HttpGet]
       
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDto=JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            },new JsonSerializerOptions { 
            ReferenceHandler= ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        
        [HttpGet]
     
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

       
        [HttpPost]
    
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto.UserName,userAddDto.PictureFile);
                var user = _mapper.Map<User>(userAddDto);
                var result = await _userManager.CreateAsync(user, userAddDto.Password);
                if (result.Succeeded)
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı adına sahip , kullanıcı başarıyla eklenmiştir",
                            User=user
                        },
                        UserAddPartial=await this.RenderViewToStringAsync("_UserAddPartial",userAddDto)
                    }) ;
                    return Json(userAddAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxErrorModel);

                }
                
            }
            var userAddAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxModelStateErrorModel);
        }


       
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus=ResultStatus.Success,
                    Message=$"{user.UserName} adlı kullanıcı adına sahip kullanıcı başarıyla silinmiştir.",
                    User=user
                });
                return Json(deletedUser);
            }
            else
            {
                string errorMesages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMesages = $"*{error.Description}\n";
                }
                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} adlı kullanıcı silinirken bazı hatalar oluştu.\n{errorMesages}",
                    User = user
                });
                return Json(deletedUserErrorModel);

            }

        }

       
        [HttpGet]
        
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(c => c.Id == userId);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
        }

      
        [HttpPost]
     
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldUserPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    isNewPictureUploaded = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded)
                    {
                        ImageDelete(oldUserPicture);
                    }
                    var userUpdateViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir",
                            User = updatedUser

                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateViewModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userUpdateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateErrorViewModel);
                }

            }
            else
            {
                var userUpdateStateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                });
                return Json(userUpdateStateErrorViewModel);
            }
        }

      
        public async Task<string> ImageUpload(string userName,IFormFile pictureFile)
        {
            string wwwroot = _env.WebRootPath;
            string fileExtensions = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            string fileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtensions}";
            var path = Path.Combine($"{wwwroot}/img", fileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }
            return fileName;
        }

    
        public bool ImageDelete(string pictureName)
        {
            string wwwroot = _env.WebRootPath;
            var fileToDelete = Path.Combine($"{wwwroot}/img", pictureName);
            if (System.IO.File.Exists(fileToDelete))
            {
                System.IO.File.Delete(fileToDelete);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
