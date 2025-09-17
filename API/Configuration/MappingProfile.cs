using AutoMapper;
using API.Contracts.Models.Home;
using API.Configuration;
using API.Contracts.Models.Devices;
using API.Contracts.Models.Rooms;
using API.Data.Models;

namespace API.Configuration
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// В конструкторе настроим соответствие сущностей при маппинге
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Address, AddressInfo>();

            // Тут идёт настройка сопоставления сущностей
            // Явно указываем что m.AddressInfo будет соответствовать строке из HomeOptions - Address
            CreateMap<HomeOptions, InfoResponse>()
                .ForMember(m => m.AddressInfo,
                    opt => opt.MapFrom(src => src.Address));

            // Валидация запросов:
            CreateMap<AddDeviceRequest, Device>()
                .ForMember(d => d.Location,
                    opt => opt.MapFrom(r => r.RoomLocation));

            CreateMap<AddRoomRequest, Room>();
            CreateMap<Device, DeviceView>();
        }
    }
}
