using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ResourceSubsystem
{
    public interface IResourceManager : IGameComponent, IDisposable, IUpdateable
    {
        //Loading routines
        Texture2D GetTexture(object owner, GameTextureKey textureKey);
        Model GetModel(GameModelKey modelKey);
        SpriteFont GetFont(GameFontKey fontKey);
        Effect GetEffect(object owner, GameEffectKey effectKey);

        //Unloading routines
        void UnloadTexture(object owner, GameTextureKey textureKey);
        void UnloadEffect(object owner, GameEffectKey effectKey);
    }
}
