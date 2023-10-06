using Arbook_tools.Infastructure.Commands;
using Arbook_tools.ViewModels.Base;
using System.Windows.Input;
using System.Windows;
using OfficeOpenXml;
using Arbook_tools.Models;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;

namespace Arbook_tools.ViewModels
{
    class MainWindowViewModal : ViewModel
    {
        private ObservableCollection<LessonModel> _lessons { get; set; } = new ObservableCollection<LessonModel>();
        public ObservableCollection<LessonModel> Lessons
        {
            get { return _lessons; }
            set
            {
                if (_lessons != value)
                {
                    _lessons = value;
                    OnPropertyChanged(nameof(Lessons));
                }
            }
        }

        private string _selectedLessonId;
        public string SelectedLessonId
        {
            get { return _selectedLessonId; }
            set
            {
                _selectedLessonId = value;
                OnPropertyChanged(nameof(SelectedLessonId));
                GetEducationPlansCommand.Execute(null);
            }
        }

        private string _selectedPlanId;
        public string SelectedPlanId
        {
            get { return _selectedPlanId; }
            set
            {
                _selectedPlanId = value;
                OnPropertyChanged(nameof(SelectedPlanId));
                GetEducationPlanChaptersCommand.Execute(null);
            }
        }

        private string _selectedPlanChapterId;
        public string SelectedPlanChapterId
        {
            get { return _selectedPlanChapterId; }
            set
            {
                _selectedPlanChapterId = value;
                OnPropertyChanged(nameof(SelectedPlanChapterId));
            }
        }

        private string _newSection;
        public string NewSection
        {
            get { return _newSection; }
            set
            {
                _newSection = value;
                OnPropertyChanged(nameof(NewSection));
            }
        }

        private string _contentAuthor;
        public string ContentAuthor
        {
            get { return _contentAuthor; }
            set
            {
                _contentAuthor = value;
                OnPropertyChanged(nameof(ContentAuthor));
            }
        }

        private int _progressBarValue;
        public int ProgressBarValue
        {
            get { return _progressBarValue; }
            set
            {
                _progressBarValue = value;
                OnPropertyChanged(nameof(ProgressBarValue));
            }
        }

        private int _checkedRows;
        public int CheckedRows
        {
            get { return _checkedRows; }
            set
            {
                _checkedRows = value;
                OnPropertyChanged(nameof(CheckedRows));
            }
        }

        private ObservableCollection<EducationPlanModel> _educationPlans { get; set; } = new ObservableCollection<EducationPlanModel>();
        public ObservableCollection<EducationPlanModel> EducationPlans
        {
            get { return _educationPlans; }
            set
            {
                if (_educationPlans != value)
                {
                    _educationPlans = value;
                    OnPropertyChanged(nameof(EducationPlans));
                }
            }
        }

        private ObservableCollection<EducationPlanChapterModel> _educationPlanChapters { get; set; } = new ObservableCollection<EducationPlanChapterModel>();
        public ObservableCollection<EducationPlanChapterModel> EducationPlanChapters
        {
            get { return _educationPlanChapters; }
            set
            {
                if (_educationPlanChapters != value)
                {
                    _educationPlanChapters = value;
                    OnPropertyChanged(nameof(EducationPlanChapters));
                }
            }
        }

        private ObservableCollection<FileXlsxModel> _dataGrid { get; set; }
        public ObservableCollection<FileXlsxModel> DataGrid
        {
            get { return _dataGrid; }
            set
            {
                if (_dataGrid != value)
                {
                    _dataGrid = value;
                    OnPropertyChanged(nameof(DataGrid));
                }
            }
        }

        public ICommand GetApplitactionMainCommand { get; }
        public ICommand GetEducationPlansCommand { get; }
        public ICommand GetEducationPlanChaptersCommand { get; }
        public ICommand CreateEducationPlanCommand { get; }
        public ICommand OpenXlsxFileCommand { get; }
        public ICommand ToggleIsSelectedCommand { get; }
        public ICommand LoadToArbookCommand { get; }

        #region GetApplitactionMainCommand
        private async void OnGetApplitactionMainCommandExecuted(object p)
        {
            var globalData = ((App)Application.Current).GlobalData;
            var graphQl = globalData.graphQl;
            var response = await graphQl.GetAplicationMain(globalData.Token);

            if (response != null)
            {
                foreach (var item in response?.getApplicationMainsByTeacherId?.applicationsMain)
                {
                   Lessons.Add(new LessonModel { Id = item.id, Name = item.name });
                   OnPropertyChanged(nameof(Lessons));
                }
                //MessageBox.Show(Lessons[0].Id.ToString());

            }
        }
        private bool CanGetApplitactionMainCommand(object p) => true;

        #endregion

        #region GetEducationPlansCommand
        private async void OnGetEducationPlansCommandExecuted(object p)
        {
            var globalData = ((App)Application.Current).GlobalData;
            var graphQl = globalData.graphQl;
            var response = await graphQl.GetEducationPlans(globalData.Token, SelectedLessonId);

            if (response != null)
            {
                EducationPlans.Clear();
                OnPropertyChanged(nameof(EducationPlans));
                foreach (var item in response?.getEducationPlans?.educationPlans)
                {
                    EducationPlans.Add(new EducationPlanModel { Id = item.id, Name = item.name, MainId = item.mainId});
                }

            }
        }
        private bool CanGetEducationPlansCommand(object p) => true;

        #endregion

