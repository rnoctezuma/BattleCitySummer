using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class Bullet : IGameObject
    {
        public Box box;
        public bool destroy = false;
        public IGameObject parent = null;
        public Texture2D Sprite { get; set; }
        private int frameWidth = 8;
        private int frameHeight = 8;
        private Point currentFrame = new Point(0, 0);
        private Point spriteSize = new Point(4, 1);
        public Bullet(MainGame F, double x, double y, double vx, double vy, IGameObject parent, Texture2D Sprite)
        {
            this.box = new Box(x, y, 2, 2, vx, vy, false);
            this.parent = parent;
            this.Sprite = Sprite;
            F.Boxes.Add(this.box);
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
        public void Update(MainGame F)
        {
            double angle = Math.Atan2(this.box.vy, this.box.vx);

            if (angle > -0.3 && angle < 0.3)
                currentFrame.X = 3;
            if (angle > Math.PI / 2 - 0.3 && angle < Math.PI / 2 + 0.3)
                currentFrame.X = 2;
            if (angle > Math.PI - 0.3 && angle < Math.PI + 0.3)
                currentFrame.X = 1;
            if (angle > 3 * Math.PI / 4 - 0.3 && angle < 3 * Math.PI / 4 + 0.3)
                currentFrame.X = 0;

            EnemyTank enemyTank = null;
            PlayerTank playerTank = null;

            foreach (IGameObject S in F.GameObjects)
            {
                if (S.GetType() == typeof(EnemyTank))
                {
                    enemyTank = (EnemyTank)S;
                    if (this.box.colliders.Contains(enemyTank.box))
                    {
                        enemyTank.Damage();
                        this.Destroy();
                    }
                }
                if (S.GetType() == typeof(PlayerTank))
                {
                    playerTank = (PlayerTank)S;
                    if (this.box.colliders.Contains(playerTank.box))
                    {
                        playerTank.Damage();
                        this.Destroy();
                    }
                }
            }
        /*        }
                else
                {
                    if (S.GetType() == typeof(Player) && S.GetType() != parent.GetType())
                    {
                        P = (Player)S;
                        if (this.box.colliders.Contains(P.box))
                        {
                            P.Damage(5);
                            Destroy();
                        }
                    }
                    else
                    {
                        if (S.GetType() == typeof(Wall))
                        {
                            W = (Wall)S;
                            if (this.box.colliders.Contains(W.box))
                            {
                                Destroy();
                            }
                        }
                        else
                        {
                            if (S.GetType() == typeof(Door))
                            {
                                D = (Door)S;
                                if (this.box.colliders.Contains(D.box) && D.active)
                                {
                                    Destroy();
                                    D.Damage(1);
                                }
                            }
                            else
                            {
                                if (S.GetType() == typeof(Meth))
                                {
                                    M = (Meth)S;
                                    if (this.box.colliders.Contains(M.box))
                                    {
                                        Destroy();
                                        M.Damage(10);
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {/*
            DrawRectangle(new Rectangle((int)this.bulletBox.x - (int)this.bulletBox.width, (int)this.bulletBox.y - (int)this.bulletBox.height,
                (int)this.bulletBox.width * 2, (int)this.bulletBox.height * 2), Color.Green, graphics, spriteBatch, pixel);*/

            spriteBatch.Draw(Sprite, new Vector2(((int)this.box.x - (int)this.box.width), (int)this.box.y - (int)this.box.height),
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                2, SpriteEffects.None, 0);


/*
            spriteBatch.Draw(texture, Vector2.Zero,
new Rectangle(currentFrame.X * frameWidth,
currentFrame.Y * frameHeight,
frameWidth, frameHeight),
Color.White, 0, Vector2.Zero,
1, SpriteEffects.None, 0);*/
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            pixel.SetData(new[] { color });
            spriteBatch.Draw(pixel, coords, color);
        }
    }
}
