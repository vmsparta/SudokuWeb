using SudokuWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.Interfaces
{
    /// <summary>
    /// Интерфейс для простой регистрации пользователей и операций с ними
    /// </summary>
    public interface IAccountService
    {
        void RegisterUser(string userName, string password);
        User CheckUser(string userName, string password);
        User GetUser(string userName);
    }
}
