using System;
using ChessMatchStateSubsystem.Enums;
using ChessMatchStateSubsystem.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Match
{
    internal abstract class ChessPiece
    {
        private readonly ChessMatchState m_chessGameState;
        private readonly ChessPlayer m_color;
        private readonly Chessboard m_chessboard;
        private int m_currentRow;
        private int m_currentColumn;
        private bool m_selected;
        private bool m_firstMove;

        protected ChessPiece(ChessMatchState chessGameState, ChessPlayer color, Chessboard chessboard)
        {
            m_chessGameState = chessGameState;
            m_currentRow = -1;
            m_currentColumn = -1;
            m_color = color;
            m_chessboard = chessboard;
            m_selected = false;
            m_firstMove = true;
        }

        protected ChessMatchState ChessMatchState
        {
            get { return m_chessGameState; }
        }

        protected Chessboard Chessboard
        {
            get { return m_chessboard; }
        }

        public int CurrentRow
        {
            get { return m_currentRow; }
        }

        public int CurrentColumn
        {
            get { return m_currentColumn; }
        }

        public ChessPlayer Color
        {
            get { return m_color; }
        }

        public bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }

        public bool FirstMove
        {
            get { return m_firstMove; }
            set
            {
                if(value)
                    throw new ArgumentException("Cannot set first move to true");
                m_firstMove = false;
            }
        }

        public void OnBeingMoved(int toRow, int toColumn)
        {
            m_currentRow = toRow;
            m_currentColumn = toColumn;
        }

        public void Draw(GameTime gameTime)
        {
            //Get the stuff
            Model pawnModel = GetModel();
            Matrix translation = GetTransformationMatrix();
            Effect phong = ChessMatchState.Phong;

            //Setup the phong 
            if(!Selected)
                phong.Parameters["ColorTexture"].SetValue(GetTexture());
            else
                phong.Parameters["ColorTexture"].SetValue(ChessMatchState.SelectedPiecesTexture);
            phong.Parameters["NormalTexture"].SetValue(ChessMatchState.PiecesBumpMap);

            //Draw
            ChessMatchState.GraphicsManager.DrawModel(pawnModel, ref translation, phong);
        }

        protected abstract Model GetModel();
        protected abstract Texture2D GetTexture();
        protected abstract Matrix GetTransformationMatrix();
        public abstract bool CanMove(int toRow, int toColumn);
    }
}