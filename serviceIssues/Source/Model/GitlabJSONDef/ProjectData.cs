﻿using Newtonsoft.Json;

namespace serviceIssues.Source.Model.GitlabJSONDef
{
    public class ProjectData
    {
        public int id { get; set; }
        public string description { get; set; }
        public string description_html { get; set; }
        public string name { get; set; }
        public string name_with_namespace { get; set; }
        public string path { get; set; }
        public string path_with_namespace { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string default_branch { get; set; }
        public List<string> tag_list { get; set; } // Deprecated, use Topics instead
        public List<string> topics { get; set; }
        public string ssh_url_to_repo { get; set; }
        public string http_url_to_repo { get; set; }
        public string web_url { get; set; }
        public string readme_url { get; set; }
        public string avatar_url { get; set; }
        public int forks_count { get; set; }
        public int star_count { get; set; }
        public DateTime last_activity_at { get; set; }

        [JsonProperty("namespace")]
        public Namespace namespace_ { get; set; }
        public string container_registry_image_prefix { get; set; }
        public Links _link { get; set; }
        public bool? packages_enabled { get; set; }
        public bool empty_repo { get; set; }
        public bool archived { get; set; }
        public string visibility { get; set; }
        public bool? resolve_outdated_diff_discussions { get; set; }
        public ContainerExpirationPolicy container_expiration_policy { get; set; }
        public bool issues_enabled { get; set; }
        public bool merge_requests_enabled { get; set; }
        public bool wiki_enabled { get; set; }
        public bool jobs_enabled { get; set; }
        public bool snippets_enabled { get; set; }
        public bool container_registry_enabled { get; set; }
        public bool service_desk_enabled { get; set; }
        public bool can_create_merge_request_in { get; set; }
        public string issues_access_level { get; set; }
        public string repository_access_level { get; set; }
        public string merge_requests_access_level { get; set; }
        public string forking_access_level { get; set; }
        public string wiki_access_level { get; set; }
        public string builds_access_level { get; set; }
        public string snippets_access_level { get; set; }
        public string pages_access_level { get; set; }
        public string analytics_access_level { get; set; }
        public string container_registry_access_level { get; set; }
        public string security_and_compliance_access_level { get; set; }
        public object emails_disabled { get; set; }
        public bool shared_runners_enabled { get; set; }
        public bool group_runners_enabled { get; set; }
        public bool lfs_enabled { get; set; }
        public int creator_id { get; set; }
        public object import_url { get; set; }
        public object import_type { get; set; }
        public string import_status { get; set; }
        public object import_error { get; set; }
        public int open_issues_count { get; set; }
        public int ci_default_git_depth { get; set; }
        public bool ci_forward_deployment_enabled { get; set; }
        public bool ci_allow_fork_pipelines_to_run_in_parent_project { get; set; }
        public bool ci_job_token_scope_enabled { get; set; }
        public bool ci_separated_caches { get; set; }
        public bool public_jobs { get; set; }
        public int build_timeout { get; set; }
        public string auto_cancel_pending_pipelines { get; set; }
        public string ci_config_path { get; set; }
        public List<object> shared_with_groups { get; set; }
        public string repository_storage { get; set; }
        public bool only_allow_merge_if_pipeline_succeeds { get; set; }
        public object allow_merge_on_skipped_pipeline { get; set; }
        public bool restrict_user_defined_variables { get; set; }
        public bool request_access_enabled { get; set; }
        public bool? only_allow_merge_if_all_discussions_are_resolved { get; set; }
        public bool? remove_source_branch_after_merge { get; set; }
        public bool printing_merge_request_link_enabled { get; set; }
        public string merge_method { get; set; }
        public string squash_option { get; set; }
        public bool enforce_auth_checks_on_uploads { get; set; }
        public object suggestion_commit_message { get; set; }
        public object merge_commit_template { get; set; }
        public object squash_commit_template { get; set; }
        public string issue_branch_template { get; set; }
        public bool auto_devops_enabled { get; set; }
        public string auto_devops_deploy_strategy { get; set; }
        public bool autoclose_referenced_issues { get; set; }
        public bool keep_latest_artifact { get; set; }
        public object runner_token_expiration_interval { get; set; }
        public string external_authorization_classification_label { get; set; }
        public bool requirements_enabled { get; set; }
        public string requirements_access_level { get; set; }
        public bool security_and_compliance_enabled { get; set; }
        public List<string> compliance_frameworks { get; set; }
        public Permissions permissions { get; set; }

        //public List<Issue> issues { get { return Data.GitLab.ProjectsData. } }

    }
}
