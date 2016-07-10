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
        public Box playerBox { get; set; }
        public int health { get; set; }
        public double shot = 11;
        public bool destroy = false;

        //  public int score = 0;

          public double pos; //направление взгляда

        public PlayerTank(MainGame F, int x, int y)
        {
            this.playerBox = new Box(x, y, 16, 16, 0, 0, false);
            this.health = 100;
            this.pos = 3.0 / 2.0 * Math.PI;
            F.Boxes.Add(this.playerBox);
        }

        public void Destroy()
        {
            destroy = true;
            this.playerBox.destroy = true;
        }

        public bool isDestroyed()
        {
            return destroy;
        }

        public void Damage(int x)
        {
            if (health > 0)
            {
                health -= x;
            }
        }

        public void Update(MainGame F)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            this.playerBox.vx = 0;
            this.playerBox.vy = 0;

            if (keyboardState.IsKeyDown(Keys.Left))  //motion
            {
                this.playerBox.vx -= 3;
                this.pos = Math.PI;
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                this.playerBox.vx += 3;
                this.pos = 0;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                this.playerBox.vy -= 3;
                this.pos = 3.0 / 2.0 * Math.PI;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                this.playerBox.vy += 3;
                this.pos = 1.0 / 2.0 * Math.PI;
            }
            else if (keyboardState.IsKeyDown(Keys.LeftControl))
            { 
                if (this.shot > 10)
                {
                    F.GameObjects.Add(new Bullet(F, this.playerBox.x, this.playerBox.y, Math.Cos(pos)*5, Math.Sin(pos)*5, this));
                    this.shot = 0;
                }
            }
            this.shot += 0.3;
            if (health <= 0)
            {
                F.gameover = true;
            }        
        }
       
        public void MoveCheck()
        {

        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            DrawRectangle(new Rectangle((int)this.playerBox.x - (int)this.playerBox.width, (int)this.playerBox.y - (int)this.playerBox.height, 
                (int)this.playerBox.width*2, (int)this.playerBox.height*2), Color.Green, graphics, spriteBatch, pixel);
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {         
            pixel.SetData(new[] { color });
            spriteBatch.Draw(pixel, coords, color);
        }
    }
}
