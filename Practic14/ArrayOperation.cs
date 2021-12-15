using System;
using System.Data;
using System.IO;

namespace LibMas
{
    public static class ArrayOperation
    {
        static Random random = new Random();

        public static DataTable ToDataTable<T>(this T[,] matrix)
        {
            var res = new DataTable();
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                res.Columns.Add("Col" + (i + 1), typeof(T));
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var row = res.NewRow();

                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row[j] = matrix[i, j];
                }

                res.Rows.Add(row);
            }

            return res;
        }


        public static void FillArray(this int[,] array, int minValue, int maxValue)
        {

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = random.Next(minValue, maxValue);
                }

            }
        }

        public static void CleanArray(this int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        public static void SaveArray(this int[,] array, string path)
        {
            using (StreamWriter save = new StreamWriter(path))
            {
                save.WriteLine(array.GetLength(0));
                save.WriteLine(array.GetLength(1));
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        save.WriteLine(array[i, j]);
                    }
                }
            }
        }

        public static void OpenArray(out int[,] array, string path)
        {
            using (StreamReader open = new StreamReader(path))
            {
                int columns = Convert.ToInt32(open.ReadLine());
                int rows = Convert.ToInt32(open.ReadLine());
                array = new int[columns, rows];
                for (int i = 0; i < columns; i++)
                {
                    for (int j = 0; j < rows; j++)
                    {
                        array[i, j] = Convert.ToInt32(open.ReadLine());
                    }
                }
            }
        }


    }
}
