using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using AlarmSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlarmSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private IDeviceService deviceService;

        public DeviceController(IDeviceService _deviceService)
        {
            deviceService = _deviceService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetDevices")]
        public async Task<IActionResult> GetDevices()
        {
            var (error, devices) = await deviceService.GetDevices();
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(devices);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("AddDevice/{number}")]
        public async Task<IActionResult> AddDevice([FromRoute] int number)
        {
            var (error, devices) = await deviceService.AddDevice(number);
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(devices);  
        }

        [Authorize(Roles = "User")]
        [HttpPost("ConnectDevice")]
        public async Task<IActionResult> ConnectDevice(dtoUserDevice dtoUserDevice)
        {
            var error = await deviceService.ConnectDevice(dtoUserDevice);
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(new { message = error.Name });
        }

        [Authorize(Roles = "Device")]
        [HttpPost("ReciveDeviceResponse")]
        public async Task<IActionResult> ReciveDeviceResponse(DeviceResponse deviceResponse)
        {
            var error = await deviceService.ReciveDeviceResponse(deviceResponse);
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(new { message = error.Name });
        }

        //[HttpGet("Email")]
        //public async Task<IActionResult> SendEmail()
        //{
        //    var error = await deviceService.SendEmail();
        //    return Ok(error);
        //}

        [Authorize(Roles = "User")]
        [HttpPost("GetLastResponse")]
        public async Task<IActionResult> GetLastResponse([FromBody] Guid deviceId)
        {
            var (error, lastResponse) = await deviceService.GetLastResponse(deviceId);
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(lastResponse);
        }

        [Authorize(Roles = "User")]
        [HttpPost("GetUserDevices")]
        public async Task<IActionResult> GetUserDevices([FromBody] string email)
        {
            var (error, devices) = await deviceService.GetUserDevices(email);
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(devices);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("DeleteDevice/{deviceId}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] Guid deviceId)
        {
            var error= await deviceService.DeleteDevice(deviceId);
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(new {message = error.Name });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteDeviceAdmin/{deviceId}")]
        public async Task<IActionResult> DeleteDeviceAdmin([FromRoute] Guid deviceId)
        {
            var error = await deviceService.DeleteDeviceAdmin(deviceId);
            if (error.Status == true)
                return BadRequest(new { name = error.Name });
            return Ok(new { message = error.Name });
        }

    }
}
