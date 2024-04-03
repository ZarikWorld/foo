using System.Collections.Generic;
using System;
using System.Windows;

namespace wpfIssues.Model
{
    public enum enumRole
    {
        user = 0,
        admin = 1,
    }
    public enum enumTeam
    {
        other = 0,
        programmierung = 1,
        technik = 2,
        support = 3,
    }

    public class Mitarbeiter
    {
        public string? avatar_url { get; set; }
        public int gitlab_id { get; set; }
        public string gitUsername { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public enumRole role { get; set; }
        public enumTeam team { get; set; }
        public int montag { get; set; }
        public int dienstag { get; set; }
        public int mittwoch { get; set; }
        public int donnerstag { get; set; }
        public int freitag { get; set; }
        public Dictionary<DayOfWeek, int> workDays
        {
            get
            {
                var result = new Dictionary<DayOfWeek, int>();
                result.Add(DayOfWeek.Monday, montag);
                result.Add(DayOfWeek.Tuesday, dienstag);
                result.Add(DayOfWeek.Wednesday, mittwoch);
                result.Add(DayOfWeek.Thursday, donnerstag);
                result.Add(DayOfWeek.Friday, freitag);
                return result;
            }
        }

        static void service2wpf(ServiceIssues.Mitarbeiter xxx, Mitarbeiter yyy)
        {
            yyy.avatar_url = xxx.avatar_url;
            yyy.gitlab_id = xxx.gitlab_id;
            yyy.gitUsername = xxx.gitUsername;
            yyy.id = xxx.id;
            yyy.name = xxx.name;
            yyy.role = (enumRole)xxx.mitarbeiterRole;
            yyy.team = (enumTeam)xxx.team_id;
            yyy.montag = xxx.montag;
            yyy.dienstag = xxx.dienstag;
            yyy.mittwoch = xxx.mittwoch;
            yyy.donnerstag = xxx.donnerstag;
            yyy.freitag = xxx.freitag;
        }
        public static List<Mitarbeiter>? getMitarbeiters()
        {
            try
            {
                using (var serviceIssueClient = ConnectedServices.Helper.getServiceIssueClient())
                {
                    ServiceIssues.Mitarbeiter[] service_mitarbeiters = serviceIssueClient.getMitarbeitersAsync().Result;

                    var result = new List<Mitarbeiter>();
                    foreach (var service_mitarbeiter in service_mitarbeiters)
                    {
                        var wpf_mitarbeiter = new Mitarbeiter();
                        service2wpf(service_mitarbeiter, wpf_mitarbeiter);
                        result.Add(wpf_mitarbeiter);
                    }
                    
                    return result;
                };
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, ex, "Model.Mitarbeiter");
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                return null;
            }
        }
    }
}
