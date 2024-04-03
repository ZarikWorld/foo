using Serilog;
using ServiceIssues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace wpfIssues.Model
{
    #region Enums
    public enum enumTyp
    {
        Story = 1,
        Bug = 2,
        Performance = 3,
        Luxus = 4,
        Refactor = 5,
    }
    public enum enumStatus
    {
        opened = 1,
        closed = 2,
        imTest = 3,
        inBearbeitung = 4,
        klaerungsbedarf = 5
    }
    #endregion

    public class JxTask : TaskDetails
    {

        #region Fields
        private int _id;
        private int _iid;
        private int _project_id;
        private bool _erledigt;
        private int? _prioPunkte;
        private string _titel;
        private string _titelLong = string.Empty;
        private bool _git;
        private string _web_url;
        private int? _sortOrder;
        private DateTime? _start;
        private DateTime _startDate;
        private DateTime? _ende;
        private TimeSpan _duration;
        private DateTime _finishDate;
        private DateTime? _deadline;
        private int? _mitarbeiter_id;
        private int? _mitarbeiter_gitlab_id;
        private int _creator_id;
        private string _mitarbeiter;
        private Mitarbeiter _mitarbeiterNew;
        private int? _schaetzung;
        private int? _schaetzungOffiziell;
        private DateTime _created_at;
        private enumTyp _typ = enumTyp.Story;
        private enumStatus _status = enumStatus.opened;
        private string? _aktenzahl;
        private string? _kunde;
        private string? _anmerkung;
        ObservableCollection<JxTask> _inLineItems = new ObservableCollection<JxTask>();
        #endregion

        #region Properties
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }
        public int iid
        {
            get
            {
                return _iid;
            }
            set
            {
                _iid = value;
                OnPropertyChanged(nameof(iid));
            }
        }
        public int project_id
        {
            get
            {
                return _project_id;
            }
            set
            {
                _project_id = value;
                OnPropertyChanged(nameof(project_id));
            }
        }
        public bool erledigt
        {
            get
            {
                return _erledigt;
            }
            set
            {
                _erledigt = value;
                OnPropertyChanged(nameof(erledigt));
            }
        }
        public int? prioPunkte
        {
            get
            {
                return _prioPunkte;
            }
            set
            {
                _prioPunkte = value;
                OnPropertyChanged(nameof(prioPunkte));
            }
        }
        public string titel
        {
            get
            {
                return _titel;
            }
            set
            {
                _titel = value;
                OnPropertyChanged(nameof(titel));
            }
        }
        public string titelLong
        {
            get
            {
                if (_titelLong == string.Empty)
                {
                    return $" #{this.iid} - {titel} ";
                }

                return _titelLong;
            }
            set
            {
                _titelLong = value;
            }
        }
        public bool git
        {
            get
            {
                return _git;
            }
            set
            {
                if (value != _git)
                {
                    _git = value;
                    OnPropertyChanged(nameof(git));
                }
            }
        }
        public string web_url
        {
            get
            {
                return _web_url;
            }
            set
            {
                if (value != _web_url)
                {
                    _web_url = value;
                    OnPropertyChanged(nameof(web_url));
                }
            }
        }
        public int? sortOrder
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                if (value != _sortOrder)
                {
                    _sortOrder = value;
                    OnPropertyChanged(nameof(sortOrder));
                }
            }
        }
        public int? mitarbeiter_id
        {
            get
            {
                return _mitarbeiter_id;
            }
            set
            {
                if (_mitarbeiter_id != value)
                {
                    _mitarbeiter_id = value;
                    OnPropertyChanged(nameof(mitarbeiter_id));
                }
            }
        }
        public int? mitarbeiter_gitlab_id
        {
            get
            {
                return _mitarbeiter_gitlab_id;
            }
            set
            {
                if (_mitarbeiter_gitlab_id != value)
                {
                    _mitarbeiter_gitlab_id = value;
                    OnPropertyChanged(nameof(mitarbeiter_gitlab_id));
                }
            }
        }
        public int creator_id
        {
            get
            {
                return _creator_id;
            }
            set
            {
                if (value != _creator_id)
                {
                    _creator_id = value;
                    OnPropertyChanged(nameof(creator_id));
                }
            }
        }
        public string mitarbeiter
        {
            get
            {
                return _mitarbeiter;
            }
            set
            {
                if (value != _mitarbeiter)
                {
                    _mitarbeiter = value;
                    OnPropertyChanged(nameof(mitarbeiter));
                }
            }
        }
        public Mitarbeiter mitarbeiterNew
        {
            get
            {
                return _mitarbeiterNew;
            }
            set
            {
                if (value != _mitarbeiterNew)
                {
                    _mitarbeiterNew = value;
                    //OnPropertyChanged(nameof(mitarbeiterNew));
                }
            }
        }
        public int? schaetzung
        {
            get
            {
                return _schaetzung;
            }
            set
            {
                if (_schaetzung != value)
                {
                    _schaetzung = value;
                    OnPropertyChanged(nameof(schaetzung));
                }
            }
        }
        public int? schaetzungOffiziell
        {
            get
            {
                return _schaetzungOffiziell;
            }
            set
            {
                if (_schaetzungOffiziell != value)
                {
                    _schaetzungOffiziell = value;
                    OnPropertyChanged(nameof(schaetzungOffiziell));
                }
            }
        }
        public DateTime created_at
        {
            get
            {
                return _created_at;
            }
            set
            {
                if (value != _created_at)
                {
                    _created_at = value;
                    OnPropertyChanged(nameof(created_at));
                }
            }
        }
        
        public DateTime? start
        {
            get
            {
                return _start;
            }
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged(nameof(start));
                }
            }
        }
        public DateTime? ende
        {
            get
            {
                return _ende;
            }
            set
            {
                if (value != _ende)
                {
                    _ende = value;
                    OnPropertyChanged(nameof(ende));
                }
            }
        }
        public DateTime? deadline
        {
            get { return _deadline; }
            set
            {
                if (value != _deadline)
                {
                    _deadline = value;
                    OnPropertyChanged(nameof(deadline));
                }
            }
        }
        public enumTyp typ
        {
            get
            {
                return _typ;
            }
            set
            {
                if (value != _typ)
                {
                    _typ = value;
                    OnPropertyChanged(nameof(typ));
                }
            }
        }
        public enumStatus status
        {
            get
            {
                return _status;
            }
            set
            {
                if (value != _status)
                {
                    _status = value;
                    OnPropertyChanged(nameof(status));
                }
            }
        }
        public string? aktenzahl
        {
            get
            {
                return _aktenzahl;
            }
            set
            {
                if (value != _aktenzahl)
                {
                    _aktenzahl = value;
                    OnPropertyChanged(nameof(aktenzahl));
                }
            }
        }
        public string? kunde
        {
            get
            {
                return _kunde;
            }
            set
            {
                if (value != _kunde)
                {
                    _kunde = value;
                    OnPropertyChanged(nameof(kunde));
                }
            }
        }
        public string? anmerkung
        {
            get
            {
                return _anmerkung;
            }
            set
            {
                if (value != _anmerkung)
                {
                    _anmerkung = value;
                    OnPropertyChanged(nameof(anmerkung));
                }
            }
        }
        public ObservableCollection<JxTask> InLineItems
        {
            get
            {
                return _inLineItems;
            }
            set
            {
                _inLineItems = value;

                _inLineItems.CollectionChanged += ItemsCollectionChanged;

                if (value.Count > 0)
                {
                    _inLineItems.ToList().ForEach(n =>
                    {
                        /// To listen the changes occuring in child task.
                        n.PropertyChanged += ItemPropertyChanged;
                    });
                    UpdateDates();
                }

                OnPropertyChanged(nameof(InLineItems));
            }
        }
        public override int TaskId
        {
            get
            {
                if (this.sortOrder != null && this.sortOrder.HasValue)
                {
                    return sortOrder.Value;
                }

                return -1;
            }
            set
            {
                _sortOrder = value;
                OnPropertyChanged(nameof(sortOrder));
            }
        }
        public override DateTime StartDate
        {
            get
            {
                if (this.Child?.Any() == true)
                {
                    return Child.Min(s => s.StartDate);
                }
                if (this.InLineItems.Any())
                {
                    return InLineItems.Min(s => s.StartDate);
                }
                return _startDate;
            }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }
        public override DateTime FinishDate
        {
            get
            {
                if (this.Child?.Any() == true)
                {
                    return Child.Max(s => s.FinishDate);
                }
                if (this.InLineItems?.Any() == true)
                {
                    return InLineItems.Max(s => s.FinishDate);
                }
                return _finishDate;
            }
            set
            {
                if (_finishDate != value)
                {
                    _finishDate = value;
                    OnPropertyChanged(nameof(FinishDate));
                }
            }
        }
        public override TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }
        public string DurationDisplay
        {
            get
            {
                if (Duration == TimeSpan.Zero)
                {
                    return string.Empty;
                }
                return ((int)Duration.TotalHours).ToString();
            }
        }
        #endregion

        public JxTask()
        {
            _inLineItems.CollectionChanged += ItemsCollectionChanged;
        }

        void ItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                {
                    return;
                }

                foreach (JxTask item in e.NewItems)
                {
                    if (item != null)
                    {
                        item.PropertyChanged += ItemPropertyChanged;
                    }
                }
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (JxTask item in e.OldItems)
                    {
                        if (item != null)
                        {
                            item.PropertyChanged -= ItemPropertyChanged;
                        }
                    }
                }
            }
            UpdateDates();
        }
        void ItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
                if (e.PropertyName == "StartDate" || e.PropertyName == "FinishDate" || e.PropertyName == "Progress")
                {
                    UpdateDates();
                }
        }
        private void UpdateDates()
        {
            var tempCal = 0d;

            if (_inLineItems.Count > 0)
            {
                /// Updating the start and end date based on the chagne occur in the date of child task
                StartDate = _inLineItems.Select(c => c.StartDate).Min();
                //FinishDate = _inLineItems.Select(c => c.FinishDate).Max();    //FinishDate is dynamic property without setter
                Progress = (_inLineItems.Aggregate(tempCal, (cur, task) => cur + task.Progress)) / _inLineItems.Count;
            }
        }
        private static void jxTask2ServiceIssue(JxTask xxx, ref ServiceIssues.Issue yyy)
        {
            yyy.aktenzahl = xxx.aktenzahl;
            yyy.created_at = xxx.created_at;
            yyy.creator_id = xxx.creator_id;
            yyy.deadline = xxx.deadline;
            yyy.erledigt = xxx.erledigt;
            yyy.git = xxx.git;
            yyy.id = xxx.id;
            yyy.iid = xxx.iid;
            yyy.kunde = xxx.kunde;
            yyy.anmerkung = xxx.anmerkung;
            yyy.mitarbeiter = xxx.mitarbeiter;
            yyy.mitarbeiter_id = xxx.mitarbeiter_id;
            yyy.prioPunkte = xxx.prioPunkte;
            yyy.project_id = xxx.project_id;
            yyy.schaetzung = xxx.schaetzung;
            yyy.schaetzungOffiziell = xxx.schaetzungOffiziell;            
            yyy.sortOrder = xxx.sortOrder;
            yyy.start = xxx.start;
            yyy.ende = xxx.ende;
            yyy.status = (int)xxx.status;
            yyy.titel = xxx.titel;
            yyy.typ = (int)xxx.typ;
            yyy.web_url = xxx.web_url;
        }
        private static JxTask serviceIssue2JxTask(ServiceIssues.Issue xxx)
        {
            JxTask tmpJxTask = new JxTask();

            tmpJxTask.aktenzahl = xxx.aktenzahl;
            tmpJxTask.created_at = xxx.created_at;
            tmpJxTask.creator_id = xxx.creator_id;
            tmpJxTask.deadline = xxx.deadline;
            tmpJxTask.erledigt = xxx.erledigt;
            tmpJxTask.git = xxx.git;
            tmpJxTask.id = xxx.id;
            tmpJxTask.iid = xxx.iid;
            tmpJxTask.kunde = xxx.kunde;
            tmpJxTask.anmerkung = xxx.anmerkung;
            tmpJxTask.mitarbeiter = xxx.mitarbeiter;
            tmpJxTask.mitarbeiter_id = xxx.mitarbeiter_id;
            tmpJxTask.prioPunkte = xxx.prioPunkte;
            tmpJxTask.project_id = xxx.project_id;
            tmpJxTask.schaetzung = xxx.schaetzung;
            tmpJxTask.schaetzungOffiziell = xxx.schaetzungOffiziell;
            tmpJxTask.sortOrder = xxx.sortOrder;
            tmpJxTask.start = xxx.start;
            tmpJxTask.ende = xxx.ende;
            tmpJxTask.status = (enumStatus)xxx.status;
            tmpJxTask.titel = xxx.titel;
            tmpJxTask.typ = (enumTyp)xxx.typ;
            tmpJxTask.web_url = xxx.web_url;

            return tmpJxTask;
        }
        public static Issue jxTask2ServiceIssue(JxTask jxTask)
        {
            Issue serviceIssue = new Issue();
            JxTask.jxTask2ServiceIssue(jxTask, ref serviceIssue);
            return serviceIssue;
        }
        public static async Task<JxTask?> updateIssueAsync(JxTask jxTask, int? mitarbeiter_id = null)
        {
            mitarbeiter_id ??= Properties.Settings.Default.mitarbeiter_id;

            using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
            {
                try
                {
                    var serviceIssue = jxTask2ServiceIssue(jxTask);
                    Issue? updatedIssue = await serviceIssueClient.updateIssueAsync(serviceIssue, mitarbeiter_id.Value);
                    if (updatedIssue != null)
                    {
                        return serviceIssue2JxTask(updatedIssue);
                    }
                    else
                    {
                        MessageBox.Show($"Error occurred while updating issue. \nid: {jxTask.id} \niid: {jxTask.iid} \ncreated_at: {jxTask.created_at} \nPlease refer to the log file '{App.logFileName}' for more details.", "Update Issue Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        string loggMessage = $"Model.JxTask: Update failed - no response received from the server for the following issue: \nID: {jxTask.id} \niid: {jxTask.iid} \ncreated_at: {jxTask.created_at}";
                        libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, message: loggMessage);

                        return null;
                    }
                }
                catch (Exception ex)
                {
                    var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                    string loggMessage = $"<Model.JxTask.updateIssueAsync>: id: {jxTask.id} iid: {jxTask.iid} created_at: {jxTask.created_at}";
                    libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, innerException, loggMessage);
                    throw new InvalidOperationException(loggMessage, ex);
                }
            }
        }
        public static ObservableCollection<JxTask>? changeSortOrder(int sortOrderFrom, int sortOrderTo, List<int>? projects_ids = null, int? mitarbeiter_id = null)
        {
            projects_ids ??= MainModel.projects_ids;
            mitarbeiter_id ??= Properties.Settings.Default.mitarbeiter_id;

            Issue[] unsortedIssuesArray;

            try
            {
                using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
                {
                    if (projects_ids == null)
                    {
                        projects_ids = MainModel.projects_ids;
                    }

                    unsortedIssuesArray = serviceIssueClient.changeSortOrderAsync(projects_ids.ToArray(), mitarbeiter_id.Value, sortOrderFrom, sortOrderTo).Result;
                };

                if (unsortedIssuesArray is not null)
                {
                    ObservableCollection<JxTask> unsortedJxTasksCollection = MainModel.convertIssueToJxTask(unsortedIssuesArray);
                    var sortedJxTaskList = unsortedJxTasksCollection.OrderBy(t => t.sortOrder).ToList();

                    ObservableCollection<JxTask> sortedJxTaskCollection = new ObservableCollection<JxTask>(sortedJxTaskList);

                    return sortedJxTaskCollection;
                }

            }
            catch (Exception ex)
            {
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException($"{AppDomain.CurrentDomain.BaseDirectory}{Properties.Settings.Default.logFileName}", ex);
            }

            return null;
        }        
        public async Task<List<JxTask>> UpdateIssue20240401(int mitarbeiter_id)
        {
            var result = new List<JxTask>();
            using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
            {
                var serviceIssue = MainModel.JxTask2ServiceIssue(this);
                Issue[] unsortedIssuesArray = await serviceIssueClient.UpdateIssue20240401Async(MainModel.projects_ids.ToArray(), mitarbeiter_id, serviceIssue);

                if (unsortedIssuesArray.Length == 1)
                {
                    result.Add(MainModel.serviceIssue2JxTask(unsortedIssuesArray[0]));
                }
                if (unsortedIssuesArray.Length > 1)
                {
                    for (int i = 0; i < unsortedIssuesArray.Length; i++) { result.Add(MainModel.serviceIssue2JxTask(unsortedIssuesArray[i])); }
                    _ = result.OrderBy(t => t.sortOrder).ToList();
                }
            };

            return result;
        }
        public static List<JxTask> movePrioPunkte(int id, bool isUpward)
        {
            List<JxTask> issues = new List<JxTask>();
            using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
            {
                var updatedIssuesArray = serviceIssueClient.movePrioPunkteAsync(id, isUpward).Result;

                if (updatedIssuesArray.Length != 0)
                {
                    foreach (var issue in updatedIssuesArray)
                    {
                        issues.Add(serviceIssue2JxTask(issue));
                    }
                }
            };

            return issues;
        }
        public static async Task<List<JxTask>> movePrioPunkteList(List<int> projects_ids, List<int> ids, bool isUpward, int mitarbeiter_id)
        {
            List<JxTask> issues = new List<JxTask>();
            using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
            {
                var updatedIssuesArray = await serviceIssueClient.movePrioPunkteListAsync(MainModel.projects_ids.ToArray(), ids.ToArray(), isUpward, mitarbeiter_id);

                if (updatedIssuesArray.Length != 0)
                {
                    foreach (var issue in updatedIssuesArray)
                    {
                        issues.Add(serviceIssue2JxTask(issue));
                    }
                    var sortedJxTaskList = issues.OrderBy(t => t.sortOrder).ToList();
                    return sortedJxTaskList;
                }
            };

            return issues;
        }
        internal void Dispose()
        {
            InLineItems.CollectionChanged -= ItemsCollectionChanged;

            if (InLineItems.Count > 0)
            {
                InLineItems.ToList().ForEach(node =>
                {
                    node.PropertyChanged -= ItemPropertyChanged;
                });
            }
        }
        public void updateIssueProperties(JxTask updatedJxTask)
        {
            Log.Information("Enter1 {MethodName} method.", nameof(updateIssueProperties));
            if (this.erledigt != updatedJxTask.erledigt)                            { this.erledigt = updatedJxTask.erledigt;}
            if (this.prioPunkte != updatedJxTask.prioPunkte)                        { this.prioPunkte = updatedJxTask.prioPunkte;}
            if (this.titel != updatedJxTask.titel)                                  { this.titel = updatedJxTask.titel;}
            if (this.sortOrder != updatedJxTask.sortOrder)                          { this.sortOrder = updatedJxTask.sortOrder;}
            if (this.start != updatedJxTask.start)                                  { this.start = updatedJxTask.start;}
            this.ende = updatedJxTask.ende;
            if (this.mitarbeiter_id != updatedJxTask.mitarbeiter_id)                { this.mitarbeiter_id = updatedJxTask.mitarbeiter_id;}
            if (this.mitarbeiter_gitlab_id != updatedJxTask.mitarbeiter_gitlab_id)  { this.mitarbeiter_gitlab_id = updatedJxTask.mitarbeiter_gitlab_id;}
            if (this.creator_id != updatedJxTask.creator_id)                        { this.creator_id = updatedJxTask.creator_id;}
            if (this.mitarbeiter != updatedJxTask.mitarbeiter)                      { this.mitarbeiter = updatedJxTask.mitarbeiter;}
            if (this.mitarbeiterNew != updatedJxTask.mitarbeiterNew)                { this.mitarbeiterNew = updatedJxTask.mitarbeiterNew;}
            if (this.schaetzung != updatedJxTask.schaetzung)                        { this.schaetzung = updatedJxTask.schaetzung;}
            if (this.schaetzungOffiziell != updatedJxTask.schaetzungOffiziell)      { this.schaetzungOffiziell = updatedJxTask.schaetzungOffiziell;}
            if (this.deadline != updatedJxTask.deadline)                            { this.deadline = updatedJxTask.deadline;}
            if (this.typ != updatedJxTask.typ)                                      { this.typ = updatedJxTask.typ;}
            if (this.status != updatedJxTask.status)                                { this.status = updatedJxTask.status;}
            if (this.aktenzahl != updatedJxTask.aktenzahl)                          { this.aktenzahl = updatedJxTask.aktenzahl;}
            if (this.kunde != updatedJxTask.kunde)                                  { this.kunde = updatedJxTask.kunde;}
            if (this.anmerkung != updatedJxTask.anmerkung)                          { this.anmerkung = updatedJxTask.anmerkung;}
            Log.Information("Exit {MethodName} method.", nameof(updateIssueProperties));
        }
    }
}
