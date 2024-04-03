using libJxIssues;
using serviceIssues.Source.Data.GitData;
using serviceIssues.Source.Model.GitlabJSONDef;
using serviceIssues.Source.Model.WpfClient;

namespace serviceIssues.Source.Data.ServiceData
{
    public class issueServiceBusiness
    {
        #region Issue
        private static DateTime getProjectLastSyncDateTime(int project_id)
        {
            return clsProgramInformation.getProjectLastSyncDateTime(project_id);
        }
        public static List<Issue> UpdateIssue20240401(List<int> projects_ids, int mitarbeiter_id, Issue modifiedIssue)
        {
            var result = new List<Issue>();
            var originalIssue = clsJxIssue.getIssueByGitIID(modifiedIssue.iid, modifiedIssue.project_id);

            if (modifiedIssue.sortOrder != originalIssue!.sortOrder)
            {
                if (issueServiceBusiness.changeSortOrder(projects_ids, mitarbeiter_id, originalIssue.sortOrder!.Value, modifiedIssue.sortOrder!.Value) is List<Issue> updatedCollection)
                {
                    foreach (var updatedIssue in updatedCollection) { result.Add(updatedIssue); }
                }
            }
            else
            {
                if (issueServiceBusiness.updateIssue(modifiedIssue, mitarbeiter_id) is Issue updatedIssue) 
                { 
                    result.Add(updatedIssue); 
                }
            }

            return result;
        }
        public static Issue? updateIssue(Issue wpf_modifiedIssue, int mitarbeiter_id)
        {
            string gitlabPrivateToken = getMitarbeiterToken(mitarbeiter_id);

            updateGitlabIssue(wpf_modifiedIssue, gitlabPrivateToken);

            if (wpf_modifiedIssue.start != null && wpf_modifiedIssue.start.Value.Hour == 0)
            {
                wpf_modifiedIssue.start = wpf_modifiedIssue.start.Value.AddHours(8);
            }
            clsJxIssue tmpJxIssue = issueServiceBusiness.serviceIssue2JxIssue(wpf_modifiedIssue);

            var mitarbeiter = issueServiceBusiness.getMitarbeiter(mitarbeiter_id) ?? throw new NullReferenceException($"Invalid mitarbeiter_id: {mitarbeiter_id} ");
            tmpJxIssue.ende = issueServiceBusiness.calculateEndDate(tmpJxIssue.schaetzung, tmpJxIssue.start, mitarbeiter);

            tmpJxIssue.update();

            UpdateDb(wpf_modifiedIssue.project_id, gitlabPrivateToken);

            var updatedJxIssue = clsJxIssue.getIssueByGitIID(wpf_modifiedIssue.iid, wpf_modifiedIssue.project_id);

            if (updatedJxIssue == null) return null;

            return jxIssue2ServiceIssue(updatedJxIssue);
        }
        private static Issue createJxIssue(GitIssue gitIssueX, int? indexOfGitlabList = null)
        {
            clsJxIssue issueX = new clsJxIssue();

            if (clsProgramInformation.projectFirstTimeDbSync(gitIssueX.project_id) && clsProgramInformation.getProjects().Count < 1)
            {
                issueX.sortOrder = indexOfGitlabList != null ? indexOfGitlabList : null;
            }
            else
            {
                var newSortOrder = getNextNagativeSortOrder();
                issueX.sortOrder = newSortOrder == 0 ? newSortOrder - 1 : newSortOrder;
            }

            issueX.created_at = gitIssueX.created_at.AddHours(2);

            var creatorX_id = clsMitarbeiter.getMitarbeiter(gitIssueX.author.username);
            if (creatorX_id == null)
            {
                var newMitarbeiter = createMitarbeiter(gitIssueX.author.id, gitIssueX.author.name, gitIssueX.author.username, gitIssueX.author.avatar_url);
                issueX.creator_id = newMitarbeiter.id;

            }
            else
            {
                issueX.creator_id = creatorX_id.id;
            }


            if (gitIssueX.due_date != null)
            {
                issueX.deadline = DateTime.Parse(gitIssueX.due_date);
            }

            issueX.erledigt = false;
            issueX.git = true;
            issueX.iid = gitIssueX.iid;

            if (gitIssueX.assignees != null && gitIssueX.assignees.Count > 0)
            {
                var mitarbeiterX_id = clsMitarbeiter.getMitarbeiter(gitIssueX.assignees[0].username);
                if (mitarbeiterX_id == null)
                {
                    var newMitarbeiter = createMitarbeiter(gitIssueX.assignees[0].id,
                                                           gitIssueX.assignees[0].name,
                                                           gitIssueX.assignees[0].username,
                                                           gitIssueX.assignees[0].avatar_url);
                    issueX.mitarbeiter_id = newMitarbeiter.id;
                }
                else
                {
                    issueX.mitarbeiter_id = mitarbeiterX_id.id;
                }
            }

            issueX.prioPunkte = gitIssueX.time_stats.time_estimate / 3600;

            if (issueX.prioPunkte == 0) issueX.prioPunkte = null;

            issueX.project_id = gitIssueX.project_id;

            switch (gitIssueX.state)
            {
                case "opened":
                    issueX.status = clsJxIssue.enumStatus.opened; break;
                case "closed":
                    issueX.status = clsJxIssue.enumStatus.closed; break;
                default:
                    throw new InvalidOperationException("Unexpected issue state: " + gitIssueX.state);
            }

            issueX.titel = gitIssueX.title;

            issueX.typ = clsJxIssue.enumTyp.story;

            foreach (string label in gitIssueX.labels)
            {
                if (!label.Any()) continue;

                //curently due to the service conflicts, there is no definition for the enumStatus and enumTyp
                //in the service provider. the values are directly mapped using the integer value from the wpf 

                var labelLowerCase = label.ToLower();
                if (labelLowerCase == "bug")
                {
                    issueX.typ = clsJxIssue.enumTyp.bug; break;
                }
                else if (labelLowerCase == "performance")
                {
                    issueX.typ = clsJxIssue.enumTyp.performance; break;
                }
                else if (labelLowerCase == "luxus")
                {
                    issueX.typ = clsJxIssue.enumTyp.luxus; break;
                }
                else if (labelLowerCase == "refactor")
                {
                    issueX.typ = clsJxIssue.enumTyp.refactor; break;
                }
                else if (labelLowerCase == "imtest")
                {
                    issueX.status = clsJxIssue.enumStatus.imTest; break;
                }
            }

            issueX.web_url = gitIssueX.web_url;

            issueX.insert();

            return jxIssue2ServiceIssue(issueX);
        }
        private static Issue updateJxIssue(GitIssue gitIssueX)
        {
            clsJxIssue? issueX = clsJxIssue.getIssueByGitIID(gitIssueX.iid, gitIssueX.project_id);
            if (issueX is null)
            {
                throw new InvalidOperationException($"issue with iid: {gitIssueX.iid} and project_id: {gitIssueX.project_id} not found.");
            }

            issueX.iid = gitIssueX.iid;
            issueX.project_id = gitIssueX.project_id;

            bool issueClosed = (!issueX.erledigt && gitIssueX.state == "closed");
            bool issueReopened = (issueX.erledigt && gitIssueX.state == "opened");

            if (issueClosed)
            {
                issueX.erledigt = true;
                issueX.status = clsJxIssue.enumStatus.closed;
                issueX.sortOrder = null;
            }
            else if (issueReopened)
            {
                issueX.erledigt = false;
                issueX.status = clsJxIssue.enumStatus.opened;
                var tmpSortOrder = getNextNagativeSortOrder();
                issueX.sortOrder = tmpSortOrder == 0 ? tmpSortOrder - 1 : tmpSortOrder;
            }

            issueX.prioPunkte = gitIssueX.time_stats.time_estimate / 3600;

            if (issueX.prioPunkte == 0) issueX.prioPunkte = null;

            issueX.titel = gitIssueX.title;
            issueX.git = true;

            issueX.web_url = gitIssueX.web_url;

            //TEMP
            var creatorX_id = clsMitarbeiter.getMitarbeiter(gitIssueX.author.username);
            issueX.creator_id = creatorX_id.id;
            //END TEMP

            if (gitIssueX.assignees != null && gitIssueX.assignees.Count > 0)
            {
                var mitarbeiterX_id = clsMitarbeiter.getMitarbeiter(gitIssueX.assignees[0].username);
                if (mitarbeiterX_id == null)
                {
                    var newMitarbeiter = createMitarbeiter(gitIssueX.assignees[0].id,
                                                           gitIssueX.assignees[0].name,
                                                           gitIssueX.assignees[0].username,
                                                           gitIssueX.assignees[0].avatar_url);
                    issueX.mitarbeiter_id = newMitarbeiter.id;
                }
                else
                {
                    issueX.mitarbeiter_id = mitarbeiterX_id.id;
                }
            }

            if (gitIssueX.due_date != null)
            {
                issueX.deadline = DateTime.Parse(gitIssueX.due_date);
            }

            issueX.typ = clsJxIssue.enumTyp.story;
            issueX.status = clsJxIssue.enumStatus.opened;

            if (gitIssueX.labels.Count > 0)
            {
                foreach (string label in gitIssueX.labels)
                {
                    var labelX = label.ToLower();

                    if (issueX.typ == clsJxIssue.enumTyp.story)
                    {
                        if (labelX == "bug") issueX.typ = clsJxIssue.enumTyp.bug;
                        else if (labelX == "performance") issueX.typ = clsJxIssue.enumTyp.performance;
                        else if (labelX == "luxus") issueX.typ = clsJxIssue.enumTyp.luxus;
                        else if (labelX == "refactor") issueX.typ = clsJxIssue.enumTyp.refactor;
                    }

                    if (issueX.status == clsJxIssue.enumStatus.opened)
                    {
                        if (labelX == "imtest") issueX.status = clsJxIssue.enumStatus.imTest;
                        else if (labelX == "inbearbeitung") issueX.status = clsJxIssue.enumStatus.inBearbeitung;
                        else if (labelX == "klaerungsbedarf") issueX.status = clsJxIssue.enumStatus.klaerungsbedarf;
                    }
                }
            }

            if (gitIssueX.state == "opened")
            {
                if (issueX.status == clsJxIssue.enumStatus.closed)
                {
                    issueX.status = clsJxIssue.enumStatus.opened;
                }
            }
            else if (gitIssueX.state == "closed")
            {
                issueX.status = clsJxIssue.enumStatus.closed;
            }
            else
            {
                throw new InvalidOperationException("Unexpected issue state: " + gitIssueX.state);
            }

            issueX.update();

            return jxIssue2ServiceIssue(issueX);
        }
        public static void updateGitlabIssue(Issue wpf_modifiedIssue, string gitlabPrivateToken)
        {
            clsJxIssue? database_originalIssue = clsJxIssue.getIssueByGitIID(wpf_modifiedIssue.iid, wpf_modifiedIssue.project_id);
            if (database_originalIssue != null)
            {
                int? mitarbeiter_git_id = null;
                if (database_originalIssue.mitarbeiter_id != wpf_modifiedIssue.mitarbeiter_id)
                {
                    if (wpf_modifiedIssue.mitarbeiter_id != null)
                    {
                        var mitarbeiter = clsMitarbeiter.getMitarbeiter(wpf_modifiedIssue.mitarbeiter_id.Value);
                        if (mitarbeiter != null)
                        {
                            mitarbeiter_git_id = mitarbeiter.gitlab_id;
                        }
                    }
                }

                DateTime? deadline = null;
                if (database_originalIssue.deadline != wpf_modifiedIssue.deadline)
                {
                    deadline = wpf_modifiedIssue.deadline;
                }

                List<string> removeLables = new List<string>();
                List<string> newLables = new List<string>();

                if (wpf_modifiedIssue.status != (int)database_originalIssue.status)
                {
                    //enumStatus

                    //    opened = 1,
                    //    closed = 2,
                    //    imTest = 3,
                    //    inBearbeitung = 4,
                    //    klaerungsbedarf = 5

                    // client doesn't alter issue 'opened'/'closed' status, so these labels won't be used.

                    removeLables.Add(database_originalIssue.status.ToString());

                    if (wpf_modifiedIssue.status != 1)
                    {
                        newLables.Add(((clsJxIssue.enumStatus)wpf_modifiedIssue.status).ToString());
                    }
                }

                if (wpf_modifiedIssue.typ != (int)database_originalIssue.typ)
                {
                    //enumTyp
                    //    Story = 1,
                    //    Bug = 2,
                    //    Performance = 3,
                    //    Luxus = 4,
                    //    Refactor = 5,

                    removeLables.Add(database_originalIssue.typ.ToString());
                    newLables.Add(((clsJxIssue.enumTyp)wpf_modifiedIssue.typ).ToString());
                }

                IssueData.updateIssue(wpf_modifiedIssue.iid, wpf_modifiedIssue.project_id, gitlabPrivateToken, mitarbeiter_git_id, deadline, newLables, removeLables);
            }
        }
        private static void UpdateDb(int project_id_, string gitlabPrivateToken)
        {
            var dbLastSyncWithGit = issueServiceBusiness.getProjectLastSyncDateTime(project_id_).AddHours(-2);

            List<GitIssue>? gitLabIssues;

            bool dbUpdated = false;
            if (clsProgramInformation.projectFirstTimeDbSync(project_id_))
            {
                gitLabIssues = IssueData.getIssuesAsync(project_id_, gitlabPrivateToken, clsProgramInformation.historyStartDate, EnumIssueState.opened).Result;
                if (gitLabIssues is null) return;

                //there is any other project in the programinformation go to normal creation process (else block)
                if (clsProgramInformation.getProjects().Count > 1)
                {
                    foreach (GitIssue gitlabIssueX in gitLabIssues)
                    {
                        if (gitlabIssueX is null) continue;

                        clsJxIssue? dbIssueRecord = clsJxIssue.getIssueByGitIID(gitlabIssueX.iid, gitlabIssueX.project_id);
                        if (dbIssueRecord is null)
                        {
                            issueServiceBusiness.createJxIssue(gitlabIssueX); continue;
                        }
                        else
                        {
                            issueServiceBusiness.updateJxIssue(gitlabIssueX);
                        }

                        if (!dbUpdated) dbUpdated = true;
                    }
                }
                else
                {
                    int sortOrder = 1;
                    foreach (GitIssue gitlabIssueX in gitLabIssues)
                    {
                        if (gitlabIssueX is null) continue;

                        createJxIssue(gitlabIssueX, sortOrder);
                        sortOrder++;

                        if (!dbUpdated) dbUpdated = true;
                    }
                }
            }
            else
            {
                gitLabIssues = IssueData.getIssuesAsync(project_id_, gitlabPrivateToken, dbLastSyncWithGit, EnumIssueState.all).Result;
                if (gitLabIssues is null) return;

                foreach (GitIssue gitlabIssueX in gitLabIssues)
                {
                    if (gitlabIssueX is null || gitlabIssueX.state == "closed") continue;

                    clsJxIssue? dbIssueRecord = clsJxIssue.getIssueByGitIID(gitlabIssueX.iid, gitlabIssueX.project_id);
                    if (dbIssueRecord is null)
                    {
                        issueServiceBusiness.createJxIssue(gitlabIssueX); continue;
                    }
                    else
                    {
                        issueServiceBusiness.updateJxIssue(gitlabIssueX);
                    }

                    if (!dbUpdated) dbUpdated = true;
                }
            }

            if (dbUpdated) clsProgramInformation.updateLastSync(project_id_, DateTime.Now);
        }
        public static List<Issue>? getProjectIssues(int project_id, int mitarbeiter_id, bool updateDb = true)
        {
            string gitlabPrivateToken = getMitarbeiterToken(mitarbeiter_id);

            if (updateDb) issueServiceBusiness.UpdateDb(project_id, gitlabPrivateToken);

            List<clsJxIssue>? issues = clsJxIssue.getIssuesByProjectId(project_id);

            if (issues == null || issues.Count() == 0) return null;

            List<Issue> wpfIssues = new List<Issue>();
            foreach (clsJxIssue issue in issues ?? Enumerable.Empty<clsJxIssue>())
            {
                if (issue == null || issue.erledigt) { continue; }

                var mitarbeiter = issueServiceBusiness.getMitarbeiter(mitarbeiter_id) ?? throw new NullReferenceException($"Invalid mitarbeiter_id: {mitarbeiter_id} ");
                issue.ende = issueServiceBusiness.calculateEndDate(issue.schaetzung, issue.start, mitarbeiter);
                issue.update();

                Issue issueX = new Issue()
                {
                    id = issue.id,
                    iid = issue.iid,
                    project_id = project_id,
                    erledigt = issue.erledigt,
                    prioPunkte = issue.prioPunkte,
                    titel = issue.titel,
                    git = issue.git,
                    web_url = issue.web_url,
                    sortOrder = issue.sortOrder,
                    start = issue.start,
                    ende = issue.ende,
                    deadline = issue.deadline,
                    creator_id = issue.creator_id,
                    mitarbeiter_id = issue.mitarbeiter_id,
                    mitarbeiter = issue.xxxMitarbeiterName,
                    schaetzung = issue.schaetzung,
                    schaetzungOffiziell = issue.schaetzungOffiziell,
                    created_at = issue.created_at,
                    typ = (int)issue.typ,
                    status = (int)issue.status,
                    aktenzahl = issue.aktenzahl,
                    kunde = issue.kunde,
                    anmerkung = issue.anmerkung
                };
                wpfIssues.Add(issueX);
            }

            wpfIssues = (from issue in wpfIssues select issue).OrderBy(issue => issue.sortOrder).ToList();

            return wpfIssues;
        }
        private static List<Issue>? getProjectsIssues(List<int> projects_ids, int mitarbeiter_id)
        {
            List<Issue>? wpfIssues = new List<Issue>();

            foreach (int project_id in projects_ids)
            {
                List<Issue>? projectIssues = issueServiceBusiness.getProjectIssues(project_id, mitarbeiter_id);
                if (projectIssues != null)
                {
                    wpfIssues.AddRange(projectIssues);
                }
            }

            return wpfIssues;
        }
        private static clsJxIssue serviceIssue2JxIssue(Issue xxx)
        {
            clsJxIssue tmpClsJxIssue = new clsJxIssue();
            tmpClsJxIssue.id = xxx.id;
            tmpClsJxIssue.iid = xxx.iid;
            tmpClsJxIssue.project_id = xxx.project_id;
            tmpClsJxIssue.erledigt = xxx.erledigt;
            tmpClsJxIssue.prioPunkte = xxx.prioPunkte;
            tmpClsJxIssue.titel = xxx.titel;
            tmpClsJxIssue.git = xxx.git;
            tmpClsJxIssue.web_url = xxx.web_url;
            tmpClsJxIssue.sortOrder = xxx.sortOrder;
            tmpClsJxIssue.created_at = xxx.created_at;
            tmpClsJxIssue.start = xxx.start;
            tmpClsJxIssue.ende = xxx.ende;
            tmpClsJxIssue.creator_id = xxx.creator_id;
            tmpClsJxIssue.mitarbeiter_id = xxx.mitarbeiter_id;
            tmpClsJxIssue.schaetzung = xxx.schaetzung;
            tmpClsJxIssue.schaetzungOffiziell = xxx.schaetzungOffiziell;
            tmpClsJxIssue.deadline = xxx.deadline;
            tmpClsJxIssue.typ = (clsJxIssue.enumTyp)xxx.typ;
            tmpClsJxIssue.status = (clsJxIssue.enumStatus)xxx.status;
            tmpClsJxIssue.aktenzahl = xxx.aktenzahl;
            if (xxx.kunde != null)
            {
                tmpClsJxIssue.kunde = xxx.kunde;
            }
            if (xxx.anmerkung != null)
            {
                tmpClsJxIssue.anmerkung = xxx.anmerkung;
            }

            return tmpClsJxIssue;
        }
        private static int? getNextNagativeSortOrder()
        {
            return libJxIssues.clsJxIssue.getNextAvailableNegativeSortOrder();
        }
        public static List<Issue>? changeSortOrder(List<int> projects_id, int mitarbeiter_id, int sortOrderFrom, int sortOrderTo)
        {
            clsJxIssue.ChangeSortOrder(sortOrderFrom, sortOrderTo);
            List<Issue>? latestIssues = issueServiceBusiness.getProjectsIssues(projects_id, mitarbeiter_id);

            return latestIssues;
        }
        private static void mitarbeiterIssuesAnordnen(int mitarbeiter_id, int? sortOrder = null)            //gantt
        {
            List<clsJxIssue> issues = new List<clsJxIssue>();

            if (sortOrder is null)   //Alle Mitarbeiter-Issues anordnen"
            {
                issues = (from issue in clsJxIssue.getIssues()
                          where issue.mitarbeiter_id == mitarbeiter_id &&
                                issue.sortOrder.HasValue && issue.sortOrder.Value > 0 &&
                                !issue.erledigt
                          orderby issue.sortOrder ascending
                          select issue).ToList();
            }
            else   //Alle Mitarbeiter-Issues ab hier anordnen"
            {
                issues = (from issue in clsJxIssue.getIssues()
                          where issue.mitarbeiter_id == mitarbeiter_id &&
                                issue.sortOrder.HasValue && issue.sortOrder.Value > 0 &&
                                !issue.erledigt &&
                                issue.sortOrder >= sortOrder
                          orderby issue.sortOrder ascending
                          select issue).ToList();
            }

            if (!issues.Any()) { return; }

            var mitarbeiter = issueServiceBusiness.getMitarbeiter(mitarbeiter_id) ?? throw new NullReferenceException($"Invalid mitarbeiter_id: {mitarbeiter_id} ");

            DateTime? nextIssueStart = DateTime.MinValue;
            for (int i = 0; i < issues.Count; i++)
            {
                clsJxIssue issueX = issues[i];

                if (nextIssueStart != DateTime.MinValue)
                {
                    if (nextIssueStart!.Value.Hour == 16)
                    {
                        nextIssueStart = new DateTime(nextIssueStart.Value.Year, nextIssueStart.Value.Month, nextIssueStart.Value.Day, 8, 0, 0).AddDays(1);
                    }
                    issueX.start = nextIssueStart;
                }
                else if (issueX.start == null)
                {
                    issueX.start = generateIssueStart(issueX, mitarbeiter);
                }

                
                var tmpSchaetzung = issueX.schaetzung.HasValue ? issueX.schaetzung.Value : 0;
                
                nextIssueStart = calculateEndDate(tmpSchaetzung, issueX.start, mitarbeiter);
                issueX.update();
            }
        }
        public static void issuesAnordnen(int? sortOrder = null, int? mitarbeiter_id = null)
        {
            if (mitarbeiter_id is null)
            {
                issuesAnordnen(sortOrder);
            }
            else
            {
                mitarbeiterIssuesAnordnen(mitarbeiter_id.Value, sortOrder);
            }
        }
        private static void issuesAnordnen(int? sortOrder = null)
        {
            List<clsJxIssue> issues = new List<clsJxIssue>();
            if (sortOrder is null)    //issues anordnen"
            {
                issues = (from issue in clsJxIssue.getIssues()
                          where issue.schaetzung.HasValue &&
                                issue.sortOrder.HasValue &&
                                issue.mitarbeiter_id.HasValue &&    //issue is assigned
                                issue.sortOrder > 0
                          orderby issue.sortOrder
                          select issue).ToList();
            }
            else   //ab hier anordnen"
            {
                issues = (from issue in clsJxIssue.getIssues()
                          where issue.schaetzung.HasValue &&
                                issue.sortOrder.HasValue &&
                                issue.mitarbeiter_id.HasValue &&    //issue is assigned
                                issue.sortOrder >= sortOrder
                          orderby issue.sortOrder
                          select issue).ToList();
            }

            if (!issues.Any())
            {
                return;
            }

            Dictionary<int, clsJxIssue> issuesDic = new Dictionary<int, clsJxIssue>();
            foreach (var issueX in issues)
            {
                if (issueX.mitarbeiter_id.HasValue)
                {
                    var mitarbeiter = issueServiceBusiness.getMitarbeiter(issueX.mitarbeiter_id.Value) ?? throw new NullReferenceException($"Invalid mitarbeiter_id: {issueX.mitarbeiter_id.Value} ");

                    if (issuesDic.ContainsKey(issueX.mitarbeiter_id.Value))
                    {
                        issueX.start = calculateEndDate(issuesDic[issueX.mitarbeiter_id.Value].schaetzung.Value, issuesDic[issueX.mitarbeiter_id.Value].start, mitarbeiter);
                        issuesDic[issueX.mitarbeiter_id.Value] = issueX;
                    }
                    else
                    {
                        if (!issueX.start.HasValue)
                        {
                            issueX.start = DateTime.Today.AddHours(8);
                            while (issueX.start.Value.DayOfWeek == DayOfWeek.Saturday || issueX.start.Value.DayOfWeek == DayOfWeek.Sunday)
                            {
                                issueX.start = issueX.start.Value.AddDays(1);
                            }
                        }
                        issuesDic.Add(issueX.mitarbeiter_id.Value, issueX);
                    }
                    issueX.update();
                }
            }
        }
        private static DateTime? calculateEndDate(int? schaetzung, DateTime? start, Mitarbeiter mitarbeiter)
        {
            if (start == null || schaetzung == null) { return null; }

            DateTime result = start.Value;
            int totalHours = schaetzung.Value;

            while (totalHours > 0)
            {
                if (!mitarbeiter.workDays.TryGetValue(result.DayOfWeek, out int availableWorkHours))
                {
                    // If the day is not found in workDays, it's considered a non-working day.
                    availableWorkHours = 0;
                }

                // Adjust the start hour based on your workday start time (assuming 8 AM here)
                DateTime startOfWorkDay = new DateTime(result.Year, result.Month, result.Day, 8, 0, 0);
                DateTime endOfWorkDay = startOfWorkDay.AddHours(availableWorkHours);


                //calculate workable hours left in the day
                int hoursLeftToday = (int)((endOfWorkDay - result).TotalHours);

                if (totalHours <= hoursLeftToday)
                {
                    //if the task can be finished today, do it!!!!
                    result = result.AddHours(totalHours);
                    break;
                }
                else
                {
                    //if not, add the remaining hours for today and get the rest to next avialable day
                    result = result.AddHours(hoursLeftToday);
                    totalHours -= hoursLeftToday;
                }

                //move on to the next day, skipping weekends
                do
                {
                    result = result.AddDays(1);
                    result = new DateTime(result.Year, result.Month, result.Day, 8, 0, 0);
                }
                while (result.DayOfWeek == DayOfWeek.Saturday || result.DayOfWeek == DayOfWeek.Sunday || mitarbeiter.workDays[result.DayOfWeek] == 0);
            }

            return result;
        }

