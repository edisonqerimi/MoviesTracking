using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Models;
using Movies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//User Administration
namespace Movies.Controllers
{
    [Authorize(Roles = "Admin")]
    public partial class AdministrationController : Controller
    {
        private readonly AppDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Roles()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Users()
        {
            var users = userManager.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult UserMovies(string id)
        {
            //var movies = context.Users.Where(x => x.Id == id).SelectMany(x => x.Movies).ToList();
            var movies = context.UserMovie.Include(m => m.Movie).Where(m => m.UserID == id).ToList();
            return View(movies);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
            }
            return RedirectToAction("Users");
        }
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var roles = await userManager.GetRolesAsync(user);
            var model = new EditUserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Roles = roles.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.UserName = model.Email;
                    user.City = model.City;
                    user.Email = model.Email;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Users");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                return View(model);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> RemoveRoleFromUser(string id, string role)
        {
            var applicationUser = await userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                var result = await userManager.RemoveFromRoleAsync(applicationUser, role);
                if (result.Succeeded)
                {
                    return RedirectToAction("EditUser", new { id });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return RedirectToAction("EditUser", new { id });

        }

        //Roles

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            var newRole = new EditRoleViewModel()
            {
                RoleName = role.Name,
                Id = role.Id
            };

            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    newRole.Users.Add(user.UserName);
                }
            }
            return View(newRole);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, EditRoleViewModel editedRole)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(id);

                if (role != null)
                {
                    if (role.Name != editedRole.RoleName)
                    {
                        role.Name = editedRole.RoleName;
                        var result = await roleManager.UpdateAsync(role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Roles");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "No changes detected!");
                    }

                }
                return View(editedRole);
            }
            return View();
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(model.RoleName);
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {

                return RedirectToAction("Roles");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("Roles");
        }

        public IActionResult AddUserToRole(string id)
        {
            var model = new AddUserToRoleViewModel() { Id = id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                var role = await roleManager.FindByIdAsync(model.Id);

                if (role != null && user != null)
                {
                    var result = await userManager.AddToRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("EditRole", new { id = model.Id });
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                ModelState.AddModelError("", "User doesn't exist");
                return View(model);
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserFromRole(string id, string user)
        {
            var roleName = await roleManager.GetRoleNameAsync(await roleManager.FindByNameAsync(id));
            /*
            var users = await userManager.GetUsersInRoleAsync(roleName);
            var userNames = new List<string>();
            foreach(var u in users)
            {
                userNames.Add(u.UserName);
            }
            var model = new EditRoleViewModel() { Id=id,Users=userNames,RoleName=roleName};*/

            var applicationUser = await userManager.FindByNameAsync(user);
            if (applicationUser != null)
            {
                var result = await userManager.RemoveFromRoleAsync(applicationUser, roleName);
                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole", new { id });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return RedirectToAction("EditRole", new { id });

        }

    }
}

/*        public async Task<IActionResult> AddUserToRole(string id)
             {
                 ViewBag.RoleId = id;
                 var role = await roleManager.FindByIdAsync(id);
                 var model = new List<AddUserToRoleViewModel>();
                 foreach (var user in userManager.Users.ToList())
                 {

                     var newRole = new AddUserToRoleViewModel()
                     {
                         Username = user.UserName,
                         UserId = user.Id,
                     };
                     if (await userManager.IsInRoleAsync(user, role.Name))
                     {
                         newRole.IsSelected = true;
                     }
                     else
                     {
                         newRole.IsSelected = false;
                     }
                     model.Add(newRole);
                 }
                 return View(model);
             }*/
/*        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToRole(List<AddUserToRoleViewModel> model, string id)
        {
            if (ModelState.IsValid)
            {
                var role = await roleManager.FindByIdAsync(id);
                for (int i = 0; i < model.Count; i++)
                {


                    IdentityResult result = null;
                    var newUser = await userManager.FindByIdAsync(model[i].UserId);
                    if (model[i].IsSelected && !(await userManager.IsInRoleAsync(newUser, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(newUser, role.Name);


                    }
                    else if (!model[i].IsSelected && await userManager.IsInRoleAsync(newUser, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(newUser, role.Name);
                    }
                    else
                    {
                        continue;
                    }
                    if (result.Succeeded)
                    {
                        if (i < model.Count - 1)
                            continue;
                        else
                            return RedirectToAction("EditRole", new { id });
                    }

                }
            }
            return View();
        }*/