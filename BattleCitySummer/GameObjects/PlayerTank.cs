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
    public class PlayerTank : IGameObject 
    {
        public Box box { get; set; }
        public Texture2D Sprite { get; set; }
        public int health { get; set; }
        public double shot = 21;
        public bool destroy = false;
        private int frameWidth = 16;
        private int frameHeight = 16;
        private Point currentFrame = new Point(0, 0);
        private Point spriteSize = new Point(8, 1);
        private double animation = 0;
        public int score = 0;

        public double pos; //направление взгляда

        public PlayerTank(MainGame F, int x, int y, Texture2D Sprite)
        {
            this.box = new Box(x, y, 16, 16, 0, 0, false);
            this.health = 3;
            this.pos = 3.0 / 2.0 * Math.PI;
            F.Boxes.Add(this.box);
            this.Sprite = Sprite;
        }

        public void Destroy()
        {
            destroy = true;
            this.box.destroy = true;
        }

        public bool isDestroyed()
        {
            return destroy;
        }


        public void Update(MainGame mainGame, GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            this.box.vx = 0;
            this.box.vy = 0;

            if (keyboardState.IsKeyDown(Keys.Left))  //motion
            {
                this.box.vx = -1.5;
                this.pos = Math.PI;
                currentFrame.X = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.box.vx = 1.5;
                this.pos = 0;
                currentFrame.X = 3;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.box.vy = -1.5;
                this.pos = 3.0 / 2.0 * Math.PI;
                currentFrame.X = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.box.vy = 1.5;
                this.pos = 1.0 / 2.0 * Math.PI;
                currentFrame.X = 2;
            }
            else if (keyboardState.IsKeyDown(Keys.LeftControl))
            { 
                if (this.shot > 20)
                {
                    this.Shot(mainGame);             
                    this.shot = 0;
                }
            }
            this.shot += 0.15;
            if (Math.Abs(this.box.vx)>0 || Math.Abs(this.box.vy) > 0)
                 this.animation += 0.1;
            if (this.animation >= 2)
                this.animation = 0;
            if (health < 0)
            {
                this.Destroy();
                mainGame.isGameover = true;
            }    
        }

        public void Shot(MainGame mainGame)
        {
            if (pos == 0)
                mainGame.GameObjects.Add(new Bullet(mainGame, this.box.x + this.box.width + 4, this.box.y-1, Math.Cos(pos)  * 5, Math.Sin(pos) * 5, this, mainGame.Sprites[1]));
            if (pos == Math.PI)
                mainGame.GameObjects.Add(new Bullet(mainGame, this.box.x - this.box.width - 4, this.box.y, Math.Cos(pos) * 5, Math.Sin(pos) * 5, this, mainGame.Sprites[1]));
            if (pos == 3.0 / 2.0 * Math.PI)
                mainGame.GameObjects.Add(new Bullet(mainGame, this.box.x, this.box.y - this.box.height - 4, Math.Cos(pos) * 5, Math.Sin(pos) * 5, this, mainGame.Sprites[1]));
            if (pos == 1.0 / 2.0 * Math.PI)
                mainGame.GameObjects.Add(new Bullet(mainGame, this.box.x-2, this.box.y + this.box.height + 2, Math.Cos(pos) * 5, Math.Sin(pos) * 5, this, mainGame.Sprites[1]));
        }

        public void newPlayerSpawn()
        {
            this.box.x = 210;
            this.box.y = 535;
        }

        public void addScore()
        {
            this.score += 100;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2 (((int)this.box.x - (int)this.box.width), (int)this.box.y - (int)this.box.height),
                new Rectangle((currentFrame.X * 2 + (int)animation) * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                2, SpriteEffects.None, 0);
        }
    }
}
