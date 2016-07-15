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
            Base basePlayer = null;
            for (int i = 0; i < mainGame.GameObjects.Count; i++)
            {
                if (mainGame.GameObjects[i].GetType() == typeof(EnemyTank) && mainGame.GameObjects[i].GetType() != parent.GetType())
                {
                    enemyTank = (EnemyTank)mainGame.GameObjects[i];
                    if (this.box.colliders.Contains(enemyTank.box))
                    {
                        MakeExplosion(mainGame, 1);
                        enemyTank.Destroy();
                        mainGame.tankCounter--;
                        mainGame.player.addScore();
                        this.Destroy();
                    }
                }
                if (mainGame.GameObjects[i].GetType() == typeof(PlayerTank) && mainGame.GameObjects[i].GetType() != parent.GetType())
                {
                    playerTank = (PlayerTank)mainGame.GameObjects[i];
                    if (this.box.colliders.Contains(playerTank.box))
                    {
                        MakeExplosion(mainGame, 1);
                        this.Destroy();
                        playerTank.health--;
                        playerTank.newPlayerSpawn();
                        mainGame.GameObjects.Add(new Explosion(playerTank.box.x, playerTank.box.y,
                               mainGame.Sprites[5], mainGame.Sprites[6], mainGame.Sprites[8], mainGame.Sprites[9], 2));
                    }
                }
                if (mainGame.GameObjects[i].GetType() == typeof(BrickWall))
                {
                    brickWall = (BrickWall)mainGame.GameObjects[i];
                    if (this.box.colliders.Contains(brickWall.box))
                    {

                        MakeExplosion(mainGame, 0);
                        brickWall.Destroy();
                        int brickMapPosX = (int)Math.Floor(brickWall.box.x / 32d);
                        int brickMapPosY = (int)Math.Floor(brickWall.box.y / 32d);
                        mainGame.map.gameMap[brickMapPosX, brickMapPosY] = 0;
                        this.Destroy();

                    }
                }

                if (mainGame.GameObjects[i].GetType() == typeof(IronWall))
                {
                    ironWall = (IronWall)mainGame.GameObjects[i];
                    if (this.box.colliders.Contains(ironWall.box))
                    {
                        MakeExplosion(mainGame, 0);
                        this.Destroy();
                    }
                }

                if (mainGame.GameObjects[i].GetType() == typeof(Base))
                {
                    basePlayer = (Base)mainGame.GameObjects[i];
                    if (this.box.colliders.Contains(basePlayer.box))
                    {
                        MakeExplosion(mainGame, 1);
                        basePlayer.Damage();
                        this.Destroy();
                    }
                }
            }
        }

        public void MakeExplosion(MainGame mainGame, int chooseExplosion)
        {
            if (pos == 0)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x + 5, this.box.y - 2,
                    mainGame.Sprites[5], mainGame.Sprites[6], mainGame.Sprites[8], mainGame.Sprites[9], chooseExplosion));
            }
            if (pos == 2)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x - 20, this.box.y - 2,
                    mainGame.Sprites[5], mainGame.Sprites[6], mainGame.Sprites[8], mainGame.Sprites[9], chooseExplosion));
            }
            if (pos == 1)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x - 5, this.box.y + 5,
                    mainGame.Sprites[5], mainGame.Sprites[6], mainGame.Sprites[8], mainGame.Sprites[9], chooseExplosion));
            }
            if (pos == 3)
            {
                mainGame.GameObjects.Add(new Explosion(this.box.x - 9, this.box.y - 12,
                    mainGame.Sprites[5], mainGame.Sprites[6], mainGame.Sprites[8], mainGame.Sprites[9], chooseExplosion));
            }
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(((int)this.box.x - (int)this.box.width), (int)this.box.y - (int)this.box.height),
                new Rectangle(currentFrame.X * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                2, SpriteEffects.None, 0);
        }
    }
}
