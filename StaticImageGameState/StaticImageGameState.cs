using GameStateManagmentSubsystem;
using GraphicsSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ResourceSubsystem;

namespace StaticImageGameStateSubsystem
{
    public class StaticImageGameState : IGameState
    {
        #region Storage

        private IResourceManager m_resourceManager;
        private IGraphicsManager m_graphicsManager;
        private IGameStateManager m_gameStateManager;

        private readonly GameTextureKey m_key;
        private readonly double m_fadeTime;
        private readonly double m_maxTime;
        private IGameState m_nextState;
        private readonly bool m_change;
        private double m_timeElapsed;
        private Color m_tintColor;

        private Texture2D m_displayedImage;
        private SpriteBatch m_spriteBatch;

        #endregion

        #region Construction

        public StaticImageGameState(Game game, GameTextureKey key, double maxTime) 
            : this(game, key, maxTime, maxTime/5)
        { }

        public StaticImageGameState(Game game, GameTextureKey key, double maxTime, double fadeTime)
            : this(game, key, maxTime, fadeTime, true)
        {}

        public StaticImageGameState(Game game, GameTextureKey key, double maxTime, double fadeTime, bool change)
        {
            m_resourceManager = (IResourceManager)game.Services.GetService(typeof(IResourceManager));
            m_graphicsManager = (IGraphicsManager)game.Services.GetService(typeof(IGraphicsManager));
            m_gameStateManager = (IGameStateManager)game.Services.GetService(typeof(IGameStateManager));

            m_key = key;
            m_maxTime = maxTime;
            m_timeElapsed = 0;
            m_fadeTime = fadeTime;
            m_tintColor = new Color(255, 255, 255, (byte)(m_fadeTime > 0 ? 0 : 255));
            m_change = change;
        }

        #endregion

        #region Properties

        public IGameState NextState
        {
            get { return m_nextState; }
            set { m_nextState = value; }
        }

        #endregion

        #region IGameState Members

        public void LoadContent()
        {
            m_displayedImage = m_resourceManager.GetTexture(this, m_key);
            m_spriteBatch = m_graphicsManager.GetSpriteBatch();
        }

        public void Activated()
        { }

        public void Update(GameTime gameTime)
        {
            m_timeElapsed += gameTime.ElapsedRealTime.TotalSeconds;

            float alphaFract = (float)(m_timeElapsed/m_fadeTime);
            byte alphaValue = (byte)MathHelper.Clamp(255 * alphaFract, 0, 255);
            m_tintColor = new Color(255, 255, 255, alphaValue);

            if(m_timeElapsed > m_maxTime)
            {
                m_tintColor = new Color(255, 255, 255, 255);
                if(m_change)
                    m_gameStateManager.Change(m_nextState);
                else
                    m_gameStateManager.Push(m_nextState);
            }
        }

        public void Draw(GameTime gameTime)
        {
            m_spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);

            Rectangle rect = new Rectangle(0, 0, m_graphicsManager.Width, m_graphicsManager.Height);
            m_spriteBatch.Draw(m_displayedImage, rect, m_tintColor);

            m_spriteBatch.End();
        }

        public void Deactivated()
        { }

        public void UnloadContent()
        {
            m_resourceManager.UnloadTexture(this, m_key);
            m_graphicsManager.UnloadSpriteBatch(m_spriteBatch);

            m_displayedImage = null;
            m_spriteBatch = null;            
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            m_resourceManager = null;
            m_graphicsManager = null;
            m_gameStateManager = null;
        }

        #endregion
    }
}
