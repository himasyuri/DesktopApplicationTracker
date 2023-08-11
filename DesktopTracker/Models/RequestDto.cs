using Newtonsoft.Json;

namespace DesktopTracker.Models
{
    internal class RequestDto
    {
        [JsonProperty("window_name")]
        public string WindowName { get; set; } = string.Empty;

        [JsonProperty("process_name")]
        public string ProcessName { get; set; } = string.Empty;

        [JsonProperty("timestamp")]
        public string TimeStamp { get; set; } = string.Empty;
    }
}
