using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using AlarmSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlarmSystem.Controllers
{
    [Route("api/[controller]")]
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


        [HttpPost("AddDevice")]
        public async Task<IActionResult> AddDevice(Device device)
        {
            var error = await deviceService.AddDevice(device);
            if (error.Status == true)
                return BadRequest(error.Name);
            return Ok(error.Name);  
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
    }
}
