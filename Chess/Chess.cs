using ChessMatchStateSubsystem;
using GraphicsSubsystem;
using Microsoft.Xna.Framework;
using InputSubsystem;
using AudioSubsystem;
using GameStateManagmentSubsystem;
using ResourceSubsystem;
using StaticImageGameStateSubsystem;
using MenuGameStateSubsystem;

namespace Chess
{
    public class Chess : Game
    {
        private readonly IInputManager m_inputManager;
        private readonly IAudioManager m_audioManager;
        private readonly IGameStateManager m_gameStateManager;
        private readonly IResourceManager m_resourceManager;
        private readonly IGraphicsManager m_graphicsManager;

        public Chess()
        {
            Content.RootDirectory = "Content";

            new GraphicsDeviceManager(this);

            m_inputManager = InputManagerFactory.GetInputManager(this);
            Components.Add(m_inputManager);

            m_audioManager = AudioManagerFactory.GetAudioManager(this);
            Components.Add(m_audioManager);

            m_resourceManager = ResourceManagerFactory.GetResourceManager(this);
            Components.Add(m_resourceManager);

            m_graphicsManager = GraphicsManagerFactory.GetGraphicsManager(this);
            Components.Add(m_graphicsManager);

            m_gameStateManager = GameStateManagerFactory.GetGameStateManager(this);
            Components.Add(m_gameStateManager);
        }

        protected override void Initialize()
        {
            base.Initialize();

            StaticImageGameState splashScreenState = new StaticImageGameState(this, GameTextureKey.GameStartupImage, 5.0, 0.0, false);
            MenuGameState menuState = new MenuGameState(this);
            ChessMatchState chessMatchGameState = new ChessMatchState(this);

            splashScreenState.NextState = menuState;
            menuState.ChessMatchState = chessMatchGameState;
            chessMatchGameState.MenuState = menuState;

            m_gameStateManager.Change(splashScreenState);
            m_gameStateManager.Push(menuState);
        }
    }
}
