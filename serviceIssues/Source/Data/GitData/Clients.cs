using HtmlAgilityPack;
using RestSharp;

namespace serviceIssues.Source.Data.GitData
{
    public class Clients
    {
        private static HttpClient? _httpClient;
        public static HttpClient httpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = createHttpClient();
                }
                return _httpClient;
            }
        }
        public static RestClient apiClient = new RestClient(AppConfig.gitlabRestAPI);

        private static HttpClient createHttpClient()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12; // Or Tls13 if available and supported.
            var cookieContainer = new System.Net.CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };
            var client = new HttpClient(handler);
            var signInResponseTask = client.GetAsync($"{AppConfig.gitlabWebAPI}/users/sign_in").Result;
            var signInResponse = signInResponseTask.Content.ReadAsStringAsync().Result;

            var doc = new HtmlDocument();
            doc.LoadHtml(signInResponse);
            var csrfToken = doc.DocumentNode.SelectSingleNode("//input[@name='authenticity_token']").GetAttributeValue("value", "");

            // Step 2: Identification
            var content = new FormUrlEncodedContent(new[]
            {
                    new KeyValuePair<string, string>("username", AppConfig.gitlabUsername),
                    new KeyValuePair<string, string>("password", AppConfig.gitlabPassword),
                    new KeyValuePair<string, string>("authenticity_token", csrfToken),
                });

            var postResponse = client.PostAsync($"{AppConfig.gitlabWebAPI}/users/auth/ldapmain/callback", content).Result;
            postResponse.EnsureSuccessStatusCode();

            return client;
        }

        public static RestRequest createApiRequest(string restource, Method method)
        {
            var request = new RestRequest(restource, method);
            request.AddHeader("PRIVATE-TOKEN", AppConfig.gitlabPrivateToken);
            return request;
        }

        public static RestRequest createApiRequest(string restource, string gitlabPrivateToken, Method method)
        {
            var request = new RestRequest(restource, method);
            request.AddHeader("PRIVATE-TOKEN", gitlabPrivateToken);
            return request;
        }
    }
}
