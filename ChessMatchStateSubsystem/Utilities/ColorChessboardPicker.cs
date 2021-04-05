using System;
using System.Collections.Generic;
using System.Diagnostics;
using ChessMatchStateSubsystem.Enums;
using GraphicsSubsystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ChessMatchStateSubsystem.Utilities
{
    static class ColorChessboardPicker
    {
        #region Storage

        private const int s_maxAllowedDistance = 3;

        private static readonly Color s_neutralColor = new Color(255, 255, 255);
        private static readonly List<Color> s_colorMap = new List<Color>();
        private static readonly List<ChessboardTile> s_tileMap = new List<ChessboardTile>();

        #endregion

        #region Static construction

        static ColorChessboardPicker()
        {
            //Initialize the map
            for(int i = 0;i < 8;++i)
            {
                for(int j = 0;j < 8;++j)
                {
                    Color color = new Color((byte)(i * 32), (byte)(j * 32), 128);
                    ChessboardTile tile = (ChessboardTile)((i*8) + j);

                    s_colorMap.Add(color);
                    s_tileMap.Add(tile);
                }
            }
        }

        #endregion

        #region Static methods

        public static ChessboardTile GetTile(Vector2 mousePosition, IGraphicsManager graphicsManager,
            Model chessboardModel, Effect chessboardPickingEffect)
        {
            graphicsManager.GraphicsDevice.Clear(s_neutralColor);

            //Get the needed stuff
            Matrix world = Matrix.Identity;

            //Draw the screen and retrive the map
            graphicsManager.DrawModel(chessboardModel, ref world, chessboardPickingEffect);
            Texture2D pickingTexture = graphicsManager.GetScreenContents();
            Color[] pickingMap = new Color[pickingTexture.Width * pickingTexture.Height];
            pickingTexture.GetData(pickingMap);

            //Get the color and a tile
            Color selectedColor = pickingMap[(int)(mousePosition.Y * pickingTexture.Width + mousePosition.X)];
            ChessboardTile tile = GetTileForColor(selectedColor);

            //Return the result
            return tile;
        }

        private static ChessboardTile GetTileForColor(Color color)
        {
            for (int i = 0; i < s_colorMap.Count;++i)
            {
                if (Math.Abs(color.R - s_colorMap[i].R) <= s_maxAllowedDistance &&
                    Math.Abs(color.G - s_colorMap[i].G) <= s_maxAllowedDistance &&
                    Math.Abs(color.B - s_colorMap[i].B) <= s_maxAllowedDistance)
                {
                    return s_tileMap[i];                    
                }
            }
            return ChessboardTile.None;
        }

        #endregion
    }
}