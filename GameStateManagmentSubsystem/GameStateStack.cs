using System.Collections.Generic;

namespace GameStateManagmentSubsystem
{
    class GameStateStack
    {
        #region Storage

        private readonly List<IGameState> m_stack;

        #endregion

        #region Construction

        public GameStateStack()
        {
            m_stack = new List<IGameState>();
        }

        #endregion

        #region Properties

        public int Count
        {
            get
            {
                return m_stack.Count;
            }
        }

        public IGameState Top
        {
            get
            {
                return m_stack[m_stack.Count - 1];
            }
        }

        public IGameState this[int index]
        {
            get
            {
                return m_stack[m_stack.Count - index - 1];
            }
        }

        #endregion

        #region Methods

        public IGameState Pop()
        {
            IGameState topState = m_stack[m_stack.Count - 1];
            m_stack.RemoveAt(m_stack.Count - 1);
            return topState;
        }

        public void Push(IGameState state)
        {
            m_stack.Add(state);
        }

        #endregion
    }
}
