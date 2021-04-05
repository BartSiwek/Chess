using AudioSubsystem;
using ChessMatchStateSubsystem.Match;
using ChessMatchStateSubsystem.Match.States;
using GameStateManagmentSubsystem;
using GraphicsSubsystem;
using InputSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ResourceSubsystem;

namespace ChessMatchStateSubsystem
{
    public class ChessMatchState : IGameState
    {
        #region Storage

        //Game stuff
        private readonly IInputManager m_inputManger;
        private readonly IGameStateManager m_gameStateManager;
        private readonly IGraphicsManager m_graphicsManager;
        private readonly IResourceManager m_resourceManager;
        private readonly IAudioManager m_audioManager;
        private IGameState m_menuState;

        //Models
        private Model m_chessboardModel;
        private Model m_lettersModel;
        private Model m_whitePawnModel;
        private Model m_whiteRookModel;
        private Model m_whiteKnightModel;
        private Model m_whiteBishopModel;
        private Model m_whiteQueenModel;
        private Model m_whiteKingModel;
        private Model m_blackPawnModel;
        private Model m_blackRookModel;
        private Model m_blackKnightModel;
        private Model m_blackBishopModel;
        private Model m_blackQueenModel;
        private Model m_blackKingModel;

        //Effects
        private Effect m_chessboardPickingEffect;
        private Effect m_phongBumpReflect;
        private Effect m_phong;

        //Textures
        private Texture2D m_chessboardDiffuse;
        private Texture2D m_chessboardBump;
        private Texture2D m_chessboardPickingTexture;
        private Texture2D m_lettersTexture;
        private Texture2D m_whitePiecesTexture;
        private Texture2D m_blackPiecesTexture;
        private Texture2D m_selectedPiecesTexture;
        private Texture2D m_piecesBumpMap;

        //Fonts
        private SpriteFont m_messageFont;

        //Chess match stuff
        private readonly ChessMatch m_chessMatch;
        private IMatchState m_matchState;

        #endregion

        #region Construction

        public ChessMatchState(Game game)
        {
            //Managers
            m_inputManger = (IInputManager)game.Services.GetService(typeof(IInputManager));
            m_gameStateManager = (IGameStateManager)game.Services.GetService(typeof(IGameStateManager));
            m_graphicsManager = (IGraphicsManager)game.Services.GetService(typeof(IGraphicsManager));
            m_resourceManager = (IResourceManager)game.Services.GetService(typeof(IResourceManager));
            m_audioManager = (IAudioManager) game.Services.GetService(typeof (IAudioManager));

            //Chess match stuff
            m_matchState = WhitePiecePicking.GetMatchState(this);
            m_chessMatch = new ChessMatch(this);
        }

        #endregion

        #region Properties

        //Managers
        public IGameState MenuState
        {
            get { return m_menuState; }
            set { m_menuState = value; }
        }

        internal IGraphicsManager GraphicsManager
        {
            get { return m_graphicsManager; }
        }

        internal IInputManager InputManager
        {
            get { return m_inputManger; }
        }

        internal IAudioManager AudioManager
        {
            get { return m_audioManager; }
        }

        //Internal stuff
        internal ChessMatch Match
        {
            get { return m_chessMatch; }
        }

        //Effects
        internal Effect ChessboardPickingEffect
        {
            get { return m_chessboardPickingEffect; }
        }

        internal Effect Phong
        {
            get { return m_phongBumpReflect; }
        }

        //Models
        internal Model ChessboardModel
        {
            get { return m_chessboardModel; }
        }

        internal Model WhitePawnModel
        {
            get { return m_whitePawnModel; }
        }

        internal Model WhiteRookModel
        {
            get { return m_whiteRookModel; }
        }

        internal Model WhiteKnightModel
        {
            get { return m_whiteKnightModel; }
        }

        internal Model WhiteBishopModel
        {
            get { return m_whiteBishopModel; }
        }

        internal Model WhiteQueenModel
        {
            get { return m_whiteQueenModel; }
        }

        internal Model WhiteKingModel
        {
            get { return m_whiteKingModel; }
        }

        internal Model BlackPawnModel
        {
            get { return m_blackPawnModel; }
        }

        internal Model BlackRookModel
        {
            get { return m_blackRookModel; }
        }

        internal Model BlackKnightModel
        {
            get { return m_blackKnightModel; }
        }

        internal Model BlackBishopModel
        {
            get { return m_blackBishopModel; }
        }

        internal Model BlackQueenModel
        {
            get { return m_blackQueenModel; }
        }

        internal Model BlackKingModel
        {
            get { return m_blackKingModel; }
        }

        //Fonts
        internal SpriteFont MessageFont
        {
            get { return m_messageFont; }
        }

        //Textures
        internal Texture2D WhitePiecesTexture
        {
            get { return m_whitePiecesTexture; }
        }

        internal Texture2D BlackPiecesTexture
        {
            get { return m_blackPiecesTexture; }
        }

        internal Texture2D SelectedPiecesTexture
        {
            get { return m_selectedPiecesTexture; }
        }

        internal Texture2D PiecesBumpMap
        {
            get { return m_piecesBumpMap; }
        }

        #endregion

        #region Chess match support methods

        internal void ExitMatch()
        {
            m_gameStateManager.Push(m_menuState);
        }

        #endregion

        #region Game state methods

