using System;
using System.Diagnostics;
using AudioSubsystem.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AudioSubsystem
{
    class AudioManager : GameComponent, IAudioManager
    {
        private const string s_xactContent = @"Content\Sound\";
        private const string s_xactFileName = @"GameSound.xap";
        private const string s_musicCategory = "Music";
        private const string s_soundCategory = "Sounds";

        private readonly AudioEngine m_engine;
        private readonly WaveBank m_waveBank;
        private readonly SoundBank m_soundBank;

        private AudioCategory m_musicCategory;
        private AudioCategory m_soundCategory;

        private Cue m_currentMusic;

        private bool disposed;

        public AudioManager(Game game) : base(game)
        {
            string xactFileName = s_xactFileName.Replace(".xap", "");

            m_engine = new AudioEngine(s_xactContent + xactFileName + ".xgs");
            m_waveBank = new WaveBank(m_engine, s_xactContent + "Wave Bank.xwb");
            m_soundBank = new SoundBank(m_engine, s_xactContent + "Sound Bank.xsb");

            m_currentMusic = null;

            game.Deactivated += OnDeactivated;
            game.Activated += OnActivated;

            disposed = false;
        }

        public override void Initialize()
        {
            //Categories
            m_musicCategory = m_engine.GetCategory(s_musicCategory);
            m_soundCategory = m_engine.GetCategory(s_soundCategory);

            //Set the volumes
            m_musicCategory.SetVolume(Settings.Default.MusicVolume);
            m_soundCategory.SetVolume(Settings.Default.SoundVolume);
        }

        void OnActivated(object sender, EventArgs e)
        {
            if(m_currentMusic != null)
                m_musicCategory.Resume();
        }

        void OnDeactivated(object sender, EventArgs e)
        {
            if (m_currentMusic != null)
                m_musicCategory.Pause();
        }

        public override void Update(GameTime gameTime)
        {
            m_engine.Update();
            base.Update(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if(!disposed && disposing)
            {
                //Save the settings
                Settings.Default.Save();

                //Do the disposing
                if (m_currentMusic != null)
                    m_currentMusic.Dispose();

                m_musicCategory = new AudioCategory();
                m_soundCategory = new AudioCategory();

                m_engine.Dispose();
                m_waveBank.Dispose();
                m_soundBank.Dispose();

                disposed = true;
            }
        }

        public void ChangeMusic(MusicKey key)
        {
            CheckDisposed();

            if(m_currentMusic != null)
            {
                m_currentMusic.Stop(AudioStopOptions.AsAuthored);
                m_currentMusic.Dispose();
                m_currentMusic = null;
            }

            if(key != MusicKey.None)
            {
                m_currentMusic = m_soundBank.GetCue(key.ToString());
                m_currentMusic.Play();   
            }
        }

        public void PlaySound(SoundKey key)
        {
            CheckDisposed();

            Cue sound = m_soundBank.GetCue(key.ToString());
            sound.Play();
        }

        public float MusicVolume
        {
            get
            {
                CheckDisposed();

                return Settings.Default.MusicVolume;
            }
            set
            {
                CheckDisposed();

                float realMusicVolume = Clip(value);
                Settings.Default.MusicVolume = realMusicVolume;
                m_musicCategory.SetVolume(realMusicVolume);
                m_musicCategory.Resume();
            }
        }

        public float SoundVolume
        {
            get
            {
                CheckDisposed();

                return Settings.Default.SoundVolume;
            }
            set
            {
                CheckDisposed();

                float realSoundVolume = Clip(value);
                Settings.Default.SoundVolume = realSoundVolume;
                m_soundCategory.SetVolume(realSoundVolume);   
                m_soundCategory.Resume();
            }
        }

        public bool IsDisposed
        {
            get { return disposed; }
        }

        private static float Clip(float value)
        {
            if (value < 0)
                return 0;
            if (value > 1)
                return 1;
            return value;
        }

        private void CheckDisposed()
        {
            if (disposed)
                throw new ObjectDisposedException("The audio manager has been disposed");
        }
    }
}