        private static Issue jxIssue2ServiceIssue(clsJxIssue issue)
        {
            Issue result = new Issue()
            {
                aktenzahl = issue.aktenzahl,
                created_at = issue.created_at,
                creator_id = issue.creator_id,
                deadline = issue.deadline,
                erledigt = issue.erledigt,
                git = issue.git,
                id = issue.id,
                iid = issue.iid,
                kunde = issue.kunde,
                anmerkung = issue.anmerkung,
                mitarbeiter = issue.xxxMitarbeiterName,
                mitarbeiter_id = issue.mitarbeiter_id,
                prioPunkte = issue.prioPunkte,
                project_id = issue.project_id,
                schaetzung = issue.schaetzung,
                schaetzungOffiziell= issue.schaetzungOffiziell,
                sortOrder = issue.sortOrder,
                start = issue.start,
                ende = issue.ende,
                status = (int)issue.status,
                titel = issue.titel,
                typ = (int)issue.typ,
                web_url = issue.web_url
            };
            return result;
        }
        public static List<Issue> movePrioPunkte(int id, bool isUpward)
        {
            var updatedIssues = clsJxIssue.movePrioPunkte(id, isUpward);

            var result = new List<Issue>();
            if (updatedIssues.Count != 00)
            {
                foreach (var issue in updatedIssues)
                {
                    var tmpIssue = jxIssue2ServiceIssue(issue);
                    result.Add(tmpIssue);
                }
            }
            return result;
        }
        public static void /*List<Issue>*/ movePrioPunkteList(List<int> projects_ids, IEnumerable<int> ids, bool isUpward, int mitarbeiter_id)
        {
            clsJxIssue.movePrioPunkteList(ids, isUpward);
        }

