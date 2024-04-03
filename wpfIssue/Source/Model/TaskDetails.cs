#nullable disable
using Syncfusion.Windows.Controls.Gantt;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace wpfIssues.Model
{
    public class TaskDetails : IGanttTask, INotifyPropertyChanged
    {
        #region Fields
        private int _taskId;
        private string _taskName;
        private DateTime _startDate;
        private DateTime _finishDate;
        private TimeSpan _duration;
        private double _cost;
        private DateTime _baselineStart;
        private DateTime _baselineFinish;
        private double _baselineCost;
        private ObservableCollection<Predecessor> _predecessor;
        private ObservableCollection<Resource> _resources;
        private double _progress;
        private bool _isActive;
        private double _fixedCost;
        private double _totalCost;
        private double _baseline;
        private double _variance;
        private double _actualCost;
        private double _remainingCost;
        private bool _isSummaryRow;
        private IGanttTask _parentNode;
        private ObservableCollection<IGanttTask> _child;
        private bool _isMileStone;
        #endregion

        #region Properties
        public ObservableCollection<IGanttTask> Child
        {
            get
            {
                return _child;
            }
            set
            {
                _child = value;
                OnPropertyChanged("Child");
            }
        }
        public virtual int TaskId
        {
            get
            {
                return _taskId;
            }
            set
            {
                _taskId = value;
                OnPropertyChanged("TaskId");
            }
        }
        public string TaskName
        {
            get
            {
                return _taskName;
            }
            set
            {
                _taskName = value;
                OnPropertyChanged("TaskName");
            }
        }
        public virtual DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }
        public virtual DateTime FinishDate
        {
            get
            {
                return _finishDate;
            }
            set
            {
                if (_finishDate != value)
                {
                    _finishDate = value;
                    OnPropertyChanged("FinishDate");
                }
            }
        }
        public virtual TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public double Cost
        {
            get
            {
                return _cost;
            }
            set
            {
                _cost = value;
                OnPropertyChanged("Cost");
            }
        }
        public DateTime BaselineStart
        {
            get
            {
                return _baselineStart;
            }
            set
            {
                _baselineStart = value;
                OnPropertyChanged("BaselineStart");
            }
        }
        public DateTime BaselineFinish
        {
            get
            {
                return _baselineFinish;
            }
            set
            {
                if (_baselineFinish != value)
                {
                    _baselineFinish = value;
                    OnPropertyChanged("BaselineFinish");
                }
            }
        }
        public double BaselineCost
        {
            get
            {
                return _baselineCost;
            }
            set
            {
                if (_baselineCost != value)
                {
                    _baselineCost = value;
                    OnPropertyChanged("BaselineCost");
                }
            }
        }
        public ObservableCollection<Predecessor> Predecessor
        {
            get
            {
                return _predecessor;
            }
            set
            {
                _predecessor = value;
                OnPropertyChanged("Predecessor");
            }
        }
        public ObservableCollection<Resource> Resources
        {
            get
            {
                return _resources;
            }
            set
            {
                _resources = value;
                OnPropertyChanged("Resources");
            }
        }
        public double Progress
        {
            get
            {
                return Math.Round(_progress, 2);
            }
            set
            {
                if (value <= 100.0 && _progress != value)
                {
                    _progress = value;
                    OnPropertyChanged("Progress");
                }
            }
        }
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }
        public double FixedCost
        {
            get
            {
                return _fixedCost;
            }
            set
            {
                _fixedCost = value;
                OnPropertyChanged("FixedCost");
            }
        }
        public double TotalCost
        {
            get
            {
                return _totalCost;
            }
            set
            {
                _totalCost = value;
                OnPropertyChanged("TotalCost");
            }
        }
        public double Baseline
        {
            get
            {
                return _baseline;
            }
            set
            {
                _baseline = value;
                OnPropertyChanged("Baseline");
            }
        }
        public double Variance
        {
            get
            {
                return _variance;
            }
            set
            {
                _variance = value;
                OnPropertyChanged("Variance");
            }
        }
        public double ActualCost
        {
            get
            {
                return _actualCost;
            }
            set
            {
                _actualCost = value;
                OnPropertyChanged("ActualCost");
            }
        }
        public double RemainingCost
        {
            get
            {
                return _remainingCost;
            }
            set
            {
                _remainingCost = value;
                OnPropertyChanged("RemainingCost");
            }
        }
        public bool IsSummaryRow
        {
            get
            {
                return _isSummaryRow;
            }
            set
            {
                _isSummaryRow = value;
                OnPropertyChanged("IsSummaryRow");
            }
        }
        public IGanttTask ParentNode
        {
            get
            {
                return _parentNode;
            }
            set
            {
                _parentNode = value;
                OnPropertyChanged("ParentNode");
            }
        }
        public bool IsMileStone
        {
            get
            {
                return _isMileStone;
            }
            set
            {
                _isMileStone = value;
                OnPropertyChanged("IsMileStone");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public TaskDetails()
        {
            Child = new ObservableCollection<IGanttTask>();
            _predecessor = new ObservableCollection<Predecessor>();
            _resources = new ObservableCollection<Resource>();
        }
        public void OnPropertyChanged(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(propertyName);
            }

            //the reason for the the null check (?) is that, we will raise an event, only if already someone is subscribed to it,
            // otherwise the is no point to rise events!
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
