using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.UI.Xaml.Utility;
using System;
using System.Windows.Input;
using wpfIssues.Model;
using wpfIssues.ViewModel;

namespace wpfIssues.Command
{
    public class ContextMenuCommands
    {
        #region Fields
        private ICommand _statusImTest;
        private ICommand _statusInBearbeitung;
        private ICommand _statusKlaerungsbedarf;
        private MainViewModel _mainViewModel;
        #endregion

        #region Properties

        public ContextMenuCommands(MainViewModel mainViewModel)
        {
            this._mainViewModel = mainViewModel ?? throw new ArgumentNullException(nameof(mainViewModel));
        }

        public ICommand statusImTest
        {
            get
            {
                if (_statusImTest == null)
                    _statusImTest = new BaseCommand(onStatusImTestClicked);
                return _statusImTest;
            }
        }
        public ICommand statusInBearbeitung
        {
            get
            {
                if (_statusInBearbeitung == null)
                    _statusInBearbeitung = new BaseCommand(onStatusInBearbeitungClicked);
                return _statusInBearbeitung;
            }
        }
        public ICommand statusKlaerungsbedarf
        {
            get
            {
                if (_statusKlaerungsbedarf == null)
                    _statusKlaerungsbedarf = new BaseCommand(onStatusKlaerungsbedarfClicked);
                return _statusKlaerungsbedarf;
            }
        }
        #endregion

        private async static void onStatusImTestClicked(object obj)
        {
            //var selectedToDo = obj as JxTask;
            //if (selectedToDo != null)
            //{
            //    var currentStatus = selectedToDo.status;


            //    selectedToDo.status = enumStatus.imTest;
            //    var updatedIssue = await JxTask.updateIssueAsync(selectedToDo);
            //    //var foo = "blat!";
            //}

            //JxTask? selectedItem = (obj as GridRecordContextMenuInfo)?.Record as JxTask;

            //if (obj != null && selectedItem != null && selectedItem.status != enumStatus.imTest)
            //{
            //    selectedItem.status = enumStatus.imTest;
            //    await JxTask.updateIssueAsync(selectedItem);

            //    GridRecordContextMenuInfo? castedObj = obj as GridRecordContextMenuInfo;
            //    castedObj?.DataGrid.RefreshColumns();
            //}
        }
        private async static void onStatusInBearbeitungClicked(object obj)
        {
            //JxTask? selectedItem = (obj as GridRecordContextMenuInfo)?.Record as JxTask;

            //if (obj != null && selectedItem != null && selectedItem.status != enumStatus.inBearbeitung)
            //{
            //    selectedItem.status = enumStatus.inBearbeitung;
            //    await JxTask.updateIssueAsync(selectedItem);

            //    GridRecordContextMenuInfo? castedObj = obj as GridRecordContextMenuInfo;
            //    castedObj?.DataGrid.RefreshColumns();
            //}
        }
        private async static void onStatusKlaerungsbedarfClicked(object obj)
        {
            //JxTask? selectedItem = (obj as GridRecordContextMenuInfo)?.Record as JxTask;

            //if (obj != null && selectedItem != null && selectedItem.status != enumStatus.klaerungsbedarf)
            //{
            //    selectedItem.status = enumStatus.klaerungsbedarf;
            //    await JxTask.updateIssueAsync(selectedItem);

            //    GridRecordContextMenuInfo? castedObj = obj as GridRecordContextMenuInfo;
            //    castedObj?.DataGrid.RefreshColumns();
            //}
        }
    }
}