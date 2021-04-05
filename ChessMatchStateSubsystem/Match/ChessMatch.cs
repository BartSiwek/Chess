using System.Diagnostics;
using ChessMatchStateSubsystem.Enums;
using ChessMatchStateSubsystem.Match.Pieces;
using ChessMatchStateSubsystem.Utilities;
using Microsoft.Xna.Framework;

namespace ChessMatchStateSubsystem.Match
{
    class ChessMatch
    {
        private readonly ChessMatchState m_owner;
        private readonly Chessboard m_board;
        private WhiteKing m_whiteKing;
        private BlackKing m_blackKing;
        private int m_selectedRow;
        private int m_selectedColumn;
        private ChessPlayer? m_winningPlayer;

        public ChessMatch(ChessMatchState owner)
        {
            m_owner = owner;
            m_board = new Chessboard();
            m_whiteKing = null;
            m_blackKing = null;
            m_winningPlayer = null;
        }

        public ChessPlayer? WinningPlayer
        {
            get
            {
                return m_winningPlayer;
            }
        }

        public void SetupGame()
        {
            m_board.Clean();

            //White pieces
            m_board[ChessboardTile.A2] = new WhitePawn(m_owner, m_board);
            m_board[ChessboardTile.B2] = new WhitePawn(m_owner, m_board);
            m_board[ChessboardTile.C2] = new WhitePawn(m_owner, m_board);
            m_board[ChessboardTile.D2] = new WhitePawn(m_owner, m_board);
            m_board[ChessboardTile.E2] = new WhitePawn(m_owner, m_board);
            m_board[ChessboardTile.F2] = new WhitePawn(m_owner, m_board);
            m_board[ChessboardTile.G2] = new WhitePawn(m_owner, m_board);
            m_board[ChessboardTile.H2] = new WhitePawn(m_owner, m_board);

            m_board[ChessboardTile.A1] = new WhiteRook(m_owner, m_board);
            m_board[ChessboardTile.H1] = new WhiteRook(m_owner, m_board);

            m_board[ChessboardTile.B1] = new WhiteKnight(m_owner, m_board);
            m_board[ChessboardTile.G1] = new WhiteKnight(m_owner, m_board);

            m_board[ChessboardTile.C1] = new WhiteBishop(m_owner, m_board);
            m_board[ChessboardTile.F1] = new WhiteBishop(m_owner, m_board);

            m_board[ChessboardTile.D1] = new WhiteQueen(m_owner, m_board);

            m_whiteKing = new WhiteKing(m_owner, m_board);
            m_board[ChessboardTile.E1] = m_whiteKing;

            //Black pieces
            m_board[ChessboardTile.A7] = new BlackPawn(m_owner, m_board);
            m_board[ChessboardTile.B7] = new BlackPawn(m_owner, m_board);
            m_board[ChessboardTile.C7] = new BlackPawn(m_owner, m_board);
            m_board[ChessboardTile.D7] = new BlackPawn(m_owner, m_board);
            m_board[ChessboardTile.E7] = new BlackPawn(m_owner, m_board);
            m_board[ChessboardTile.F7] = new BlackPawn(m_owner, m_board);
            m_board[ChessboardTile.G7] = new BlackPawn(m_owner, m_board);
            m_board[ChessboardTile.H7] = new BlackPawn(m_owner, m_board);

            m_board[ChessboardTile.A8] = new BlackRook(m_owner, m_board);
            m_board[ChessboardTile.H8] = new BlackRook(m_owner, m_board);

            m_board[ChessboardTile.B8] = new BlackKnight(m_owner, m_board);
            m_board[ChessboardTile.G8] = new BlackKnight(m_owner, m_board);

            m_board[ChessboardTile.C8] = new BlackBishop(m_owner, m_board);
            m_board[ChessboardTile.F8] = new BlackBishop(m_owner, m_board);

            m_board[ChessboardTile.D8] = new BlackQueen(m_owner, m_board);

            m_blackKing = new BlackKing(m_owner, m_board);
            m_board[ChessboardTile.E8] = m_blackKing;

            //Set no winner
            m_winningPlayer = null;
        }

        public void Draw(GameTime gameTime)
        {
            m_board.Draw(gameTime);
        }

        public bool TrySelect(ChessPlayer player, ChessboardTile tile)
        {
            int row, column;
            ChessboardTileHelper.TileToIndexes(tile, out row, out column);

            if (m_board[row, column] != null && m_board[row, column].Color == player)
            {
                m_board[row, column].Selected = true;
                m_selectedRow = row;
                m_selectedColumn = column;
                return true;
            }

            return false;
        }

