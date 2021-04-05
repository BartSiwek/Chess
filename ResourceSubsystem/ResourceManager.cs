using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ResourceSubsystem.Properties;

namespace ResourceSubsystem
{
    internal class ResourceManager : GameComponent, IResourceManager
    {
        private const string s_texturesContentKey = @"Textures";
        private const string s_modelsContentKey = @"Models";
        private const string s_fontsContentKey = @"Fonts";
        private const string s_effectsContentKey = @"Effects";

        private Dictionary<GameTextureKey, Dictionary<object, Texture2D>> m_textures;
        private Dictionary<GameModelKey, Model> m_models;
        private Dictionary<GameFontKey, SpriteFont> m_fonts;
        private Dictionary<GameEffectKey, Dictionary<object, Effect>> m_effects;
        private ContentManager m_contentManager;
        private bool disposed;

        public ResourceManager(Game game) : base(game)
        {
            m_textures = new Dictionary<GameTextureKey, Dictionary<object, Texture2D>>();
            m_models = new Dictionary<GameModelKey, Model>();
            m_fonts = new Dictionary<GameFontKey, SpriteFont>();
            m_effects = new Dictionary<GameEffectKey, Dictionary<object, Effect>>();

            m_contentManager = game.Content;
            disposed = false;
        }

        public Texture2D GetTexture(object owner, GameTextureKey textureKey)
        {
            CheckDisposed();

            if (!m_textures.ContainsKey(textureKey))
            {
                Texture2D texture = LoadTexture(textureKey);
                m_textures.Add(textureKey, new Dictionary<object, Texture2D>());
                m_textures[textureKey].Add(owner, texture);
            }
            else if(!m_textures[textureKey].ContainsKey(owner))
            {
                Texture2D texture = LoadTexture(textureKey);
                m_textures[textureKey].Add(owner, texture);
            }
            return m_textures[textureKey][owner];
        }

        public Model GetModel(GameModelKey modelKey)
        {
            CheckDisposed();

            if (!m_models.ContainsKey(modelKey))
            {
                Model model = LoadModel(modelKey);
                m_models.Add(modelKey, model);
            }
            return m_models[modelKey];
        }

        public SpriteFont GetFont(GameFontKey fontKey)
        {
            CheckDisposed();

            if (!m_fonts.ContainsKey(fontKey))
            {
                SpriteFont font = LoadFont(fontKey);
                m_fonts.Add(fontKey, font);
            }
            return m_fonts[fontKey];
        }

        public Effect GetEffect(object owner, GameEffectKey effectKey)
        {
            CheckDisposed();

            if (!m_effects.ContainsKey(effectKey))
            {
                Effect effect = LoadEffect(effectKey);
                m_effects.Add(effectKey, new Dictionary<object, Effect>());
                m_effects[effectKey].Add(owner, effect);
            }
            else if(!m_effects[effectKey].ContainsKey(owner))
            {
                Effect effect = LoadEffect(effectKey);
                m_effects[effectKey].Add(owner, effect);
            }
            return m_effects[effectKey][owner];
        }

        public void UnloadTexture(object owner, GameTextureKey textureKey)
        {
            if(m_textures == null)
                return;
            if (m_textures.ContainsKey(textureKey))
            {
                if (m_textures[textureKey].ContainsKey(owner))
                    m_textures[textureKey].Remove(owner);

                if (m_textures[textureKey].Count == 0)
                    m_textures.Remove(textureKey);
            }
        }

        public void UnloadEffect(object owner, GameEffectKey effectKey)
        {
            if (m_effects == null)
                return;
            if (m_effects.ContainsKey(effectKey))
            {
                if (m_effects[effectKey].ContainsKey(owner))
                    m_effects[effectKey].Remove(owner);
                if (m_effects[effectKey].Count == 0)
                    m_effects.Remove(effectKey);                
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                foreach (Dictionary<object, Texture2D> textureEntry in m_textures.Values)
                {
                    foreach (Texture2D texture in textureEntry.Values)
                    {
                        texture.Dispose();
                    }
                }

                foreach (Dictionary<object, Effect> effectEntry in m_effects.Values)
                {
                    foreach (Effect effect in effectEntry.Values)
                    {
                        effect.Dispose();
                    }
                }

                m_textures = null;
                m_models = null;
                m_fonts = null;
                m_effects = null;
                m_contentManager = null;

                disposed = true;
            }
        }

        private Texture2D LoadTexture(GameTextureKey textureKey)
        {
            string resourceName = string.Format("{0}/{1}", s_texturesContentKey, textureKey);
            Texture2D texture = m_contentManager.Load<Texture2D>(resourceName);
            return texture;
        }

        private Model LoadModel(GameModelKey modelKey)
        {
            string resourceName = string.Format("{0}/{1}", s_modelsContentKey, modelKey);
            Model model = m_contentManager.Load<Model>(resourceName);
            return model;
        }

        private SpriteFont LoadFont(GameFontKey fontKey)
        {
            string resourceName = string.Format("{0}/{1}", s_fontsContentKey, fontKey);
            SpriteFont font = m_contentManager.Load<SpriteFont>(resourceName);
            return font;
        }

        private Effect LoadEffect(GameEffectKey effectKey)
        {
            string resourceName = string.Format("{0}/{1}", s_effectsContentKey, effectKey);
            Effect effect = m_contentManager.Load<Effect>(resourceName);
            return effect;
        }

        private void CheckDisposed()
        {
            if(disposed)
                throw new ObjectDisposedException("The resource manager has been disposed");
        }
    }
}