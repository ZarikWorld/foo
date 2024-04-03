using CsvHelper;
using Newtonsoft.Json;
using RestSharp;
using serviceIssues.Source.Model.GitlabJSONDef;
using serviceIssues.Source.Model.GitlabJSONDef.Enums;
using serviceIssues.Source.Model.WpfClient;
using System.Globalization;
using System.Text.RegularExpressions;

namespace serviceIssues.Source.Data.GitData
{
    public class IssueData
    {
        //TEMP
        public static List<Note> Notes = new List<Note>();
        private static string _appImageFolder = @"C:\img";
        private static string _dataPath = @"C:\export";
        private static string _gitlabBaseWeb = @"https://gitlab.medix.at:7443";
        private static string _logFileName = @"serviceIssues.log";
        //TEMP
        public static List<string> emailAddresses = new List<string>();
        public static void AddEmailAddress(string emailAddress)
        {
            // Check if the email address is already present in the List.
            // If it is not present, add it.
            if (!emailAddresses.Any(e => e.Equals(emailAddress, StringComparison.OrdinalIgnoreCase)))
            {
                emailAddresses.Add(emailAddress);
            }
        }
        public static string ReplaceImageLinks(string input)
        {
            string pattern = @"!\[image]\(.*?\)";
            string filePattern = @"(?<=\/)[^\/]*?(?=\))";

            int matchNumber = 1;

            MatchCollection matches = Regex.Matches(input, pattern);
            foreach (Match match in matches)
            {
                // Extract filename from match using filePattern
                string filename = Regex.Match(match.Value, filePattern).Value;
                string newFilename = $"{matchNumber}_{filename}";
                matchNumber++;
                input = input.Replace(match.Value, "{color:blue}[" + newFilename + "]{color}");

                //TODO: To Convert the Attach Images to Inline Images
                //input = input.Replace(match.Value, $"!{newFilename} | width = 842,height = 888!"); //!difference.png | width = 842,height = 888!
            }

            return input;
        }
        public static string DescriptionCleaner(string gitlabIssueDescription)
        {
            //Remove Gitlab Header Markdowns
            gitlabIssueDescription = gitlabIssueDescription.Replace("###", "").Replace("####", "").Replace("#####", "");

            // Replace newline characters with double backslashes
            gitlabIssueDescription = gitlabIssueDescription.Replace("\n", @" \\");

            return ReplaceImageLinks(gitlabIssueDescription);
        }
        // Returns all the issue and also the maximum number of notes for a single issue (for generating csv with dynamic column numbers)
        public static async Task<(List<GitIssue>, int)> getProjectIssuesAsync(int project_id, EnumIssueState issueState)
        {
            RestRequest request = GitData.Clients.createApiRequest($"projects/{project_id}/issues", Method.Get);
            request.AddParameter("state", issueState.ToString());

            int x_page = 1;
            int x_total_pages = 1;
            var issues = new List<GitIssue>();

            int maxNoteCount = 0;
            while (x_page <= x_total_pages)
            {
                request.AddOrUpdateParameter("page", x_page);

                var response = await GitData.Clients.apiClient.ExecuteGetAsync(request);

                if (x_page == 1) x_total_pages = totalPages((RestResponse)response);

                List<GitIssue> currentPageIssues = JsonConvert.DeserializeObject<List<GitIssue>>(response.Content);

                foreach (GitIssue issue in currentPageIssues)
                {
                    issue.notes = await getAllNotes(issue.project_id, issue.iid, sort: EnumSort.asc);
                    issue.description += $"\n\n[Gitlab Original Issue|{issue.web_url}]\n";
                    if (issue.notes.Count > maxNoteCount)
                    {
                        maxNoteCount = issue.notes.Count;
                    }
                }

                issues.AddRange(currentPageIssues);

                x_page++;
            }

            return (issues, maxNoteCount);
        }
        public static async Task<(List<GitIssue>, int)> getProjectIssuesByPageAsync(int project_id, int page, EnumIssueState issueState)
        {
            RestRequest request = GitData.Clients.createApiRequest($"projects/{project_id}/issues", Method.Get);
            request.AddParameter("state", issueState.ToString());
            request.AddOrUpdateParameter("page", page);

            var response = await GitData.Clients.apiClient.ExecuteGetAsync(request);

            List<GitIssue> currentPageIssues = JsonConvert.DeserializeObject<List<GitIssue>>(response.Content);
            int maxNoteCount = 0;

            foreach (GitIssue issue in currentPageIssues)
            {
                issue.notes = await getAllNotes(issue.project_id, issue.iid, sort: EnumSort.asc);
                issue.description += $"\n\n[Gitlab Original Issue|{issue.web_url}]\n";
                if (issue.notes.Count > maxNoteCount)
                {
                    maxNoteCount = issue.notes.Count;
                }
            }

            return (currentPageIssues, maxNoteCount);
        }
        public static async Task<List<GitIssue>> getIssuesFromDate(int project_id, DateTime fromDateTime,
                                                                   EnumIssueState state = EnumIssueState.opened,
                                                                   EnumOrder orderBy = EnumOrder.created_at,
                                                                   EnumSort sort = EnumSort.asc)
        {
            RestRequest request = Clients.createApiRequest($"projects/{project_id}/issues", Method.Get);

            //convert to Gitlab GMT time
            string iso8601DateTime = fromDateTime.AddHours(-2).ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);     //ISO 8601 (2019-03-15T08:00:00Z)
            request.AddQueryParameter("updated_after", iso8601DateTime);
            request.AddQueryParameter("state", state.ToString());   //TODO: Remove the Opened parameter so can remove the closed issues fromDB
            request.AddQueryParameter("order_by", orderBy.ToString());
            request.AddQueryParameter("sort", sort.ToString());

