using Microsoft.Xna.Framework;

namespace ChessMatchStateSubsystem.Match.States
{
    internal class BlackToWhiteTransfer : IMatchState
    {
        #region Storage

        private const string s_transferMessage = "LIGHT player, it's Your turn.";
        private static BlackToWhiteTransfer s_instance;

        private ChessMatchState m_owner;

        #endregion

        #region Construction

        private BlackToWhiteTransfer(ChessMatchState owner)
        {
            m_owner = owner;
        }

        public static BlackToWhiteTransfer GetMatchState(ChessMatchState owner)
        {
            if(s_instance == null)
            {
                s_instance = new BlackToWhiteTransfer(owner);
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
            if (!m_owner.GraphicsManager.MoveToWhiteViewport(gameTime))
                return this;
            return WhitePiecePicking.GetMatchState(m_owner);
        }

        public void Draw(GameTime gameTime)
        {
            m_owner.GraphicsManager.DrawStringOnScreen(s_transferMessage, m_owner.MessageFont);
        }

        #endregion
    }
}
