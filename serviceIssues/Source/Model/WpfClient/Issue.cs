namespace serviceIssues.Source.Model.WpfClient
{

    //#region Enums
    //[DataContract]
    //public enum enumTyp
    //{
    //    [EnumMember] story = 1,
    //    [EnumMember] bug = 2,
    //    [EnumMember] performance = 3,
    //    [EnumMember] luxus = 4,
    //}
    
    //[DataContract]
    //public enum enumStatus
    //{
    //    [EnumMember] opened = 1,
    //    [EnumMember] closed = 2
    //}
    //#endregion

    [DataContract]
    public class Issue
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public int iid { get; set; }

        [DataMember]
        public int project_id { get; set; }

        [DataMember]
        public bool erledigt { get; set; }

        [DataMember]
        public int? prioPunkte { get; set; }

        [DataMember]
        public string titel { get; set; }

        [DataMember]
        public bool git { get; set; }

        [DataMember]
        public string web_url { get; set; }

        [DataMember]
        public int? sortOrder { get; set; }

        [DataMember]
        public DateTime? start { get; set; }
        
        [DataMember]
        public DateTime? ende { get; set; }

        [DataMember]
        public DateTime? deadline { get; set; }

        [DataMember]
        public int creator_id { get; set; }

        [DataMember]
        public int? mitarbeiter_id { get; set; }

        [DataMember]
        public string? mitarbeiter { get; set; }

        [DataMember]
        public int? schaetzung { get; set; }

        [DataMember]
        public int? schaetzungOffiziell { get; set; }

        [DataMember]
        public DateTime created_at { get; set; }

        [DataMember]
        public int typ { get; set; }

        [DataMember]
        public int status { get; set; }

        [DataMember]
        public string? aktenzahl { get; set; }

        [DataMember]
        public string? kunde { get; set; }

        [DataMember]
        public string? anmerkung { get; set; }
    }
}