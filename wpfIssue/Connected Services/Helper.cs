namespace wpfIssues.ConnectedServices
{
    public class Helper
    {
        //internal static ServiceIssues.ServiceIssueClient getServiceIssueClient()
        //{
        //    return new ServiceIssues.ServiceIssueClient
        //        (ServiceIssues.ServiceIssueClient.EndpointConfiguration.WSHttpBinding_IServiceIssue,
        //        "https://localhost:5001/ServiceIssue/WSHttps");
        //}

        /*
            Temporary solution until a method is identified to create a valid SSL certificate for a static IP address.
        */
        internal static ServiceIssues.ServiceIssueClient getServiceIssueClient()
        {
            return new ServiceIssues.ServiceIssueClient
            (
                ServiceIssues.ServiceIssueClient.EndpointConfiguration.BasicHttpBinding_IServiceIssue,
                Properties.Settings.Default.serviceIssue
            );
        }
    }
}
