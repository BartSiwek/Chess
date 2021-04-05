using AudioSubsystem;
using GameStateManagmentSubsystem;
using GraphicsSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ResourceSubsystem;
using xWinFormsLib;

namespace MenuGameStateSubsystem
{
    public class MenuGameState : IGameState
    {
        #region Storage

        private const string s_mainMenuTitle = "Game menu";

        private const string s_mainMenuStartGame = "Start Game";
        private const string s_mainMenuOptions = "Options";
        private const string s_mainMenuExit = "Exit game";

        private const string s_optionsMenuMusic = "Music";
        private const string s_optionsMenuSounds = "Sounds";
        private const string s_optionsResolution = "Resolution";

        private const string s_optionsResolution800x600 = "800x600";
        private const string s_optionsResolution1024x768 = "1024x768";
        private const string s_optionsResolution1280x960 = "1280x960";

        private readonly ServiceHelper m_serviceHelper;
        private readonly FormCollection m_forms;
        private bool m_prevOptionsFormVisibility;

        private readonly IGameStateManager m_gameStateManager;
        private readonly IResourceManager m_resourceManager;
        private readonly IAudioManager m_audioManager;
        private readonly IGraphicsManager m_graphicsManager;

        private IGameState m_chessMatchState;

        #endregion

        #region Construction

        public MenuGameState(Game game)
        {
            m_gameStateManager = (IGameStateManager)game.Services.GetService(typeof (IGameStateManager));
            m_resourceManager = (IResourceManager)game.Services.GetService(typeof(IResourceManager));
            m_audioManager = (IAudioManager)game.Services.GetService(typeof(IAudioManager));
            m_graphicsManager = (IGraphicsManager)game.Services.GetService(typeof(IGraphicsManager));

            m_serviceHelper = new ServiceHelper();
            m_serviceHelper.Initialize(m_graphicsManager.GraphicsDevice, game.Content);

            m_forms = new FormCollection();
            InitializeUI();
        }

        private void InitializeUI()
        {
            Form mainForm = new Form(
                new Vector2(50, 50),
                new Vector2(200, 200),
                s_mainMenuTitle,
                Color.DarkGray,
                Color.Black,
                "Verdana",
                0.7f,
                false,
                false,
                false,
                false,
                Form.BorderStyle.FixedSingle,
                Form.Style.Default
            );

            TextButton startGameButton = new TextButton(
                s_mainMenuStartGame, 
                new Vector2(20, 50), 
                s_mainMenuStartGame,
                160,
                Color.White,
                m_resourceManager.GetFont(GameFontKey.Verdana),
                Form.Style.Default
            );
            startGameButton.OnPress += startGameButton_OnPress;
            mainForm.Add(startGameButton);

            TextButton optionsButton = new TextButton(
                s_mainMenuOptions,
                new Vector2(20, 80),
                s_mainMenuOptions,
                160,
                Color.White,
                m_resourceManager.GetFont(GameFontKey.Verdana),
                Form.Style.Default
            );
            optionsButton.OnPress += optionsButton_OnPress;
            mainForm.Add(optionsButton);

            TextButton exitButton = new TextButton(
                s_mainMenuExit,
                new Vector2(20, 110),
                s_mainMenuExit,
                160,
                Color.White,
                m_resourceManager.GetFont(GameFontKey.Verdana),
                Form.Style.Default
            );
            exitButton.OnPress += exitButton_OnPress;
            mainForm.Add(exitButton);

            Form optionsForm = new Form(
                new Vector2(270, 50),
                new Vector2(400, 500),
                s_mainMenuOptions,
                Color.DarkGray,
                Color.Black,
                "Verdana",
                0.7f,
                false,
                false,
                false,
                false,
                Form.BorderStyle.FixedSingle,
                Form.Style.Default
            );

            Label musicLabel = new Label(
                s_optionsMenuMusic + "Label",
                new Vector2(45, 50),
                s_optionsMenuMusic + ":",
                Label.Alignment.Left,
                300,
                Color.Black,
                m_resourceManager.GetFont(GameFontKey.Verdana)
            );
            optionsForm.Add(musicLabel);

            Slider musicVolumeSlider = new Slider(
                s_optionsMenuMusic,
                new Vector2(50, 70), 
                300,
                0,
                100,
                (int)(m_audioManager.MusicVolume * 100),
                Form.Style.Default
            );
            optionsForm.Add(musicVolumeSlider);

            Label soundLabel = new Label(
                s_optionsMenuSounds + "Label",
                new Vector2(45, 90),
                s_optionsMenuSounds + ":",
                Label.Alignment.Left,
                300,
                Color.Black,
                m_resourceManager.GetFont(GameFontKey.Verdana)
            );
            optionsForm.Add(soundLabel);

            Slider soundVolumeSlider = new Slider(
                s_optionsMenuSounds,
                new Vector2(50, 110),
                300,
                0,
                100,
                (int)(m_audioManager.SoundVolume * 100),
                Form.Style.Default
            );
            optionsForm.Add(soundVolumeSlider);

            Label resolutionLabel = new Label(
                s_optionsResolution + "Label",
                new Vector2(45, 130),
                s_optionsResolution + ":",
                Label.Alignment.Left,
                50,
                Color.Black,
                m_resourceManager.GetFont(GameFontKey.Verdana)
            );
            optionsForm.Add(resolutionLabel);

            Combo resolutionComboBox = new Combo(
                s_optionsResolution,
                new Vector2(145, 130),
                200,
                m_resourceManager.GetFont(GameFontKey.Verdana),
                Form.Style.Default
            );

            resolutionComboBox.AddItem(s_optionsResolution800x600);
            resolutionComboBox.AddItem(s_optionsResolution1024x768);
            resolutionComboBox.AddItem(s_optionsResolution1280x960);

            if (m_graphicsManager.ViewportSize == ViewportSizePreset.Size800x600)
                resolutionComboBox.SelectedItem = s_optionsResolution800x600;
            if (m_graphicsManager.ViewportSize == ViewportSizePreset.Size1024x768)
                resolutionComboBox.SelectedItem = s_optionsResolution1024x768;
            if (m_graphicsManager.ViewportSize == ViewportSizePreset.Size1280x960)
                resolutionComboBox.SelectedItem = s_optionsResolution1280x960;

            optionsForm.Add(resolutionComboBox);

            m_forms.Add(mainForm);
            m_forms.Add(optionsForm);
        }

