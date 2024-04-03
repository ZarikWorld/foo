namespace serviceIssues.Source.Model.GitlabJSONDef
{
    public class Milestone
    {
        public int project_id { get; set; }
        public string description { get; set; }
        public string state { get; set; }
        public object due_date { get; set; }
        public int iid { get; set; }
        public DateTime created_at { get; set; }
        public string title { get; set; }
        public int id { get; set; }
        public DateTime updated_at { get; set; }
    }
}
