#nullable disable
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using wpfIssues.Model;
using wpfIssues.ViewModel;

namespace wpfIssues.View
{
    public partial class TodosView : UserControl
    {
        public TodosView()
        {
            Log.Information("Enter {MethodName} method.", nameof(TodosView));
            InitializeComponent();

            this.TodosViewModel = DataContext as TodosViewModel;

            this.TodosViewModel.MitarbeiterId = this.TodosViewModel.jxIssuesDataService.currentUser.id;
            
            this.cmbxTeammates.DisplayMemberPath = "name";
            this.cmbxTeammates.SelectedValuePath = "id";
            this.cmbxTeammates.ItemsSource = this.TodosViewModel.currentUserTeammates;
            this.cmbxTeammates.SelectedValue = this.TodosViewModel.jxIssuesDataService.currentUser.id;
            this._ignoreSelectionChanged = false;

            this.lvTodos.ItemContainerGenerator.StatusChanged += OnItemContainerGenerator_StatusChanged;

            Log.Information("Enter {MethodName} method.", nameof(TodosView));
        }

        #region fields/properties
        private TodosViewModel TodosViewModel { get; set; }
        private bool _ignoreSelectionChanged = true;        // ignore the initial hits to OnCmbxTeammates_SelectionChanged due to ItemSource update
        #endregion

        #region events
        private async void OnMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is not Todo todoItem
                                   || e.OriginalSource is not MenuItem menuItem){ return; }

            switch (menuItem.Header)
            {
                case "Offen":           todoItem.status = enumStatus.opened; break;
                case "Im Test":         todoItem.status = enumStatus.imTest; break;
                case "In Bearbeitung":  todoItem.status = enumStatus.inBearbeitung; break;
                case "Klärungsbedarf":  todoItem.status = enumStatus.klaerungsbedarf; break;

                case "Oben" or "Unten":
                    await this.MovePrioPunkteObenUnten(sender, e);
                    break;

                case "In Zwischenablage kopieren":
                    var clipboardString = Misc.formatInZwischenablage(new JxTask() { titel = todoItem.title, web_url = todoItem.web_url });
                    Misc.InZwischenablageKopieren(clipboardString);
                    break;

                case "Daten Refreshen":
                    this.lvTodos.ItemsSource = null;
                    await this.TodosViewModel.jxIssuesDataService.datenRefreshen();
                    this.lvTodos.ItemsSource = this.TodosViewModel.todosTasks;
                    break;

                case "Priorität ändern":
                    this.OnPrioritaetAendern_Clicked(sender, e);
                    this.lvTodos.ItemsSource = null;
                    this.lvTodos.ItemsSource = this.TodosViewModel.todosTasks;
                    break;
            }
            e.Handled = true;
        }
        private void OnLvTodos_DoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Todo todoItem = ((FrameworkElement)sender).DataContext as Todo;

            if (todoItem != null && todoItem.web_url != null)
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = todoItem.web_url,
                    UseShellExecute = true
                });
            }
        }
        private void OnCmbxTeammates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this._ignoreSelectionChanged) { return; }

            this._ignoreSelectionChanged = true;
            this.TodosViewModel.MitarbeiterId = (int)this.cmbxTeammates.SelectedValue;
            this._ignoreSelectionChanged = false;

            this.lvTodos.ItemContainerGenerator.StatusChanged += OnItemContainerGenerator_StatusChanged;
        }
        private void OnItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (lvTodos.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) { return; }
            foreach (var item in lvTodos.Items)
            {
                var listViewItem = (ListViewItem)lvTodos.ItemContainerGenerator.ContainerFromItem(item);
                if (listViewItem == null || listViewItem.ContextMenu is not ContextMenu menu) { continue; }

                foreach (var menuItem in menu.Items.OfType<MenuItem>()) { menuItem.Click += OnMenuItem_Click; }
            }
            lvTodos.ItemContainerGenerator.StatusChanged -= OnItemContainerGenerator_StatusChanged;
        }
        private void OnPrioritaetAendern_Clicked(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnPrioritaetAendern_Clicked));
            if (((FrameworkElement)sender).DataContext is not Todo todoItem || e.OriginalSource is not MenuItem menuItem) { return; }

            var newSortOrder = PrioritaetAendern.getNewPrioPunkteFromDialog();

            if (newSortOrder <= 0) { return; }
            
            todoItem.sortOrder = newSortOrder;

            Log.Information("Exit {MethodName} method.", nameof(OnPrioritaetAendern_Clicked));
        }
        #endregion

        private async Task MovePrioPunkteObenUnten(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(MovePrioPunkteObenUnten));
            if (((FrameworkElement)sender).DataContext is not Todo todoItem || e.OriginalSource is not MenuItem menuItem) { return; }

            await this.TodosViewModel.MovePrioPunkteObenUnten(todoItem, (string)menuItem.Header == "Oben");

            Log.Information("Exit {MethodName} method.", nameof(MovePrioPunkteObenUnten));
        }
    }
}
