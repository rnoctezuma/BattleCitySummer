using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class Explosion : IGameObject
    {
        public Texture2D Sprite1 { get; set; }
        public Texture2D Sprite2 { get; set; }
        public double x;
        public double y;
        private double animation1 = 0;
        private double animation2 = 0;
        //1st Sprite
        private int frameWidth1 = 16;
        private int frameHeight1 = 16;
        private Point currentFrame1 = new Point(0, 0);
        private Point spriteSize1 = new Point(3, 1);

        //2nd Sprite
        private int frameWidth2 = 32;
        private int frameHeight2 = 32;
        private Point currentFrame2 = new Point(0, 0);
        private Point spriteSize2 = new Point(2, 1);
        public bool destroy = false;

        public Explosion(double x, double y, Texture2D Sprite1, Texture2D Sprite2)
        {
            this.x = x;
            this.y = y;
            this.Sprite1 = Sprite1;
            this.Sprite2 = Sprite2;  
        }
        public bool isDestroyed()
        {
            return destroy;
        }
        public void Destroy()
        {
            destroy = true;
        }

        public void Update(MainGame F, GameTime gameTime)
        {
            this.animation1 += 0.10;
      //      this.animation2 += 0.10;
            if (this.animation1 >= 3)
                this.Destroy();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite1, new Vector2((int)x - this.frameWidth1/2, (int)y-this.frameHeight1/2-5),
                new Rectangle((currentFrame1.X + (int)animation1) * frameWidth1,
                    currentFrame1.Y * frameHeight1,
                    frameWidth1, frameHeight1),
                Color.White, 0, Vector2.Zero,
                2, SpriteEffects.None, 0);
        }
    }
}

