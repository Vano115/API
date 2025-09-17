using System.Threading.Tasks;
using API.Data.Models;
using API.Data.Queries;

namespace API.Data.Repos
{
    /// <summary>
    /// Интерфейс определяет методы для доступа к объектам типа Room в базе 
    /// </summary>
    public interface IRoomRepository
    {
        Task<Room> GetRoomByName(string name);

        Task<Room> GetRoomById(Guid id);
        Task AddRoom(Room room);

        Task UpdateRoom(Room room, UpdateRoomQuery query);
    }
}