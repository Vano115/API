using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using API.Configuration;
using API.Contracts.Models.Devices;


namespace API.Controllers
{
    /// <summary>
    /// Контроллер статусов устройств
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private IOptions<HomeOptions> _options;
        private IHostEnvironment _env;
        private IMapper _mapper;
        private IValidator<AddDeviceRequest> _validator;

        public DevicesController(IOptions<HomeOptions> options, IMapper mapper, IHostEnvironment environment, 
            IValidator<AddDeviceRequest> validator)
        {
            _options = options;
            _mapper = mapper;
            _env = environment;
            _validator = validator;
        }

        /// <summary>
        /// Просмотр списка подключенных устройств
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return StatusCode(200, "Устройства отсутствуют");
        }

        /// <summary>
        /// Добавление нового устройства
        /// </summary>
        [HttpPost]
        [Route("Add")]
        public async Task <IActionResult> Add([FromBody]AddDeviceRequest request)
        // Объект запроса - [FromBody] - он указывает на то что данные запроса
        //  будут в JSON файле 
        // [FromRoute] означает что получать он будет строку из переменной переданой в GET запрос
        // Атрибут, указывающий, откуда брать значение объекта -  AddDeviceRequest request

        {
            var valid = await _validator.ValidateAsync(request);

            if (!valid.IsValid)
            {
                valid.AddToModelState(this.ModelState);
                return BadRequest(ModelState.Values);
            }
            return StatusCode(200, $"Устройство {request.Name} добавлено!");
        }
    }
}
