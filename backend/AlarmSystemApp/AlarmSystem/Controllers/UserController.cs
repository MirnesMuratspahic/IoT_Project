using AlarmSystem.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AlarmSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AlarmSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            userService = _userService;
        }


        [HttpPost("Registration")]
        public async Task<IActionResult> UserRegistration(dtoUserRegistration user)
        {
            var errorStatus = await userService.UserRegistration(user);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin(dtoUserLogin user)
        {
            var (errorStatus , token) = await userService.UserLogin(user);
            if (errorStatus.Status == true)
                return BadRequest(new { name = errorStatus.Name });
            return Ok(token);

        }

        [HttpPost("GetUserInformations")]
        public async Task<IActionResult> GetUserInformations([FromBody] string email)
        {
            var (errorStatus, user) = await userService.GetUserInformations(email);
            if (errorStatus.Status == true)
                return BadRequest(errorStatus.Name);
            return Ok(user);

        }

        [HttpPut("UpdateUserInformations")]
        public async Task<IActionResult> UpdateUserInformations(dtoUserInformations userInformations)
        {
            var errorStatus = await userService.UpdateUserInformations(userInformations);
            if(errorStatus.Status == true)  
                return BadRequest(errorStatus.Name);
            return Ok(errorStatus.Name);
        }


        [HttpPost("AcceptUserCode")]
        public async Task<IActionResult> AcceptUserCode(dtoUserCode dtoUserCode)
        {
            var errorStatus = await userService.AcceptUserCode(dtoUserCode);
            if (errorStatus.Status == true)
                return BadRequest(new { name = errorStatus.Name });
            return Ok(new { name = errorStatus.Name });
        }


    }
}
