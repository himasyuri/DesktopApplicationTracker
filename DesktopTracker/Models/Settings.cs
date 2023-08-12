namespace DesktopTracker.Models
{
    internal class Settings
    {
        public string SendTo { get; set; } = string.Empty;

        public float Delay { get; set; }

        public string AuthKey { get; set; } = string.Empty;
    }
}
