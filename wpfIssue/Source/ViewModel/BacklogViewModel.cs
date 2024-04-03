using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using wpfIssues.Model;
using Serilog;
using System.Collections.Specialized;

namespace wpfIssues.ViewModel
{
    public class BacklogViewModel : INotifyPropertyChanged
    {
        public BacklogViewModel(JxIssuesDataService taskDataService)
        {
            Log.Information("Enter {MethodName} method.", nameof(taskDataService));
            _jxIssuesDataService = taskDataService;
            _jxIssuesDataService.PropertyChanged += OnDataServicePropertyChanged;
            this.jxIssuesDataService.tasks.CollectionChanged += OnJxTaskCollectionChanged!;

            foreach (var mitarbeiter in jxIssuesDataService.mitarbeiters!) { this.backlogMitarbeiters!.Add(mitarbeiter); }
            
            foreach (var task in jxIssuesDataService.tasks)
            {
                //task.PropertyChanged += OnJxTaskPropertyChanged!;     //its not required since the backlogTasks are conntected by ref to the jxIssuesDataServce.tasks
                this.backlogTasks.Add(task);
            }

            Log.Information("Exit {MethodName} method.", nameof(taskDataService));
        }

        #region fields/properties
        private readonly JxIssuesDataService _jxIssuesDataService;
        public JxIssuesDataService jxIssuesDataService
        {
            get => _jxIssuesDataService;
        }

        private ObservableCollection<JxTask> _backlogTasks = new ObservableCollection<JxTask>();
        public ObservableCollection<JxTask> backlogTasks
        {
            get => _backlogTasks;
            set
            {
                if (_backlogTasks != value)
                {
                    _backlogTasks = value;
                    OnPropertyChanged(nameof(backlogTasks));
                }
            }
        }

        private ObservableCollection<Mitarbeiter>? _backlogMiarbeiters = new ObservableCollection<Mitarbeiter>();
        public ObservableCollection<Mitarbeiter>? backlogMitarbeiters
        {
            get
            {
                return _backlogMiarbeiters;
            }
            set
            {
                if (_backlogMiarbeiters != value)
                {
                    _backlogMiarbeiters = value;
                    OnPropertyChanged(nameof(backlogMitarbeiters));
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

        private bool _isLoading = true;
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

        #region events/subscriptions
        //events
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(MitarbeiterId):
                    this.GetSelectedMitarbeiterTasks();
                    break;

                case nameof(IsLoading):
                    this.jxIssuesDataService.IsLoading = this.IsLoading;
                    break;

                default:
                    break;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                        if (this.MitarbeiterId != 0 && newItem.mitarbeiter_id != this.MitarbeiterId) { continue; }
                        this.backlogTasks.Add(newItem);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (JxTask oldItem in e.OldItems!)
                    {
                        if (this.MitarbeiterId != 0 && oldItem.mitarbeiter_id != this.MitarbeiterId) { continue; }
                        
                        var currentBacklogTask = (from x in this.backlogTasks where x.id == oldItem.id select x).FirstOrDefault();

                        this.backlogTasks.Remove(currentBacklogTask!);
                    }
                    break;
            }
        }
        #endregion

        private void GetSelectedMitarbeiterTasks()
        {
            Log.Information("Enter {MethodName} method.", nameof(GetSelectedMitarbeiterTasks));
            IEnumerable<JxTask>? tasks = null;

            if (this.MitarbeiterId == 0)
            {
                tasks = from x in _jxIssuesDataService.tasks select x;
            }
            else
            {
                tasks = (from x in this.jxIssuesDataService.tasks where x.mitarbeiter_id == this.MitarbeiterId select x).ToList();
            }

            if (tasks == null) { return; }

            if (this.backlogTasks.Count > 0) { for (int i = backlogTasks.Count - 1; i >= 0; i--) { this.backlogTasks.RemoveAt(i); } }
            foreach (var task in tasks) { this.backlogTasks.Add(task); }
            Log.Information("Exit {MethodName} method.", nameof(GetSelectedMitarbeiterTasks));
        }
    }
}
