using Serilog;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wpfIssues.Model;
using wpfIssues.ViewModel;
using libCodLibCS.nsFileSystem;

namespace wpfIssues.View
{
    public partial class BacklogView : UserControl
    {
        public BacklogView()
        {
            Log.Information("Enter {MethodName} method.", nameof(BacklogView));
            InitializeComponent();

            this.BacklogViewModel = (DataContext as BacklogViewModel)!;

            this.dgBacklog.RowDragDropController.DragStart += OnDgBacklogDrag_Start;
            this.dgBacklog.RowDragDropController.Drop += OnDgBacklogRowDragDropController_Drop;
            this.dgBacklog.RowDragDropController.DragOver += OnDgBacklogRowDragDropController_DragOver;
            this.dgBacklog.RowDragDropController.Dropped += OnDgBacklogRowDragDropController_Dropped;
            this.dgBacklog.AllowEditing = this.BacklogViewModel.jxIssuesDataService.currentUser.role == enumRole.admin;
            this.dgBacklog.CurrentCellValidating += OnDgBacklog_CurrentCellValidating;

            this.cmbxSelectedMitarbeier.SelectionChanged += OnCmbxSelectedMitarbeier_SelectionChanged;

            this.BacklogViewModel.backlogTasks.CollectionChanged += RefreshDataGrid!;

            Log.Information("Exit {MethodName} method.", nameof(BacklogView));
        }

        private void OnDgBacklogRowDragDropController_Dropped(object? sender, GridRowDroppedEventArgs e)
        {
        }

        #region fields/properties
        private BacklogViewModel _backlogViewModel;
        public BacklogViewModel BacklogViewModel
        {
            get { return _backlogViewModel; }
            set { _backlogViewModel = value; }
        }

        private JxTask? _originalValue = new JxTask();
        public JxTask? originalValue
        {
            get
            {
                return _originalValue;
            }
            set
            {
                _originalValue = value;
            }
        }
        
        private int _currentMatchIndex = -1;
        private List<JxTask> _currentMatches;
        #endregion