        #endregion

        #region Properties

        public IGameState ChessMatchState
        {
            get { return m_chessMatchState; }
            set { m_chessMatchState = value; }
        }

        #endregion

        #region IGameState Members

        public void LoadContent()
        { }

        public void Activated()
        {
            m_forms[s_mainMenuTitle].Show();
            m_prevOptionsFormVisibility = false;
            if (!m_audioManager.IsDisposed)
                m_audioManager.ChangeMusic(MusicKey.MenuMusic);
        }

        public void Update(GameTime gameTime)
        {
            //Update the forms
            m_forms.Update();

            //Do the event handling
            if(!m_forms[s_mainMenuTitle].bVisible)
                mainForm_Closed();
            if(m_forms[s_mainMenuOptions].bVisible != m_prevOptionsFormVisibility)
            {
                if (!m_forms[s_mainMenuOptions].bVisible)
                    optionsForm_Closed();                    
            }

            //Update the event variables
            m_prevOptionsFormVisibility = m_forms[s_mainMenuOptions].bVisible;
        }

        public void Draw(GameTime gameTime)
        {
            m_gameStateManager[1].Draw(gameTime);
            m_forms.Draw();
        }

        public void Deactivated()
        {
            if (!m_audioManager.IsDisposed)
                m_audioManager.ChangeMusic(MusicKey.None);
        }

        public void UnloadContent()
        { }

        #endregion

        #region IDisposable Members

        public void Dispose()
        { }

        #endregion

        #region Event handlers

        private void startGameButton_OnPress(object sender, System.EventArgs e)
        {
            m_gameStateManager.Change(m_chessMatchState);
        }

        private void optionsButton_OnPress(object sender, System.EventArgs e)
        {
            m_forms[s_mainMenuOptions].Show();
        }

        private void exitButton_OnPress(object sender, System.EventArgs e)
        {
            m_gameStateManager.Change(null);
        }

        private void mainForm_Closed()
        {
            m_gameStateManager.Change(null);            
        }

        private void optionsForm_Closed()
        {
            Slider musicSlider = (Slider)m_forms[s_mainMenuOptions].Controls[s_optionsMenuMusic];
            m_audioManager.MusicVolume = (float) (musicSlider.Value)/100;

            Slider soundSlider = (Slider)m_forms[s_mainMenuOptions].Controls[s_optionsMenuSounds];
            m_audioManager.SoundVolume = (float)(soundSlider.Value) / 100;

            Combo resolutionCombo = (Combo)m_forms[s_mainMenuOptions].Controls[s_optionsResolution];
            if (resolutionCombo.SelectedItem == s_optionsResolution800x600)
                m_graphicsManager.ViewportSize = ViewportSizePreset.Size800x600;
            if (resolutionCombo.SelectedItem == s_optionsResolution1024x768)
                m_graphicsManager.ViewportSize = ViewportSizePreset.Size1024x768;
            if (resolutionCombo.SelectedItem == s_optionsResolution1280x960)
                m_graphicsManager.ViewportSize = ViewportSizePreset.Size1280x960;
        }

        #endregion
    }
}
