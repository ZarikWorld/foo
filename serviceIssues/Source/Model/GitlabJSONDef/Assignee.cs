#nullable disable
namespace serviceIssues.Source.Model.GitlabJSONDef
{
    public class Assignee
    {
        public string state { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string web_url { get; set; }
        public string? avatar_url { get; set; }
        public string username { get; set; }
    }
}
