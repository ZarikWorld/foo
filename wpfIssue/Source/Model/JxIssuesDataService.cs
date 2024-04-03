using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using wpfIssues.Common;
using wpfIssues.Model;

namespace wpfIssues
{
    //WHO EVER CALLS ME, IS RESPONSBILE FOR EXCEPTION HANDLING (Bubble Up)!
    public class JxIssuesDataService : ObservableObject
    {
        public JxIssuesDataService()
        {
            _tasks.CollectionChanged += Tasks_CollectionChanged;
        }

        #region fields/properties
        private ObservableCollection<JxTask> _tasks = new ObservableCollection<JxTask>();
        public ObservableCollection<JxTask> tasks
        {
            get => _tasks;
            set
            {
                _tasks = value;
            }
        }
        
        private ObservableCollection<Mitarbeiter>? _mitarbeiters = new ObservableCollection<Mitarbeiter>();
        public ObservableCollection<Mitarbeiter>? mitarbeiters
        {
            get
            {
                return _mitarbeiters;
            }
            set
            {
                _mitarbeiters = value;
            }
        }

        public Mitarbeiter currentUser { get; set; }

        private JxTask _originalTask = new JxTask();
        public JxTask OriginalTask
        {
            get { return _originalTask; }
            set { _originalTask = value; }
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

        #region events
        private void Tasks_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => { this.IsLoading = true; });
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (JxTask newTask in e.NewItems!)
                    {
                        newTask.PropertyChanged += Task_PropertyChanged!;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (JxTask oldTask in e.OldItems!)
                    {
                        oldTask.PropertyChanged -= Task_PropertyChanged!;
                    }
                    break;
            }
            Application.Current.Dispatcher.Invoke(() => { this.IsLoading = false; });
        }
        private void Task_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            //if (sender is not JxTask modifiedTask) { return; }
            //if (e.PropertyName == "ende" || e.PropertyName == "StartDate" || e.PropertyName == "FinishDate" || e.PropertyName == "Duration") { return; }

            //Application.Current.Dispatcher.Invoke(() => { this.IsLoading = true; });
            //Task.Run(async () => await modifiedTask.UpdateIssue20240401(currentUser.id))
            //        .ContinueWith(task =>
                    //{
                        
                        
                    //    // Check for errors in the preceding task
                    //    if (task.Exception != null)
                    //    {
                    //        var modifiedTask = (from x in this.tasks where x.id == this.OriginalTask.id select x).First();
                    //        modifiedTask.updateIssueProperties(this.OriginalTask);

                    //        var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(task.Exception);
                    //        Log.Error($"Task konnte nicht aktualisiert werden. Fehler beim Aktualisieren der Task id({modifiedTask.id}). Details: {innerException}", innerException);

                    //        MessageBox.Show($"Task konnte nicht aktualisiert werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    }
                    //    else
                    //    {
                    //        List<JxTask> updatedTasks = task.Result;
                    //        Application.Current.Dispatcher.Invoke(() =>
                    //        {
                    //            if (task.Result.Count == 0)
                    //            {
                    //                var modifiedTask = (from x in this.tasks where x.id == this.OriginalTask.id select x).First();
                    //                modifiedTask.updateIssueProperties(this.OriginalTask);

                    //                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(task.Exception);
                    //                Log.Error($"Task konnte nicht aktualisiert werden. Fehler beim Aktualisieren der Task id({modifiedTask.id}). Details: {innerException}", innerException);

                    //                MessageBox.Show($"Task konnte nicht aktualisiert werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);

                    //            }
                    //            else if (task.Result.Count > 1)
                    //            {
                    //                this.UpdateJxTaskCollection(updatedTasks);
                    //            } else if (task.Result.Count == 1)
                    //            {
                    //                modifiedTask.updateIssueProperties(task.Result[0]);
                    //            }

                    //        });
                    //    }
                    //}, TaskScheduler.Default); // Specifies the scheduler to use for the continuation task. Using the default scheduler.            



            if (sender is not JxTask modifiedTask) { return; }
            // Return immediately if the property change is not relevant.
            if (e.PropertyName == "ende" || e.PropertyName == "StartDate" || e.PropertyName == "FinishDate" || e.PropertyName == "Duration") return;

