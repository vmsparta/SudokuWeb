using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий методы хранения информации о текущем состоянии игры
    /// </summary>
    public interface IStorageService
    {
        void SaveGame(string map);
        string LoadGame();
    }
}
