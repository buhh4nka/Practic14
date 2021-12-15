using System;
using System.Windows;

namespace Practic14
{
    public partial class SettingsWindow : Window
    {
        public int RowCount { get;  set; }
        public int ColumnCount { get; set; }
        public SettingsWindow(int rowCount, int columnCount)
        {
            InitializeComponent();
            RowCount = rowCount;
            ColumnCount = columnCount;
            rowText.Text = rowCount.ToString();
            columnText.Text = columnCount.ToString();
        }
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(rowText.Text, out int row) && row < 1)
            {
                MessageBox.Show("Укажите верное число строк", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(columnText.Text, out int column) && column < 1)
            {
                MessageBox.Show("Укажите верное число столбцов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            RowCount = row;
            ColumnCount = column;
            this.Close();
        }
    }
}
