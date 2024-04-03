#nullable disable
namespace serviceIssues.Source.Model.GitlabJSONDef
{
    public enum EnumIssueType
    {
        issue,
        incident,
        testcase
    }
    public enum EnumIssueState
    {
        all,
        opened,
        closed
    }

    public class GitIssue
    {
        public string state { get; set; }
        public string description { get; set; }
        public Author author { get; set; }
        public Milestone milestone { get; set; }
        public int project_id { get; set; }
        public List<Assignee> assignees { get; set; }
        [Obsolete("The assignee property is deprecated. Use assignees instead.")]
        public Assignee assignee { get; set; }
        public string type { get; set; }
        public DateTime updated_at { get; set; }
        public object closed_at { get; set; }
        public object closed_by { get; set; }
        public int id { get; set; }
        public int iid { get; set; }
        public string title { get; set; }
        public DateTime created_at { get; set; }
        public object moved_to_id { get; set; }
        public List<string> labels { get; set; }
        public int upvotes { get; set; }
        public int downvotes { get; set; }
        public int merge_requests_count { get; set; }
        public int user_notes_count { get; set; }
        public string due_date { get; set; }
        public string web_url { get; set; }
        public References references { get; set; }
        public TimeStats time_stats { get; set; }
        public bool has_tasks { get; set; }
        public string task_status { get; set; }
        public bool confidential { get; set; }
        public bool? discussion_locked { get; set; }
        public string issue_type { get; set; }
        public string severity { get; set; }
        public Dictionary<string, string> _links { get; set; }
        public TaskCompletionStatus task_completion_status { get; set; }
        public List<Note> notes { get; set; }
    }
}
