using Arbook_tools.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Arbook_tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModal _mainWindowViewModal;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _mainWindowViewModal = new MainWindowViewModal();

            DataContext = _mainWindowViewModal;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModal _mainWindowViewModal)
            {
                if (_mainWindowViewModal.GetApplitactionMainCommand.CanExecute(null))
                {
                    _mainWindowViewModal.GetApplitactionMainCommand.Execute(null);
                }

              /*  if (_mainWindowViewModal.GetEducationPlansCommand.CanExecute(null))
                {
                    _mainWindowViewModal.GetEducationPlansCommand.Execute(null);
                }*/
            }
        }

        private void SelectFile_Click(object sender, RoutedEventArgs e)
        {
          /*  // Откройте диалоговое окно выбора файла(.xlsx)
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
              
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    // Используйте библиотеку EPPlus для чтения данных из файла
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        var workbook = package.Workbook;
                        var worksheet = workbook.Worksheets.FirstOrDefault(); // Выберите лист, который хотите прочитать


                        ObservableCollection<MyDataModel> dataCollection = new ObservableCollection<MyDataModel>();

                        // Преобразуйте данные из листа в DataTable
                        DataTable dataTable = new DataTable();

                        foreach (var firstRowCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                        {
                            dataTable.Columns.Add(firstRowCell.Text);
                        }

                        for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                        {
                            var wsRow = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                            DataRow row = dataTable.Rows.Add();
                            List<string> r = new List<string>();

                            foreach (var cell in wsRow)
                            {
                                row[cell.Start.Column - 1] = cell.Text;
                                r.Add(cell.Text);
                            }

                            dataCollection.Add(new MyDataModel
                            {
                                IsSelected = false,
                                Name = r[0],
                                Link = r[1],
                                Description = r[2],
                                Count = r[3]
                            });

                        }

                        // Получаем стиль ячейки для переноса текста
                        Style cellStyle = new Style(typeof(DataGridCell));
                        cellStyle.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

                        // Применяем стиль ко всем ячейкам в DataGrid
                        dataGrid.CellStyle = cellStyle;

                        // Отобразите данные в DataGrid
                        _ = dataCollection;
                        dataGrid.ItemsSource = dataCollection;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при чтении файла: " + ex.Message);
                }
            }*/
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                if (e.Delta > 0)
                {
                    scrollViewer.LineLeft();
                }
                else
                {
                    scrollViewer.LineRight();
                }

                e.Handled = true;
            }
        }
    }
}
