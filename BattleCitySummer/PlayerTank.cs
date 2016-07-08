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

        // public bool right;
        // public bool left;
        // public bool up;
        // public bool down;
        //  public bool shoot;
        //  public bool shot;
        //  public bool destroy = false;
        //  public int score = 0;

        //  public double pos;

        public PlayerTank(MainGame F, int x, int y)
        {
            this.playerBox = new Box(x, y, 8, 8, 0, 0, false);
            health = 100;
           // shoot = false;
           // pos = 0;
            F.Boxes.Add(this.playerBox);
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
                this.playerBox.vx -= 3;
            if (keyboardState.IsKeyDown(Keys.Right))
                this.playerBox.vx += 3;
            if (keyboardState.IsKeyDown(Keys.Up))
                this.playerBox.vy -= 3;
            if (keyboardState.IsKeyDown(Keys.Down))
                this.playerBox.vy += 3;
            if (Math.Abs(this.playerBox.vy) > 0)
                this.playerBox.vx = 0;
            if (Math.Abs(this.playerBox.vx) > 0)
                this.playerBox.vy = 0;

            if (health <= 0)
            {
                F.gameover = true;
            }
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
