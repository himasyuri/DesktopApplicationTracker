namespace DesktopTracker.Utils
{
    internal class SettingsUtil
    {

        private static string FileName => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        public static T ReadConfiguration<T>()
        {
            IJsonUtil jsonUtil = new JsonUtil();

            if (!File.Exists(FileName))
            {
                CreateConfig();
            }

            var json = File.ReadAllText(FileName);

            return jsonUtil.JsonToItem<T>(json);
        }

        public static void SaveChanges<T>(T sender, bool format)
        {
            IJsonUtil jsonUtil = new JsonUtil();

            jsonUtil.JsonToFile<T>(sender, FileName, format);
        }

        private static void CreateConfig()
        {
            using (StreamWriter sw = new StreamWriter(FileName, true))
            {
                string json =
                    $$"""
                    {
                        "Settings": {
                            "SendTo": "https://",
                            "Delay": 1,
                            "AuthKey":  ""
                        }
                    }
                    """;

                sw.WriteLine(json);
            }
        }
    }
}