        public void LoadContent()
        {
            //Load models
            m_chessboardModel = m_resourceManager.GetModel(GameModelKey.Chessboard);
            m_lettersModel = m_resourceManager.GetModel(GameModelKey.Letters);
            m_whitePawnModel = m_resourceManager.GetModel(GameModelKey.WhitePawn);
            m_whiteRookModel = m_resourceManager.GetModel(GameModelKey.WhiteRook);
            m_whiteKnightModel = m_resourceManager.GetModel(GameModelKey.WhiteKnight);
            m_whiteBishopModel = m_resourceManager.GetModel(GameModelKey.WhiteBishop);
            m_whiteQueenModel = m_resourceManager.GetModel(GameModelKey.WhiteQueen);
            m_whiteKingModel = m_resourceManager.GetModel(GameModelKey.WhiteKing);
            m_blackPawnModel = m_resourceManager.GetModel(GameModelKey.BlackPawn);
            m_blackRookModel = m_resourceManager.GetModel(GameModelKey.BlackRook);
            m_blackKnightModel = m_resourceManager.GetModel(GameModelKey.BlackKnight);
            m_blackBishopModel = m_resourceManager.GetModel(GameModelKey.BlackBishop);
            m_blackQueenModel = m_resourceManager.GetModel(GameModelKey.BlackQueen);
            m_blackKingModel = m_resourceManager.GetModel(GameModelKey.BlackKing);

            //Load effects
            m_phongBumpReflect = m_resourceManager.GetEffect(this, GameEffectKey.PhongBumpReflect);
            m_chessboardPickingEffect = m_resourceManager.GetEffect(this, GameEffectKey.SimpleTexture);
            m_phong = m_resourceManager.GetEffect(this, GameEffectKey.Phong);

            //Load textures
            m_chessboardPickingTexture = m_resourceManager.GetTexture(this, GameTextureKey.ChessboardPicking);
            m_chessboardDiffuse = m_resourceManager.GetTexture(this, GameTextureKey.ChessboardDiffuse);
            m_chessboardBump = m_resourceManager.GetTexture(this, GameTextureKey.ChessboardBump);
            m_lettersTexture = m_resourceManager.GetTexture(this, GameTextureKey.LettersDiffuse);
            m_whitePiecesTexture = m_resourceManager.GetTexture(this, GameTextureKey.WhitePiecesDiffuse);
            m_blackPiecesTexture = m_resourceManager.GetTexture(this, GameTextureKey.BlackPiecesDiffuse);
            m_selectedPiecesTexture = m_resourceManager.GetTexture(this, GameTextureKey.SelectedPiecesDiffuse);
            m_piecesBumpMap = m_resourceManager.GetTexture(this, GameTextureKey.PiecesBumpMap);

            //Load fonts
            m_messageFont = m_resourceManager.GetFont(GameFontKey.VerdanaBig);

            //Setup
            m_chessboardPickingEffect.Parameters["Texture"].SetValue(m_chessboardPickingTexture);

            //Setup the match
            m_chessMatch.SetupGame();
            m_matchState = WhitePiecePicking.GetMatchState(this);

            //Reset the camera
            m_graphicsManager.ResetCamera();
        }

        public void Activated()
        {
            m_audioManager.ChangeMusic(MusicKey.GameMusic);
        }

        public void Update(GameTime gameTime)
        {
            //Update the state
            m_matchState = m_matchState.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            //Clear
            m_graphicsManager.GraphicsDevice.Clear(Color.Black);

            //Setup
            m_phongBumpReflect.Parameters["ColorTexture"].SetValue(m_chessboardDiffuse);
            m_phongBumpReflect.Parameters["NormalTexture"].SetValue(m_chessboardBump);

            //Draw the board
            Matrix world = Matrix.Identity;
            m_graphicsManager.DrawModel(m_chessboardModel, ref world, m_phongBumpReflect);

            //Setup the phong shader
            m_phong.Parameters["ColorTexture"].SetValue(m_lettersTexture);

            //Draw the letters
            m_graphicsManager.DrawModel(m_lettersModel, ref world, m_phong);

            //Draw the chess pieces
            m_chessMatch.Draw(gameTime);

            //Draw the state
            m_matchState.Draw(gameTime);
        }

        public void Deactivated()
        {
            if(!m_audioManager.IsDisposed)
                m_audioManager.ChangeMusic(MusicKey.None);
        }

        public void UnloadContent()
        {
            //Unload textures
            m_resourceManager.UnloadTexture(this, GameTextureKey.ChessboardPicking);
            m_resourceManager.UnloadTexture(this, GameTextureKey.ChessboardDiffuse);
            m_resourceManager.UnloadTexture(this, GameTextureKey.ChessboardBump);
            m_resourceManager.UnloadTexture(this, GameTextureKey.LettersDiffuse);
            m_resourceManager.UnloadTexture(this, GameTextureKey.WhitePiecesDiffuse);
            m_resourceManager.UnloadTexture(this, GameTextureKey.BlackPiecesDiffuse);
            m_resourceManager.UnloadTexture(this, GameTextureKey.SelectedPiecesDiffuse);
            m_resourceManager.UnloadTexture(this, GameTextureKey.PiecesBumpMap);

            //Unload effects
            m_resourceManager.UnloadEffect(this, GameEffectKey.SimpleTexture);
            m_resourceManager.UnloadEffect(this, GameEffectKey.PhongBumpReflect);
            m_resourceManager.UnloadEffect(this, GameEffectKey.Phong);
        }

        public void Dispose()
        { }

        #endregion
    }
}
