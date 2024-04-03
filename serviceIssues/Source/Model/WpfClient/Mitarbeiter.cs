#nullable disable

namespace serviceIssues.Source.Model.WpfClient
{
    [DataContract]
    public class Mitarbeiter
    {
        [DataMember]
        public string? avatar_url { get; set; }
        
        [DataMember]
        public int gitlab_id { get; set; }
        
        [DataMember]
        public string gitUsername { get; set; }
        
        [DataMember]
        public string gitToken { get; set; }

        [DataMember]
        public int id { get; set; }
        
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int mitarbeiterRole { get; set; }

        [DataMember]
        public int team_id { get; set; }

        [DataMember]
        public int montag { get; set; }

        [DataMember]
        public int dienstag { get; set; }

        [DataMember]
        public int mittwoch { get; set; }

        [DataMember]
        public int donnerstag { get; set; }

        [DataMember]
        public int freitag { get; set; }

        public Dictionary<DayOfWeek, int> workDays
        {
            get
            {
                var result = new Dictionary<DayOfWeek, int>();
                result.Add(DayOfWeek.Monday, montag );
                result.Add(DayOfWeek.Tuesday, dienstag );
                result.Add(DayOfWeek.Wednesday, mittwoch);
                result.Add(DayOfWeek.Thursday, donnerstag);
                result.Add(DayOfWeek.Friday, freitag);
                result.Add(DayOfWeek.Saturday, 0);
                result.Add(DayOfWeek.Sunday, 0);

                return result;
            }
        }
    }
}