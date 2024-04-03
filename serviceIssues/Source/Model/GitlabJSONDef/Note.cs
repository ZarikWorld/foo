using Newtonsoft.Json;

namespace serviceIssues.Source.Model.GitlabJSONDef
{
    public class Note
    {
        public int id { get; set; }
        public string body { get; set; }
        public string attachment { get; set; }
        public Author author { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public bool system { get; set; }
        public int noteable_id { get; set; }
        public string noteable_type { get; set; }
        public int project_id { get; set; }
        public int noteable_iid { get; set; }
        public bool resolvable { get; set; }
        public bool confidential { get; set; }

        [JsonProperty("internal")]
        public bool isInternal { get; set; }
    }
}
