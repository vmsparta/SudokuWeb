using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SudokuGenerator;
using SudokuWeb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.Services
{
    /// <summary>
    /// Операции игрового процесса
    /// </summary>
    public class SudokuService : ISudokuService
    {
        IStorageService _storage;
        private readonly ILogger<SudokuService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        Sudoku _sudoku = new Sudoku();
        public SudokuService(IStorageService storage, ILogger<SudokuService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _storage = storage;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Получить игровой уровень
        /// </summary>
        /// <param name="createNewGame">Признак необходимости созданияя нового игрового поля</param>
        /// <param name="visibleNumCount">Количество видимых цифр</param>
        /// <returns>Игровое поле</returns>
        public SudokuCell[,] GetLevel(bool createNewGame, int visibleNumCount)
        {
            string map = null;
            if (!createNewGame)
                map = _storage.LoadGame();

            if (!string.IsNullOrEmpty(map))
                _sudoku.Generate(map);

            if (string.IsNullOrEmpty(map) || _sudoku.IsGameCompleted)
            {
                CreateNewGame(visibleNumCount);
            }

            return _sudoku.GetField();
        }

        /// <summary>
        /// Задать значение ячейки
        /// </summary>
        /// <param name="rowIndex">Номер строки</param>
        /// <param name="columnIndex">Номер столбца</param>
        /// <param name="value">Значение</param>
        /// <returns>Ошибочное значение</returns>
        public bool SetCellValue(int rowIndex, int columnIndex, int value)
        {
            RestoreFromStorage();
            bool isErrorValue = false;
            if (!_sudoku.IsGameCompleted)
            {
                isErrorValue = _sudoku.SetValue(rowIndex, columnIndex, value);
                SaveToStorage();
            }
            return isErrorValue;
        }

        /// <summary>
        /// Проверка на завершение уровня
        /// </summary>
        /// <returns>Уровень завершён</returns>
        public bool CheckGameCompleted()
        {
            RestoreFromStorage();
            return _sudoku.IsGameCompleted;
        }

        /// <summary>
        /// Получить подсказку
        /// </summary>
        /// <returns>Ячейка с подходящим значением</returns>
        public SudokuCell GetHint()
        {
            RestoreFromStorage();
            return _sudoku.GetRandomUnresolvedCell();
        }

        /// <summary>
        /// Создать новый уровень
        /// </summary>
        /// <param name="visibleNumCount">Количество видимых ячеек</param>
        private void CreateNewGame(int visibleNumCount)
        {
            _sudoku.Generate(visibleNumCount);
            SaveToStorage();
            _logger.LogInformation(string.Format("Пользователь {0} начал новую игру.", _httpContextAccessor.HttpContext.User.Identity.Name));
        }

        /// <summary>
        /// Загрузка уровня из БД
        /// </summary>
        private void RestoreFromStorage()
        {
            if (!_sudoku.IsLevelGenerated)
            {
                var map = _storage.LoadGame();
                _sudoku.Generate(map);
            }
        }

        /// <summary>
        /// Созранить уровень в БД
        /// </summary>
        private void SaveToStorage()
        {
            var map = _sudoku.GetLevelStringRepresentation();
            _storage.SaveGame(map);
        }
    }
}
