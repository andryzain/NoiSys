using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoiSys.Areas.Identity.Data;
using NoiSys.Areas.MasterData.Repository;
using NoiSys.Areas.MasterData.ViewModels;
using NoiSys.Repository;

namespace NoiSys.Areas.MasterData.Controllers
{
    [Area("MasterData")]
    [Route("MasterData/[Controller]/[Action]")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RoleController(
            RoleManager<IdentityRole> roleManager,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetUsers();
            return View(users);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> CreateRole()
        {
            var role = new RoleViewModel();               

            return View(role);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRole(RoleViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var newRole = new ApplicationRole
                {
                    Name = vm.RoleName,
                    //Module = vm.Module,
                    //Menu = vm.Menu,
                    //Keterangan = vm.Keterangan,
                };

                var result = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role " + vm.RoleName + " Berhasil Disimpan";
                    return RedirectToAction("Index", "Role");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailRole(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var role = _unitOfWork.Role.GetRoles();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = role.Select(roles => new SelectListItem(
                    roles.Name,
                    roles.Id,
                    userRoles.Any(ur => ur.Contains(roles.Name)))).ToList();

            var vm = new RoleViewModel
            {
                User = user,
                Roles = roleItems
            };

            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DetailRole(RoleViewModel vm)
        {
            var user = _unitOfWork.User.GetUser(vm.User.Id);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            //Lakukan perulangan pada roles di ViewModel
            //Cek pada Database apakah role yang sedang digunakan
            //Jika digunakan -> tidak perlu melakukan perubahan
            //Jika tidak digunakan -> tambahkan role baru

            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();

            foreach (var role in vm.Roles)
            {
                var roleCheked = userRoles.FirstOrDefault(ur => ur == role.Text);

                if (role.Selected)
                {
                    if (roleCheked == null)
                    {
                        //Tambahkan Checked Role Baru
                        rolesToAdd.Add(role.Text);
                        //await signInManager.UserManager.AddToRoleAsync(user, role.Text);
                    }
                }
                else
                {
                    if (roleCheked != null)
                    {
                        //Hapus Checked Role
                        rolesToDelete.Add(role.Text);
                        //await signInManager.UserManager.RemoveFromRoleAsync(user, role.Text);
                    }
                }
            }

            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToDelete.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }

            user.KodePengguna = vm.User.KodePengguna;
            user.NamaLengkap = vm.User.NamaLengkap;
            user.Email = vm.User.Email;

            _unitOfWork.User.UpdateUser(user);

            return RedirectToAction("Index", new { id = user.Id });
        }
    }
}