            int x_page = 1;
            int x_total_pages = 1;
            var issues = new List<GitIssue>();

            while (x_page <= x_total_pages)
            {
                request.AddOrUpdateParameter("page", x_page);
                var response = await Clients.apiClient.ExecuteGetAsync(request);

                if (x_page == 1) x_total_pages = totalPages((RestResponse)response);

                if (response.Content is null)
                    continue;

                List<GitIssue>? currentPageIssues = JsonConvert.DeserializeObject<List<GitIssue>>(response.Content);

                if (currentPageIssues is not null)
                {
                    issues.AddRange(currentPageIssues);
                }

                x_page++;
            }

            return issues;
        }
        public static async Task<List<GitIssue>?> getIssuesAsync(int project_id,
                                                                 string gitlabPrivateToken,
                                                                 DateTime fromDateTime,
                                                                 EnumIssueState state,
                                                                 EnumOrder orderBy = EnumOrder.created_at,
                                                                 EnumSort sort = EnumSort.asc)
        {
            RestRequest request = Clients.createApiRequest($"projects/{project_id}/issues", gitlabPrivateToken, Method.Get);

            //TODO: Localization : convert to Gitlab GMT time
            string iso8601DateTime = fromDateTime.AddHours(-2).ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);     //ISO 8601 (2019-03-15T08:00:00Z)
            request.AddQueryParameter("updated_after", iso8601DateTime);
            request.AddQueryParameter("state", state.ToString());
            request.AddQueryParameter("order_by", orderBy.ToString());
            request.AddQueryParameter("sort", sort.ToString());

            int x_page = 1;
            int x_total_pages = 1;
            var issues = new List<GitIssue>();

            while (x_page <= x_total_pages)
            {
                request.AddOrUpdateParameter("page", x_page);
                var response = await Clients.apiClient.ExecuteGetAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new HttpRequestException(response.ErrorException?.Message);
                }

                if (x_page == 1) x_total_pages = totalPages((RestResponse)response);

                if (response.Content is null)
                    continue;

                List<GitIssue>? currentPageIssues = JsonConvert.DeserializeObject<List<GitIssue>>(response.Content);

                if (currentPageIssues is not null)
                {
                    issues.AddRange(currentPageIssues);
                }

