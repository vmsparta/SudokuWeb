using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.Models
{
    /// <summary>
    /// Модель пользователя
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CurrentGame { get; set; }

    }
}
