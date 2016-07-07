using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BattleCitySummer
{
    public class ActionScreen : GameScreen
    {
        public MainGame game = new MainGame();

        public override void LoadContent()
        {
            this.game.Init();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.game.Update();
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin();
            this.game.Draw(graphics, spriteBatch);
            spriteBatch.End();
        }
    }
}
