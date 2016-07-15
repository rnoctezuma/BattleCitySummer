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
        public Texture2D Sprite3 { get; set; }
        public Texture2D Sprite4 { get; set; }
        public double x;
        public double y;
        public int chooseExplosion = 0;
        private double animation = 0;
        public bool destroy = false;
        //1st Sprite (Small Explosion)
        private int frameWidth1 = 16;
        private int frameHeight1 = 16;
        private Point currentFrame1 = new Point(0, 0);
        private Point spriteSize1 = new Point(3, 1);

        //2nd Sprite (Large Explosion)
        private int frameWidth2 = 32;
        private int frameHeight2 = 32;
        private Point currentFrame2 = new Point(0, 0);
        private Point spriteSize2 = new Point(5, 1);

        //3rd Sprite (Player Spawn)
        private int frameWidth3 = 16;
        private int frameHeight3 = 16;
        private Point currentFrame3 = new Point(0, 0);
        private Point spriteSize3 = new Point(2, 1);

        //4th Sprite (Enemy Spawn)
        private int frameWidth4 = 16;
        private int frameHeight4 = 16;
        private Point currentFrame4 = new Point(0, 0);
        private Point spriteSize4 = new Point(4, 1);

        public Explosion(double x, double y, Texture2D Sprite1, Texture2D Sprite2, Texture2D Sprite3, Texture2D Sprite4, int chooseExplosion)
        {
            this.x = x;
            this.y = y;
            this.Sprite1 = Sprite1;
            this.Sprite2 = Sprite2;
            this.Sprite3 = Sprite3;
            this.Sprite4 = Sprite4;
            this.chooseExplosion = chooseExplosion;
        }
        public bool isDestroyed()
        {
            return destroy;
        }
        public void Destroy()
        {
            destroy = true;
        }

        public void Update(MainGame mainGame, GameTime gameTime)
        {
            this.animation += 0.10;
            switch (chooseExplosion)
            {
                case 0:
                    if (this.animation >= 3)
                        this.Destroy();
                    break;
                case 1:
                    if (this.animation >= 5)
                        this.Destroy();
                    break;
                case 2:
                    if (this.animation >= 2)
                        this.Destroy();
                    break;
                case 3:
                    if (this.animation >= 4)
                        this.Destroy();
                    break;
            }

        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            switch (chooseExplosion)
            {
                case 0:
                    spriteBatch.Draw(Sprite1, new Vector2((int)x - this.frameWidth1 / 2, (int)y - this.frameHeight1 / 2 - 5),
                        new Rectangle((currentFrame1.X + (int)animation) * frameWidth1,
                            currentFrame1.Y * frameHeight1,
                            frameWidth1, frameHeight1),
                        Color.White, 0, Vector2.Zero,
                        2, SpriteEffects.None, 0);
                    break;
                case 1:
                    spriteBatch.Draw(Sprite2, new Vector2((int)x - this.frameWidth2 / 2 - 7, (int)y - this.frameHeight2 / 2 - 16),
                        new Rectangle((currentFrame2.X + (int)animation) * frameWidth2,
                            currentFrame2.Y * frameHeight2,
                            frameWidth2, frameHeight2),
                        Color.White, 0, Vector2.Zero,
                        2, SpriteEffects.None, 0);
                    break;
                case 2:
                    spriteBatch.Draw(Sprite3, new Vector2((int)x - this.frameWidth3 / 2 - 12, (int)y - this.frameHeight3 / 2 - 6),
                        new Rectangle((currentFrame3.X + (int)animation) * frameWidth3,
                            currentFrame3.Y * frameHeight3,
                            frameWidth3, frameHeight3),
                        Color.White, 0, Vector2.Zero,
                        2, SpriteEffects.None, 0);
                    break;
                case 3:
                    spriteBatch.Draw(Sprite4, new Vector2((int)x - this.frameWidth4 / 2 - 7, (int)y - this.frameHeight4 / 2 - 6),
                        new Rectangle((currentFrame4.X + (int)animation) * frameWidth4,
                            currentFrame4.Y * frameHeight4,
                            frameWidth4, frameHeight4),
                        Color.White, 0, Vector2.Zero,
                        2, SpriteEffects.None, 0);
                    break;
            }
        }
    }
}

