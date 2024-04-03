using Serilog;
using ServiceIssues;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace wpfIssues.Model
{
    class MainModel
    {
        #region Properties
        public static int project_id { set; get; } = 160;
        public static List<int> projects_ids { set; get; } = new List<int> { 111,          //JurXpert
                                                                             100,          //XpertOutlookSync
                                                                             13,
                                                                             61,
                                                                             108,           //EWS Bugtracker
                                                                             7,              //medixpro
                                                                             160
                                                                            };
        #endregion

        public static async Task<ObservableCollection<JxTask>?> getProjectsIssues(List<int>? projects_ids = null, int? mitarbeiter_id = null)
        {
            projects_ids ??= MainModel.projects_ids;
            mitarbeiter_id ??= Properties.Settings.Default.mitarbeiter_id;

            try
            {
                using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
                {
                    int[] projectsArray = projects_ids.ToArray();
                    Issue[] issuesArray = await serviceIssueClient.getProjectsIssuesAsync(projectsArray, mitarbeiter_id.Value);
                    ObservableCollection<JxTask> jxTasks = MainModel.convertIssueToJxTask(issuesArray);
                    var sortedTasksList = jxTasks.OrderBy(t => t.sortOrder).ToList();

                    return new ObservableCollection<JxTask>(sortedTasksList);
                };
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, ex, "Model.MainModel");
            }

            return null;
        }
        private static async void updateIssueAsync(JxTask jxTask, int? mitarbeiter_id = null)
        {
            mitarbeiter_id ??= Properties.Settings.Default.mitarbeiter_id;
            
            try
            {
                using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
                {
                    var modifiedIssue = JxTask.jxTask2ServiceIssue(jxTask);
                    await serviceIssueClient.updateIssueAsync(modifiedIssue, mitarbeiter_id.Value);
                };

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred, check log file for details", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException($"{AppDomain.CurrentDomain.BaseDirectory}{Properties.Settings.Default.logFileName}", ex, "Model.MainModel.updateIssueAsync");
            }
        }
        public static ObservableCollection<JxTask> convertIssueToJxTask(Issue[] issues)
        {
            var mitarbeiters = Model.Mitarbeiter.getMitarbeiters();
            bool issueIsUpdate;

            var tmpJxTasksList = new List<JxTask>();
            foreach (var issue in issues)
            {
                issueIsUpdate = false;
                var taskX = new JxTask();
                taskX.aktenzahl = issue.aktenzahl;
                taskX.created_at = issue.created_at;
                taskX.creator_id = issue.creator_id;
                taskX.deadline = issue.deadline;
                taskX.erledigt = issue.erledigt;
                taskX.git = issue.git;
                taskX.id = issue.id;
                taskX.TaskId = issue.mitarbeiter_id != null ? issue.mitarbeiter_id.Value : 0;
                taskX.iid = issue.iid;
                taskX.kunde = issue.kunde;
                taskX.anmerkung = issue.anmerkung;
                taskX.mitarbeiter = issue.mitarbeiter;
                if (mitarbeiters != null && issue != null && issue.mitarbeiter_id.HasValue)
                {
                    taskX.mitarbeiterNew = (from mitarbeiter in mitarbeiters
                                            where issue.mitarbeiter_id.HasValue &&
                                                  mitarbeiter.id == issue.mitarbeiter_id.Value
                                            select mitarbeiter).FirstOrDefault();
                }
                taskX.mitarbeiter_id = issue.mitarbeiter_id;
                taskX.prioPunkte = issue.prioPunkte;
                taskX.project_id = issue.project_id;
                taskX.schaetzung = issue.schaetzung;
                if (issue.schaetzung != null) { taskX.Duration = TimeSpan.FromHours(issue.schaetzung.Value); } else { taskX.Duration = TimeSpan.FromHours(0); }
                taskX.schaetzungOffiziell = issue.schaetzungOffiziell;
                taskX.sortOrder = issue.sortOrder;
                taskX.start = issue.start;
                taskX.ende = issue.ende;
                taskX.status = (enumStatus)issue.status;
                taskX.titel = issue.titel;
                taskX.TaskName = issue.titel; 
                taskX.typ = (enumTyp)issue.typ;
                taskX.web_url = issue.web_url;

                if (issueIsUpdate)
                {
                    MainModel.updateIssueAsync(taskX);
                }
                tmpJxTasksList.Add(taskX);
            }

            var sortedTasks = tmpJxTasksList.OrderBy(t => !t.schaetzung.HasValue ? 0 : 1).ToList();

            return new ObservableCollection<JxTask>(sortedTasks);
        }
        public static async Task issuesAnordnenAsync(int? sortOrder = null, int? mitarbeiter_id = null, int? project_id = null)
        {
            using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
            {
                await serviceIssueClient.issuesAnordnenAsync(sortOrder, mitarbeiter_id);
            }
        }
        public static Issue JxTask2ServiceIssue(JxTask jxTasks)
        {
            Log.Information("Enter {MethodName} method.", nameof(JxTask2ServiceIssue));
            var serviceIssue = new ServiceIssues.Issue();
            serviceIssue.id = jxTasks.id;
            serviceIssue.iid = jxTasks.iid;
            serviceIssue.project_id = jxTasks.project_id;
            serviceIssue.erledigt = jxTasks.erledigt;
            serviceIssue.prioPunkte = jxTasks.prioPunkte;
            serviceIssue.titel = jxTasks.titel;
            serviceIssue.git = jxTasks.git;
            serviceIssue.web_url = jxTasks.web_url;
            serviceIssue.sortOrder = jxTasks.sortOrder;
            serviceIssue.start = jxTasks.start;
            serviceIssue.ende = jxTasks.ende;
            serviceIssue.deadline = jxTasks.deadline;
            serviceIssue.creator_id = jxTasks.creator_id;
            serviceIssue.mitarbeiter_id = jxTasks.mitarbeiter_id;
            serviceIssue.mitarbeiter = jxTasks.mitarbeiter;
            serviceIssue.schaetzung = jxTasks.schaetzung;
            serviceIssue.schaetzungOffiziell = jxTasks.schaetzungOffiziell;
            serviceIssue.created_at = jxTasks.created_at;
            serviceIssue.typ = (int)jxTasks.typ;
            serviceIssue.status = (int)jxTasks.status;
            serviceIssue.aktenzahl = jxTasks.aktenzahl;
            serviceIssue.kunde = jxTasks.kunde;
            serviceIssue.anmerkung = jxTasks.anmerkung;
            Log.Information("Exit {MethodName} method.", nameof(JxTask2ServiceIssue));
            return serviceIssue;
        }
        public static JxTask serviceIssue2JxTask(ServiceIssues.Issue xxx)
        {
            Log.Information("Enter {MethodName} method.", nameof(serviceIssue2JxTask));
            JxTask tmpJxTask = new JxTask();
            tmpJxTask.id = xxx.id;
            tmpJxTask.iid = xxx.iid;
            tmpJxTask.project_id = xxx.project_id;
            tmpJxTask.erledigt = xxx.erledigt;
            tmpJxTask.prioPunkte = xxx.prioPunkte;
            tmpJxTask.titel = xxx.titel;
            tmpJxTask.git = xxx.git;
            tmpJxTask.web_url = xxx.web_url;
            tmpJxTask.sortOrder = xxx.sortOrder;
            tmpJxTask.start = xxx.start;
            tmpJxTask.ende = xxx.ende;
            tmpJxTask.deadline = xxx.deadline;
            tmpJxTask.creator_id = xxx.creator_id;
            tmpJxTask.mitarbeiter_id = xxx.mitarbeiter_id;
            tmpJxTask.mitarbeiter = xxx.mitarbeiter;
            tmpJxTask.schaetzung = xxx.schaetzung;
            tmpJxTask.schaetzungOffiziell = xxx.schaetzungOffiziell;
            tmpJxTask.created_at = xxx.created_at;
            tmpJxTask.typ = (enumTyp)xxx.typ;
            tmpJxTask.status = (enumStatus)xxx.status;
            tmpJxTask.aktenzahl = xxx.aktenzahl;
            tmpJxTask.kunde = xxx.kunde;
            tmpJxTask.anmerkung = xxx.anmerkung;
            Log.Information("Exit {MethodName} method.", nameof(serviceIssue2JxTask));
            return tmpJxTask;
        }
    }
}
