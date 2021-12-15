using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using LibMas;
using Microsoft.Win32;

namespace Practic14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[,] _matrix;
        int _columns;
        int _rows;
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Info_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Практическая работа №13 Назаров Д. ИСП-31. Вариант 2.\nЗадание: \n  Дана целочисленная матрица размера M * N. Найти номер первого из ее столбцов, содержащих максимальное количество одинаковых элементов..", "Информация о программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveArray_Click(object sender, RoutedEventArgs e)
        {
            if (_matrix == null)
            {
                MessageBox.Show("Таблица пуста", "Ошибка");
                return;
            }
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.Filter = "Все файлы (*.*)|*.*|Текстовые файлы|*.txt";
            save.FilterIndex = 2;
            save.Title = "Сохранение таблицы";
            if (save.ShowDialog() == true)
            {
                _matrix.SaveArray(save.FileName);
            }
        }

        private void openArray_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Все файлы (*.*)|*.*|Текстовые файлы|*.txt";
            open.FilterIndex = 2;
            open.Title = "Открытие таблицы";

            if (open.ShowDialog() == true)
            {
                if (open.FileName != string.Empty)
                {
                    ArrayOperation.OpenArray(out _matrix, open.FileName);
                    dataGrid.ItemsSource = ArrayOperation.ToDataTable(_matrix).DefaultView;
                }
            }
            size.Text = string.Format($"Выбранная ячейка: {_matrix.GetLength(0)}х{_matrix.GetLength(1)}");
        }

        private void clearArray_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            _matrix.CleanArray();
            size.Text = string.Format($"Выбранная ячейка: {0}х{0}");
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dataGrid.CurrentColumn == null) return;
            selectedCell.Text = string.Format("Выбранная ячейка: {0}х{1}", dataGrid.Items.IndexOf(dataGrid.CurrentItem) + 1, dataGrid.CurrentColumn.DisplayIndex + 1);
        }

        private void dataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int indexColumn = e.Column.DisplayIndex;//Определяем номер строки
            int indexRow = e.Row.GetIndex();//Проверяем правильное значение ввел пользователь
            if (!int.TryParse(((TextBox)e.EditingElement).Text, out int value))
            {
                MessageBox.Show("Введены неверные данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }//Заносим введенное значение в матрицу
            _matrix[indexRow, indexColumn] = value;
        }

        private void fillArray_Click(object sender, RoutedEventArgs e)
        {
            bool isNotErrorInColumns = Int32.TryParse(numberOfColumns.Text, out _columns);
            bool isNotErrorInRows = Int32.TryParse(numberOfRows.Text, out _rows);
            if (isNotErrorInColumns && isNotErrorInRows)
            {
                _matrix = new int[_columns, _rows];
                _matrix.FillArray(0, 10);
                dataGrid.ItemsSource = ArrayOperation.ToDataTable(_matrix).DefaultView;
            }
            else
            {
                MessageBox.Show("Число столбцов или строк введено неверно. \nВведите другое значение.", "Ошибка");
                numberOfColumns.Clear();
                numberOfRows.Clear();
            }
            size.Text = string.Format($"Выбранная ячейка: {_matrix.GetLength(0)}х{_matrix.GetLength(1)}");
        }

        private void Text_TextChanged(object sender, TextChangedEventArgs e)
        {
            endRow.Clear();
        }


        private void initialChanged_Click(object sender, RoutedEventArgs e)
        {
            numberOfColumns.Clear();
            numberOfRows.Clear();
        }

        private void startProgram_Click(object sender, RoutedEventArgs e)
        {
            int outRow = 1;
            int countSameNumbersInRow = -1;
            int countNumberOfMaxElementsInRow = 0;
            int maxSameElementsRow = 0;
            int correctRow = 0;

            for (int kol = 0; kol < _rows; kol++)
            {
                for (int str = 0; str < _columns; str++)
                {
                    if (countSameNumbersInRow > countNumberOfMaxElementsInRow)
                    {
                        countNumberOfMaxElementsInRow = countSameNumbersInRow;
                    }
                    countSameNumbersInRow = 0;
                    for (int numInArr = 0; numInArr < _columns; numInArr++)
                    {
                        if (_matrix[str, kol] == _matrix[numInArr, kol])
                        {
                            countSameNumbersInRow++;
                        }
                    }
                }
                correctRow = countNumberOfMaxElementsInRow;
                if (maxSameElementsRow < correctRow)
                {
                    maxSameElementsRow = correctRow;
                    outRow = kol;
                    countNumberOfMaxElementsInRow = 0;
                }

            }
            endRow.Text = $"Столбец {outRow + 1} имеет максимальное значение одинаковых элементов";
        }

        private void result_Click(object sender, RoutedEventArgs e)
        {
            endRow.Clear();
        }

        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow window;
            if (_matrix == null) window = new SettingsWindow();
            else window = new SettingsWindow(_matrix.GetLength(0), _matrix.GetLength(1));

            window.ShowDialog();

            _matrix = new int[window.RowCount, window.ColumnCount];
            dataGrid.ItemsSource = ArrayOperation.ToDataTable(_matrix).DefaultView;
            size.Text = string.Format("Размер таблицы: {0}х{1}", window.RowCount, window.ColumnCount);
            numberOfRows.Text = window.RowCount.ToString();
            numberOfColumns.Text = window.ColumnCount.ToString();

            using (StreamWriter save = new StreamWriter("config.ini"))
            {
                save.WriteLine(window.RowCount);
                save.WriteLine(window.ColumnCount);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindow password = new LoginWindow();
            password.Owner = this;
            password.ShowDialog();
            try
            {
                int rowCount;
                int columnCount;
                using (StreamReader open = new StreamReader("config.ini"))
                {
                    rowCount = Convert.ToInt32(open.ReadLine());
                    columnCount = Convert.ToInt32(open.ReadLine());
                }
                _matrix = new int[rowCount, columnCount];
                _matrix.FillArray(0, 10);
                dataGrid.ItemsSource = ArrayOperation.ToDataTable(_matrix).DefaultView;
                size.Text = string.Format($"Размер таблицы: {rowCount}х{columnCount}");
                numberOfColumns.Text = rowCount.ToString();
                numberOfRows.Text = columnCount.ToString();
            }
            catch
            {
                MessageBox.Show("Последний конфиг не существует/поврежден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
