using Microsoft.Xna.Framework;

namespace ChessMatchStateSubsystem.Match.States
{
    internal class BlackWon : IMatchState
    {
        #region Storage

        private const string s_message = "Congratulatisons DARK player!";
        private const double s_messageShowingTime = 15.0;

        private static BlackWon s_instance;

        private ChessMatchState m_owner;
        private double m_timePassed;

        #endregion

        #region Construction

        private BlackWon(ChessMatchState owner)
        {
            m_owner = owner;
            m_timePassed = 0;
        }

        public static BlackWon GetMatchState(ChessMatchState owner)
        {
            if (s_instance == null)
            {
                s_instance = new BlackWon(owner);
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

            if (m_timePassed >= s_messageShowingTime)
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
