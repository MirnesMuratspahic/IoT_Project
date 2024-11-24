using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using AlarmSystem.Services.Interfaces;
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

        [HttpGet("GetDevices")]
        public async Task<IActionResult> GetDevices()
        {
            var (error, devices) = await deviceService.GetDevices();
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(devices);
        }


        [HttpGet("AddDevice")]
        public async Task<IActionResult> AddDevice()
        {
            var (error, device) = await deviceService.AddDevice();
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(device);  
        }

        [HttpPost("ConnectDevice")]
        public async Task<IActionResult> ConnectDevice(dtoUserDevice dtoUserDevice)
        {
            var error = await deviceService.ConnectDevice(dtoUserDevice);
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(error.Name);
        }

        [HttpPost("ReciveDeviceResponse")]
        public async Task<IActionResult> ReciveDeviceResponse(DeviceResponse deviceResponse)
        {
            var error = await deviceService.ReciveDeviceResponse(deviceResponse);
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(error.Name);
        }

        //[HttpGet("Email")]
        //public async Task<IActionResult> SendEmail()
        //{
        //    var error = await deviceService.SendEmail();
        //    return Ok(error);
        //}

        [HttpPost("GetLastResponse")]
        public async Task<IActionResult> GetLastResponse([FromBody] Guid deviceId)
        {
            var (error, lastResponse) = await deviceService.GetLastResponse(deviceId);
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(lastResponse);
        }

        [HttpPost("GetUserDevices")]
        public async Task<IActionResult> GetUserDevices([FromBody] string email)
        {
            var (error, devices) = await deviceService.GetUserDevices(email);
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(devices);
        }

        [HttpDelete("DeleteDevice/{deviceId}")]
        public async Task<IActionResult> DeleteDevice([FromRoute] Guid deviceId)
        {
            var error= await deviceService.DeleteDevice(deviceId);
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(new {message = error.Name });
        }

    }
}