        private static DateTime generateIssueStart(clsJxIssue issueX, Mitarbeiter mitarbeiter)
        {
            //---------------------------------------------------------------------------------------------------------------------------------------------------------
            //Clarification 2024-03-26: issueServiceBusiness.generateIssueStart => If it's already past 8:00 AM on a workday, the start date should be the next workday
            // Start from current date/time if before 8:00 AM on a workday
            //var result = now.TimeOfDay < new TimeSpan(8, 0, 0)
            //            ? now.Date : now.AddDays(1);

            //while (result.DayOfWeek == DayOfWeek.Saturday || result.DayOfWeek == DayOfWeek.Sunday || mitarbeiter.workDays[result.DayOfWeek] == 0)
            //{
            //    result = result.AddDays(1);
            //}

            //return result;
            //---------------------------------------------------------------------------------------------------------------------------------------------------------

            var result = DateTime.Today.AddHours(8);

            var workingDays = mitarbeiter.workDays;

            while (result.DayOfWeek == DayOfWeek.Saturday || result.DayOfWeek == DayOfWeek.Sunday || workingDays[result.DayOfWeek] == 0)
            {
                result = result.AddDays(1);
            }

            return result;
        }
        #endregion

        #region Mitarbeiter
        public static Mitarbeiter? getMitarbeiter(int id_)
        {
            var mitarbeiter = clsMitarbeiter.getMitarbeiter(id_);
            Mitarbeiter? result = null;
            if (mitarbeiter != null)
            {
                result = new Mitarbeiter
                {
                    avatar_url = mitarbeiter.avatar_url,
                    gitlab_id = mitarbeiter.gitlab_id,
                    gitUsername = mitarbeiter.gitUsername,
                    gitToken = mitarbeiter.gitToken,
                    id = mitarbeiter.id,
                    name = mitarbeiter.name,
                    mitarbeiterRole = (int)mitarbeiter.mitarbeiterRole,
                    team_id = (int)mitarbeiter.team_id,
                    montag = mitarbeiter.montag,
                    dienstag = mitarbeiter.dienstag,
                    mittwoch = mitarbeiter.mittwoch,
                    donnerstag = mitarbeiter.donnerstag,
                    freitag = mitarbeiter.freitag
                };
            }
            return result;
        }
        public static List<Mitarbeiter> getMitarbeiters()
        {
            var mitarbeiters = clsMitarbeiter.getMitarbeiters();

            var result = new List<Mitarbeiter>();
            foreach (var mitarbeiter in mitarbeiters)
            {
                var mitarbeiterX = new Mitarbeiter()
                {
                    avatar_url = mitarbeiter.avatar_url,
                    gitlab_id = mitarbeiter.gitlab_id,
                    gitUsername = mitarbeiter.gitUsername,
                    gitToken = mitarbeiter.gitToken,
                    id = mitarbeiter.id,
                    name = mitarbeiter.name,
                    mitarbeiterRole = (int)mitarbeiter.mitarbeiterRole,
                    team_id = (int)mitarbeiter.team_id,
                    montag = mitarbeiter.montag,
                    dienstag = mitarbeiter.dienstag,
                    mittwoch = mitarbeiter.mittwoch,
                    donnerstag = mitarbeiter.donnerstag,
                    freitag = mitarbeiter.freitag
                };
                result.Add(mitarbeiterX);
            }
            return result;
        }
        private static clsMitarbeiter createMitarbeiter(int gitlab_id, string name, string username, string? avatar_url)
        {
            if (clsMitarbeiter.getMitarbeiter(username) != null)
            {
                throw new InvalidOperationException($"mitarbeiter username: {username} already exist!");
            }

            clsMitarbeiter? newMitarbeiter = new clsMitarbeiter()
            {
                avatar_url = avatar_url,
                gitlab_id = gitlab_id,
                gitUsername = username,
                name = name,
                mitarbeiterRole = libJxIssues.enumMitarbeiterRole.user,
                team_id = libJxIssues.enumTeam.other,
            };
            newMitarbeiter.insert();

            return newMitarbeiter;
        }
        private static string getMitarbeiterToken(int mitarbeiter_id)
        {
            var mitarbeiter = clsMitarbeiter.getMitarbeiter(mitarbeiter_id);
            if (mitarbeiter == null)
            {
                throw new KeyNotFoundException($"Mitarbeiter with ID {mitarbeiter_id} could not be found.");
            }

            if (string.IsNullOrEmpty(mitarbeiter.gitToken))
            {
                throw new InvalidOperationException($"GitToken for Mitarbeiter with ID {mitarbeiter.id} is null or empty.");
            }

            return mitarbeiter.gitToken;
        }
        #endregion
    }
}