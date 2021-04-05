using System;
using ChessMatchStateSubsystem.Enums;

namespace ChessMatchStateSubsystem.Utilities
{
    internal static class ChessboardTileHelper
    {
        public const int BoardSize = 8;

        public static void TileToIndexes(ChessboardTile tile, out int row, out int column)
        {
            if (tile == ChessboardTile.None)
                throw new ArgumentException("The tile cannot be None");

            row = BoardSize - ((int)tile / BoardSize) - 1;
            column = (int)tile % BoardSize;
        }

        public static ChessboardTile IndexesToTile(int row, int column)
        {
            return (ChessboardTile) (row*BoardSize + column);
        }
    }
}
