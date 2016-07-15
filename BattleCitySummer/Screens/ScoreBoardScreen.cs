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
    public class ScoreBoardScreen : GameScreen
    {
        public int score = 0;
        public SpriteFont spriteFont;
        
        public ScoreBoardScreen (int score)
        {
            this.score = score;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            spriteFont = content.Load<SpriteFont>("basicFont");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Back))
            {
                BackToMenu();
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.DrawString(spriteFont, "TOTAL SCORE: " + this.score.ToString(), new Vector2 (100, 200), Color.White);
            spriteBatch.DrawString(spriteFont, "PRESS \"BACKSPACE\" TO MENU", new Vector2(100, 300), Color.White);
        }

        private void BackToMenu()
        {
            ScreenManager.Instance.ChangeScreen(new MenuScreen());
            this.content.Dispose();
        }
    }
}
