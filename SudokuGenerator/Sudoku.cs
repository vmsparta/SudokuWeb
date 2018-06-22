using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGenerator
{
    /// <summary>
    /// Класс игры Судоку
    /// </summary>
    public class Sudoku
    {
        const int SECTION_SIZE = 3;
        const int ROW_COUNT = 9;
        const int COLUMN_COUNT = 9;

        IEnumerable<int> _numsRange = Enumerable.Range(1, 9);

        SudokuCell[,] sudokuField = new SudokuCell[ROW_COUNT, COLUMN_COUNT];

        Random _rnd = new Random();

        /// <summary>
        /// Генератор игрового поля
        /// </summary>
        /// <param name="visibleNumCount">Количество видимых цифр</param>
        public void Generate(int visibleNumCount)
        {
            int[,] sudokuFieldTemp = new int[ROW_COUNT, COLUMN_COUNT];

            for (int i = 0; i < ROW_COUNT; i++)
            {
                for (int j = 0; j < COLUMN_COUNT; j++)
                {
                    var row = sudokuFieldTemp.GetRow(i);
                    var column = sudokuFieldTemp.GetColumn(j);
                    var section = sudokuFieldTemp.GetSection(i, j, SECTION_SIZE, SECTION_SIZE);
                    var existingNums = row.Union(column).Union(section);
                    var avaliableNums = _numsRange.Except(existingNums);
                    if (avaliableNums.Count() == 0)
                    {
                        sudokuFieldTemp = new int[9, 9];
                        i = -1;
                        break;
                    }

                    sudokuFieldTemp[i, j] = avaliableNums.ElementAt(_rnd.Next(0, avaliableNums.Count()));
                }
            }

            for (int i = 0; i < ROW_COUNT; i++)
            {
                for (int j = 0; j < COLUMN_COUNT; j++)
                {
                    sudokuField[i, j] = new SudokuCell()
                    {
                        RowIndex = i,
                        ColumnIndex = j,
                        UserValue = 0,
                        Value = sudokuFieldTemp[i, j]
                    };
                }
            }

            int totalNumCount = ROW_COUNT * COLUMN_COUNT;
            var arrayIndexes = Enumerable.Range(0, totalNumCount - 1).ToList();
            for (int i = 0; i < visibleNumCount; i++)
            {
                var indexForRemoving = arrayIndexes[_rnd.Next(0, arrayIndexes.Count())];
                arrayIndexes.Remove(indexForRemoving);
                var cell = sudokuField[indexForRemoving / COLUMN_COUNT, indexForRemoving % COLUMN_COUNT];
                cell.IsFixed = true;
                cell.UserValue = cell.Value;
            }

            IsLevelGenerated = true;
        }

        /// <summary>
        /// Генератор игрового поля
        /// </summary>
        /// <param name="map">Игровое поле в формате строки</param>
        public void Generate(string map)
        {
            if (string.IsNullOrWhiteSpace(map))
                return;

            try
            {
                var rowStrArray = map.Split(';');
                for (int i = 0; i < ROW_COUNT; i++)
                {
                    var cellArray = rowStrArray[i].Split(',');
                    for (int j = 0; j < COLUMN_COUNT; j++)
                    {
                        sudokuField[i, j] = new SudokuCell
                        {
                            RowIndex = i,
                            ColumnIndex = j,
                            Value = Convert.ToInt32(cellArray[j][0].ToString()),
                            UserValue = Convert.ToInt32(cellArray[j][1].ToString())
                        };
                        if (cellArray[j].Length > 2 && cellArray[j][2] == 'f')
                        {
                            sudokuField[i, j].IsFixed = true;
                        }
                        else if (cellArray[j].Length > 2 && cellArray[j][2] == 'e')
                        {
                            sudokuField[i, j].IsError = true;
                        }
                    }
                }

                IsLevelGenerated = true;
            }
            catch
            {
                IsLevelGenerated = false;
                sudokuField = new SudokuCell[ROW_COUNT, COLUMN_COUNT];
            }
        }

        /// <summary>
        /// Конвертер игрового поля в строку
        /// </summary>
        /// <returns>Игровое поле в строковом формате</returns>
        public string GetLevelStringRepresentation()
        {
            var map = "";
            for (int i = 0; i < ROW_COUNT; i++)
            {
                for (int j = 0; j < COLUMN_COUNT; j++)
                {
                    map += (sudokuField[i, j].Value.ToString() + sudokuField[i, j].UserValue.ToString());
                    if (sudokuField[i, j].IsFixed)
                        map += "f";
                    else if (sudokuField[i, j].IsError)
                        map += "e";
                    map += ",";
                }
                map = map.TrimEnd(',') + ";"; 
            }

            return map;
        }

        /// <summary>
        /// Возвращает текущий сгенерированный массив уровня
        /// </summary>
        /// <returns>Массив уровня</returns>
        public SudokuCell[,] GetField()
        {
            return sudokuField.Clone() as SudokuCell[,];
        }

        /// <summary>
        /// Запись значения в ячейку
        /// </summary>
        /// <param name="rowIndex">Индекс строки</param>
        /// <param name="columnIndex">Индекс столбца</param>
        /// <param name="value">Значение</param>
        /// <returns>Возвращает true, если значение некорректное, иначе false</returns>
        public bool SetValue(int rowIndex, int columnIndex, int value)
        {
            if (rowIndex < 0 || rowIndex >= ROW_COUNT || columnIndex < 0 || columnIndex >= COLUMN_COUNT)
                throw new IndexOutOfRangeException();

            sudokuField[rowIndex, columnIndex].UserValue = value;

            var row = sudokuField.GetRow(rowIndex).Select(c => c.UserValue);
            var repeatingInRow = row.Count(r => r == value);
            var column = sudokuField.GetColumn(columnIndex).Select(c => c.UserValue);
            var repeatingInColumn = column.Count(r => r == value);
            var section = sudokuField.GetSection(rowIndex, columnIndex, SECTION_SIZE, SECTION_SIZE).Select(c => c.UserValue);
            var repeatingInSection = section.Count(r => r == value);

            if (repeatingInRow > 1 || repeatingInColumn > 1 || repeatingInSection > 1)
                sudokuField[rowIndex, columnIndex].IsError = true;
            else
                sudokuField[rowIndex, columnIndex].IsError = false;

            return sudokuField[rowIndex, columnIndex].IsError;
        }

        /// <summary>
        /// Возвращает рандомную неоткрытую ячейку
        /// </summary>
        /// <returns>Неоткрытая ячейка поля</returns>
        public SudokuCell GetRandomUnresolvedCell()
        {
            var cells = sudokuField.Cast<SudokuCell>().Where(sc => sc != null && sc.Value != sc.UserValue && !sc.IsFixed).ToList();
            if (cells.Count == 0)
                return null;

            return cells[_rnd.Next(0, cells.Count)];
        }

        /// <summary>
        /// Возвращает true, если судоку разгадан
        /// </summary>
        public bool IsGameCompleted
        {
            get
            {
                var sudokuCellsList = sudokuField.Cast<SudokuCell>().Where(sc => sc != null).ToList();
                return !sudokuCellsList.Any(sc => sc.IsError || sc.UserValue == 0);
            }
        }

        /// <summary>
        /// Возвращает true, если уровень сгенерирован
        /// </summary>
        public bool IsLevelGenerated { get; private set; }
    }
}
