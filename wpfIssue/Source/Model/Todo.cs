#nullable disable
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using Serilog;

namespace wpfIssues.Model
{
    public class Todo : INotifyPropertyChanged
    {
        #region fields/properties
        private DateTime _created_at;
        public DateTime created_at
        {
            get => _created_at;
            set
            {
                if (_created_at != value)
                {
                    _created_at = value;
                    OnPropertyChanged(nameof(created_at));
                }
            }
        }

        private DateTime _deadline;
        public DateTime deadline
        {
            get => _deadline;
            set
            {
                if (_deadline != value)
                {
                    _deadline = value;
                    OnPropertyChanged(nameof(deadline));
                }
            }
        }

        private string _ersteller_name;
        public string ersteller_name
        {
            get => _ersteller_name;
            set
            {
                if (_ersteller_name != value)
                {
                    _ersteller_name = value;
                    OnPropertyChanged(nameof(ersteller_name));
                }
            }
        }

        private int _id;
        public int id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(id));
                }
            }
        }

        private int _iid;
        public int iid
        {
            get => _iid;
            set
            {
                if (_iid != value)
                {
                    _iid = value;
                    OnPropertyChanged(nameof(iid));
                }
            }
        }

        private int? _sortOrder;
        public int? sortOrder
        {
            get => _sortOrder;
            set
            {
                if (_sortOrder != value)
                {
                    _sortOrder = value;
                    OnPropertyChanged(nameof(sortOrder));
                }
            }
        }

        private int _mitarbeiter_id;
        public int mitarbeiter_id
        {
            get => _mitarbeiter_id;
            set
            {
                if (_mitarbeiter_id != value)
                {
                    _mitarbeiter_id = value;
                    OnPropertyChanged(nameof(mitarbeiter_id));
                }
            }
        }

        private int? _prioPunkte;
        public int? prioPunkte
        {
            get
            {
                return _prioPunkte;
            }
            set
            {
                if (_prioPunkte != value)
                {
                    _prioPunkte = value;
                    OnPropertyChanged(nameof(prioPunkte));
                }
            }
        }

        private int _schaetzung;
        public int schaetzung
        {
            get => _schaetzung;
            set
            {
                if (_schaetzung != value)
                {
                    _schaetzung = value;
                    OnPropertyChanged(nameof(schaetzung));
                }
            }
        }

        private int? _schaetzungOffiziell;
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

        private DateTime _start;
        public DateTime start
        {
            get => _start;
            set
            {
                if (_start != value)
                {
                    _start = value;
                    OnPropertyChanged(nameof(start));
                }
            }
        }

        private DateTime _ende;
        public DateTime ende
        {
            get => _ende;
            set
            {
                if (_ende != value)
                {
                    _ende = value;
                    OnPropertyChanged(nameof(ende));
                }
            }
        }

        private enumStatus _status = enumStatus.opened;
        public enumStatus status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(status));
                }
            }
        }

        private string _title;
        public string title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(title));
                }
            }
        }

        private string _web_url;
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
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void UpdateTodo(JxTask modifiedTask, ObservableCollection<Mitarbeiter> mitarbeiters)
        {
            this.created_at = modifiedTask.created_at;

            if (modifiedTask.deadline.HasValue)
            {
                this.deadline = modifiedTask.deadline.Value;
            }

            var ersteller = (from x in mitarbeiters
                             where x.id == modifiedTask.creator_id
                             select x).FirstOrDefault();
            if (ersteller != null) this.ersteller_name = ersteller.name;

            this.id = modifiedTask.id;
            this.iid = modifiedTask.iid;

            this.sortOrder = modifiedTask.sortOrder;
            if (modifiedTask.mitarbeiter_id != null)
            {
                this.mitarbeiter_id = modifiedTask.mitarbeiter_id.Value;
            }

            this.prioPunkte = modifiedTask.prioPunkte;

            this.schaetzung = (int)modifiedTask.Duration.TotalHours;
            this.schaetzungOffiziell = modifiedTask.schaetzungOffiziell;
            if (modifiedTask.start.HasValue)
            {
                this.start = modifiedTask.start.Value;
            }
            if (modifiedTask.ende.HasValue)
            {
                this.ende = modifiedTask.ende.Value;
            }
            this.status = modifiedTask.status;

            //TruncateString
            const int maxLength = 90;
            if (modifiedTask.titel.Length <= maxLength)
                this.title = modifiedTask.titel;
            else
                this.title = modifiedTask.titel.Substring(0, maxLength) + "...";

            this.web_url = modifiedTask.web_url;

            //this.ende = modifiedTask.ende;
        }
        public static Todo JxTask2Todo(JxTask task, ObservableCollection<Mitarbeiter> mitarbeiters)
        {
            var todo = new Todo();

            todo.created_at = task.created_at;

            if (task.deadline.HasValue)
            {
                todo.deadline = task.deadline.Value;
            }
            todo.id = task.id;
            todo.iid = task.iid;

            //TruncateString
            const int maxLength = 90;
            if (task.titel.Length <= maxLength)
                todo.title = task.titel;
            else
                todo.title = task.titel.Substring(0, maxLength) + "...";

            todo.prioPunkte = task.prioPunkte;
            var ersteller = (from x in mitarbeiters
                             where x.id == task.creator_id
                             select x).FirstOrDefault();

            if (ersteller != null) todo.ersteller_name = ersteller.name;

            todo.sortOrder = task.sortOrder;
            if (task.mitarbeiter_id != null)
            {
                todo.mitarbeiter_id = task.mitarbeiter_id.Value;
            }
            todo.schaetzung = (int)task.Duration.TotalHours;
            todo.schaetzungOffiziell = task.schaetzungOffiziell;
            if (task.start.HasValue)
            {
                todo.start = task.start.Value;
            }
            if (task.ende.HasValue)
            {
                todo.ende = task.ende.Value;
            }
            todo.status = task.status;
            todo.web_url = task.web_url;

            return todo;
        }

        private DateTime? endzeitBerechnen()
        {
            DateTime? result = null;

            if (this.schaetzung > 0)
            {
                result = this.start;
                decimal daysX = (decimal)(this.schaetzung/ 8);

                int days = (int)Math.Floor(daysX);
                int hours = this.schaetzung % 8;

                result = result.Value.AddHours(hours);
                result = result.Value.AddDays(days);

                if (result.Value.DayOfWeek == DayOfWeek.Sunday)
                {
                    result = result.Value.AddDays(1);
                }

                if (result.Value.DayOfWeek == DayOfWeek.Saturday)
                {
                    result = result.Value.AddDays(2);
                }
            }

            return result;
        }
        public void updateTodo(JxTask jxTask, ObservableCollection<Mitarbeiter> mitarbeiters )
        {
            Log.Information("Enter {MethodName} method.", nameof(updateTodo));
            
            this.id = jxTask.id;
            this.iid = jxTask.iid;
            this.created_at = jxTask.created_at;
            this.sortOrder = jxTask.sortOrder;
            this.prioPunkte = jxTask.prioPunkte;
            this.status = jxTask.status;
            this.schaetzung = (int)jxTask.Duration.TotalHours;
            this.schaetzungOffiziell = jxTask.schaetzungOffiziell;

            if (jxTask.deadline.HasValue) { this.deadline = jxTask.deadline.Value; }
            if (jxTask.mitarbeiter_id != null) { this.mitarbeiter_id = jxTask.mitarbeiter_id.Value; }
            var ersteller = (from x in mitarbeiters where x.id == jxTask.creator_id select x).FirstOrDefault();
            if (ersteller != null) this.ersteller_name = ersteller.name;
            if (jxTask.start.HasValue) { this.start = jxTask.start.Value; }
            if (jxTask.ende.HasValue) { this.ende = jxTask.ende.Value; }

            //TruncateString
            const int maxLength = 90;
            if (jxTask.titel.Length <= maxLength)
                this.title = jxTask.titel;
            else
                this.title = jxTask.titel.Substring(0, maxLength) + "...";

            this.web_url = jxTask.web_url;

            //this.ende = updatedJxTask.ende;

            Log.Information("Exit {MethodName} method.", nameof(updateTodo));
        }
    }
}
