using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using wpfIssues.Model;
using wpfIssues.Common;
using System.ComponentModel;
using System.Collections.Specialized;
using Syncfusion.Windows.Controls.Gantt;

namespace wpfIssues.ViewModel
{
    public class GanttViewModel : ObservableObject
    {
        public GanttViewModel(JxIssuesDataService taskDataService)
        {
            Log.Information("Enter {MethodName} method.", nameof(taskDataService));
            _jxIssuesDataService = taskDataService;
            this.jxIssuesDataService.tasks.CollectionChanged += OnJxTaskCollectionChanged!;
            foreach (var mitarbeiter in jxIssuesDataService.mitarbeiters!) { this.ganttMitarbeiters.Add(mitarbeiter); }
            Log.Information("Exit {MethodName} method.", nameof(taskDataService));
        }

        #region fields/properties
        private readonly JxIssuesDataService _jxIssuesDataService;
        public JxIssuesDataService jxIssuesDataService
        {
            get => _jxIssuesDataService;
        }

        private ObservableCollection<JxTask> _ganttTasks = new ObservableCollection<JxTask>();
        public ObservableCollection<JxTask> ganttTasks
        {
            get { return _ganttTasks; }
            set
            {
                if (_ganttTasks != value)
                {
                    _ganttTasks = value;
                    OnPropertyChanged(nameof(_ganttTasks));
                }
            }
        }

        private ObservableCollection<Mitarbeiter> _ganttMitarbeiters = new ObservableCollection<Mitarbeiter>();
        public ObservableCollection<Mitarbeiter> ganttMitarbeiters
        {
            get
            {
                return _ganttMitarbeiters;
            }
            set
            {
                if (_ganttMitarbeiters != value)
                {
                    _ganttMitarbeiters = value;
                    OnPropertyChanged(nameof(ganttMitarbeiters));
                }
            }
        }
        #endregion

