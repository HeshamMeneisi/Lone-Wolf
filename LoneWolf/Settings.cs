namespace LoneWolf
{
    internal class Settings
    {
        public Settings()
        {
            MusicVolume = SFXVolume = 0.8f;
        }
        public ThemeType CurrentTheme { get; internal set; }
        public float MusicVolume { get; internal set; }
        public float SFXVolume { get; internal set; }
    }
}