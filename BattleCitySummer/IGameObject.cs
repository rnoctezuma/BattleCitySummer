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
        void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch);
        void Update(MainGame mainGame, GameTime gameTime);
        void Destroy();
        bool isDestroyed();
    }
}
