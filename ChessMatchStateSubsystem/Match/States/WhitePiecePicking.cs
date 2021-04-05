using AudioSubsystem;
using ChessMatchStateSubsystem.Enums;
using ChessMatchStateSubsystem.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessMatchStateSubsystem.Match.States
{
    internal class WhitePiecePicking : IMatchState
    {
        #region Storage

        private static WhitePiecePicking s_instance;

        private ChessMatchState m_owner;

        #endregion

        #region Construction

        private WhitePiecePicking(ChessMatchState owner)
        {
            m_owner = owner;
        }

        public static WhitePiecePicking GetMatchState(ChessMatchState owner)
        {
            if(s_instance == null)
            {
                s_instance = new WhitePiecePicking(owner);
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
            //Check for camera movement and exit
            if (m_owner.InputManager.IsKeyDown(Keys.Escape))
                m_owner.ExitMatch();
            if (m_owner.InputManager.IsKeyHoldDown(Keys.Left))
                m_owner.GraphicsManager.CameraLeft(gameTime);
            if (m_owner.InputManager.IsKeyHoldDown(Keys.Right))
                m_owner.GraphicsManager.CameraRight(gameTime);
            if (m_owner.InputManager.IsKeyHoldDown(Keys.Up))
                m_owner.GraphicsManager.CameraUp(gameTime);
            if (m_owner.InputManager.IsKeyHoldDown(Keys.Down))
                m_owner.GraphicsManager.CameraDown(gameTime);

            //Check for a click
            if(m_owner.InputManager.IsLeftMouseButtonDown())
            {
                Vector2 mousePosition = m_owner.InputManager.GetMousePosition();
                if(m_owner.GraphicsManager.IsMouseIn(mousePosition))
                {
                    ChessboardTile pickedTile = ColorChessboardPicker.GetTile(mousePosition,
                                                                              m_owner.GraphicsManager,
                                                                              m_owner.ChessboardModel,
                                                                              m_owner.ChessboardPickingEffect);

                    if(pickedTile == ChessboardTile.None)
                        return this;

                    if (m_owner.Match.TrySelect(ChessPlayer.White, pickedTile))
                    {
                        m_owner.AudioManager.PlaySound(SoundKey.PieceUp);
                        return WhitePieceMoving.GetMatchState(m_owner);
                    }
                }
            }

            //If not returned yet return yourself
            return this;
        }

        public void Draw(GameTime gameTime)
        { }

        #endregion
    }
}