using Microsoft.AspNetCore.Http;
using SudokuWeb.Data;
using SudokuWeb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.Services
{
    /// <summary>
    /// Операции с БД в контексте игрового процесса
    /// </summary>
    public class EFDatabaseStorage : IStorageService
    {
        GameDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EFDatabaseStorage(GameDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext.User.Identity.Name;
            }
        }

        /// <summary>
        /// Загрузить игру авторизованного пользователя
        /// </summary>
        /// <returns>Игровое поле в строковом представлении</returns>
        public string LoadGame()
        {
            var user = _context.Users.Single(u => u.UserName == UserName);
            return user.CurrentGame;

        }

        /// <summary>
        /// Сохранить игру авторизованного пользователя
        /// </summary>
        /// <param name="map">Игровое поле в строковом представлении</param>
        public void SaveGame(string map)
        {
            var user = _context.Users.Single(u => u.UserName == UserName);
            user.CurrentGame = map;

            _context.SaveChanges();
        }
    }
}
