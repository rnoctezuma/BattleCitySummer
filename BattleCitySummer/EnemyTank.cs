using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class EnemyTank : IGameObject
    {
        public Box enemyBox { get; set; }
        public int health { get; set; }

        public EnemyTank(MainGame F, int x, int y)
        {
            this.enemyBox = new Box(x, y, 20, 20, 0, 0, true);
            health = 100;
            // shoot = false;
            // pos = 0;
            F.Boxes.Add(this.enemyBox);
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
        }


        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            DrawRectangle(new Rectangle((int)this.enemyBox.x - (int)this.enemyBox.width, (int)this.enemyBox.y - (int)this.enemyBox.height, 
                (int)this.enemyBox.width*2, (int)this.enemyBox.height*2), Color.Red, graphics, spriteBatch, pixel);
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            pixel.SetData(new[] { color });
            spriteBatch.Draw(pixel, coords, color);
        }
    }
}
