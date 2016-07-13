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
        public double shot = 11;
        public bool destroy = false;
        private int frameWidth = 16;
        private int frameHeight = 16;
        private Point currentFrame = new Point(0, 0);
        private Point spriteSize = new Point(4, 1);

        //  public int score = 0;

        public double pos; //направление взгляда

        public PlayerTank(MainGame F, int x, int y, Texture2D Sprite)
        {
            this.box = new Box(x, y, 16, 16, 0, 0, false);
            this.health = 100;
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

        public void Damage()
        {
            this.Destroy();
        }

        public void Update(MainGame F)
        {
            
            KeyboardState keyboardState = Keyboard.GetState();
            this.box.vx = 0;
            this.box.vy = 0;

            if (keyboardState.IsKeyDown(Keys.Left))  //motion
            {
                this.box.vx -= 3;
                this.pos = Math.PI;
                currentFrame.X = 1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.box.vx += 3;
                this.pos = 0;
                currentFrame.X = 3;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.box.vy -= 3;
                this.pos = 3.0 / 2.0 * Math.PI;
                currentFrame.X = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.box.vy += 3;
                this.pos = 1.0 / 2.0 * Math.PI;
                currentFrame.X = 2;
            }
            else if (keyboardState.IsKeyDown(Keys.LeftControl))
            { 
                if (this.shot > 10)
                {
                    this.Shot(F);             
                    this.shot = 0;
                }
            }
            this.shot += 0.15;
            if (health <= 0)
            {
                F.gameover = true;
            }    
        }

        public void Shot(MainGame F)
        {
            if (pos == 0)
                F.GameObjects.Add(new Bullet(F, this.box.x + this.box.width + 4, this.box.y - 6, Math.Cos(pos)  * 5, Math.Sin(pos) * 5, this, F.Sprites[1]));
            if (pos == Math.PI)
                F.GameObjects.Add(new Bullet(F, this.box.x - this.box.width - 16, this.box.y - 8, Math.Cos(pos) * 5, Math.Sin(pos) * 5, this, F.Sprites[1]));
            if (pos == 3.0 / 2.0 * Math.PI)
                F.GameObjects.Add(new Bullet(F, this.box.x-8, this.box.y - this.box.height - 16, Math.Cos(pos) * 5, Math.Sin(pos) * 5, this, F.Sprites[1]));
            if (pos == 1.0 / 2.0 * Math.PI)
                F.GameObjects.Add(new Bullet(F, this.box.x-6, this.box.y + this.box.height + 4, Math.Cos(pos) * 5, Math.Sin(pos) * 5, this, F.Sprites[1]));
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            spriteBatch.Draw(Sprite, new Vector2 (((int)this.box.x - (int)this.box.width), (int)this.box.y - (int)this.box.height),
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                2, SpriteEffects.None, 0);
        }
    }
}
