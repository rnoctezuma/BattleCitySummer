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
        private int pos = 0;
        public Texture2D Sprite { get; set; }
        private int frameWidth = 7;
        private int frameHeight = 7;
        private Point currentFrame = new Point(0, 0);
        private Point spriteSize = new Point(4, 1);
        public Bullet(MainGame F, double x, double y, double vx, double vy, IGameObject parent, Texture2D Sprite)
        {
            this.box = new Box(x, y, 6, 6, vx, vy, false);
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
        public void Update(MainGame mainGame, GameTime gameTime)
        {
            double angle = Math.Atan2(this.box.vy, this.box.vx);

            if (angle > -0.3 && angle < 0.3)
            {
                currentFrame.X = 3;
                pos = 0;
            }
            if (angle > Math.PI / 2 - 0.3 && angle < Math.PI / 2 + 0.3)
            {
                currentFrame.X = 2;
                pos = 1;
            }
            if (angle > Math.PI - 0.3 && angle < Math.PI + 0.3)
            {
                currentFrame.X = 1;
                pos = 2;
            }
            if (angle > -Math.PI / 2 - 0.3 && angle < -Math.PI / 2 + 0.3)
            {
                currentFrame.X = 0;
                pos = 3;
            }

            EnemyTank enemyTank = null;
            PlayerTank playerTank = null;
            BrickWall brickWall = null;
            IronWall ironWall = null;
            bool explosion = false;
            foreach (IGameObject gameObject in mainGame.GameObjects)
            {
                if (gameObject.GetType() == typeof(EnemyTank) && gameObject.GetType() != parent.GetType())
                {
                    enemyTank = (EnemyTank)gameObject;
                    if (this.box.colliders.Contains(enemyTank.box))
                    {
                        explosion = true;
                        enemyTank.Damage();
                        this.Destroy();
                    }
                }
                else
                {
                    if (gameObject.GetType() == typeof(PlayerTank) && gameObject.GetType() != parent.GetType())
                    {
                        playerTank = (PlayerTank)gameObject;
                        if (this.box.colliders.Contains(playerTank.box))
                        {
                            explosion = true;
                            playerTank.Damage();
                            this.Destroy();
                        }
                    }
                    else
                    {
                        if (gameObject.GetType() == typeof(BrickWall))
                        {
                            brickWall = (BrickWall)gameObject;
                            if (this.box.colliders.Contains(brickWall.box))
                            {

                                explosion = true;
                                brickWall.Destroy();
                                this.Destroy();

                            }
                        }
                        else
                        {
                            if (gameObject.GetType() == typeof(IronWall))
                            {
                                ironWall = (IronWall)gameObject;
                                if (this.box.colliders.Contains(ironWall.box))
                                {
                                    explosion = true;
                                    this.Destroy();
                                }
                            }
                        }
                    }
                }
                
                /*
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


            if (explosion)
                MakeExplosion(mainGame);            
        }

        public void MakeExplosion(MainGame mainGame)
        {
            if (pos == 0)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x + 5, this.box.y - 2,
                    mainGame.Sprites[5], mainGame.Sprites[6]));                
            }
            if (pos == 2)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x - 20, this.box.y - 2,
                    mainGame.Sprites[5], mainGame.Sprites[6]));        
            }
            if (pos == 1)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x-5, this.box.y + 5,
                    mainGame.Sprites[5], mainGame.Sprites[6]));                
            }
            if (pos == 3)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x - 9, this.box.y - 12,
                    mainGame.Sprites[5], mainGame.Sprites[6]));
            }
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            
        //    DrawRectangle(new Rectangle((int)this.box.x - (int)this.box.width, (int)this.box.y - (int)this.box.height,
         //       (int)this.box.width * 2, (int)this.box.height * 2), Color.White, graphics, spriteBatch, texture);
            
            spriteBatch.Draw(Sprite, new Vector2(((int)this.box.x - (int)this.box.width), (int)this.box.y - (int)this.box.height),
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                2, SpriteEffects.None, 0);
            
        }
    }
}
