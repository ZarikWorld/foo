using Newtonsoft.Json;
using RestSharp;
using serviceIssues.Source.Model.GitlabJSONDef;

namespace serviceIssues.Source.Data.GitData
{
    public class ProjectData
    {
        public static async Task<Model.GitlabJSONDef.ProjectData> getProjectAsync(int project_id)
        {
            RestRequest request = GitData.Clients.createApiRequest($"/projects/{project_id}", Method.Get);
            var response = await GitData.Clients.apiClient.ExecuteGetAsync(request);

            return JsonConvert.DeserializeObject<Model.GitlabJSONDef.ProjectData>(response.Content);
        }
    }
}
