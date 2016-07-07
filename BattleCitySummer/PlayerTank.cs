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

            if (keyboardState.IsKeyDown(Keys.Left))  //mothion
                this.playerBox.vx -= 3;
            if (keyboardState.IsKeyDown(Keys.Right))
                this.playerBox.vx += 3;
            if (keyboardState.IsKeyDown(Keys.Up))
                this.playerBox.y -= 3;
            if (keyboardState.IsKeyDown(Keys.Down))
                this.playerBox.y += 3;

            if (health <= 0)
            {
                F.gameover = true;
            }
        }
        

        /*
        public void BarrierCheck()                    //Как-то передать размеры окна???
        {
            if (this.playerBox.x < 0)
                this.playerBox.x = 0;
            if (this.playerBox.y < 0)
                this.playerBox.y = 0;
            if (this.playerBox.x > ClientBounds.Width - goodSpriteSize.X)
                this.playerBox.x = Window.ClientBounds.Width - goodSpriteSize.X;
            if (goodSpritePosition.Y > Window.ClientBounds.Height - goodSpriteSize.Y)
                goodSpritePosition.Y = Window.ClientBounds.Height - goodSpriteSize.Y;
        }
        */

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            DrawRectangle(new Rectangle((int)this.playerBox.x, (int)this.playerBox.y, (int)this.playerBox.width, (int)this.playerBox.height), Color.Green, graphics, spriteBatch);
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            var rect = new Texture2D(graphics.GraphicsDevice, 1, 1);
            rect.SetData(new[] { color });
            spriteBatch.Draw(rect, coords, color);
        }
    }
}
