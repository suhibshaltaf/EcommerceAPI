using Castle.Core.Smtp;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.DTO;
using Ecommerce.Core.IRepositories;
using Ecommerce.Core.IRepositories.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<LocalUser> userManager;
        private readonly IEmailServices emailServices;

        public UsersController(IUserRepository userRepository,UserManager<LocalUser> userManager,IEmailServices emailServices)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.emailServices = emailServices;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterationRequestDTO model)
        {
            try
            {
                bool uniqueEmail = userRepository.IsUniqueUser(model.Email);
                if (!uniqueEmail)
                {
                    return BadRequest(new ApiResponse(400, "Email already Exists !"));
                }
                var user= await userRepository.Register(model);
                if (user == null) {
                    return BadRequest(new ApiResponse(400, "Error while registeration user  !"));
                }
                else
                {
                    return Ok(new ApiResponse(201,result:user));
                }

            }
            catch (Exception ex)
            {
return StatusCode(500,new APIValidationResponse(new List<string>() { ex.Message,"an error occurred while processing your request " }));
                    }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (ModelState.IsValid)
            {
                var user=await userRepository.Login(model);
                if (user.User == null)
                {
                    return Unauthorized(new APIValidationResponse(new List<string>() { "Email or passwoed inCorrect" }, 401));

                }
                return Ok(user);

            }
            return BadRequest(new APIValidationResponse(new List<string>() { "please try to enter the email and password correctly" }, 400));
        }

        [HttpPost("sendEmail")]
        public async Task<IActionResult>SendEmailForUser(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            { 
                return BadRequest(new APIValidationResponse(new List<string> { $"This Email{email} not found :" }, 400));
            }
            var token =await userManager.GeneratePasswordResetTokenAsync(user);
            var ForgetPasswoedLink = Url.Action("RestPassword", "Users", new { token = token, email = user.Email }, Request.Scheme);
            var subject = "Rest Password Request";
            var massage = $" Please click on the Link to rest your password {ForgetPasswoedLink}";
            await emailServices.SendEmailAsync(user.Email,subject, massage);
            return Ok(new ApiResponse(200, "Password reset Link has been sent  to your email ...check your email  "));


        }
        [HttpPost("RestPassword")]
        public async Task<IActionResult> RestPassword([FromBody]RestPasswordDTO model)
        {
           
            if (ModelState.IsValid) 
            { var user= await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound(new ApiResponse(404, "Email incorrect "));


                }
                if (string.Compare(model.newPassword, model.confirmNewPassword) != 0)
                {
                    return BadRequest(new ApiResponse(400, "password  not Match"));
                }
                if (string.IsNullOrEmpty(model.Token))
                {
                    return BadRequest(new ApiResponse(400, "Token InValid"));

                }
                var result= await userManager.ResetPasswordAsync(user,model.Token,model.newPassword);
                if (result.Succeeded)
                {
                    return Ok(new ApiResponse(200, " Reseting  successfully"));
                }
                else {
                    return BadRequest(new ApiResponse(400, "erorr while reseting"));

                }
            }
            return BadRequest(new ApiResponse(400, "check your info")) ;
        }

        [HttpPost("reset-token")]
        public async Task<IActionResult> TokenToRestPassword([FromBody] string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return Ok(new {token=token});
        }
    }
}
