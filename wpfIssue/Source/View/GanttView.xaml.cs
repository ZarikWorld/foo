using Microsoft.Xaml.Behaviors;
using Syncfusion.Data.Extensions;
using Syncfusion.Windows.Controls.Gantt;
using Syncfusion.Windows.Controls.Gantt.Grid;
using Syncfusion.Windows.Controls.Grid;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using wpfIssues.Behavior;
using wpfIssues.Model;
using wpfIssues.ViewModel;

namespace wpfIssues.View
{
    public partial class GanttView : UserControl, IDisposable
    {
        public GanttView()
        {
            InitializeComponent();
            this.GanttViewModel = (DataContext as GanttViewModel)!;
            this.miIssuesAnordnen.Visibility = this.GanttViewModel.jxIssuesDataService.currentUser.role == enumRole.admin ? Visibility.Visible : Visibility.Collapsed;
            this.GanttViewModel.ganttTasks.CollectionChanged += GanttTasks_CollectionChanged;
        }

        #region fields/properties
        private bool _disposed;

        private GanttViewModel _ganttViewModel;
        public GanttViewModel GanttViewModel
        {
            get { return _ganttViewModel; }
            set { _ganttViewModel = value; }
        }
        #endregion

        #region events/subscriptions
        private void GanttTasks_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CollapseGanttNodes();
        }
        private async void OnContextMenuMenuItemAlleIssuesAnordnen_Click(object sender, RoutedEventArgs e)
        {
            if (Gantt.SelectedItems == null || Gantt.SelectedItems[0] == null) { return; }
            JxTask? selectedTask = Gantt.SelectedItems[0] as JxTask;
            if (selectedTask != null)
            {
                await this.GanttViewModel.issuesAnordnenAsync();
            }
            
            CollectionViewSource.GetDefaultView(this.Gantt.ItemsSource).Refresh();
        }
        private async void OnContextMenuMenuItemAlleIssuesAbHierAnordnen_Click(object sender, RoutedEventArgs e)
        {
            if (Gantt.SelectedItems == null || Gantt.SelectedItems[0] == null) { return; }
            JxTask? selectedTask = Gantt.SelectedItems[0] as JxTask;
            if (selectedTask != null)
            {
                await GanttViewModel.issuesAnordnenAsync(sortOrder: selectedTask.sortOrder);
            }

            CollectionViewSource.GetDefaultView(this.Gantt.ItemsSource).Refresh();
        }
        private async void OnContextMenuMenuItemAlleMitarbeiterIssuesAnordnen_Click(object sender, RoutedEventArgs e)
        {
            if (Gantt.SelectedItems == null || Gantt.SelectedItems[0] is not JxTask selectedTask) { return; }

            await GanttViewModel.issuesAnordnenAsync(mitarbeiter_id: selectedTask.mitarbeiter_id);

                CollectionViewSource.GetDefaultView(this.Gantt.ItemsSource).Refresh();
        }
        private async void OnContextMenuMenuItemAlleMitarbeiterIssuesAbHierAnordnen_Click(object sender, RoutedEventArgs e)
        {
            if (Gantt.SelectedItems != null && Gantt.SelectedItems[0] != null)
            {
                JxTask? selectedTask = Gantt.SelectedItems[0] as JxTask;
                if (selectedTask != null)
                {
                    await GanttViewModel.issuesAnordnenAsync(mitarbeiter_id: selectedTask.mitarbeiter_id, sortOrder: selectedTask.sortOrder);
                }
            }

            CollectionViewSource.GetDefaultView(this.Gantt.ItemsSource).Refresh();
        }
        private void OnMenuItemInZwischenablageKopieren_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedNodes = this.Gantt.GanttGrid.SelectedNodes;
                if (selectedNodes == null)
                {
                    return;
                }

                string clipboardString = String.Empty;

