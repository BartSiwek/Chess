using GraphicsSubsystem;
using Microsoft.Xna.Framework;

namespace GameStateManagmentSubsystem
{
    public class GameStateManager : DrawableGameComponent, IGameStateManager
    {
        #region Storage

        private readonly GameStateStack m_stateStack;
        private IGraphicsManager m_graphicsManager;

        #endregion

        #region Construction

        public GameStateManager(Game game) 
            : base(game)
        {
            m_stateStack = new GameStateStack();
        }

        #endregion

        #region IGameComponent interface

        public override void Initialize()
        {
            base.Initialize();

            m_graphicsManager = (IGraphicsManager)Game.Services.GetService(typeof (IGraphicsManager));
        }

        #endregion

        #region IGameStateManager Members

        public void Push(IGameState state)
        {
            m_stateStack.Top.Deactivated();
            m_stateStack.Push(state);
            m_stateStack.Top.LoadContent();
            m_stateStack.Top.Activated();
        }

        public void Change(IGameState state)
        {
            //Deactivate the top state
            if(m_stateStack.Count != 0)
                m_stateStack.Top.Deactivated();

            //Dispose of all the states
            while(m_stateStack.Count > 0)
            {
                IGameState topState = m_stateStack.Pop();
                topState.UnloadContent();
                topState.Dispose();
            }

            //Put a new state on top and activate it
            if(state != null)
            {
                m_stateStack.Push(state);
                m_stateStack.Top.LoadContent();
                m_stateStack.Top.Activated();
            }
            else
            {
                Game.Exit();
            }
        }

        public void Pop()
        {
            //Deactivate and dispose the top state
            IGameState topState = m_stateStack.Pop();
            topState.Deactivated();
            topState.UnloadContent();
            topState.Dispose();

            //Activate the new top state
            m_stateStack.Top.Activated();
        }

        public IGameState Top
        {
            get 
            {
                return m_stateStack.Top;
            }
        }

        public int StateCount
        {
            get
            {
                return m_stateStack.Count;
            }
        }

        public IGameState this[int index]
        {
            get
            {
                return m_stateStack[index];
            }
        }

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            if(m_stateStack.Count != 0)
                m_stateStack.Top.Deactivated();

            while(m_stateStack.Count > 0)
            {
                IGameState topState = m_stateStack.Pop();
                topState.UnloadContent();
                topState.Dispose();
            }
        }

        #endregion

        #region IUpdateable Members

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(m_stateStack.Count != 0)
                m_stateStack.Top.Update(gameTime);
        }

        #endregion

        #region IDrawable Members

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if(m_stateStack.Count != 0)
            {
                m_graphicsManager.PrepareBackBuffer();
                m_stateStack.Top.Draw(gameTime);
                m_graphicsManager.ShowBackBuffer();
            }
        }

        #endregion
    }
}