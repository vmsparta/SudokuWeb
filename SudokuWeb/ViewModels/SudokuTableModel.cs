using SudokuGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWeb.ViewModels
{
    /// <summary>
    /// VM игрового поля
    /// </summary>
    public class SudokuTableModel
    {
        public SudokuCell[,] Cells { get; set; }
    }
}
