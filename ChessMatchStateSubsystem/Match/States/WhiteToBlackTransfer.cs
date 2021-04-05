using Microsoft.Xna.Framework;

namespace ChessMatchStateSubsystem.Match.States
{
    internal class WhiteToBlackTransfer : IMatchState
    {
        #region Storage

        private const string s_transferMessage = "DARK player, it's Your turn.";
        private static WhiteToBlackTransfer s_instance;

        private ChessMatchState m_owner;

        #endregion

        #region Construction

        private WhiteToBlackTransfer(ChessMatchState owner)
        {
            m_owner = owner;
        }

        public static WhiteToBlackTransfer GetMatchState(ChessMatchState owner)
        {
            if(s_instance == null)
            {
                s_instance = new WhiteToBlackTransfer(owner);
            }
            else
            {
                s_instance.m_owner = owner;
            }

            return s_instance;
        }

        #endregion

        #region IMatchState Members

        public IMatchState Update(GameTime gameTime)
        {
            if (!m_owner.GraphicsManager.MoveToBlackViewport(gameTime))
                return this;
            return BlackPiecePicking.GetMatchState(m_owner);
        }

        public void Draw(GameTime gameTime)
        {
            m_owner.GraphicsManager.DrawStringOnScreen(s_transferMessage, m_owner.MessageFont);
        }

        #endregion
    }
}
