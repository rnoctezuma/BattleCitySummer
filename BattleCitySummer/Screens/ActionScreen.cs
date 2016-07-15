using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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
            this.game.Sprites.Add(content.Load<Texture2D>("SpritePlayer"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteBullet"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteEnemy"));
            this.game.Sprites.Add(content.Load<Texture2D>("BrickWall"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteBase"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteExlosion1"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteExlosion2"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteIronWall"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteSpawnPlayer"));
            this.game.Sprites.Add(content.Load<Texture2D>("SpriteSpawnEnemy"));
            this.game.Init();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {           
            this.game.Update(gameTime);
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Back))
            {
                BackToMenu();
            }

            if (this.game.isGameover)
            {
                ToScoreBoardScreen(this.game.player.score);
            }

        }

        public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
               this.game.Draw(graphics, spriteBatch, texture);
        }

        private void BackToMenu()
        {
            ScreenManager.Instance.ChangeScreen(new MenuScreen());
            this.content.Dispose();
        }

        private void ToScoreBoardScreen(int score)
        {
            ScreenManager.Instance.ChangeScreen(new ScoreBoardScreen(score));
            this.content.Dispose();
        }
    }
}