        public bool TryMove(ChessPlayer player, ChessboardTile tile)
        {
            int row, column;
            ChessboardTileHelper.TileToIndexes(tile, out row, out column);

            //Normal move
            if(m_board[m_selectedRow, m_selectedColumn].CanMove(row, column))
            {
                //Pick up the piece
                ChessPiece movingPiece = m_board[m_selectedRow, m_selectedColumn];
                m_board[m_selectedRow, m_selectedColumn] = null;
                m_selectedRow = -1;
                m_selectedColumn = -1;

                //Update the piece
                movingPiece.Selected = false;
                movingPiece.FirstMove = false;

                //Check for winning/losing
                if (row == m_whiteKing.CurrentRow && column == m_whiteKing.CurrentColumn)
                    m_winningPlayer = ChessPlayer.Black;
                if (row == m_blackKing.CurrentRow && column == m_blackKing.CurrentColumn)
                    m_winningPlayer = ChessPlayer.White;

                //Check for rochade (white, kingside)
                if (movingPiece == m_whiteKing && row == 0 && column == 6)
                {
                    WhiteRook rookSpot = m_board[0, 7] as WhiteRook;
                    if (rookSpot != null && rookSpot.FirstMove)
                    {
                        //We can do the rochade
                        m_board[0, 7] = movingPiece;
                        m_board[0, 6] = rookSpot;
                        rookSpot.FirstMove = false;
                    }
                }

                //Check for rochade (black, queenside)
                if (movingPiece == m_blackKing && row == 7 && column == 1)
                {
                    BlackRook rookSpot = m_board[7, 0] as BlackRook;
                    if (rookSpot != null && rookSpot.FirstMove)
                    {
                        //We can do the rochade
                        m_board[7, 0] = movingPiece;
                        m_board[7, 1] = rookSpot;
                        rookSpot.FirstMove = false;
                    }
                }

                //Check for rochade (black, kingside)
                if (movingPiece == m_blackKing && row == 7 && column == 6)
                {
                    BlackRook rookSpot = m_board[7, 7] as BlackRook;
                    if (rookSpot != null && rookSpot.FirstMove)
                    {
                        //We can do the rochade
                        m_board[7, 7] = movingPiece;
                        m_board[7, 6] = rookSpot;
                        rookSpot.FirstMove = false;
                    }
                }

                //Put down the piece
                m_board[row, column] = movingPiece;

                //Clear selection

                return true;
            }

            //Rochade (white, queenside)
            if(m_board[m_selectedRow, m_selectedColumn] is WhiteKing &&
               m_whiteKing.FirstMove &&
               row == 0 &&
               column == 1 &&
               m_board[0, 1] == null &&
               m_board[0, 2] == null &&
               m_board[0, 3] == null)
            {
                WhiteRook rookSpot = m_board[0, 0] as WhiteRook;
                if (rookSpot != null && rookSpot.FirstMove)
                {
                    //Pickup the king
                    m_board[m_selectedRow, m_selectedColumn] = null;

                    //Deselect him
                    m_selectedRow = -1;
                    m_selectedColumn = -1;
                    m_whiteKing.Selected = false;

                    //Move
                    m_board[0, 1] = m_whiteKing;
                    m_board[0, 2] = rookSpot;

                    //Mark first moves
                    m_whiteKing.FirstMove = false;
                    rookSpot.FirstMove = false;

                    //Teel the move was a success
                    return true;
                }                
            }

            //Rochade (white, kingside)
            if (m_board[m_selectedRow, m_selectedColumn] is WhiteKing &&
                m_whiteKing.FirstMove &&
                row == 0 &&
                column == 6 &&
                m_board[0, 5] == null &&
                m_board[0, 6] == null)
            {
                WhiteRook rookSpot = m_board[0, 7] as WhiteRook;
                if (rookSpot != null && rookSpot.FirstMove)
                {
                    //Pickup the king
                    m_board[m_selectedRow, m_selectedColumn] = null;

                    //Deselect him
                    m_selectedRow = -1;
                    m_selectedColumn = -1;
                    m_whiteKing.Selected = false;

                    //Move
                    m_board[0, 6] = m_whiteKing;
                    m_board[0, 5] = rookSpot;

                    //Mark first moves
                    m_whiteKing.FirstMove = false;
                    rookSpot.FirstMove = false;

                    //Teel the move was a success
                    return true;
                }
            }

            //Rochade (black, queenside)
            if (m_board[m_selectedRow, m_selectedColumn] is BlackKing &&
                m_blackKing.FirstMove &&
                row == 7 &&
                column == 1 &&
                m_board[7, 1] == null &&
                m_board[7, 2] == null &&
                m_board[7, 3] == null)
            {
                BlackRook rookSpot = m_board[7, 0] as BlackRook;
                if (rookSpot != null && rookSpot.FirstMove)
                {
                    //Pickup the king
                    m_board[m_selectedRow, m_selectedColumn] = null;

                    //Deselect him
                    m_selectedRow = -1;
                    m_selectedColumn = -1;
                    m_blackKing.Selected = false;

                    //Move
                    m_board[7, 1] = m_blackKing;
                    m_board[7, 2] = rookSpot;

                    //Mark first moves
                    m_blackKing.FirstMove = false;
                    rookSpot.FirstMove = false;

                    //Teel the move was a success
                    return true;
                }
            }

            //Rochade (black, kingside)
            if (m_board[m_selectedRow, m_selectedColumn] is BlackKing &&
                m_blackKing.FirstMove &&
                row == 7 &&
                column == 6 &&
                m_board[7, 5] == null &&
                m_board[7, 6] == null)
            {
                BlackRook rookSpot = m_board[7, 7] as BlackRook;
                if (rookSpot != null && rookSpot.FirstMove)
                {
                    //Pickup the king
                    m_board[m_selectedRow, m_selectedColumn] = null;

                    //Deselect him
                    m_selectedRow = -1;
                    m_selectedColumn = -1;
                    m_blackKing.Selected = false;

                    //Move
                    m_board[7, 6] = m_blackKing;
                    m_board[7, 5] = rookSpot;

                    //Mark first moves
                    m_blackKing.FirstMove = false;
                    rookSpot.FirstMove = false;

                    //Teel the move was a success
                    return true;
                }
            }

            return false;
        }

        public bool IsUndo(ChessboardTile tile)
        {
            int row, column;
            ChessboardTileHelper.TileToIndexes(tile, out row, out column);

            if(row == m_selectedRow && column == m_selectedColumn)
            {
                m_board[m_selectedRow, m_selectedColumn].Selected = false;
                m_selectedRow = -1;
                m_selectedColumn = -1;
                return true;
            }

            return false;
        }
    }
}