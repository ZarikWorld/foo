using serviceIssues.Source.Model.WpfClient;

namespace serviceIssues.Source.Service
{
    [ServiceContract]
    public interface IServiceIssue
    {
        #region Issue
        [OperationContract]
        List<Issue>? getProjectIssues(int project_id, int mitarbeiter_id);

        [OperationContract]
        List<Issue>? getProjectsIssues(List<int> projects_ids, int mitarbeiter_id);
        
        [OperationContract]
        Issue? updateIssue(Issue issue, int mitarbeiter_id);

        [OperationContract]
        List<Issue> UpdateIssue20240401(List<int> projects_ids, int mitarbeiter_id, Issue modifiedIssue);


        [OperationContract]
        List<Issue>? changeSortOrder(List<int> projects_ids, int mitarbeiter_id, int sortOrderFrom, int sortOrderTo);

        [OperationContract]
        List<Issue> movePrioPunkte(int id, bool isUpward);

        [OperationContract]
        List<Issue> movePrioPunkteList(List<int> projects_ids, IEnumerable<int> ids, bool isUpward, int mitarbeiter_id);

        [OperationContract]
        void issuesAnordnen(int? sortOrder = null, int? mitarbeiter_id = null);
        #endregion

        #region Mitarbeiter
        [OperationContract]
        List<Mitarbeiter>? getMitarbeiters();

        [OperationContract]
        Mitarbeiter? getMitarbeiter(int mitarbeiter_id);
        #endregion
    }
}
