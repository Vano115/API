using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using API.Configuration;
using API.Contracts.Models.Home;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        // Ссылка на объект конфигурации
        private readonly IOptions<HomeOptions> _options;

        // Вот тут!!! Подключение глобальных настроек хостинга, это нужно чтобы указать мой общий путь 
        // для файлов приложения
        private readonly IHostEnvironment _environment;

        private readonly IMapper _mapper;

        // Инициализация конфигурации при вызове конструктора
        public HomeController(IOptions<HomeOptions> options, IHostEnvironment env, IMapper mapper)
        {
            _options = options;
            _environment = env;
            _mapper = mapper;
        }

        /// <summary>
        /// Метод для получения информации о доме
        /// </summary>
        [HttpGet] // Для обслуживания Get-запросов
        [HttpHead]
        [Route("{info}")] // Настройка маршрута с помощью атрибутов
        public IActionResult Info([FromRoute] string info)
        {
            // Объект Stringbuilder, в который будем "собирать" результат из конфигурации
            var pageResult = new StringBuilder();

            // Проставляем все значения из конфигурации для последующего вывода на страницу
            pageResult.Append($"Добро пожаловать в API вашего дома!{Environment.NewLine}");
            pageResult.Append($"Здесь вы можете посмотреть основную информацию.{Environment.NewLine}"); 
            pageResult.Append($"{Environment.NewLine}"); 
            pageResult.Append($"Количество этажей:         {_options.Value.FloorAmount}{Environment.NewLine}"); 
            pageResult.Append($"Стационарный телефон:      {_options.Value.Telephone}{Environment.NewLine}"); 
            pageResult.Append($"Тип отопления:             {_options.Value.Heating}{Environment.NewLine}"); 
            pageResult.Append($"Напряжение электросети:    {_options.Value.CurrentVolts}{Environment.NewLine}"); 
            pageResult.Append($"Подключен к газовой сети:  {_options.Value.GasConnected}{Environment.NewLine}"); 
            pageResult.Append($"Жилая площадь:             {_options.Value.Area} м2{Environment.NewLine}"); 
            pageResult.Append($"Материал:                  {_options.Value.Material}{Environment.NewLine}"); 
            pageResult.Append($"{Environment.NewLine}"); 
            pageResult.Append($"Адрес:                     {_options.Value.Address.Street} {_options.Value.Address.House}/" +
                                                        $"{_options.Value.Address.Building}{Environment.NewLine}");
            pageResult.Append($"Переданная в запрос строка: {info}");

            var staticPath = Path.Combine(_environment.ContentRootPath, "Static");
            Console.WriteLine(staticPath);
            var filePath = Directory.GetFiles(staticPath)
                .FirstOrDefault(f => 
                f.Split("\\")
                .Last()
                .Split('.')[0] == info);

            if (string.IsNullOrEmpty(filePath))
            {
                // Возврат если файла такого нет
                return StatusCode(404, $"Инструкции для производителя {info} " +
                    $"не найдено на сервере, проверьте название производителя");
            }

            var fileType = "application/pdf";
            var fileName = $"{info}.pdf";

            /// Тут остановился
            // Скачиваем найденый файл с сервера
            return PhysicalFile(filePath, fileType, fileName);
        }

        /// <summary>
        /// Метод для получения информации о доме
        /// </summary>
        [HttpGet] // Для обслуживания Get-запросов
        [Route("info")] // Настройка маршрута с помощью атрибутов
        public IActionResult Info()
        {
            // Получим запрос, "смапив" конфигурацию на модель запроса
            var infoResponse = _mapper.Map<HomeOptions, InfoResponse>(_options.Value);

            // Вернём ответ
            return StatusCode(200, infoResponse);
        }
    }
}
