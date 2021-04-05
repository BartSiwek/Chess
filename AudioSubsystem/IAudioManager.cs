using System;
using Microsoft.Xna.Framework;

namespace AudioSubsystem
{
    public interface IAudioManager : IGameComponent, IDisposable, IUpdateable
    {
        //Basic functions
        void ChangeMusic(MusicKey key);
        void PlaySound(SoundKey key);
        float MusicVolume { get; set; }
        float SoundVolume { get; set; }
        bool IsDisposed { get; }
    }
}
