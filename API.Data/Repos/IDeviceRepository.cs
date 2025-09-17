using System;
using System.Threading.Tasks;
using API.Data.Models;
using API.Data.Queries;

namespace API.Data.Repos
{
    /// <summary>
    /// Интерфейс определяет методы для доступа к объектам типа Device в базе 
    /// </summary>
    public interface IDeviceRepository
    {
        Task<Device[]> GetDevices();
        Task<Device> GetDeviceByName(string name);
        Task<Device> GetDeviceById(Guid id);
        Task SaveDevice(Device device, Room room);
        Task UpdateDevice(Device device, Room room, UpdateDeviceQuery query);
        Task DeleteDevice(Device device);
    }
}