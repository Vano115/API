using API.Contracts.Models.Devices;
using API.Contracts.Models.Rooms;
using API.Data.Models;
using API.Data.Queries;
using API.Data.Repos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
        /// <summary>
        /// Контроллер комнат
        /// </summary>
        [ApiController]
        [Route("controller")]
        public class RoomsController : ControllerBase
        {
            private IRoomRepository _repository;
            private IMapper _mapper;

            public RoomsController(IRoomRepository repository, IMapper mapper)
            {
                _repository = repository;
                _mapper = mapper;
            }

            //TODO: Задание - добавить метод на получение всех существующих комнат

            /// <summary>
            /// Добавление комнаты
            /// </summary>
            [HttpPost]
            [Route("")]
            public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
            {
                var existingRoom = await _repository.GetRoomByName(request.Name);
                if (existingRoom == null)
                {
                    var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                    await _repository.AddRoom(newRoom);
                    return StatusCode(201, $"Комната {request.Name} добавлена!");
                }

                return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
            }
        /// <summary>
        /// Обновление существующего устройства
        /// </summary>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Edit(
            [FromRoute] Guid id,
            [FromBody] EditRoomRequest request)
        {
            var room = await _repository.GetRoomById(id);
            if (room == null)
                return StatusCode(400, $"Ошибка: Комнаты с идентификатором {id} не существует");

            if (request.NewName != null)
            {
                var withSameName = await _repository.GetRoomByName(request.NewName);
                if (withSameName != null)
                    return StatusCode(400, $"Ошибка: Комната с именем {request.NewName} уже существует. Выберите другое имя!");
            }

            await _repository.UpdateRoom(
                room,
                new UpdateRoomQuery(request.NewName, request.NewArea, request.NewVoltage)
            );

            return StatusCode(200, $"Параметры комнаты обновлены! " +
                $"Имя - {request.NewName}, место - {request.NewArea},  Напряжение электросети - {request.NewVoltage}");
        }
    }   
}
