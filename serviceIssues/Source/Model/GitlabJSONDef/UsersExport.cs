using System.Text.RegularExpressions;
using CsvHelper;

namespace serviceIssues.Source.Model.GitlabJSONDef
{
    public class UserInformation
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public class UserInformationExtractor
    {
        private string _filePath;

        public UserInformationExtractor(string filePath)
        {
            _filePath = filePath;
        }

        public List<UserInformation> ExtractUserInformation()
        {
            var users = new List<UserInformation>();
            using (var reader = new StreamReader(_filePath))
            {
                using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var dn = csv.GetField("dn");
                        var displayName = csv.GetField("displayname");
                        var mail = csv.GetField("mail");

                        var userId = ExtractUserId(dn);
                        users.Add(new UserInformation { UserId = userId, FullName = displayName, Email = mail });
                    }
                }
            }
            return users;
        }

        private string ExtractUserId(string dn)
        {
            var match = Regex.Match(dn, @"uid=(.*?),");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }
    }















































}
