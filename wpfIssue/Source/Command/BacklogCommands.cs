using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.UI.Xaml.Utility;
using System.Windows.Input;
using wpfIssues.Model;
using wpfIssues.ViewModel;

namespace wpfIssues.Command
{
    public static class BacklogCommands
    {
        #region Fields
        static ICommand _statusOffen;
        static ICommand _statusImTest;
        static ICommand _statusInBearbeitung;
        static ICommand _statusKlaerungsbedarf;
        static ICommand _datenRefreshen;
        #endregion

        #region Properties
        public static ICommand StatusOffen
        {
            get
            {
                if (_statusOffen == null)
                    _statusOffen = new BaseCommand(onStatusOffenClicked);
                return _statusOffen;
            }
        }
        public static ICommand statusImTest
        {
            get
            {
                if (_statusImTest == null)
                    _statusImTest = new BaseCommand(onStatusImTestClicked);
                return _statusImTest;
            }
        }
        public static ICommand statusInBearbeitung
        {
            get
            {
                if (_statusInBearbeitung == null)
                    _statusInBearbeitung = new BaseCommand(onStatusInBearbeitungClicked);
                return _statusInBearbeitung;
            }
        }
        public static ICommand statusKlaerungsbedarf
        {
            get
            {
                if (_statusKlaerungsbedarf == null)
                    _statusKlaerungsbedarf = new BaseCommand(onStatusKlaerungsbedarfClicked);
                return _statusKlaerungsbedarf;
            }
        }
        public static ICommand datenRefreshen
        {
            get
            {
                if (_datenRefreshen == null)
                    _datenRefreshen = new BaseCommand(OnDatenRefreshenClicked);
                return _datenRefreshen;
            }
        }
        #endregion

        private async static void onStatusOffenClicked(object obj)
        {
            //BacklogView
            //JxTask? selectedItem = (obj as GridRecordContextMenuInfo)?.Record as JxTask;

            //if (obj != null && selectedItem != null)
            //{
            //    selectedItem.status = enumStatus.opened;
            //    await JxTask.updateIssueAsync(selectedItem);

            //    GridRecordContextMenuInfo? castedObj = obj as GridRecordContextMenuInfo;
            //    castedObj?.DataGrid.RefreshColumns();
            //}
        }
        private async static void onStatusImTestClicked(object obj)
        {
            //var selectedToDo = obj as JxTask;
            //if (selectedToDo != null)
            //{
            //    selectedToDo.status = enumStatus.imTest;
            //    var updatedIssue = await JxTask.updateIssueAsync(selectedToDo);
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
            //var selectedToDo = obj as JxTask;
            //if (selectedToDo != null)
            //{
            //    selectedToDo.status = enumStatus.inBearbeitung;
            //    var updatedIssue = await JxTask.updateIssueAsync(selectedToDo);
            //}

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
            //var selectedToDo = obj as JxTask;
            //if (selectedToDo != null)
            //{
            //    selectedToDo.status = enumStatus.klaerungsbedarf;
            //    var updatedIssue = await JxTask.updateIssueAsync(selectedToDo);
            //}

            //JxTask? selectedItem = (obj as GridRecordContextMenuInfo)?.Record as JxTask;

            //if (obj != null && selectedItem != null && selectedItem.status != enumStatus.klaerungsbedarf)
            //{
            //    selectedItem.status = enumStatus.klaerungsbedarf;
            //    await JxTask.updateIssueAsync(selectedItem);

            //    GridRecordContextMenuInfo? castedObj = obj as GridRecordContextMenuInfo;
            //    castedObj?.DataGrid.RefreshColumns();
            //}
        }
        private static void OnDatenRefreshenClicked(object obj)
        {
            //var contextMenuInfo = obj as GridRecordContextMenuInfo;

            //if (contextMenuInfo != null &&
            //    contextMenuInfo.DataGrid != null &&
            //    contextMenuInfo.DataGrid.DataContext != null)
            //{
            //    MainViewModel? mainViewModel = contextMenuInfo.DataGrid.DataContext as MainViewModel;
            //    if (mainViewModel != null)
            //    {
            //        mainViewModel.datenRefreshen();
            //    }
            //}
        }
    }
}