        #region events/subscriptions
        public void OnTaskPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnTaskPropertyChanged));
            Log.Information("Exit {MethodName} method.", nameof(OnTaskPropertyChanged));
        }

        public event EventHandler TaskReplaced;
        private void OnTasksReplaced(object? sender, EventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnTasksReplaced));

            this.getGanttTasks();

            TaskReplaced?.Invoke(this, EventArgs.Empty);
            Log.Information("Exit {MethodName} method.", nameof(OnTasksReplaced));
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
        private void OnJxTaskCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    //foreach (JxTask newItem in e.NewItems!)
                    //{
                    //    this.ganttTasks.Add(newItem);
                    //}
                    this.getGanttTasks();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (JxTask oldItem in e.OldItems!)
                    {
                        var currentGanttTask = (from x in this.ganttTasks where x.id == oldItem.id select x).FirstOrDefault();
                        this.ganttTasks.Remove(currentGanttTask!);
                    }
                    break;
            }
        }
        #endregion

        #region miscellaneous
        private bool getGanttTasks()
        {
            Log.Information("Enter {MethodName} method.", nameof(getGanttTasks));

            var newGanttTasks = (from task in this.jxIssuesDataService.tasks
                                 where task != null &&
                                       task.sortOrder.HasValue &&
                                       task.sortOrder.Value > 0
                                 select task).ToList();

            foreach (var task in this.ganttTasks)
            {
                if (task.Child != null)
                {
                    var existingTask = (from x in task.Child where x.TaskName == task.titel select x).FirstOrDefault();
                    task.Child.Remove(existingTask);
                }
            }

            this.getTeamProgrammierungTask(newGanttTasks);
            this.getTeamTechnicTask(newGanttTasks);
            this.getTeamsupportTask(newGanttTasks);

            Log.Information("Exit {MethodName} method.", nameof(getGanttTasks));
            return true;
        }
        private void getTeamProgrammierungTask(List<JxTask> allGanttTasks)
        {
            Log.Information("Enter {MethodName} method.", nameof(getTeamProgrammierungTask));
            var teamProgrammierung = (from ma in this.ganttMitarbeiters
                                      where ma.team == enumTeam.programmierung
                                      select ma).ToList();

            if (teamProgrammierung.Count > 0)
            {
                JxTask programmierungScope = (from x in this.ganttTasks where x.titelLong == "Programmierung" select x).FirstOrDefault()!;

                if (programmierungScope == null)
                {
                    JxTask newScope = new JxTask() { titelLong = "Programmierung" };
                    this.ganttTasks.Add(newScope);
                    programmierungScope = newScope;
                }

                foreach (var mitarbeiterX in teamProgrammierung)
                {
                    var mitarbeiterX_tasks = (from task in allGanttTasks
                                              where task.mitarbeiter_id.HasValue &&
                                              task.mitarbeiter_id == mitarbeiterX.id
                                              select task).ToList();

                    if (mitarbeiterX_tasks == null || mitarbeiterX_tasks.Count == 0) { continue; }

                    IGanttTask? mitarbeiterHierarchy_idx_new = null;

                    var mitarbeiterScope = (from scope in programmierungScope.Child where scope.TaskId == mitarbeiterX.id select scope).FirstOrDefault();

                    if (mitarbeiterScope == null)
                    {
                        var newMitarbeiterScope = new JxTask()
                        {
                            TaskId = mitarbeiterX.id,
                            mitarbeiter_id = mitarbeiterX.id,
                            //sortOrder = mitarbeiterHierarchy_idx,
                            titelLong = mitarbeiterX.name
                        };
                        programmierungScope.Child.Add(newMitarbeiterScope);
                        mitarbeiterScope = newMitarbeiterScope;
                    }
                    else
                    {
                        mitarbeiterHierarchy_idx_new = mitarbeiterScope;
                    }

                    foreach (var taskX in mitarbeiterX_tasks)
                    {
                        var exisitingTask = (from x in mitarbeiterScope.Child where x.TaskName == taskX.TaskName select x).FirstOrDefault();
                        if (exisitingTask != null) continue;

                        taskX.StartDate = this.SetGanttTaskStartDate(taskX.start);
                        taskX.FinishDate = this.SetGanttTaskFinishDate(taskX.ende);
                        taskX.Duration = this.SetGanntTaskDuration(taskX.schaetzung);
                        mitarbeiterScope.Child.Add(taskX);
                    }
                }
            }
            Log.Information("Exit {MethodName} method.", nameof(getTeamProgrammierungTask));
        }
        private void getTeamTechnicTask(List<JxTask> allGanttTasks)
        {
            Log.Information("Enter {MethodName} method.", nameof(getTeamTechnicTask));
            var teamTechnik = (from ma in this.ganttMitarbeiters
                                      where ma.team == enumTeam.technik
                                      select ma).ToList();

            if (teamTechnik.Count > 0)
            {
                JxTask TechnikScope = (from x in this.ganttTasks where x.titelLong == "Technik" select x).FirstOrDefault()!;

                if (TechnikScope == null)
                {
                    JxTask newScope = new JxTask() { titelLong = "Technik" };
                    this.ganttTasks.Add(newScope);
                    TechnikScope = newScope;
                }

                foreach (var mitarbeiterX in teamTechnik)
                {
                    var mitarbeiterX_tasks = (from task in allGanttTasks
                                              where task.mitarbeiter_id.HasValue &&
                                              task.mitarbeiter_id == mitarbeiterX.id
                                              select task).ToList();

                    if (mitarbeiterX_tasks == null || mitarbeiterX_tasks.Count == 0) { continue; }

                    IGanttTask? mitarbeiterHierarchy_idx_new = null;

                    var mitarbeiterScope = (from scope in TechnikScope.Child where scope.TaskId == mitarbeiterX.id select scope).FirstOrDefault();

                    if (mitarbeiterScope == null)
                    {
                        var newMitarbeiterScope = new JxTask()
                        {
                            TaskId = mitarbeiterX.id,
                            mitarbeiter_id = mitarbeiterX.id,
                            //sortOrder = mitarbeiterHierarchy_idx,
                            titelLong = mitarbeiterX.name
                        };
                        TechnikScope.Child.Add(newMitarbeiterScope);
                        mitarbeiterScope = newMitarbeiterScope;
                    }
                    else
                    {
                        mitarbeiterHierarchy_idx_new = mitarbeiterScope;
                    }

                    foreach (var taskX in mitarbeiterX_tasks)
                    {
                        var exisitingTask = (from x in mitarbeiterScope.Child where x.TaskName == taskX.TaskName select x).FirstOrDefault();
                        if (exisitingTask != null) continue;
                        mitarbeiterScope.Child.Add(taskX);
                    }

                }
            }
            Log.Information("Exit {MethodName} method.", nameof(getTeamTechnicTask));
        }
        private void getTeamsupportTask(List<JxTask> allGanttTasks)
        {
            Log.Information("Enter {MethodName} method.", nameof(getTeamProgrammierungTask));
            var teamSupport = (from ma in this.ganttMitarbeiters
                                      where ma.team == enumTeam.support
                                      select ma).ToList();

            if (teamSupport.Count > 0)
            {
                JxTask SupportScope = (from x in this.ganttTasks where x.titelLong == "Support" select x).FirstOrDefault()!;

                if (SupportScope == null)
                {
                    JxTask newScope = new JxTask() { titelLong = "Support" };
                    this.ganttTasks.Add(newScope);
                    SupportScope = newScope;
                }

                foreach (var mitarbeiterX in teamSupport)
                {
                    var mitarbeiterX_tasks = (from task in allGanttTasks
                                              where task.mitarbeiter_id.HasValue &&
                                              task.mitarbeiter_id == mitarbeiterX.id
                                              select task).ToList();

                    if (mitarbeiterX_tasks == null || mitarbeiterX_tasks.Count == 0) { continue; }

                    IGanttTask? mitarbeiterHierarchy_idx_new = null;

                    var mitarbeiterScope = (from scope in SupportScope.Child where scope.TaskId == mitarbeiterX.id select scope).FirstOrDefault();

                    if (mitarbeiterScope == null)
                    {
                        var newMitarbeiterScope = new JxTask()
                        {
                            TaskId = mitarbeiterX.id,
                            mitarbeiter_id = mitarbeiterX.id,
                            //sortOrder = mitarbeiterHierarchy_idx,
                            titelLong = mitarbeiterX.name
                        };
                        SupportScope.Child.Add(newMitarbeiterScope);
                        mitarbeiterScope = newMitarbeiterScope;
                    }
                    else
                    {
                        mitarbeiterHierarchy_idx_new = mitarbeiterScope;
                    }

                    foreach (var taskX in mitarbeiterX_tasks)
                    {
                        var exisitingTask = (from x in mitarbeiterScope.Child where x.TaskName == taskX.TaskName select x).FirstOrDefault();
                        if (exisitingTask != null) continue;
                        mitarbeiterScope.Child.Add(taskX);
                    }

                }
            }
            Log.Information("Exit {MethodName} method.", nameof(getTeamProgrammierungTask));
        }
        public async Task issuesAnordnenAsync(int? sortOrder = null, int? mitarbeiter_id = null, int? project_id = null)
        {
            Log.Information("Enter {MethodName} method.", nameof(issuesAnordnenAsync));
            await MainModel.issuesAnordnenAsync(sortOrder, mitarbeiter_id, project_id);
            jxIssuesDataService.datenRefreshen();
            Log.Information("Exit {MethodName} method.", nameof(issuesAnordnenAsync));
        }
        private DateTime SetGanttTaskStartDate(DateTime? start)
        {
            var result = new DateTime();
            
            if (start != null)
            {
                result = start.Value;
            }
            else
            {
                var tmpDay = DateTime.Today;
                while (tmpDay.DayOfWeek == DayOfWeek.Saturday || tmpDay.DayOfWeek == DayOfWeek.Sunday)      //TODO 2024-03-27 => Include mitarbeite working hours
                {
                    tmpDay = tmpDay.AddDays(1);
                }
                result = tmpDay.AddHours(8);
            }

            return result;
        }
        private DateTime SetGanttTaskFinishDate(DateTime? ende)
        {
            var result = new DateTime();
            
            if (ende != null)
            {
                result = ende.Value;
            }
            else
            {
                var tmpDay = DateTime.Today;
                while (tmpDay.DayOfWeek == DayOfWeek.Saturday || tmpDay.DayOfWeek == DayOfWeek.Sunday)      //TODO 2024-03-27 => Include mitarbeite working hours
                {
                    tmpDay = tmpDay.AddDays(1);
                }
                tmpDay = tmpDay.AddHours(8);
            }
            return result;
        }
        private TimeSpan SetGanntTaskDuration(int? schaetzung)
        {
            TimeSpan result = new TimeSpan(0);
            
            if (schaetzung.HasValue)
            {
                result = TimeSpan.FromHours(schaetzung.Value);
            }

            return result;
        }
        #endregion
    }
}
