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
        public Texture2D texture;
        public MainGame game = new MainGame();
        public Map map = new Map();

        public override void LoadContent()
        {
            base.LoadContent();
            this.game.Init();
            texture = content.Load<Texture2D>("pixel");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
          //  this.game.Update();
            this.map.Update();

        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
         //  spriteBatch.Draw(texture, Vector2.Zero, Color.White);
         //  this.game.Draw(graphics, spriteBatch, texture);
            this.map.Draw(graphics, spriteBatch, texture);
        }
    }
}
