using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace wpfIssues.Model
{
    public class AppConfig
    {
        #region Fields
        private class configData
        {
            public int mitarbeiter_id { get; set; }
            public List<int> projects_ids { get; set; }
        }
        private int _mitarbeiter_id;
        private List<int> _projects_ids;
        private readonly string _configPath;
        #endregion

        #region Properties
        [JsonPropertyName("mitarbeiter_id")]
        public int mitarbeiter_id
        {
            get => _mitarbeiter_id;
            set => _mitarbeiter_id = value;
        }

        [JsonPropertyName("projects_ids")]
        public List<int> projects_ids
        {
            get => _projects_ids;
            set => _projects_ids = value;
        }
        #endregion

        public AppConfig(string configPath)
        {
            _configPath = configPath;
            LoadConfig();
        }
        private void LoadConfig()
        {
            var jsonData = File.ReadAllText(_configPath);
            var config = JsonSerializer.Deserialize<configData>(jsonData);

            _mitarbeiter_id = config.mitarbeiter_id;
            _projects_ids = config.projects_ids;
        }
        public void SaveConfig()
        {
            var configData = new configData
            {
                mitarbeiter_id = _mitarbeiter_id,
                projects_ids = _projects_ids,
            };

            var jsonData = JsonSerializer.Serialize(configData);
            File.WriteAllText(_configPath, jsonData);
        }
    }
}