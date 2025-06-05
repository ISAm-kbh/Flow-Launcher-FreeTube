namespace Flow.Launcher.Plugin.FreeTube
{
    public class Settings
    {
        public bool favorNewInstance { get; set; } = true;
        public bool manuallySpecifyPath { get; set; } = false;
        public string userSpecifiedPath { get; set; } = string.Empty;
    }
}
