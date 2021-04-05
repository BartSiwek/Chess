using Microsoft.Xna.Framework;

namespace AudioSubsystem
{
    public static class AudioManagerFactory
    {
        private static AudioManager s_audioManager;

        public static IAudioManager GetAudioManager(Game game)
        {
            if(s_audioManager == null)
            {
                s_audioManager = new AudioManager(game);
                game.Services.AddService(typeof(IAudioManager), s_audioManager);
            }
            return s_audioManager;
        }
    }
}
