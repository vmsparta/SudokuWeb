using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    /// <summary>
    /// Вспомогательный класс обработки массивов
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// Получить целиком строку поля по индексу
        /// </summary>
        /// <typeparam name="T">Тип массива</typeparam>
        /// <param name="matrix">Двумерный массив</param>
        /// <param name="row">Номер строки</param>
        /// <returns>Строка массива</returns>
        public static T[] GetRow<T>(this T[,] matrix, int row)
        {
            var rowLength = matrix.GetLength(0);
            var rowArray = new T[rowLength];

            for (var i = 0; i < rowLength; i++)
                rowArray[i] = matrix[row, i];

            return rowArray;
        }

        /// <summary>
        /// Получить целиком колонку поля по индексу
        /// </summary>
        /// <typeparam name="T">Тип массива</typeparam>
        /// <param name="matrix">Двумерный массив</param>
        /// <param name="col">Номер столбца</param>
        /// <returns>Столбец массива</returns>
        public static T[] GetColumn<T>(this T[,] matrix, int col)
        {
            var colLength = matrix.GetLength(1);
            var colArray = new T[colLength];

            for (var i = 0; i < colLength; i++)
                colArray[i] = matrix[i, col];

            return colArray;
        }

        /// <summary>
        /// Получить целиком секцию поля
        /// </summary>
        /// <typeparam name="T">Тип массива</typeparam>
        /// <param name="matrix">Двумерный массив</param>
        /// <param name="row">Номер строки</param>
        /// <param name="col">Номер столбца</param>
        /// <param name="rowSize">Размер секции в высоту</param>
        /// <param name="colSize">Размер секции в ширину</param>
        /// <returns>Секция массива</returns>
        public static T[] GetSection<T>(this T[,] matrix, int row, int col, int rowSize, int colSize)
        {
            var rowLength = matrix.GetLength(0);
            var colLength = matrix.GetLength(1);

            int rowSectionIndex = row / rowSize;
            int rowSectionStart = rowSectionIndex * rowSize;
            int rowSectionEnd = rowSectionStart + rowSize - 1;
            if (rowSectionEnd >= rowLength)
                rowSectionEnd = rowLength - 1;

            int columnSectionIndex = col / colSize;
            int columnSectionStart = columnSectionIndex * colSize;
            int columnSectionEnd = columnSectionStart + colSize - 1;
            if (columnSectionEnd >= colLength)
                columnSectionEnd = colLength - 1;

            var sectionArray = new T[rowSize*colSize];

            var index = 0;
            for (var i = rowSectionStart; i <= rowSectionEnd; i++)
            {
                for (var j = columnSectionStart; j <= columnSectionEnd; j++)
                {
                    sectionArray[index] = matrix[i, j];
                    index++;
                }
            }

            return sectionArray;
        }
    }
}
