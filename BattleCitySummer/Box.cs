using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class Box
    {
        public List<Box> colliders = new List<Box>();
        public double x { get; set; } //top-left corner
        public double y { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public double vx { get; set; }
        public double vy { get; set; }
        public Rectangle boxRect { get; set; }

        public bool Static;   
        public bool destroy = false;

        public Box()
        {
        }

        public Box(double x, double y, double width, double height, double vx, double vy, bool Static)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            boxRect = new Rectangle((int)x, (int)y, (int)width, (int)height);
            this.vx = vx;
            this.vy = vy;
            this.Static = Static;
        }


        public bool Collides(Box box, ref double dx, ref double dy)
        {
            double ax = this.x - box.x; //расстояние между центрами проверяемых коробок
            double ay = this.y - box.y;
            double aW = this.width + box.width; //идеальное значение между центрами коробок
            double aH = this.height + box.height;

            if (Math.Abs(ax) > aW)         //если пересечения нет
            { return false; }              //
            if (Math.Abs(ay) > aH)         //
            { return false; }              //
            if (ax < 0)
            { dx = -ax - aW; }
            else
            { dx = aW - ax; }  //насколько заходит одна коробка в другую по y
            if (ay < 0)        //и по x
            { dy = -ay - aH; }
            else
            { dy = aH - ay; }
            if (Math.Abs(dx) < Math.Abs(dy))
            { dy = 0; }
            else
            { dx = 0; }
            return true;
        }

        public void Collide(Box secondBox, double dx, double dy)
        {
            if (!colliders.Contains(secondBox))
            {
                colliders.Add(secondBox);
            }
            this.x += dx; //выталкивание коробки с которой пересеклись
            this.y += dy;
        }

        public void Update()
        {
            colliders.Clear();
            this.x += this.vx;
            this.y += this.vy;
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {
            DrawRectangle(new Rectangle((int)this.x - (int)this.width, (int)this.y - (int)this.height,
                (int)this.width * 2, (int)this.height * 2), Color.Red, graphics, spriteBatch, texture);
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {
            texture.SetData(new[] { color });
            spriteBatch.Draw(texture, coords, color);
        }
    }
}
