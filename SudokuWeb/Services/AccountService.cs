using Microsoft.EntityFrameworkCore;
using SudokuWeb.Data;
using SudokuWeb.Interfaces;
using SudokuWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.Services
{
    /// <summary>
    /// Операции с аккаунтом
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly GameDbContext _context;
        public AccountService(GameDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Проверка валидности пары логин-пароль
        /// </summary>
        /// <param name="userName">Логин</param>
        /// <param name="password">Пароль</param>
        /// <returns>Пользователь</returns>
        public User CheckUser(string userName, string password)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }

        /// <summary>
        /// Получение пользователя
        /// </summary>
        /// <param name="userName">Логин</param>
        /// <returns>Пользователь</returns>
        public User GetUser(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == userName);
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="userName">Логин</param>
        /// <param name="password">Пароль</param>
        public void RegisterUser(string userName, string password)
        {
            _context.Users.Add(new User { UserName = userName, Password = password });
            _context.SaveChanges();
        }
    }
}