                x_page++;
            }

            return issues;
        }
        public static async Task<GitIssue> GetIssueAsync(int projectId, int issueIid)
        {
            RestRequest request = GitData.Clients.createApiRequest($"projects/{projectId}/issues/{issueIid}", Method.Get);

            var response = await GitData.Clients.apiClient.ExecuteGetAsync(request);
            var issue = JsonConvert.DeserializeObject<GitIssue>(response.Content);

            // Retrieve notes for the issue
            issue.notes = await getAllNotes(issue.project_id, issue.iid, sort: EnumSort.asc);

            // Append issue URL to the description
            issue.description += $"\n\n[Gitlab Original Issue|{issue.web_url}]\n";

            return issue;
        }
        public static void ConvertGitLabIssuesToCSV(List<GitIssue> gitlabIssues, int maxNoteCount, string path)
        {

            int maxLabelCount = gitlabIssues.Max(issue => issue.labels.Count);
            int maxTimeEstimate = gitlabIssues.Max(issue => issue.time_stats.time_estimate);
            int minTimeEstimate = gitlabIssues.Min(issue => issue.time_stats.time_estimate);


            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                //Headers
                csv.WriteField("GitlabIssueId");
                csv.WriteField("Date_Created");
                csv.WriteField("Due_Date");
                csv.WriteField("Assignee");

                csv.WriteField("Priority");
                //csv.WriteField("EstimateTime");
                //csv.WriteField("HumanTimeEstimate");

                csv.WriteField("Reporter");
                csv.WriteField("Issue_Type");
                csv.WriteField("Summary");
                csv.WriteField("Description");

                for (int i = 1; i <= maxLabelCount; i++)
                {
                    //TODO : Define Issue taype based on the Labels

                    csv.WriteField($"Labels1{i}");
                }
                for (int i = 1; i <= maxNoteCount; i++)
                {
                    csv.WriteField($"Comment{i}");
                }

                csv.NextRecord();

                //Rows
                foreach (GitIssue issue in gitlabIssues)
                {
                    csv.WriteField(issue.iid.ToString());
                    csv.WriteField(issue.created_at.ToString("MM.dd.yyyy HH:mm:ss"));  //07.19.2023 20:51:48
                    csv.WriteField(issue.due_date);
                    //csv.WriteField(Helpers.MapGitlabUserToEmailAddress(issue.assignee != null ? issue.assignee.username : "" ));
                    if (issue.assignee != null)
                    {
                        if (issue.assignee.username != null)
                        {
                            csv.WriteField(MapGitlabUserToEmailAddress(issue.assignee.username));
                        }
                        else
                        {
                            csv.WriteField("");
                        }
                    }
                    else
                    {
                        csv.WriteField("");
                    }

                    csv.WriteField(TimeEstimateToPriority(issue.time_stats.time_estimate, minTimeEstimate, maxTimeEstimate));
                    //TODO: Use Mapping function to convert all usernames to the (x-bs.at) domain
                    csv.WriteField(MapGitlabUserToEmailAddress(issue.author.username));

                    if (issue.labels.Contains("bug", StringComparer.OrdinalIgnoreCase))
                    {
                        issue.issue_type = "Bug";
                    }
                    else if (issue.labels.Contains("neues feature", StringComparer.OrdinalIgnoreCase))
                    {
                        issue.issue_type = "Story";
                    }
                    else
                    {
                        issue.issue_type = "Task";
                    }

                    csv.WriteField(issue.issue_type);

                    csv.WriteField(issue.title);
                    csv.WriteField(DescriptionCleaner(issue.description));

                    for (int i = 0; i < maxLabelCount; i++)
                    {
                        if (i < issue.labels.Count)
                        {
                            csv.WriteField(issue.labels[i]);
                        }
                        else
                        {
                            csv.WriteField("");
                        }
                    }

                    for (int i = 0; i < maxNoteCount; i++)
                    {
                        if (i < issue.notes.Count)
                        {
                            Note noteX = issue.notes[i];
                            csv.WriteField($"{noteX.created_at.ToString("MM.dd.yyyy HH:mm:ss")};{MapGitlabUserToEmailAddress(noteX.author.username)};{noteX.body}");
                        }
                        else
                        {
                            csv.WriteField("");
                        }
                    }

                    csv.NextRecord();
                }
            }
        }
        private static int totalPages(RestResponse response)
        {
            var header = response.Headers.ToList().Find(x => x.Name == "X-Total-Pages");

            if (header != null && header.Value != null)
            {
                if (int.TryParse(header.Value.ToString(), out int totalPages))
                {
                    return totalPages;
                }
            }

            // Return a default value if the header was not found or couldn't be parsed
            return 0;
        }
        public static string ReplaceUsernamesWithEmailsInNoteBody(string noteBody)
        {
            string pattern = @"@(\w+)";
            var matches = Regex.Matches(noteBody, pattern);

            string newBody = noteBody;

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string username = match.Groups[1].Value;
                    string email = MapGitlabUserToEmailAddress(username);

                    //string jiraAccountId = Data.Jira.UserData.getUserAccountId(email);

                    //if (jiraAccountId == string.Empty)
                    //{
                    //    newBody = newBody.Replace("@" + username, email);
                    //}
                    //else
                    //{
                    //    string userTagString = $"[~accountid:{jiraAccountId}]";   //[~accountid:712020:577cdfd3-1064-41ce-9607-de9a4da55666]
                    //    newBody = newBody.Replace("@" + username, userTagString);
                    //}
                }
            }
            return newBody;
        }
        public static string ReplaceDynamicImgLinkWithStaticInNoteBody(string noteBody)
        {

            string ReplaceLink(Match m)
            {
                //TODO: Make Projectname Dynamic
                string projectName = "jurxpert";

                string url = $"https://gitlab.medix.at:7443/acp/{projectName}" + m.Groups[1].Value;
                return $"\\\\ \\\\[Attached Image|{url}]\\\\ \\\\";
            }

            string pattern = @"!\[image\]\((.*?)\)";
            string newBody = Regex.Replace(noteBody, pattern, new MatchEvaluator(ReplaceLink));

            return newBody;
        }
        public static async Task<List<Note>> getAllNotes(int projectId,
                                                     int issue_iid,
                                                     EnumSort sort = EnumSort.desc,
                                                     EnumOrder order = EnumOrder.created_at)
        {
            var request = GitData.Clients.createApiRequest($"/projects/{projectId}/issues/{issue_iid}/notes", Method.Get);

            request.AddParameter("sort", sort.ToString());
            request.AddParameter("order_by", order.ToString());

            int x_page = 1;
            int x_total_pages = 1;

            var notes = new List<Note>();
            while (x_page <= x_total_pages)
            {
                request.AddOrUpdateParameter("page", x_page);

                var response = await GitData.Clients.apiClient.ExecuteGetAsync(request);

                if (x_page == 1) x_total_pages = totalPages((RestResponse)response);

                notes.AddRange(JsonConvert.DeserializeObject<List<Note>>(response.Content));

                x_page++;
            }

            foreach (Note note in notes)
            {
                note.body = ReplaceUsernamesWithEmailsInNoteBody(note.body);
                note.body = ReplaceDynamicImgLinkWithStaticInNoteBody(note.body);
            }

            Notes.AddRange(notes);

            return notes;
        }
        public static async Task DownloadImages(GitIssue issue, string? dataPath = null)
        {
            bool success = false;
            int count = 0;
            while (!success)
            {
                try
                {
                    string pattern = @"!\[image\]\((\/uploads\/.*?\/(.+?))\)";
                    if (issue == null || issue.description == null) return;

                    Regex regex = new Regex(pattern);
                    MatchCollection matchCollection = regex.Matches(issue.description);

                    string imgFolder = string.Empty;
                    if (dataPath != null)
                    {
                        imgFolder = $"{dataPath}\\{_appImageFolder}";
                    }
                    else
                    {
                        imgFolder = $"{_dataPath}\\{_appImageFolder}";
                    }


                    int matchNumber = 1;
                    foreach (Match match in matchCollection)
                    {
                        GroupCollection groups = match.Groups;
                        string fullPath = groups[1].Value; // Full path
                        string fileName = groups[2].Value; // File name with extension

                        // mostly the attached images in the issue having same names like image.png. here we just add 
                        // a ordinal number so we escape overwriting the files when downloading them. 
                        string newFileName = $"{matchNumber}_{fileName}";

                        var fileResponse = await GitData.Clients.httpClient.GetStreamAsync($"{_gitlabBaseWeb}/acp/jurxpert{fullPath}");

                        if (!Directory.Exists($"{imgFolder}\\{issue.iid}"))
                        {
                            Directory.CreateDirectory($"{imgFolder}\\{issue.iid}");
                        }

                        using (var fileStream = File.Create(Path.Combine($"{imgFolder}\\{issue.iid}\\", newFileName)))
                        {
                            await fileResponse.CopyToAsync(fileStream);
                        }
                        matchNumber++;
                    }

                    success = true;
                }
                catch (Exception ex)
                {
                    count++;

                    if (count == 10)
                    {
                        success = true;
                    }

                    libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException($"{AppDomain.CurrentDomain.BaseDirectory}{_logFileName}", ex);

                    await Task.Delay(3000);
                }
            }
        }
        public static string TimeEstimateToPriority(int estimate_time, int min_estimate, int max_estimate)
        {
            if (estimate_time == 0)
            {
                return "";
            }

            int rang = max_estimate - min_estimate;
            int part = rang / 5;

            string priority = string.Empty;

            if (estimate_time <= min_estimate + part)
            {
                priority = "Lowest";
            }
            else if (estimate_time <= min_estimate + 2 * part)
            {
                priority = "Low";
            }
            else if (estimate_time <= min_estimate + 3 * part)
            {
                priority = "Medium";
            }
            else if (estimate_time <= min_estimate + 4 * part)
            {
                priority = "High";
            }
            else
            {
                priority = "Highest";
            }

            return priority;
        }
        public static string MapGitlabUserToEmailAddress(string gitlabUsername)
        {
            Dictionary<string, string> userToEmailMap = new Dictionary<string, string>()
            {
                {"adrian","Adrian.Schauer@x-bs.at"},
                {"alexander","alexander.cech@alex-c.at"},
                {"alexanderp","alexander.pollek@x-bs.at"},
                {"alexandra","alexandra.falk - schmidtberger@x - bs.at"},
                {"alexandram","alexandra.mauritz@x-bs.at"},
                {"alexandrina","alexandrina.zwetzich@x-bs.at"},
                {"andreasb","andreas.bader@x-bs.at"},
                {"andreash","andreas.hauk@x-bs.at"},
                {"annamaria","anna.rupp@x-bs.at"},
                {"bernhard","bernhard.laireiter@x-bs.at"},
                {"bettina","bettina.steyrer@x-bs.at"},
                {"bodomar","bodomar.grasl@x-bs.at"},
                {"cedrick","cedrick.hodari@x-bs.at"},
                {"christianw","christian.weinmuellner@x-bs.at"},
                {"christine","christine.smol@x-bs.at"},
                {"clemens","clemens.horak@x-bs.at"},
                {"daniela","daniela.hutterer@x-bs.at"},
                {"dominika","dominika.hoffmann@x-bs.at"},
                {"elisabeth","elisabeth.skrianz@x-bs.at"},
                {"eva","eva.gelb@x-bs.at"},
                {"gerhard","gerhard.novak@x-bs.at"},
                {"guram","guram.matschawariani@x-bs.at"},
                {"hamidreza","Hamidreza.Rahimi@x-bs.at"},
                {"hartmut","Hartmut.Beck@x-bs.at"},
                {"hasan","hasan.mutlu@x-bs.at"},
                {"herbert","herbert.samek@x-bs.at"},
                {"hueseyin","hueseyin.emre.akdin@x-bs.at"},
                {"iwona","iwona.kowalska@x-bs.at"},
                {"jan","jan.mieth@x-bs.at"},
                {"jeannie","jeannie.schranz@x-bs.at"},
                {"jie","jie.shen@x-bs.at"},
                {"julia","julia.kroneiser@x-bs.at"},
                {"karl","Karl.Beranek@x-bs.at"},
                {"katharina","katharina.hafenbradl@x-bs.at"},
                {"konstantin","konstantin.stany@x-bs.at"},
                {"laurin","laurin.mahr@x-bs.at"},
                {"lindah","linda.hoxhaj@x-bs.at"},
                {"lukas","lukas.gaissler@x-bs.at"},
                {"magdalena","dshfbniausbdhfu@imd.at"},
                {"manuela","manuela.ros@x-bs.at"},
                {"marcmichael","marc-michael.haupt@acp.at"},
                {"margot","Margot.Komurka@x-bs.at"},
                {"maria","maria.bakhit@x-bs.at"},
                {"markus","markus.meca@acp.at"},
                {"markusd","markus.drechsler@x-bs.at"},
                {"markuss","markus.schoos@interact.at"},
                {"martin","martin.aulehla@acp.at"},
                {"martinb","martin.buzek@x-bs.at"},
                {"martinr","Martin.Runggaldier@x-bs.at"},
                {"martins","martin.stransky@interact.at"},
                {"maximilian","Maximilian.Fischer@x-bs.at"},
                {"meryem","meryem.arslan@acp.at"},
                {"michael","michael.ladinig@x-bs.at"},
                {"michaela","michaela.begic@x-bs.at"},
                {"michaelf","Michael.Figl@x-bs.at"},
                {"michaelm","michael.meixner@x-bs.at"},
                {"milica","milica.ratkovic@x-bs.at"},
                {"niklas","niklas.ullsperger@x-bs.at"},
                {"nikola","nikola.vasilev@x-bs.at"},
                {"noa","Noa.Perkovic@x-bs.at"},
                {"office","office@medix.at"},
                {"pascal","pascal.wendl@x-bs.at"},
                {"patricia","patricia.pilz@x-bs.at"},
                {"patrick","patrick.vantulder@x-bs.at"},
                {"philipp","philipp.huber@x-bs.at"},
                {"robin","robin.dostal@medix.at"},
                {"sabrina","sabrina.kainz@apc.at"},
                {"sandra","sandra.orban@x-bs.at"},
                {"sanja","sanja.senader@medix.at"},
                {"sebastian ","Sebastian.Bruckner - Hrubesch@x - bs.at"},
                {"semir","semir.rahic@x-bs.at"},
                {"sonja","sonja.stampfl@acp.at"},
                {"sophie","sophie.rod@x-bs.at"},
                {"stefan","stefan.skrobic@x-bs.at"},
                {"thomas","thomas.baumgartner@x-bs.at"},
                {"thomash","thomas.hackl@x-bs.at"},
                {"thomasm","thomas.meixner@x-bs.at"},
                {"vladan","vladan.katanic@x-bs.at"},
                {"zoran","zoran.puskar@x-bs.at"},
                {"florian","florian.mark@x-bs.at"},
            };

            if (userToEmailMap.TryGetValue(gitlabUsername, out string? email))
            {
                AddEmailAddress(email);
                return email;

            }
            else
            {
                return gitlabUsername;
            }
        }
        public static void updateIssueassignee(Issue issue_, int newAssignee_gitlab_id)
        {
            RestRequest request = GitData.Clients.createApiRequest($"projects/{issue_.project_id}/issues/{issue_.iid}", Method.Put);
            request.AddParameter("assignee_ids", newAssignee_gitlab_id);
            var response = GitData.Clients.apiClient.ExecutePutAsync(request).Result;
            GitIssue? issueX = JsonConvert.DeserializeObject<GitIssue>(response.Content);
        }
        public static void updateIssueassignee(Issue issue_, int newAssignee_gitlab_id, string gitlabPrivateToken)
        {
            RestRequest request = GitData.Clients.createApiRequest($"projects/{issue_.project_id}/issues/{issue_.iid}", gitlabPrivateToken, Method.Put);
            request.AddParameter("assignee_ids", newAssignee_gitlab_id);
            var response = GitData.Clients.apiClient.ExecutePutAsync(request).Result;
            GitIssue? issueX = JsonConvert.DeserializeObject<GitIssue>(response.Content);
        }

        public static void updateIssue(int issue_iid, int project_id,
                                                     string gitlabPrivateToken,
                                                     int? newAssignee_gitlab_id = null,
                                                     DateTime? deadLine = null,
                                                     List<string>? newLabels = null,
                                                     List<string>? removeLables = null)
        {
            RestRequest request = GitData.Clients.createApiRequest($"projects/{project_id}/issues/{issue_iid}", gitlabPrivateToken, Method.Put);
            if (newAssignee_gitlab_id != null)
            {
                request.AddParameter("assignee_ids", newAssignee_gitlab_id.Value);
            }

            if (deadLine == null)
            {
                request.AddParameter("due_date", "");
            }
            else
            {
                request.AddParameter("due_date", deadLine.Value);
            }


            string removeLablesString = string.Empty;
            if (removeLables != null)
            {
                foreach (string label in removeLables)
                {
                    removeLablesString += label + ",";
                }
            }

            if (!string.IsNullOrEmpty(removeLablesString))
            {
                request.AddParameter("remove_labels", removeLablesString);
            }

            string newLablesString = string.Empty;
            if (newLabels != null)
            {
                foreach (string label in newLabels)
                {
                    newLablesString += label + ",";
                }
            }

            if (!string.IsNullOrEmpty(newLablesString))
            {
                request.AddParameter("add_labels", newLablesString);
            }

            var response = GitData.Clients.apiClient.ExecutePutAsync(request).Result;

            if (!response.IsSuccessful)
            {
                throw new HttpRequestException(response.ErrorException?.Message);
            }

            if (response.Content != null)
            {
                GitIssue? issueX = JsonConvert.DeserializeObject<GitIssue>(response.Content);
            }
        }
    }
}