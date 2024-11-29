using AlarmSystem.Context;
using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using AlarmSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlarmSystem.Services
{
    public class DeviceService : IDeviceService
    {
        public ApplicationDbContext dbContext { get; set; }
        public IEmailService emailService { get; set; }

        public ErrorProvider error = new ErrorProvider() { Status = false };
        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "The property must not be null" };
        public ErrorProvider userNotFoundError = new ErrorProvider() { Status = true, Name = "User not found!" };
        public ErrorProvider deviceNotFoundError = new ErrorProvider() { Status = true, Name = "Device not found!" };

        private static DateTime lastEmailSentTime = DateTime.MinValue; 
        private static readonly TimeSpan emailSendInterval = TimeSpan.FromMinutes(1);

        private static DateTime lastEmailSentTime2 = DateTime.MinValue;


        public DeviceService(ApplicationDbContext _dbContext, IEmailService _emailService)
        {
            dbContext = _dbContext;
            emailService = _emailService;
        }

        public async Task<(ErrorProvider, List<Device>)> GetDevices()
        {
            var devices = await dbContext.Devices.ToListAsync();

            if(devices == null || devices.Count == 0)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "Database is empty!"
                };
                return (error, null);
            }

            return (error, devices);
        }


        public async Task<(ErrorProvider, List<Device>)> AddDevice(int number)
        {
            for (int i = 0; i < number; i++)
            {
                var device = new Device()
                {
                    DeviceId = Guid.NewGuid(),
                    Name = await GenerateDeviceName(),
                    Status = "Inactive",
                    RegisteredAt = DateTime.Now
                };

                await dbContext.Devices.AddAsync(device);
                await dbContext.SaveChangesAsync();
            }

            var devices = await dbContext.Devices.ToListAsync();

            return (error, devices);

        }

        private async Task<string> GenerateDeviceName()
        {
            var lastDevice = await dbContext.Devices
                .OrderByDescending(d => d.RegisteredAt)
                .FirstOrDefaultAsync();

            if (lastDevice != null)
            {
                var lastNumber = int.Parse(lastDevice.Name.Split(' ')[1]);
                return $"Temperature {lastNumber + 1}";
            }

            return "Temperature 1";
        }

        //public async Task<string> SendEmail()
        //{
        //    await emailService.SendEmail("Mail poslan!");
        //    return "Poslano";
        //}

        public async Task<ErrorProvider> ConnectDevice(dtoUserDevice dtoUserDevice)
        {
            if (dtoUserDevice == null)
                return defaultError;

            var userFromDatabase = await dbContext.Users.FirstOrDefaultAsync(x=>x.Email == dtoUserDevice.UserEmail);

            if (userFromDatabase == null)
                return userNotFoundError;

            var deviceFromDatabase = await dbContext.Devices.FirstOrDefaultAsync(x=>x.DeviceId == dtoUserDevice.DeviceId);

            if (deviceFromDatabase == null)
                return deviceNotFoundError;

            if(deviceFromDatabase.Status == "Active")
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "Device already connected!"
                };
                return error;
            }

            var userDevice = new UserDevice()
            {
                User = userFromDatabase,
                Device = deviceFromDatabase,
                RegistrationDateTime = DateTime.Now
            };

            var deviceResponse = new DeviceResponse()
            {
                DeviceId = dtoUserDevice.DeviceId,
                Temperature = -100,
                MotionDetected = 0,
                ReadingDateTime = DateTime.MinValue
            };

            deviceFromDatabase.Status = "Active";

            await dbContext.UserDevices.AddAsync(userDevice);
            await dbContext.DeviceResponses.AddAsync(deviceResponse);
            await dbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Device successfully connected!"
            };

            return error;

        }

        public async Task<ErrorProvider> ReciveDeviceResponse(DeviceResponse deviceResponse)
        {
            if(deviceResponse == null) 
                return defaultError;

            var deviceFromDatabase = await dbContext.Devices.FirstOrDefaultAsync(x => x.DeviceId == deviceResponse.DeviceId);

            if (deviceFromDatabase == null)
                return deviceNotFoundError;

            deviceResponse.ReadingDateTime = DateTime.Now;

            await dbContext.DeviceResponses.AddAsync(deviceResponse);
            await dbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Data recived!"
            };

            var dataFromDatabase =  dbContext.DeviceResponses.Where(x => x.DeviceId == deviceResponse.DeviceId).Count();

            if(dataFromDatabase >= 100)
                await DeleteData(deviceResponse.DeviceId);


            var userDevice = await dbContext.UserDevices.Include(x => x.User).Include(x => x.Device).Where(x=>x.Device.DeviceId == deviceResponse.DeviceId).FirstOrDefaultAsync();
            if (userDevice == null)
                return deviceNotFoundError;

            var userEmail = userDevice.User.Email;

            if (deviceResponse.Temperature >= 20 && DateTime.Now - lastEmailSentTime > emailSendInterval)
            {
                await emailService.SendEmail("Fire", userEmail);
                lastEmailSentTime = DateTime.Now; 
            }
            
            if(deviceResponse.MotionDetected == 1 && DateTime.Now - lastEmailSentTime2 > emailSendInterval) 
            {
                await emailService.SendEmail("Motion", userEmail);
                lastEmailSentTime2 = DateTime.Now;
            }


            return error;
        }

        private async Task DeleteData(Guid deviceId)
        {
            var dataFromDatabase = await dbContext.DeviceResponses.Where(x => x.DeviceId == deviceId).ToListAsync();
            if(dataFromDatabase.Count > 0) 
            {
                dbContext.RemoveRange(dataFromDatabase);
                await dbContext.SaveChangesAsync();
            }

        }


        public async Task<(ErrorProvider, DeviceResponse)> GetLastResponse(Guid deviceId)
        {
            if (deviceId == null)
                return (defaultError, null);

            var deviceFromDatabase = await dbContext.Devices.FirstOrDefaultAsync(x => x.DeviceId == deviceId);

            if (deviceFromDatabase == null)
                return (deviceNotFoundError, null);

            var lastResponse = await dbContext.DeviceResponses.OrderByDescending(x => x.ReadingDateTime).FirstOrDefaultAsync(x => x.DeviceId == deviceId);

            if(lastResponse  == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "None response in database!"
                };
                return (error, null);
            }

            return (error, lastResponse);


        }

        public async Task<(ErrorProvider, List<Device>)> GetUserDevices(string email)
        {
            if(email == null)
                return (defaultError, null);

            var userFromDatabase = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

            if(userFromDatabase == null)
                return (userNotFoundError, null);

            var userDevices = await dbContext.UserDevices.Where(x=>x.User.Email == email).Select(x=> x.Device).ToListAsync();

            if (userDevices == null)
                return (deviceNotFoundError, null);

            return (error, userDevices);

        }

        public async Task<ErrorProvider> DeleteDevice(Guid deviceId)
        {
            if (deviceId == null)
                return defaultError;

            var deviceFromDatabase = await dbContext.UserDevices.Include(x=>x.User).Include(x=>x.Device).FirstOrDefaultAsync(x=>x.Device.DeviceId == deviceId);

            if (deviceFromDatabase == null)
                return deviceNotFoundError;

            deviceFromDatabase.Device.Status = "Inactive";
            dbContext.UserDevices.Remove(deviceFromDatabase);
            await dbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Successfully removed!"
            };

            return error;
        }

        public async Task<ErrorProvider> DeleteDeviceAdmin(Guid deviceId)
        {
            if (deviceId == null)
                return defaultError;

            var deviceFromDatabase = await dbContext.Devices.FirstOrDefaultAsync(x=> x.DeviceId == deviceId);

            if (deviceFromDatabase == null)
                return deviceNotFoundError;

            dbContext.Devices.Remove(deviceFromDatabase);
            await dbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Successfully removed!"
            };

            return error;
        }

    }

}
