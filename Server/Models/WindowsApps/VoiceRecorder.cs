namespace UtilityBox.App.Server.Models.WindowsApps
{
    public class VoiceRecorder : IWindowsApp
    {
        public string Name => "soundrecorder";

        public string DisplayName => "Voice Recorder";

        public string Description => "Windows pre-installed Voice/Sound Recorder application";
    }
}
