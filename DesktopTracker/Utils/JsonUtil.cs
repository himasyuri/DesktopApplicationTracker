using Newtonsoft.Json;

namespace DesktopTracker.Utils
{
    internal class JsonUtil : IJsonUtil
    {
        public T JsonToItem<T>(string jsonString) => JsonConvert.DeserializeObject<T>(jsonString) ??
                                                    throw new ArgumentNullException(nameof(jsonString));

        public (bool result, Exception? exception) JsonToFile<T>(T sender, string fileName, bool format)
        {
            try
            {
                var options = new JsonSerializerSettings() { Formatting = Formatting.Indented };

                if (format)
                {
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(sender, options.Formatting));

                    return (true, null);
                }

                File.WriteAllText(fileName, JsonConvert.SerializeObject(sender));

                return (true, null);

            }
            catch (Exception ex)
            {
                return (false, ex);
            }
        }
    }
}
