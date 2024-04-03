using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using wpfIssues.Model;
using Serilog;
using Syncfusion.SfSkinManager;
using System.Reflection;

namespace wpfIssues.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Fields
        private Mitarbeiter _currentUser;
        private ObservableCollection<Mitarbeiter>? _currentUserTeammates;
        private ObservableCollection<JxTask> _ganttTasks = new ObservableCollection<JxTask>();
        private ObservableCollection<Mitarbeiter>? _mitarbeiters;
        private ICommand _refreshDataCommand;
        private ObservableCollection<JxTask> _tasks = new ObservableCollection<JxTask>();
        private ObservableCollection<Todo>? _todos = new ObservableCollection<Todo>();
        private int _todoDropDownUser;
        private int _currentSelectedMitarbeiter;
        #endregion

        #region Properties
        public Mitarbeiter currentUser
        {
            get { return _currentUser; }
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                }
            }
        }
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
        public ObservableCollection<JxTask> ganttTasks
        {
            get
            {
                return _ganttTasks;
            }
            set
            {
                if (value != null && value != _ganttTasks)
                {
                    _ganttTasks.CollectionChanged -= OnGanttTasks_CollectionChanged;

                    _ganttTasks = value;

                    _ganttTasks.CollectionChanged += OnGanttTasks_CollectionChanged;
                }
            }
        }
        public ObservableCollection<Mitarbeiter>? mitarbeiters
        {
            get
            {
                return _mitarbeiters;
            }
            set
            {
                if (_mitarbeiters != value)
                {
                    _mitarbeiters = value;
                    OnPropertyChanged(nameof(mitarbeiters));
                }
            }
        }
        public ICommand RefreshDataCommand
        {
            get
            {
                if (_refreshDataCommand == null)
                {
                    _refreshDataCommand = new Command.RelayCommand(ExecuteRefreshData, CanExecuteRefreshData);
                }
                return _refreshDataCommand;
            }
        }
        public ObservableCollection<JxTask> tasks
        {
            get
            {
                return _tasks;
            }
            set
            {
                if (value != _tasks)
                {
                    if (_tasks != null)
                    {
                        _tasks.CollectionChanged -= OnTasks_CollectionChanged;
                    }

                    _tasks = value;

                    if (_tasks != null)
                    {
                        _tasks.CollectionChanged += OnTasks_CollectionChanged;
                    }
                    OnPropertyChanged(nameof(tasks));
                }
            }
        }
        public ObservableCollection<Todo>? Todos
        {
            get
            {
                return _todos;
            }

            set
            {
                if (_todos != value)
                {
                    _todos = value;
                    OnPropertyChanged(nameof(Todos));
                }
            }
        }
        public int todoDropDownUser
        {
            get { return _todoDropDownUser; }
            set 
            { 
                if (_todoDropDownUser != value)
                {
                    _todoDropDownUser = value; 
                }
            }
        }
        public int currentSelectedMitarbeiter
        {
            get { return _currentSelectedMitarbeiter; }
            set
            {
                if (_currentSelectedMitarbeiter != value)
                {
                    _currentSelectedMitarbeiter = value;
                    OnPropertyChanged(nameof(currentSelectedMitarbeiter));
                }
            }
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            try 
            {
                Log.Information("Enter {MethodName} method.", nameof(MainViewModel));
                this.getMitarbeiters();            
         
                _tasks = new ObservableCollection<JxTask>();
                _tasks.CollectionChanged += OnTasks_CollectionChanged;
            
                this.getProjectsIssues();
                this.getGanttTasks();
                var currentMitarbeiter = this.mitarbeiters?.FirstOrDefault(ma => ma.id == Properties.Settings.Default.mitarbeiter_id);
                this._todoDropDownUser = Properties.Settings.Default.mitarbeiter_id;
                this._todos = this.getTodos();
                Log.Information("Exit {MethodName} method.", nameof(MainViewModel));
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, ex, $"{Environment.MachineName}: ViewModel.MainViewModel");
                Log.Fatal("Error loading MainViewModel: {@innerException}", innerException);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
            }
        }
        private void OnGanttTasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnGanttTasks_CollectionChanged));
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (var item in e.NewItems)
                        {

                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (var item in e.OldItems)
                        {
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
            Log.Information("Exit {MethodName} method.", nameof(OnGanttTasks_CollectionChanged));
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(propertyName);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Global
        private bool CanExecuteRefreshData(object parameter)
        {
            return true;
        }
        public bool datenRefreshen()
        {
            Log.Information("Enter {MethodName} method.", nameof(datenRefreshen));
            if (_todos == null || _tasks == null || _ganttTasks == null)
            {
                return true;
            }

            Todos?.Clear();
            this.Todos = getTodos();

            if (!getProjectsIssues() || !getGanttTasks())
            {
                MessageBox.Show($"Unable to refresh data!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Log.Information("Exit {MethodName} method.", nameof(datenRefreshen));
            return true;
        }
        private void ExecuteRefreshData(object parameter)
        {
            Log.Information("Enter {MethodName} method.", nameof(ExecuteRefreshData));
            datenRefreshen();
            Log.Information("Exit {MethodName} method.", nameof(ExecuteRefreshData));
        }
        public void getMitarbeiters()
        {
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(getMitarbeiters));                
                var allMitarbeiters = Mitarbeiter.getMitarbeiters();
                if (allMitarbeiters == null)
                {
                    MessageBox.Show("Liste der Mitarbeiter konnte nicht abgerufen werden.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new InvalidOperationException("Unable to get Mitarbeiters list");
                }

                var tmpMitarbeiter = new Mitarbeiter();
                tmpMitarbeiter.id = 0;
                tmpMitarbeiter.gitlab_id = 0;
                tmpMitarbeiter.role = 0;
                tmpMitarbeiter.name = "alle Mitarbeiter";
                allMitarbeiters.Insert(0, tmpMitarbeiter);

                this.mitarbeiters = new ObservableCollection<Mitarbeiter>(allMitarbeiters);

                Log.Information("Getting current user information");
                var currentUserMitarbeiter = this.mitarbeiters.FirstOrDefault(ma => ma.id == Properties.Settings.Default.mitarbeiter_id);
                if (currentUserMitarbeiter == null)
                {
                    MessageBox.Show($"Ungültige Mitarbeiter-ID: {Properties.Settings.Default.mitarbeiter_id} in der Konfigurationsdatei.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new InvalidOperationException($"Invalid mitarbeiter_id in config file");
                }

                this.currentUser = currentUserMitarbeiter;
                var teammates = this.mitarbeiters.Where(ma => ma.team == currentUser.team).ToList();

                if (currentUser.gitUsername == "milica")        //add programming team to Mili's Todo Dropdown
                {
                    var programmierungTeam = this.mitarbeiters.Where(ma => ma.team == enumTeam.programmierung).ToList();
                    teammates.AddRange(programmierungTeam);
                }

                this.currentUserTeammates = new ObservableCollection<Mitarbeiter>(teammates);
                Log.Information("Exit {MethodName} method.", nameof(getMitarbeiters));
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred in {MethodName}", nameof(getMitarbeiters));
                //var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                Log.Fatal("{@innerException}", ex);
                Log.Information("Shutdown application!");
                Application.Current.Shutdown();
            }
        }
        public bool getProjectsIssues()
        {
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(getProjectsIssues));
                //var latestIssues = MainModel.getProjectsIssues();

                //if (latestIssues == null)
                //{
                //    return false;
                //}

                //if (latestIssues.Count == 0)
                //{
                //    return true;
                //}

                //tasks.Clear();
                //foreach (var task in latestIssues)
                //{
                //    this.tasks.Add(task);
                //}
                //Log.Information("Exit {MethodName} method.", nameof(getProjectsIssues));


                return true;
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                Log.Fatal("{@innerException}", innerException);
                Log.Information("Shutdown application!");
                Application.Current.Shutdown();
            }

            return false;
        }
        public void updateTasks(List<JxTask> updateTasks)
        {
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(updateTasks));
                if (updateTasks.Count == 0)
                {
                    return;
                }

                foreach (var taskX in updateTasks)
                {
                    var originalTask = (from task_ in this.tasks
                                        where task_ != null && task_.id == taskX.id
                                        select task_).FirstOrDefault();

                    if (originalTask != null)
                    {
                        originalTask.aktenzahl = taskX.aktenzahl;
                        originalTask.created_at = taskX.created_at;
                        originalTask.creator_id = taskX.creator_id;
                        originalTask.deadline = taskX.deadline;
                        originalTask.erledigt = taskX.erledigt;
                        originalTask.git = taskX.git;
                        originalTask.id = taskX.id;
                        originalTask.iid = taskX.iid;
                        originalTask.kunde = taskX.kunde;
                        originalTask.anmerkung = taskX.anmerkung;
                        originalTask.mitarbeiter = taskX.mitarbeiter;
                        originalTask.mitarbeiter_id = taskX.mitarbeiter_id;
                        originalTask.prioPunkte = taskX.prioPunkte;
                        originalTask.project_id = taskX.project_id;
                        originalTask.schaetzung = taskX.schaetzung;
                        originalTask.schaetzungOffiziell = taskX.schaetzungOffiziell;
                        originalTask.sortOrder = taskX.sortOrder;
                        originalTask.start = taskX.start;
                        originalTask.ende = taskX.ende;
                        originalTask.status = taskX.status;
                        originalTask.titel = taskX.titel;
                        originalTask.typ = taskX.typ;
                        originalTask.web_url = taskX.web_url;
                    }
                }


                var sortedTasks = this.tasks.OrderBy(x => x.sortOrder).ToList();

                if (sortedTasks != null)
                {
                    this.tasks.Clear();
                    foreach (var task in sortedTasks)
                    {
                        tasks.Add(task);
                    }
                }
                Log.Information("Exit {MethodName} method.", nameof(updateTasks));
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                Log.Fatal("{@innerException}", innerException);
                Log.Information("Shutdown application!");
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region Todos
        public ObservableCollection<Todo>? getTodos()
        {
            Log.Information("Enter {MethodName} method.", nameof(getTodos));
            var dropDownUserTasks = (from x in tasks
                                     where x.mitarbeiter_id == todoDropDownUser &&
                                           x.sortOrder.HasValue &&
                                           x.sortOrder.Value > 0 &&
                                           x.status != enumStatus.imTest
                                    orderby x.sortOrder ascending
                                     select x).ToList();

            ObservableCollection<Todo> result = new ObservableCollection<Todo>();
            if (Todos?.Count != 0)
            {
                Todos?.Clear();
            }
            foreach (var task in dropDownUserTasks)
            {
                result.Add(Todo.JxTask2Todo(task, mitarbeiters));
            }

            Log.Information("Exit {MethodName} method.", nameof(getTodos));
            return result;
        }
        private void updateTodos(JxTask updatedTask, string propertyName)
        {
            Log.Information("Enter {MethodName} method.", nameof(updateTodos));
            var todo = this.Todos?.FirstOrDefault(todo_ => todo_.id == updatedTask.id);

            if (todo == null && updatedTask.mitarbeiter_id == todoDropDownUser)
            {
                this.Todos?.Add(Todo.JxTask2Todo(updatedTask, mitarbeiters));
            }
            else if (todo != null && updatedTask.mitarbeiter_id != todoDropDownUser)
            {
                this.Todos?.Remove(todo);
            }
            else if (todo != null)
            {
                var newValue = updatedTask.GetType().GetProperty(propertyName)!.GetValue(updatedTask);
                var todoXUpdatedProperty = todo.GetType().GetProperty(propertyName);
                todoXUpdatedProperty?.SetValue(todo, newValue);
            }
            Log.Information("Exit {MethodName} method.", nameof(updateTodos));
        }
        #endregion

        #region Backlog
        private void JxTask_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {            
            var updatedTask = sender as JxTask;
            if (updatedTask == null || e.PropertyName == null) return;
            this.updateTodos(updatedTask, e.PropertyName);
        }
        private void OnTasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnTasks_CollectionChanged));
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (JxTask newTask in e.NewItems)
                        {
                            newTask.PropertyChanged += JxTask_PropertyChanged;
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (JxTask oldTask in e.OldItems)
                        {
                            oldTask.PropertyChanged -= JxTask_PropertyChanged;
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null)
                    {
                        foreach (JxTask newTask in e.NewItems)
                        {
                            newTask.PropertyChanged += JxTask_PropertyChanged;
                        }
                    }
                    if (e.OldItems != null)
                    {
                        foreach (JxTask oldTask in e.OldItems)
                        {
                            oldTask.PropertyChanged -= JxTask_PropertyChanged;
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
            Log.Information("Exit {MethodName} method.", nameof(OnTasks_CollectionChanged));
        }
        #endregion

        #region Gantt
        private bool getGanttTasks()
        {
            Log.Information("Enter {MethodName} method.", nameof(getGanttTasks));
            ObservableCollection<JxTask> ganttTasks_ = new ObservableCollection<JxTask>();

            if (this.mitarbeiters is null)
            {
                MessageBox.Show($"The 'mitarbeiters' list can not be null!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var allGanttTasks = from task in this.tasks
                                where task != null &&
                                       task.sortOrder.HasValue &&
                                       task.sortOrder.Value > 0
                                select task;

            var teamProgrammierung = (from ma in this.mitarbeiters
                                      where ma.team == enumTeam.programmierung
                                      select ma).ToList();

            var teamTechnic = (from ma in this.mitarbeiters
                               where ma.team == enumTeam.technik
                               select ma).ToList();

            var supportTeam = (from ma in this.mitarbeiters
                               where ma.team == enumTeam.support
                               select ma).ToList();

            if (teamProgrammierung.Count > 0)
            {
                JxTask programmierung_scope = new JxTask()
                {
                    titelLong = "Programmierung",
                };
                ganttTasks_.Add(programmierung_scope);

                int mitarbeiterHierarchy_idx = 1;
                //MitarbeiterX Scope
                foreach (var mitarbeiterX in teamProgrammierung)
                {
                    var mitarbeiterX_tasks = (from task in allGanttTasks
                                              where task.mitarbeiter_id.HasValue &&
                                              task.mitarbeiter_id == mitarbeiterX.id
                                              select task).ToList();

                    if (mitarbeiterX_tasks != null && mitarbeiterX_tasks.Count > 0)
                    {
                        var mitarbeiterX_scope = new JxTask()
                        {
                            mitarbeiter_id = mitarbeiterX.id,
                            //sortOrder = mitarbeiterHierarchy_idx,
                            titelLong = mitarbeiterX.name,
                        };
                        ganttTasks_[0].Child.Add(mitarbeiterX_scope);

                        //MitarbeiterX Tasks    
                        foreach (var taskX in mitarbeiterX_tasks)
                        {
                            ganttTasks_[0].Child[mitarbeiterHierarchy_idx - 1].Child.Add(taskX);
                        }
                        mitarbeiterHierarchy_idx++;
                    }
                }
            }
            if (teamTechnic.Count > 0)
            {
                JxTask technik_scope = new JxTask()
                {
                    titelLong = "Technik",
                };
                ganttTasks_.Add(technik_scope);

                int mitarbeiterHierarchy_idx = 1;
                //MitarbeiterX Scope
                foreach (var mitarbeiterX in teamTechnic)
                {
                    var mitarbeiterX_tasks = (from task in allGanttTasks
                                              where task.mitarbeiter_id.HasValue &&
                                              task.mitarbeiter_id == mitarbeiterX.id
                                              select task).ToList();

                    if (mitarbeiterX_tasks != null && mitarbeiterX_tasks.Count > 0)
                    {
                        var mitarbeiterX_scope = new JxTask()
                        {
                            mitarbeiter_id = mitarbeiterX.id,
                            //sortOrder = mitarbeiterHierarchy_idx,
                            titelLong = mitarbeiterX.name,
                        };
                        ganttTasks_[1].Child.Add(mitarbeiterX_scope);

                        //MitarbeiterX Tasks    
                        foreach (var taskX in mitarbeiterX_tasks)
                        {
                            ganttTasks_[1].Child[mitarbeiterHierarchy_idx - 1].Child.Add(taskX);
                        }
                        mitarbeiterHierarchy_idx++;
                    }
                }
            }
            if (supportTeam.Count > 0)
            {
                JxTask support_scope = new JxTask()
                {
                    titelLong = "Support",
                };
                ganttTasks_.Add(support_scope);

                int mitarbeiterHierarchy_idx = 1;
                //MitarbeiterX Scope
                foreach (var mitarbeiterX in supportTeam)
                {
                    var mitarbeiterX_tasks = (from task in allGanttTasks
                                              where task.mitarbeiter_id.HasValue &&
                                              task.mitarbeiter_id == mitarbeiterX.id
                                              select task).ToList();

                    if (mitarbeiterX_tasks != null && mitarbeiterX_tasks.Count > 0)
                    {
                        var mitarbeiterX_scope = new JxTask()
                        {
                            mitarbeiter_id = mitarbeiterX.id,
                            //sortOrder = mitarbeiterHierarchy_idx,
                            titelLong = mitarbeiterX.name,
                        };
                        ganttTasks_[2].Child.Add(mitarbeiterX_scope);

                        //MitarbeiterX Tasks    
                        foreach (var taskX in mitarbeiterX_tasks)
                        {
                            ganttTasks_[2].Child[mitarbeiterHierarchy_idx - 1].Child.Add(taskX);
                        }
                        mitarbeiterHierarchy_idx++;
                    }
                }
            }

            if (ganttTasks_.Count > 0)
            {
                ganttTasks.Clear();
                foreach (var ganttTaskX in ganttTasks_)
                {
                    this.ganttTasks.Add(ganttTaskX);
                }
            }

            Log.Information("Exit {MethodName} method.", nameof(getGanttTasks));
            return true;
        }
        public async  Task issuesAnordnenAsync(int? sortOrder = null, int? mitarbeiter_id = null, int? project_id = null)
        {
            Log.Information("Enter {MethodName} method.", nameof(issuesAnordnenAsync));
            await MainModel.issuesAnordnenAsync(sortOrder, mitarbeiter_id, project_id);
            datenRefreshen();
            Log.Information("Exit {MethodName} method.", nameof(issuesAnordnenAsync));
        }
        #endregion
    }
}