            Application.Current.Dispatcher.Invoke(async () =>
            {
                this.IsLoading = true;
                // A minimal delay to ensure the UI thread has time to update the UI
                await Task.Delay(50); // Adjust the delay as needed. A shorter delay might suffice.

                // Now proceed with the async operation.
                await Task.Run(async () => await modifiedTask.UpdateIssue20240401(currentUser.id))
                    .ContinueWith(task =>
                    {
                        // Back on the UI thread, update the UI based on the task's outcome.
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (task.Exception != null)
                            {
                                var modifiedTask = (from x in this.tasks where x.id == this.OriginalTask.id select x).First();
                                modifiedTask.updateIssueProperties(this.OriginalTask);

                                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(task.Exception);
                                Log.Error($"Task konnte nicht aktualisiert werden. Fehler beim Aktualisieren der Task id({modifiedTask.id}). Details: {innerException}", innerException);

                                MessageBox.Show($"Task konnte nicht aktualisiert werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                // Process the result. Update your tasks collection or UI elements as necessary.
                                var updatedTasks = task.Result;

                                if (task.Result.Count == 0)
                                {
                                    var modifiedTask = (from x in this.tasks where x.id == this.OriginalTask.id select x).First();
                                    modifiedTask.updateIssueProperties(this.OriginalTask);

                                    var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(task.Exception);
                                    Log.Error($"Task konnte nicht aktualisiert werden. Fehler beim Aktualisieren der Task id({modifiedTask.id}). Details: {innerException}", innerException);

                                    MessageBox.Show($"Task konnte nicht aktualisiert werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);

                                }
                                else if (task.Result.Count > 1)
                                {
                                    this.UpdateJxTaskCollection(updatedTasks);
                                }
                                else if (task.Result.Count == 1)
                                {
                                    modifiedTask.updateIssueProperties(task.Result[0]);
                                }
                            }

                            // Finally, set IsLoading to false to indicate completion.
                            this.IsLoading = false;
                        });
                    }, TaskScheduler.FromCurrentSynchronizationContext()); // Ensure continuation runs on the UI thread.
            });




        }
        #endregion

        public async Task datenRefreshen()
        {
            //FollowUp 2024-03-27 => Should make it async
            Log.Information("Enter {MethodName} method.", nameof(datenRefreshen));
            getMitarbeiters();
            await getTasks();
            Log.Information("Exit {MethodName} method.", nameof(datenRefreshen));
        }
        public void getMitarbeiters()
        {
            Log.Information("Enter {MethodName} method.", nameof(getMitarbeiters));
            
            var allMitarbeiters = Mitarbeiter.getMitarbeiters();

            if (allMitarbeiters == null) { return; }

            var tmpMitarbeiter = new Mitarbeiter();
            tmpMitarbeiter.id = 0;
            tmpMitarbeiter.gitlab_id = 0;
            tmpMitarbeiter.role = 0;
            tmpMitarbeiter.name = "alle Mitarbeiter";
            allMitarbeiters.Insert(0, tmpMitarbeiter);

            if (this.mitarbeiters!.Count > 0) { for (var i = this.mitarbeiters.Count - 1; i >= 0; i--) { this.mitarbeiters.RemoveAt(i); } }

            foreach (var mitarbeiterX in allMitarbeiters) { this.mitarbeiters.Add(mitarbeiterX); }
            Log.Information("Exit {MethodName} method.", nameof(getMitarbeiters));
        }
        public void setUserInfo ()
        {
            Log.Information("Enter {MethodName} method.", nameof(setUserInfo));

            var currentUser = this.mitarbeiters?.FirstOrDefault(ma => ma.id == Properties.Settings.Default.mitarbeiter_id);
            
            if (currentUser == null)
            {
                throw new InvalidOperationException($"Ungültige Mitarbeiter-ID: {Properties.Settings.Default.mitarbeiter_id} in der Konfigurationsdatei.");
            }

            this.currentUser = currentUser;
            Log.Information("Exit {MethodName} method.", nameof(setUserInfo));
        }
        public async Task getTasks()
        {
            Log.Information("Enter {MethodName} method.", nameof(getTasks));
            Application.Current.Dispatcher.Invoke(() => { this.IsLoading = true; });

            var latestIssues = await MainModel.getProjectsIssues();

            for (int i = this.tasks.Count - 1; i >= 0; i--) { this.tasks.RemoveAt(i); }

            if (latestIssues == null || latestIssues.Count == 0) { return; }

            foreach (var task in latestIssues)
            {
                this.tasks.Add(task);
            }
            Application.Current.Dispatcher.Invoke(() => { this.IsLoading = false; });
            Log.Information("Exit {MethodName} method.", nameof(getTasks));

        }
        public async Task movePrioPunkteObenUnten(List<int> ids, bool isUpward)
        {
            Application.Current.Dispatcher.Invoke(() => { this.IsLoading = true; });
            var updateTaks = await JxTask.movePrioPunkteList(MainModel.projects_ids, ids, isUpward, this.currentUser.id);
            this.UpdateJxTaskCollection(updateTaks);
        }
        private void UpdateJxTaskCollection(List<JxTask> updatedCollection)
        {
            if (this.tasks.Count > 0)
            {
                for (int i = this.tasks.Count - 1; i >= 0; i--)
                {
                    tasks.RemoveAt(i);
                }

                var sortedCollection = updatedCollection.OrderBy(x => x.sortOrder).ToList();
                
                for (int i = 0; i < sortedCollection.Count; i++)
                {
                    tasks.Add(sortedCollection[i]);
                }

            }
            Application.Current.Dispatcher.Invoke(() => { this.IsLoading = false; });
        }
    }
}