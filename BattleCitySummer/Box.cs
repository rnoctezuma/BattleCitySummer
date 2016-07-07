using Microsoft.Xna.Framework;
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

        //   public bool Static;   
        //   public bool destroy = false;

        public Box()
        {
        }

        public Box(double x, double y, double width, double height, double vx, double vy)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            boxRect = new Rectangle((int)x, (int)y, (int)width, (int)height);
            this.vx = vx;
            this.vy = vy;
        }
        
        public bool Collides(Box secondBox) //collides check
        {
            return this.boxRect.Intersects(secondBox.boxRect);
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
    }
}
