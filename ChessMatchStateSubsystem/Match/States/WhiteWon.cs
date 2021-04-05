using Microsoft.Xna.Framework;

namespace ChessMatchStateSubsystem.Match.States
{
    internal class WhiteWon : IMatchState
    {
        #region Storage

        private const string s_message = "Congratulations LIGHT player!";
        private const double s_messageShowingTime = 15.0;

        private static WhiteWon s_instance;

        private ChessMatchState m_owner;
        private double m_timePassed;

        #endregion

        #region Construction

        private WhiteWon(ChessMatchState owner)
        {
            m_owner = owner;
            m_timePassed = 0;
        }

        public static WhiteWon GetMatchState(ChessMatchState owner)
        {
            if(s_instance == null)
            {
                s_instance = new WhiteWon(owner);
            }
            else
            {
                s_instance.m_owner = owner;
                s_instance.m_timePassed = 0;
            }

            return s_instance;
        }

        #endregion

        #region IMatchState Members

        public IMatchState Update(GameTime gameTime)
        {
            m_timePassed += gameTime.ElapsedRealTime.TotalSeconds;

            if(m_timePassed >= s_messageShowingTime)
                m_owner.ExitMatch();

            return this;
        }

        public void Draw(GameTime gameTime)
        {
            m_owner.GraphicsManager.DrawStringOnScreen(s_message, m_owner.MessageFont);
        }

        #endregion
    }
}