        #region GetEducationPlanChaptersCommand
        private async void OnGetEducationPlanChaptersCommandExecuted(object p)
        {
            var globalData = ((App)Application.Current).GlobalData;
            var graphQl = globalData.graphQl;
            var response = await graphQl.GetEducationPlan(globalData.Token, SelectedPlanId);

            if (response != null)
            {
                EducationPlanChapters.Clear();
                OnPropertyChanged(nameof(EducationPlanChapters));
                foreach (var item in response?.getEducationPlanChapters?.educationPlanChapters)
                {
                    EducationPlanChapters.Add(new EducationPlanChapterModel { Id = item.id, Name = item.name });
                }

            }
        }
        private bool CanGetEducationPlanCommandCommand(object p) => true;

        #endregion

        #region CreateEducationPlanCommand
        private async void OnCreateEducationPlanCommandExecuted(object p)
        {
            var globalData = ((App)Application.Current).GlobalData;
            var graphQl = globalData.graphQl;
            var response = await graphQl.CreateEducationPlan(SelectedPlanId, NewSection, globalData.Token);
            if (response != null)
            {
                GetEducationPlanChaptersCommand.Execute(null);
                NewSection = "";
                OnPropertyChanged(nameof(NewSection));
                MessageBox.Show("Section create");

            }
            else
            {
                MessageBox.Show("Error, login or password wrong!!!");
            }
        }
        private bool CanCreateEducationPlanCommand(object p) => true;
        #endregion

        #region OpenXlsxFileCommand
        private void OnOpenXlsxFileCommandExecuted(object p)
        {
            // Откройте диалоговое окно выбора файла(.xlsx)
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
                        var worksheet = workbook.Worksheets[0]; // Выберите лист, который хотите прочитать


                        ObservableCollection<FileXlsxModel> dataCollection = new ObservableCollection<FileXlsxModel>();

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

                            dataCollection.Add(new FileXlsxModel
                            {
                                Id = rowNum,
                                IsSelected = false,
                                Name = r[0],
                                Link = r[1],
                                Description = r[2],
                                Count = r[3]
                            });

                        }

                        // Отобразите данные в DataGrid
                        DataGrid = dataCollection;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при чтении файла: " + ex.Message);
                }
            }
        }
        private bool CanOpenXlsxFileCommandCommand(object p) => true;

        #endregion

        private void ToggleIsSelected(object parameter)
        {
            if (parameter is FileXlsxModel row)
            {
                row.IsSelected = !row.IsSelected;
                if (row.IsSelected)
                {
                    CheckedRows++;
                } 
                else
                {
                    CheckedRows--;
                }

            }
        }

        #region LoadToArbookCommand
        private async void OnLoadToArbookCommandExecuted(object p)
        {
            var globalData = ((App)Application.Current).GlobalData;
            var graphQl = globalData.graphQl;

            if (string.IsNullOrEmpty(SelectedLessonId) || string.IsNullOrEmpty(SelectedPlanId) || string.IsNullOrEmpty(SelectedPlanChapterId) || string.IsNullOrEmpty(ContentAuthor))
            {
                MessageBox.Show("Please enter the select or author!");
                return;
            }

            if (DataGrid != null && DataGrid.Count != 0)
            {
                int countLesson = 0;
                foreach (var row in DataGrid)
                {
                    if (row.IsSelected)
                    {
                        countLesson++;
                       
                        //MessageBox.Show($"ID:{row.Id} name:{row.Name}");
                        var lessonId = await graphQl.CreateNewLesson(SelectedPlanChapterId, row.Name, globalData.Token);
                        if (lessonId != null)
                        {
                            var slideId = await graphQl.CreateSlide(lessonId, globalData.Token);

                            if (slideId != null)
                            {
                                var updateSlideId = await graphQl.UpdateSlide(slideId, row.Link, globalData.Token);

                                if (updateSlideId != null)
                                {
                                    var uploadLessonToMarketId = await graphQl.UploadNewLessonToMarket(lessonId, row.Name, ContentAuthor, SelectedLessonId, row.Description, globalData.Token);

                                    if (uploadLessonToMarketId)
                                    {
                                        ProgressBarValue += (countLesson / CheckedRows) * 100;
                                        OnPropertyChanged(nameof(ProgressBarValue));
                                    }
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Error, login or password wrong!!!");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please choose the file!");
            }
            
           
        }
        private bool CanLoadToArbookCommandCommand(object p) => true;
        #endregion
        public MainWindowViewModal()
        {
            GetApplitactionMainCommand = new LambdaCommand(OnGetApplitactionMainCommandExecuted, CanGetApplitactionMainCommand);
            GetEducationPlansCommand = new LambdaCommand(OnGetEducationPlansCommandExecuted, CanGetEducationPlansCommand);
            GetEducationPlanChaptersCommand = new LambdaCommand(OnGetEducationPlanChaptersCommandExecuted, CanGetEducationPlanCommandCommand);
            CreateEducationPlanCommand = new LambdaCommand(OnCreateEducationPlanCommandExecuted, CanCreateEducationPlanCommand);
            OpenXlsxFileCommand = new LambdaCommand(OnOpenXlsxFileCommandExecuted, CanOpenXlsxFileCommandCommand);
            ToggleIsSelectedCommand = new LambdaCommand(ToggleIsSelected);
            LoadToArbookCommand = new LambdaCommand(OnLoadToArbookCommandExecuted, CanLoadToArbookCommandCommand);
        }
    }
}
