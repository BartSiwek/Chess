using AudioSubsystem;
using ChessMatchStateSubsystem.Enums;
using ChessMatchStateSubsystem.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ChessMatchStateSubsystem.Match.States
{
    internal class WhitePieceMoving : IMatchState
    {
        #region Storage

        private static WhitePieceMoving s_instance;

        private ChessMatchState m_owner;

        #endregion

        #region Construction

        private WhitePieceMoving(ChessMatchState owner)
        {
            m_owner = owner;
        }

        public static WhitePieceMoving GetMatchState(ChessMatchState owner)
        {
            if(s_instance == null)
            {
                s_instance = new WhitePieceMoving(owner);
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
            if (m_owner.InputManager.IsLeftMouseButtonDown())
            {
                Vector2 mousePosition = m_owner.InputManager.GetMousePosition();
                if (m_owner.GraphicsManager.IsMouseIn(mousePosition))
                {
                    ChessboardTile pickedTile = ColorChessboardPicker.GetTile(mousePosition,
                                                                              m_owner.GraphicsManager,
                                                                              m_owner.ChessboardModel,
                                                                              m_owner.ChessboardPickingEffect);

                    if (pickedTile == ChessboardTile.None)
                        return this;

                    if (m_owner.Match.IsUndo(pickedTile))
                    {
                        m_owner.AudioManager.PlaySound(SoundKey.PieceDown);
                        return WhitePiecePicking.GetMatchState(m_owner);
                    }

                    if (m_owner.Match.TryMove(ChessPlayer.White, pickedTile))
                    {
                        m_owner.AudioManager.PlaySound(SoundKey.PieceDown);

                        ChessPlayer? winningPlayer = m_owner.Match.WinningPlayer;
                        if(winningPlayer != null)
                        {
                            if(winningPlayer == ChessPlayer.White)
                                return WhiteWon.GetMatchState(m_owner);
                            if (winningPlayer == ChessPlayer.Black)
                                return BlackWon.GetMatchState(m_owner);
                        }

                        return WhiteToBlackTransfer.GetMatchState(m_owner);
                    }

                    m_owner.AudioManager.PlaySound(SoundKey.Error);
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
