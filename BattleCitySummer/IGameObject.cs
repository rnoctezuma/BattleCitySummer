using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public interface IGameObject
    {
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel);
        void Update(MainGame F);
        void Destroy();
        bool isDestroyed();
    }
}
