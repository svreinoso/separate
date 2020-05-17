using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Validation;
using Separate.Data;
using Separate.Data.Entities;
using Separate.Data.Enums;
using Separate.Models;

namespace Separate.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger _logger;
        private static bool _databaseChecked;

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<User> userManager,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        //
        // POST: /Account/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            EnsureDatabaseCreated(_applicationDbContext);
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    // return StatusCode(StatusCodes.Status409Conflict);
                    return new BadRequestObjectResult(new ApiResponseModel{
                        Success = false,
                        Message = "This email is already in use."
                    });
                }

                user = new User { 
                    UserName = model.Email, 
                    Email = model.Email, 
                    FirstName = model.FirstName, 
                    LastName = model.LastName 
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, AppRoles.Client);
                    return new OkObjectResult( new ApiResponseModel {
                        Success = true
                    });
                }
                AddErrors(result);
            }

            // If we got this far, something failed.
            return BadRequest(ModelState);
        }

        [HttpGet("Me")]
        public async Task<IActionResult> Me () 
        {
            var currentUserId = _userManager.GetUserId(User);
            _logger.LogError(currentUserId);
            if (string.IsNullOrEmpty(currentUserId)) {
                return new BadRequestResult();
            }
            var user = await _applicationDbContext.Users.FindAsync(currentUserId);
            var roles = await _userManager.GetRolesAsync(user);
            return new OkObjectResult(new {
                Roles = roles,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            });
        }
        #region Helpers

        // The following code creates the database and schema if they don't exist.
        // This is a temporary workaround since deploying database through EF migrations is
        // not yet supported in this release.
        // Please see this http://go.microsoft.com/fwlink/?LinkID=615859 for more information on how to do deploy the database
        // when publishing your application.
        private static void EnsureDatabaseCreated(ApplicationDbContext context)
        {
            if (!_databaseChecked)
            {
                _databaseChecked = true;
                context.Database.EnsureCreated();
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
