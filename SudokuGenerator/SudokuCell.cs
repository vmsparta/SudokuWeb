using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuGenerator
{
    /// <summary>
    /// Ячейка игрового поля
    /// </summary>
    public class SudokuCell
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public int UserValue { get; set; }
        public int Value { get; set; }

        public bool IsFixed { get; set; }
        public bool IsError { get; set; }
    }
}
