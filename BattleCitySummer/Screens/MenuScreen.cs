using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class MenuScreen : GameScreen
    {

        public Texture2D menuSprite;
        public override void LoadContent()
        {
            base.LoadContent();
            menuSprite = content.Load<Texture2D>("SpriteMenu");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.P))
            {
                ToActionScreen();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Draw(menuSprite, Vector2.Zero,
                Color.White);
        }

        private void ToActionScreen()
        {
            ScreenManager.Instance.ChangeScreen(new ActionScreen());
            this.content.Dispose();
        }
    }
}
