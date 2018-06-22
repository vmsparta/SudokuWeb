using SudokuGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.Interfaces
{
    /// <summary>
    /// Интерфейс, описывающий основные игровые операции
    /// </summary>
    public interface ISudokuService
    {
        SudokuCell[,] GetLevel(bool createNewGame, int visibleNumCount);
        SudokuCell GetHint();
        bool SetCellValue(int rowIndex, int columnIndex, int value);
        bool CheckGameCompleted();
    }
}