        #region events
        private void OnCmbxSelectedMitarbeier_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnCmbxSelectedMitarbeier_SelectionChanged));
            this.BacklogViewModel.MitarbeiterId = (int)this.cmbxSelectedMitarbeier.SelectedValue;
            Log.Information("Exit {MethodName} method.", nameof(OnCmbxSelectedMitarbeier_SelectionChanged));
        }
        private void OnSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            Log.Information("Exit {MethodName} method.", nameof(OnSearchBox_KeyDown));
            try
            {
                if (e.Key == Key.Enter)
                {
                    HighlightNextMatch();
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred in {MethodName}", nameof(OnSearchBox_KeyDown));
                Log.Error("{@exception}", ex);
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Log.Information("Exit {MethodName} method.", nameof(OnSearchBox_KeyDown));
        }
        private void OnSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Log.Information("Exit {MethodName} method.", nameof(OnSearchBox_TextChanged));
            if (sender is not TextBox textBox) { return; }

            var searchText = textBox.Text.ToLower();
            if (string.IsNullOrEmpty(searchText))
            {
                _currentMatchIndex = -1; // Reset if search text is cleared.
                _currentMatches.Clear();
                return;
            }

            _currentMatches = FindMatches(searchText);
            HighlightNextMatch();
            Log.Information("Exit {MethodName} method.", nameof(OnSearchBox_TextChanged));
        }
        private void OnDgBacklogDrag_Start(object? sender, GridRowDragStartEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnDgBacklogDrag_Start));

            if (this.BacklogViewModel.jxIssuesDataService.currentUser.role != enumRole.admin) { e.Handled = true; return; }

            var records = e.DraggingRecords;
            if (records == null) { e.Handled = true; return; }
            if (records != null && records.Count > 1) 
            {
                MessageBox.Show("Das Ziehen mehrerer Zeilen gleichzeitig ist nicht erlaubt", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Handled = true; 
                return; 
            }            
            Log.Information("Exit {MethodName} method.", nameof(OnDgBacklogDrag_Start));
        }
        private void OnDgBacklogRowDragDropController_DragOver(object? sender, GridRowDragOverEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnDgBacklogRowDragDropController_DragOver));
            e.ShowDragUI = false;
            Log.Information("Exit {MethodName} method.", nameof(OnDgBacklogRowDragDropController_DragOver));
        }
        private void OnDgBacklogRowDragDropController_Drop(object? sender, GridRowDropEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnDgBacklogRowDragDropController_Drop));

            if (e.DropPosition == DropPosition.None
                || e.DraggingRecords.Count == 0
                || (dgBacklog.ItemsSource as ObservableCollection<JxTask>)![(int)e.TargetRecord] is not JxTask jxTaskAtDropPosition)
            { e.Handled = true; return; }

            if (jxTaskAtDropPosition.sortOrder < 0) { e.Handled = true; return; }

            this.BacklogViewModel.jxIssuesDataService.OriginalTask.updateIssueProperties((JxTask)e.DraggingRecords[0]);

            if (e.DropPosition is DropPosition.DropBelow)
            {
                ((JxTask)e.DraggingRecords[0]).sortOrder = jxTaskAtDropPosition.sortOrder!.Value + 1;
            }
            else if (e.DropPosition is DropPosition.DropAbove)
            {
                ((JxTask)e.DraggingRecords[0]).sortOrder = jxTaskAtDropPosition.sortOrder!.Value;
            }
            Log.Information("Exit  {MethodName} method.", nameof(OnDgBacklogRowDragDropController_Drop));
        } 
        private void OnDgBacklogCell_DoubleClick(object sender, GridCellDoubleTappedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnDgBacklogCell_DoubleClick));
            if (e.Column.MappingName == "titelLong")
            {
                var selectedTask = e.Record as JxTask;
                if (selectedTask != null && !string.IsNullOrEmpty(selectedTask.web_url))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = selectedTask.web_url,
                        UseShellExecute = true
                    });
                }
            }
            Log.Information("Exit {MethodName} method.", nameof(OnDgBacklogCell_DoubleClick));
        }
        private void OnDgBacklog_CurrentCellBeginEdit(object sender, CurrentCellBeginEditEventArgs e)
        {
            if (sender is not SfDataGrid dataGrid || dataGrid.SelectedItem is not JxTask jxTask)
            {
                e.Cancel = true;
                return;
            }
            this.BacklogViewModel.jxIssuesDataService.OriginalTask.updateIssueProperties(jxTask);
        }
        private void OnDgBacklog_CurrentCellValidating(object? sender, CurrentCellValidatingEventArgs e)
        {
            e.IsValid = true;
            var newValue = ((string)e.NewValue).Trim();

            SfDataGrid dataGrid = (sender as SfDataGrid)!;
            var currentItem = dataGrid?.SelectedItem as JxTask;

            if (newValue == "")
            {
                e.NewValue = null;
            }
            else if (newValue == "0" && ((e.Column.MappingName == nameof(JxTask.schaetzung)) || (e.Column.MappingName == nameof(JxTask.schaetzungOffiziell))))
            {
                    e.NewValue = null;
            }
            else if (e.Column.MappingName == (nameof(JxTask.schaetzung)) || (e.Column.MappingName == (nameof(JxTask.schaetzungOffiziell))))
            {
                if (!int.TryParse(newValue, out var value))
                {
                    MessageBox.Show("Ungültiger Wert");
                    e.IsValid = false;
                }
            }
        }
        private void OnContextMenu_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnContextMenu_Loaded));
            try
            {
                this.miStatus.IsEnabled = false;
                if (dgBacklog != null && dgBacklog.CurrentItem != null)
                {
                    JxTask currentTask = dgBacklog.CurrentItem as JxTask;
                    bool multiSelect = dgBacklog.SelectedItems.Count > 1;

                    var selectedRows = (from row in dgBacklog.SelectedItems
                                        select (row as JxTask).id).ToList();

                    if (!multiSelect)
                    {
                        this.miStatus.IsEnabled = true;
                        //this.miPrioPunkte.IsEnabled = true;
                        if (DataContext is BacklogViewModel viewModel)
                        {
                            this.miPrioPunkte.Visibility = viewModel.jxIssuesDataService.currentUser.role == enumRole.admin ? Visibility.Visible : Visibility.Collapsed;
                        }
                        this.miStatusOffen.Visibility = currentTask.status != enumStatus.opened ? Visibility.Visible : Visibility.Collapsed;
                        this.miStatusTest.Visibility = currentTask.status == enumStatus.imTest ? Visibility.Collapsed : Visibility.Visible;
                        this.miStatusBearbeitung.Visibility = currentTask.status == enumStatus.inBearbeitung ? Visibility.Collapsed : Visibility.Visible;
                        this.miStatusOffenKlaerungsbedarf.Visibility = currentTask.status == enumStatus.klaerungsbedarf ? Visibility.Collapsed : Visibility.Visible;
                    }
                    this.miPrioPunkte.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred in {MethodName}", nameof(OnContextMenu_Loaded));
                Log.Error("{@exception}", ex);
                var innerException = clsFileSystem.getInnermostException(ex);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Log.Information("Exit {MethodName} method.", nameof(OnContextMenu_Loaded));
        }
        private async void OnContextMenuMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnContextMenuMenuItem_Click));

            if (this.dgBacklog == null || this.dgBacklog.SelectedItem is not JxTask selectedTask || e.OriginalSource is not MenuItem menuItem) { return; }

            switch (menuItem.Header)
            {
                case "Offen": selectedTask.status = enumStatus.opened; break;
                case "Im Test": selectedTask.status = enumStatus.imTest; break;
                case "In Bearbeitung": selectedTask.status = enumStatus.inBearbeitung; break;
                case "Klärungsbedarf": selectedTask.status = enumStatus.klaerungsbedarf; break;

                case "In Zwischenablage kopieren":
                    if (dgBacklog.SelectedItems == null || dgBacklog.SelectedItems.Count == 0) { break; }

                    var clipboardString = String.Empty;
                    foreach (JxTask task in dgBacklog.SelectedItems) { clipboardString += Misc.formatInZwischenablage(task); }
                    Misc.InZwischenablageKopieren(clipboardString);
                    break;

                case "Daten Refreshen":
                    await this.BacklogViewModel.jxIssuesDataService.datenRefreshen();
                    this.tbSearchBox.Clear();
                    this.SetGridFocusToJxIssue(selectedTask.id);
                    break;

                case "Priorität ändern":
                    this.OnBtnPrioPunkteGo_Click(sender, e);
                    break;
            }

            e.Handled = true;
            Log.Information("Exit {MethodName} method.", nameof(OnContextMenuMenuItem_Click));
        }
        private async void OnPrioPunkteList_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnPrioPunkteList_Click));

            int id;
            bool isUpward = false;

            try
            {
                if (dgBacklog != null && dgBacklog.CurrentItem != null)
                {
                    bool multiSelect = dgBacklog.SelectedItems.Count > 1;
                    var selectedRows = (from row in dgBacklog.SelectedItems
                                        select (row as JxTask).id).ToList();

                    JxTask currentTask = dgBacklog.CurrentItem as JxTask;

                    var currentIdx = currentTask.sortOrder;

                    MenuItem? menuItem = e.OriginalSource as MenuItem;

                    List<JxTask> updateTaks = new List<JxTask>();
                    if (currentTask != null && menuItem != null)
                    {
                        id = currentTask.id;

                        switch (menuItem.Header)
                        {
                            case "Oben":
                                isUpward = true;
                                break;
                        }

                        var ids = (multiSelect) ? selectedRows : new List<int> { id };
                        await this.BacklogViewModel.jxIssuesDataService.movePrioPunkteObenUnten(selectedRows, isUpward);
                    }
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, ex, "View.BacklogView");
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Log.Information("Exit {MethodName} method.", nameof(OnPrioPunkteList_Click));
        }
        private void OnUserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           Log.Information("Enter {MethodName} method.", nameof(OnUserControl_PreviewKeyDown));
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                try
                {
                    string clipboardString = string.Empty;

                    if (dgBacklog.SelectedItems.Count > 1)
                    {
                        foreach (JxTask task in dgBacklog.SelectedItems)
                        {
                            clipboardString += Misc.formatInZwischenablage(task);
                        }
                    }
                    else if (dgBacklog.SelectedItem != null)
                    {
                        var selectedIssue = dgBacklog.SelectedItem as JxTask;

                        clipboardString += Misc.formatInZwischenablage(selectedIssue);
                    }

                    if (clipboardString != string.Empty)
                    {
                        Misc.InZwischenablageKopieren(clipboardString);
                    }
                    e.Handled = true;
                }

                catch (Exception ex)
                {
                    var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                    libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, ex, "View.BacklogView");
                    MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            Log.Information("Exit {MethodName} method.", nameof(OnUserControl_PreviewKeyDown));
        }
        private void OnBtnPrioPunkteGo_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnBtnPrioPunkteGo_Click));
            var newSortOrder = PrioritaetAendern.getNewPrioPunkteFromDialog();
            if (newSortOrder <= 0) { return; }

            if (dgBacklog != null && dgBacklog.CurrentItem != null)
            {
                if (this.dgBacklog.CurrentItem is not JxTask currentTask) { return; }
                currentTask!.sortOrder = newSortOrder;

                #region setSelectinIndex
                ObservableCollection<JxTask> aktuelleissues = (dgBacklog.ItemsSource as ObservableCollection<JxTask>)!;

                var fx = (from x in aktuelleissues
                          where x.sortOrder == currentTask.sortOrder
                          select x).FirstOrDefault();

                if (fx != null)
                {
                    dgBacklog.SelectedItem = fx;

                    if (fx != null)
                    {
                        // Ermitteln Sie den RowColumnIndex für das ausgewählte Element
                        int rowIndex = dgBacklog.ResolveToRowIndex(fx);
                        int columnIndex = dgBacklog.ResolveToStartColumnIndex();

                        // Erstellen Sie den RowColumnIndex
                        Syncfusion.UI.Xaml.ScrollAxis.RowColumnIndex rowColumnIndex = new Syncfusion.UI.Xaml.ScrollAxis.RowColumnIndex(rowIndex, columnIndex);

                        // Rufen Sie die ScrollInView-Methode auf
                        dgBacklog.ScrollInView(rowColumnIndex);
                    }
                }
                #endregion
            }
            Log.Information("Exit {MethodName} method.", nameof(OnBtnPrioPunkteGo_Click));
        }
        #endregion

        private List<JxTask> FindMatches(string searchText)
        {
            Log.Information("Enter {MethodName} method.", nameof(FindMatches));
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(FindMatches));
                var dataGridItems = dgBacklog.ItemsSource as ObservableCollection<JxTask>;

                Log.Information("Exit {MethodName} method.", nameof(FindMatches));
                return dataGridItems?.Where(task => task.titelLong.ToLower().Contains(searchText)).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred in {MethodName}", nameof(FindMatches));
                Log.Error("{@exception}", ex);
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);                                
            }

            Log.Information("Exit {MethodName} method.", nameof(FindMatches));
            return null;
        }
        private void HighlightNextMatch()
        {
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(HighlightNextMatch));
                if (_currentMatches.Count == 0)
                    return;

                _currentMatchIndex++;
                if (_currentMatchIndex >= _currentMatches.Count)
                    _currentMatchIndex = 0; // Cycle back to the first match.

                var matchedTask = _currentMatches[_currentMatchIndex];

                HighlightGridItem(matchedTask);

                Log.Information("Exit {MethodName} method.", nameof(HighlightNextMatch));
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred in {MethodName}", nameof(HighlightNextMatch));
                Log.Error("{@exception}", ex);
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void HighlightGridItem(JxTask taskToHighlight)
        {
            Log.Information("Enter {MethodName} method.", nameof(HighlightGridItem));
            try
            {
                if (taskToHighlight == null) { return; }

                dgBacklog.SelectedItem = taskToHighlight;
                int rowIndex = dgBacklog.ResolveToRowIndex(taskToHighlight);
                int columnIndex = dgBacklog.ResolveToStartColumnIndex();

                var rowColumnIndex = new Syncfusion.UI.Xaml.ScrollAxis.RowColumnIndex(rowIndex, columnIndex);
                if (rowColumnIndex.RowIndex >= 0) { dgBacklog.ScrollInView(rowColumnIndex); }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred in {MethodName}", nameof(HighlightGridItem));
                Log.Error("{@exception}", ex);
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            Log.Information("Exit {MethodName} method.", nameof(HighlightGridItem));
        }
        private void ClearSearchAndResetDropdown(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(ClearSearchAndResetDropdown));
            this.cmbxSelectedMitarbeier.SelectedIndex = 0;
            tbSearchBox.Text = string.Empty;
            tbSearchBox.Focus();
            Log.Information("Exit {MethodName} method.", nameof(ClearSearchAndResetDropdown));
        }
        private void RefreshDataGrid(object sender, EventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(RefreshDataGrid));
            this.dgBacklog.GetVisualContainer()?.InvalidateMeasureInfo();
            Log.Information("Exit {MethodName} method.", nameof(RefreshDataGrid));
        }
        private void SetGridFocusToJxIssue(int taskId)
        {
            if (dgBacklog == null || dgBacklog.ItemsSource is not ObservableCollection<JxTask> sfDataGridItemsSource) { return; }
            var fx = (from x in sfDataGridItemsSource where x.id == taskId select x).FirstOrDefault();
            if (fx == null) { return; }             // task is removed during update
            dgBacklog.SelectedItem = fx;
        }
    }
}