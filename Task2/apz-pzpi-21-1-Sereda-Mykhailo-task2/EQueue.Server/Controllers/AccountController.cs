using EQueue.Db;
using EQueue.Db.Models;
using EQueue.Server.Controllers.Base;
using EQueue.Server.Services.Interfaces;
using EQueue.Shared;
using EQueue.Shared.Dto;
using EQueue.Shared.Exceptions;
using EQueue.Shared.ServiceResponseHandling;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EQueue.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IValidationService _validationService;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole<long>> roleManager, 
            ApplicationDbContext dbContext, ITokenGenerator tokenGenerator, IValidationService validationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _tokenGenerator = tokenGenerator;
            _validationService = validationService;

        }

        [HttpPost("create")]
        public async Task<IActionResult> SignUp(Credentials credentials, CancellationToken cancellationToken)
        {
            try
            {
                var validationResponse = await _validationService.ValidateAsync(credentials);
                if (!validationResponse.IsSuccess)
                {
                    return BadRequest(validationResponse.ErrorMessage);
                }
                var identityUser = new User()
                {
                    UserName = credentials.Login
                };

                var anyUsers = await _dbContext.Users.AnyAsync(cancellationToken: cancellationToken);

                var createResult = await _userManager.CreateAsync(identityUser, credentials.Password);
                if (createResult.Succeeded is false)
                {
                    return BadRequest(createResult.Errors.First().Description);
                }
                
                if (!anyUsers)
                {
                    await AssureRoleCreatedAsync(Roles.Admin);
                    await _userManager.AddToRoleAsync(identityUser, Roles.Admin);
                }

                return Ok();
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }

        private async Task AssureRoleCreatedAsync(string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return;
            }
            await _roleManager.CreateAsync(new IdentityRole<long>(role));
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(Credentials credentials, CancellationToken cancellationToken)
        {
            try
            {
                var validationResponse = await _validationService.ValidateAsync(credentials);
                if (!validationResponse.IsSuccess)
                {
                    return BadRequest(validationResponse.ErrorMessage);
                }
                var identityUser = await _userManager.FindByNameAsync(credentials.Login);
                if (identityUser is null)
                {
                    return BadRequest("User not found");
                }

                var isCredentialsValid = await _userManager.CheckPasswordAsync(identityUser, credentials.Password);
                if (!isCredentialsValid)
                {
                    return BadRequest("Wrong password");
                }

                var token = await _tokenGenerator.GenerateAsync(identityUser);
                return Ok(new LoginResultDto() { Login = credentials.Login, Token = token });
            }
            catch
            {
                throw new UnknownErrorException();
            }
        }
    }
}
