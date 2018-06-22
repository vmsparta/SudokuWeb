using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SudokuWeb.Interfaces;
using SudokuWeb.ViewModels;

namespace SudokuWeb.Controllers
{
    /// <summary>
    /// Контроллер игры
    /// </summary>
    [Authorize]
    public class GameController : Controller
    {
        private readonly ISudokuService _sudokuService;
        private readonly ILogger<GameController> _logger;

        public GameController(ISudokuService sudoku, ILogger<GameController> logger)
        {
            _sudokuService = sudoku;
            _logger = logger;
        }

        /// <summary>
        /// Основная страница игры
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Загрузка игрового поля
        /// </summary>
        /// <param name="createNewGame">Признак необходимости созданияя нового игрового поля</param>
        /// <param name="visibleNumCount">Количество видимых цифр</param>
        /// <returns>Представление</returns>
        public IActionResult LoadTable(bool createNewGame, int visibleNumCount)
        {
            return PartialView("_SudokuTablePartial", new SudokuTableModel() { Cells = _sudokuService.GetLevel(createNewGame, visibleNumCount) });
        }

        /// <summary>
        /// Сохранить ячейку поля
        /// </summary>
        /// <param name="rowIndex">Номер строки</param>
        /// <param name="columnIndex">Номер колонки</param>
        /// <param name="value">Значение</param>
        /// <returns>Информация о корректности введённого значения и о завершении игры</returns>
        [HttpPost]
        public JsonResult SaveCellValue(int rowIndex, int columnIndex, int value)
        {
            return new JsonResult(
                new
                {
                    isErrorValue = _sudokuService.SetCellValue(rowIndex, columnIndex, value),
                    isLevelCompleted = _sudokuService.CheckGameCompleted()
                }
            );
        }

        /// <summary>
        /// Получить подсказку
        /// </summary>
        /// <returns>Ячейка поля с правильным значением</returns>
        [HttpGet]
        public JsonResult GetHint()
        {
            return new JsonResult(_sudokuService.GetHint());
        }

        /// <summary>
        /// Обработчик ошибок
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            var feature = this.HttpContext.Features.Get<IExceptionHandlerFeature>();
            _logger.LogError(feature.Error.ToString());
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
