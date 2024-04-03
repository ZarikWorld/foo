namespace serviceIssues.Source.Model.GitlabJSONDef
{
    public class TimeStats
    {
        public int time_estimate { get; set; }
        public int total_time_spent { get; set; }
        public object human_time_estimate { get; set; }
        public object human_total_time_spent { get; set; }
    }
}
