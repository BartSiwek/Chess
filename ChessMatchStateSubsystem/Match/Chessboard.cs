using ChessMatchStateSubsystem.Enums;
using ChessMatchStateSubsystem.Utilities;
using Microsoft.Xna.Framework;

namespace ChessMatchStateSubsystem.Match
{
    internal class Chessboard
    {
        #region Storage

        private const int s_boardSize = 8;

        private readonly ChessPiece[,] m_board;

        #endregion

        #region Construction

        public Chessboard()
        {
            m_board = new ChessPiece[s_boardSize, s_boardSize];
        }

        #endregion

        #region Properties

        public ChessPiece this[ChessboardTile tile]
        {
            get
            {
                int row, column;
                ChessboardTileHelper.TileToIndexes(tile, out row, out column);
                return this[row, column];
            }
            set
            {
                int row, column;
                ChessboardTileHelper.TileToIndexes(tile, out row, out column);
                this[row, column] = value;
            }
        }

        public ChessPiece this[int row, int column]
        {
            get
            {
                return m_board[row, column];
            }
            set
            {
                m_board[row, column] = value;
                if(m_board[row, column] != null)
                    m_board[row, column].OnBeingMoved(row, column);
            }
        }

        #endregion

        #region Methods

        public void Draw(GameTime gameTime)
        {
            for(int i = 0;i < s_boardSize;++i)
            {
                for(int j = 0;j < s_boardSize;++j)
                {
                    if(m_board[i, j] != null)
                        m_board[i, j].Draw(gameTime);
                }
            }
        }

        public void Clean()
        {
            for(int i = 0;i < s_boardSize;++i)
            {
                for(int j = 0;j < s_boardSize;++j)
                {
                    m_board[i, j] = null;
                }
            }
        }

        #endregion
    }
}