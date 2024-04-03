using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using wpfIssues.Model;

namespace wpfIssues.ViewModel
{
    public class TodosViewModel : INotifyPropertyChanged
    {
        public TodosViewModel(JxIssuesDataService taskDataService)
        {
            _jxIssuesDataService = taskDataService;
            _jxIssuesDataService.tasks.CollectionChanged += OnJxTaskCollectionChanged!;
            _jxIssuesDataService.PropertyChanged += OnDataServicePropertyChanged;
            _todosTasks.CollectionChanged += OnTodosTasks_CollectionChanged;
            this.getUserTeammates();            

            var mitarbeiterX = (from x in this.currentUserTeammates where x.id == this.jxIssuesDataService.currentUser.id select x).FirstOrDefault();

            //this._mitarbeiterId = this.currentUserTeammates!.IndexOf(mitarbeiterX!);
        }

        private void OnTodosTasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
        }

        #region fields/properties
        private readonly JxIssuesDataService _jxIssuesDataService;
        public JxIssuesDataService jxIssuesDataService
        {
            get => _jxIssuesDataService;
        }
        
        private ObservableCollection<Todo>? _todosTasks = new ObservableCollection<Todo>();
        public ObservableCollection<Todo> todosTasks
        {
            get => _todosTasks!;
            set
            {
                if (_todosTasks != value)
                {
                    _todosTasks = value;
                    OnPropertyChanged(nameof(todosTasks));
                }
            }
        }

        private ObservableCollection<Mitarbeiter>? _currentUserTeammates;
        public ObservableCollection<Mitarbeiter>? currentUserTeammates
        {
            get
            {
                return _currentUserTeammates;
            }
            set
            {
                if (_currentUserTeammates != value)
                {
                    _currentUserTeammates = value;
                    OnPropertyChanged(nameof(currentUserTeammates));
                }
            }
        }

        private int _mitarbeiterId;
        public int MitarbeiterId
        {
            get { return _mitarbeiterId; }
            set
            {
                if (_mitarbeiterId != value)
                {
                    _mitarbeiterId = value;
                    OnPropertyChanged(nameof(MitarbeiterId));
                }
            }
        }
        
        private bool _notShowImTestTodos = true;
        public bool HideImTestTodos
        {
            get
            {
                return _notShowImTestTodos;
            }
            set
            {
                if (_notShowImTestTodos != value)
                {
                    _notShowImTestTodos = value;
                    OnPropertyChanged(nameof(HideImTestTodos));
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
        #endregion

        #region events & subscriptions
        //events
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(HideImTestTodos):
                    this.getMitarbeiterTodos();
                    break;

                case nameof(MitarbeiterId):
                    this.getMitarbeiterTodos();
                    break;

                default:
                    break;
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? TodoTaskPropertyChanged;
        private void OnTodoTaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is not Todo modifiedTodo || e.PropertyName == null) { return; }

            var jxTask = (from x in this.jxIssuesDataService.tasks where x.id == modifiedTodo.id select x).FirstOrDefault();

            jxTask!.status = modifiedTodo.status;
            jxTask!.sortOrder = modifiedTodo.sortOrder;
        }

        //subscriptions
        private void OnDataServicePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(JxIssuesDataService.IsLoading))
            //{
            //    if (sender is JxIssuesDataService jxIssuesDataService)
            //    {
            //        this.IsLoading = jxIssuesDataService.IsLoading;
            //    }
            //}
        }
        private void OnJxTaskCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {            
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (JxTask newItem in e.NewItems!)
                    {
                        if (!this.TodoConditionMet(newItem)) { continue; }

                        var todo = Todo.JxTask2Todo(newItem, jxIssuesDataService.mitarbeiters);
                        newItem.PropertyChanged += OnJxTaskPropertyChanged!;
                        todo.PropertyChanged += OnTodoTaskPropertyChanged!;
                        this.todosTasks.Add(todo);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (JxTask oldItem in e.OldItems!)
                    {
                        if ((from x in this.todosTasks where x.id == oldItem.id select x).FirstOrDefault() is not Todo todo) { continue; }

                        oldItem.PropertyChanged -= OnJxTaskPropertyChanged!;
                        todo.PropertyChanged -= OnTodoTaskPropertyChanged!;
                        this.todosTasks.Remove(todo);
                    }
                    break;
            }            
        }
        private void OnJxTaskPropertyChanged(object sender, PropertyChangedEventArgs e) { this.getMitarbeiterTodos(); }
        #endregion

        private void getUserTeammates()
        {
            Log.Information("Enter {MethodName} method.", nameof(getUserTeammates));
            var teammates = jxIssuesDataService.mitarbeiters!.Where(ma => ma.team == jxIssuesDataService.currentUser.team).ToList();

            if (jxIssuesDataService.currentUser.gitUsername == "milica")
            {
                var programmierungTeam = jxIssuesDataService.mitarbeiters!.Where(ma => ma.team == enumTeam.programmierung).ToList();
                teammates?.AddRange(programmierungTeam!);
            }

            this.currentUserTeammates = new ObservableCollection<Mitarbeiter>(teammates!);
            Log.Information("Exit {MethodName} method.", nameof(getUserTeammates));
        }
        public void getMitarbeiterTodos()
        {
            Log.Information("Enter {MethodName} method.", nameof(getMitarbeiterTodos));
            if (this.todosTasks?.Count > 0)
            {
                for (int i = this.todosTasks.Count - 1; i >= 0; i--)
                {
                    this.todosTasks[i].PropertyChanged -= OnTodoTaskPropertyChanged!;
                    this.todosTasks.RemoveAt(i);
                }
            }

            var mitarbeiterTasks = (from x in jxIssuesDataService.tasks
                                    where this.TodoConditionMet(x)
                                    orderby x.sortOrder ascending
                                    select x).ToList();

            foreach (var taskX in mitarbeiterTasks)
            {
                var todo = Todo.JxTask2Todo(taskX, jxIssuesDataService.mitarbeiters);
                todo!.PropertyChanged += OnTodoTaskPropertyChanged!;
                todosTasks!.Add(todo);
            }
            Log.Information("Exit {MethodName} method.", nameof(getMitarbeiterTodos));
        }
        private bool TodoConditionMet(JxTask jxTaskX)
        {
            if (jxTaskX.mitarbeiter_id == null || jxTaskX.mitarbeiter_id != this.MitarbeiterId) { return false; }
            if (!jxTaskX.sortOrder.HasValue || jxTaskX.sortOrder < 0) { return false; }
            if (this.HideImTestTodos && jxTaskX.status == enumStatus.imTest) { return false; }

            return true;
        }

        //SHOUÖD CHECK IF THEY ARE LEGIT
        public async Task MovePrioPunkteObenUnten(Todo todo, bool isUpward)
        {
            Log.Information("Exit {MethodName} method.", nameof(MovePrioPunkteObenUnten));
            try
            {
                await this.jxIssuesDataService.movePrioPunkteObenUnten(new List<int>() { todo.id }, isUpward);
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                Log.Error("Beim Ändern des Priopunkts ist ein Fehler aufgetreten. Ausnahme: {@innerException}", innerException);
                MessageBox.Show("Beim Ändern des Priopunkts ist ein Fehler aufgetreten.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Log.Information("Exit {MethodName} method.", nameof(MovePrioPunkteObenUnten));
        }
    }
}
