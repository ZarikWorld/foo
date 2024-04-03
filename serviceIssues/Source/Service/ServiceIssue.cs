using serviceIssues.Source.Data.ServiceData;
using serviceIssues.Source.Model.WpfClient;
using serviceIssues.Source.Service;
using libCodLibCS.nsFileSystem;

namespace serviceIssues.Service
{
    public class ServiceIssue : IServiceIssue
    {
        public static string logFileName = clsFileSystem.getLogFilePath();

        #region Issue
        public List<Issue>? getProjectIssues(int project_id, int mitarbeiter_id)
        {
            return issueServiceBusiness.getProjectIssues(project_id, mitarbeiter_id);
        }
        public List<Issue>? getProjectsIssues(List<int> projects_ids, int mitarbeiter_id)
        {
            try
            {
                var projectsIssues = new List<Issue>();
                foreach (int project_id in projects_ids)
                {
                    var projectX_issues = getProjectIssues(project_id, mitarbeiter_id);
                    if (projectX_issues != null)
                    {
                        projectsIssues.AddRange(projectX_issues);
                    }
                }
                return projectsIssues;
            }
            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                clsFileSystem.addProtokollException(logFileName, innerException, "<ServiceIssue.getProjectIssues>");
                throw new FaultException(innerException.Message);
            }
        }
        public Issue? updateIssue(Issue issue, int mitarbeiter_id)
        {
            try
            {
                return issueServiceBusiness.updateIssue(issue, mitarbeiter_id);
            }
            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                clsFileSystem.addProtokollException(logFileName, innerException, $"<ServiceIssue.updateIssue>: id: {issue.id} iid: {issue.iid}");
                return null;
            }
        }
        //TODO 2024-04-01 => Add new Implementatin for ChangeSortOrderNew
        public List<Issue> UpdateIssue20240401(List<int> projects_ids, int mitarbeiter_id, Issue modifiedIssue)
        {
            return issueServiceBusiness.UpdateIssue20240401(projects_ids, mitarbeiter_id, modifiedIssue);
        }
        public List<Issue>? changeSortOrder(List<int> projects_ids, int mitarbeiter_id, int sortOrderFrom, int sortOrderTo)
        {
            try
            {
                return issueServiceBusiness.changeSortOrder(projects_ids, mitarbeiter_id, sortOrderFrom, sortOrderTo);
            }
            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                clsFileSystem.addProtokollException(logFileName, innerException, $"<ServiceIssue.changeSortOrder>: sortOrderFrom: {sortOrderFrom} sortOrderTo:{sortOrderTo}");
                return null;
            }
        }
        public void issuesAnordnen(int? sortOrder = null, int? mitarbeiter_id = null)
        {
            try
            {
                issueServiceBusiness.issuesAnordnen(sortOrder, mitarbeiter_id);
            }
            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                string messageSortOrder = sortOrder.HasValue ? $"sortOrder: {sortOrder}" : "sortOrder: null";
                string messageMitarbeiter_id = mitarbeiter_id.HasValue ? $"mitarbeiter_id: {mitarbeiter_id}" : "mitarbeiter_id: null";
                clsFileSystem.addProtokollException(logFileName, innerException, $"<ServiceIssue.issuesAnordnen>: {messageSortOrder} {messageMitarbeiter_id}");
            }
        }
        public List<Issue> movePrioPunkte(int id, bool isUpward)
        {
            try
            {
                return issueServiceBusiness.movePrioPunkte(id, isUpward);
            }
            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                clsFileSystem.addProtokollException(logFileName, innerException, $"<ServiceIssue.movePrioPunkte>: id: {id} isUpward:{isUpward}");
                return new List<Issue>();
            }
        }
        public List<Issue> movePrioPunkteList(List<int> projects_ids, IEnumerable<int> ids, bool isUpward, int mitarbeiter_id)
        {
            var result = new  List<Issue>();

            try
            {
                /*return*/ issueServiceBusiness.movePrioPunkteList(projects_ids, ids, isUpward, mitarbeiter_id);
                var tmpResult = getProjectsIssues(projects_ids, mitarbeiter_id);
                if (tmpResult != null)
                {
                    result = tmpResult;
                }
            }
            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                clsFileSystem.addProtokollException(logFileName, innerException, $"<ServiceIssue.movePrioPunkte>: id: {ids} isUpward:{isUpward}");
            }

            return result;
        }
        #endregion

        #region Mitarbeiter
        public List<Mitarbeiter>? getMitarbeiters()
        {
            try
            {
                return issueServiceBusiness.getMitarbeiters();
            }

            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                clsFileSystem.addProtokollException(logFileName, innerException, $"<ServiceIssue.getMitarbeiters>");
                return null;
            }
        }
        public Mitarbeiter? getMitarbeiter(int mitarbeiter_id)
        {
            try
            {
                return issueServiceBusiness.getMitarbeiter(mitarbeiter_id);
            }

            catch (Exception ex)
            {
                var innerException = clsFileSystem.getInnermostException(ex);
                clsFileSystem.addProtokollException(logFileName, innerException, $"<ServiceIssue.getMitarbeiter>: {mitarbeiter_id}");
                return null;
            }
        }
        #endregion
    }
}