                //single row
                if (selectedNodes.Count == 1)
                {
                    var scope = selectedNodes[0].Item as JxTask;
                    if (scope == null)
                    {
                        return;
                    }

                    //determine scope
                    if (scope.Child.Count > 0)
                    {
                        //team scope
                        if (scope.Child[0].Child.Count > 0)
                        {
                            e.Handled = true;
                            return;
                        }
                        //mitarbeiter
                        foreach (JxTask nodeX in scope.Child)
                        {
                            if (nodeX != null) clipboardString += Misc.formatInZwischenablage(nodeX);
                        }
                    }
                    else
                    {
                        //team scope
                        if (scope.Child.Count > 0 && scope.Child[0].Child.Count > 0)
                        {
                            return;
                        }
                        clipboardString += Misc.formatInZwischenablage(scope);
                    }
                }
                else
                {
                    //multi row
                    var sortedNodes = selectedNodes.Select(node => node.Item as JxTask)
                                                   .Where(ganttTask => ganttTask != null)
                                                   .OrderBy(ganttTask => ganttTask?.mitarbeiterNew?.team)
                                                   .ThenBy(ganttTask => ganttTask?.mitarbeiter_id)
                                                   .ThenBy(ganttTask => ganttTask?.sortOrder)
                                                   .ToList();

                    foreach (var nodeX in sortedNodes)
                    {
                        if (nodeX is JxTask && nodeX != null && nodeX.web_url != string.Empty && nodeX.titel != null)
                        {
                            clipboardString += Misc.formatInZwischenablage(nodeX);
                        }
                    }
                }

                if (clipboardString != string.Empty)
                {
                    Misc.InZwischenablageKopieren(clipboardString);
                }
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, ex, $"{Environment.MachineName}: View.GanttView");
                MessageBox.Show(innerException.Message, "Warning111", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            e.Handled = true;
        }
        private void OnGanttView_Loaded(object sender, RoutedEventArgs e)
        {
            var ganttGrid = this.Gantt.GanttGrid;
            if (ganttGrid != null)
            {
                this.Gantt.GanttGrid.Model.Options.ListBoxSelectionMode = Syncfusion.Windows.Controls.Grid.GridSelectionMode.MultiExtended;
                this.Gantt.GanttGrid.Model.ActiveGridView.CellClick += OnGanttTaskTitel_DoubleClick;
            }
        }
        private void OnGanttTaskTitel_DoubleClick(object sender, GridCellClickEventArgs e)
        {
            if (e.ClickCount == 2 && e.ColumnIndex == 1)
            {
                var currentRowRecord = (e.Source as GridTreeControlImpl)?.Nodes[e.RowIndex - 1]?.Item as JxTask;
                int? sortOrder = currentRowRecord?.sortOrder ?? 0;
                if (sortOrder.HasValue && sortOrder > 0)
                {
                    var web_url = (from task in GanttViewModel.jxIssuesDataService.tasks
                                   where task != null && task.sortOrder == sortOrder
                                   select task).FirstOrDefault()?.web_url;

                    if (web_url != null)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = web_url,
                            UseShellExecute = true
                        });
                    }
                }
            }
            e.Handled = true;
        }
        private void OnGanttNodeDragDelta(object sender, NodeDragAndDropEventArgs args)
        {
            args.Handled = true;
        }
        private void OnGanttNodeDragCompleted(object sender, NodeDragAndDropEventArgs args)
        {
            args.Handled = true;
        }
        private void OnGanttNodeResizingDelta(object sender, NodeResizingEventArgs args)

        {
            args.Handled = true;
        }
        private void OnGanttNodeResizingCompleted(object sender, NodeResizingEventArgs args)

        {
            args.Handled = true;
        }
        private void OnGanttZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            if (slider != null)
            {
                this.Gantt.ZoomFactor = slider.Value;
            }
        }
        private void OnGanttJumptToday_Click(object sender, RoutedEventArgs e)
        {
            Gantt.ScrollGanttChartTo(DateTime.Now.AddDays(-1));
        }
        private void OnDatenRefreshen_Click(object sender, RoutedEventArgs e)
        {
            GanttViewModel.jxIssuesDataService.datenRefreshen();
            var behaviors = Interaction.GetBehaviors(this.Gantt);

            foreach (var behavior in behaviors)
            {
                if (behavior is GridCustomizationBehavior customizationBehavior)
                {
                    customizationBehavior.TriggerCustomizationManually();
                    break;
                }
            }
        }
        #endregion

        private void CollapseGanttNodes()
        {
            if (this.Gantt == null || this.Gantt.GanttGrid == null)
            {
                return;
            }
            
            var nodes = this.Gantt.GanttGrid.Model.Views.OfType<GridTreeControlImpl>()
                .SelectMany(view => view.Nodes).Where(node => node.Level == 1 && node.ParentNode != null && node.HasChildNodes).ToList();

            foreach (var item in nodes)
            {
                this.Gantt.GanttGrid.CollapseNode(item);
                CollectionViewSource.GetDefaultView(this.Gantt.ItemsSource).Refresh();
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
                _disposed = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) method.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Gantt_ItemsSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(this.Gantt.ItemsSource).Refresh();
        }
    }
}