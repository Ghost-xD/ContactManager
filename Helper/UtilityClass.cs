using ContactManager.Controllers;
using ContactManager.Models;
using Newtonsoft.Json;

namespace ContactManager.Helper
{
    public class UtilityClass
    {
        private readonly ILogger<UtilityClass> _logger;
        public UtilityClass(ILogger<UtilityClass> logger)
        {
            _logger = logger;
        }
        public UtilityClass()
        {

        }
        public async Task<List<Contacts>> LoadDataAsync(string path, string type)
        {
            try
            {
                switch (type)
                {
                    case "json":
                        return await LoadJsonData(path);
                    case "mssql":
                        return await LoadMsSQL(path);
                    default:
                        return new List<Contacts>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new List<Contacts>();
            }
        }

        private static async Task<List<Contacts>> LoadJsonData(string path)
        {
            string jsonContent = await System.IO.File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<List<Contacts>>(jsonContent) ?? new List<Contacts>();
        }

        private static async Task<List<Contacts>> LoadMsSQL(string path)
        {
            return new List<Contacts>();
        }

        public async Task SaveJsonAsync(string path, string json)
        {
            try
            {
                await File.WriteAllTextAsync(path, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
