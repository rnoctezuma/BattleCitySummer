using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class MainGame
    {
        public List<IGameObject> GameObjects { get; set; }
        public List<Box> Boxes { get; set; }
        public List<Texture2D> Sprites = new List<Texture2D>();
        public PlayerTank player = null;
        public Map map;
        public Box barrier = new Box(288, 288, 277, 277, 0, 0, false);
        public bool isGameover { get; set; }
        private Random rand = new Random();
        public double spawnTimer = 20;
        public int tankCounter = 30;
        public void Clear()
        {
            this.GameObjects.Clear();
            this.Boxes.Clear();
        }


        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {

            foreach (IGameObject gameObject in GameObjects)
            {
                gameObject.Draw(graphics, spriteBatch);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!isGameover)
            {
                if (tankCounter==0)
                {
                    this.isGameover = true;
                }
                this.spawnTimer += 0.1;
                if (this.spawnTimer >= 20)
                {
                    EnemySpawn();
                    this.spawnTimer = 0;
                }

                foreach (Box box in Boxes)
                {
                    box.Update();
                }
                // Физика


                for (int i = 0; i < Boxes.Count - 1; i++)  //проверяю каждую коробку в с каждой (кроме себя)
                {
                    for (int j = i + 1; j < Boxes.Count; j++)
                    {
                        double dx = 0;
                        double dy = 0;
                        if (Boxes[i].Collides(Boxes[j], ref dx, ref dy))
                        {

                            if (Boxes[i].Static && !Boxes[j].Static) //если коробка i статическая, а не статическая, то мы выталкивает динамическую коробку
                            {
                                Boxes[j].Collide(Boxes[i], -dx, -dy);
                                Boxes[i].Collide(Boxes[j], 0, 0);
                            }
                            else if (!Boxes[i].Static && Boxes[j].Static) //тоже самое но для j,i
                            {
                                Boxes[i].Collide(Boxes[j], dx, dy);
                                Boxes[j].Collide(Boxes[i], 0, 0);
                            }
                            else
                            {
                                Boxes[i].Collide(Boxes[j], 0, 0);  //две динамические коробки, фиксир. пересечение и добавляются в списки colliders
                                Boxes[j].Collide(Boxes[i], 0, 0);
                            }
                        }
                    }
                }

                double tmp = 0;
                EnemyTank enemyTank = null;
                for (int i = 0; i < GameObjects.Count; i++)
                {
                    GameObjects[i].Update(this, gameTime);
                    if (GameObjects[i].GetType() == typeof(Bullet))
                    {
                        if (!((Bullet)GameObjects[i]).box.Collides(barrier, ref tmp, ref tmp)) //если не внутри барьера
                        {
                            GameObjects[i].Destroy();
                        }
                    }
                }

                for (int i = 0; i < GameObjects.Count; i++)
                {
                    if (GameObjects[i].GetType() == typeof(EnemyTank))
                    {
                        enemyTank = (EnemyTank)GameObjects[i];
                        if (enemyTank.box.Collides(this.player.box, ref tmp, ref tmp)) //если не внутри барьера
                        {
                            enemyTank.box.vx = 0;
                            enemyTank.box.vy = 0;
                        }
                    }
                }


                //GarbageCollect
                for (int i = 0; i < GameObjects.Count; i++) //удаление всех игровых коробок и объектов которые должны быть удалены
                {
                    if (GameObjects[i].isDestroyed())
                    {
                        GameObjects.RemoveAt(i);
                        i--;
                    }
                }

                for (int i = 0; i < Boxes.Count; i++)
                {
                    if (Boxes[i].destroy)
                    {
                        Boxes.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void Init()
        {
            Boxes = new List<Box>();
            GameObjects = new List<IGameObject>();
            map = new Map();
            LoadMap();
            //barrier
            this.Boxes.Add(new Box(288, 588, 295, 10, 0, 0, true));
            this.Boxes.Add(new Box(288, -12, 295, 10, 0, 0, true));
            this.Boxes.Add(new Box(588, 288, 10, 290, 0, 0, true));
            this.Boxes.Add(new Box(-12, 288, 10, 290, 0, 0, true));

            this.player = new PlayerTank(this, 210, 535, Sprites[0]);
            this.GameObjects.Add(player);
            this.isGameover = false;
        }

        public void LoadMap()
        {
            for (int i = 0; i < map.gameMap.GetLength(0); i++)
            {
                for (int j = 0; j < map.gameMap.GetLength(1); j++)
                {
                    if (map.gameMap[i, j] == 1)
                    {
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 8, j * 32 + 8, 8, 8, Sprites[3]));
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 24, j * 32 + 8, 8, 8, Sprites[3]));
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 8, j * 32 + 24, 8, 8, Sprites[3]));
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 24, j * 32 + 24, 8, 8, Sprites[3]));
                    }
                    
                    if (map.gameMap[i, j] == 9)
                    {
                        this.GameObjects.Add(new Base(this, i * 32 + 16, j * 32 + 16, 16, 16, Sprites[4]));
                    }
                    if (map.gameMap[i,j] == 2)
                    {
                        this.GameObjects.Add(new IronWall(this, i * 32 + 16, j * 32 + 16, 16, 16, Sprites[7]));
                    }
                }
            }           
        }

        public void EnemySpawn()
        {
            int enemyTankCounter = 0;
            bool flag = false;

            EnemyTank enemyTank = null;
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].GetType() == typeof(EnemyTank))
                {
                    enemyTankCounter++;
                    enemyTank = (EnemyTank)GameObjects[i];
                    if ((int)Math.Floor(enemyTank.box.y / 32d) == this.map.gameMap.GetLength(1)-1 && (int)Math.Floor(enemyTank.box.x / 32d) == 0)
                        flag = true;
                }
            }
            if (enemyTankCounter < 4)
            {
                int spawnPos = rand.Next(0, this.map.gameMap.GetLength(1));
                int lr = rand.Next(0, 2);
                int playerPosX = (int)Math.Floor(this.player.box.x / 32d);
                int playerPosY = (int)Math.Floor(this.player.box.y / 32d);
                if (playerPosY == 0)
                {
                    if (playerPosX == spawnPos)
                    {
                        if (playerPosX == 0)
                            spawnPos = rand.Next(1, this.map.gameMap.GetLength(1));
                        else if (playerPosX == this.map.gameMap.GetLength(1) - 1)
                            spawnPos = rand.Next(0, this.map.gameMap.GetLength(1) - 1);
                        else if (playerPosX != 0 && playerPosX != this.map.gameMap.GetLength(1) - 1)
                        {
                            if (lr == 0)
                                spawnPos = rand.Next(0, playerPosX);
                            else if (lr == 1 && flag == false)
                                spawnPos = rand.Next(playerPosX + 1, this.map.gameMap.GetLength(1));
                            else
                                spawnPos = rand.Next(playerPosX + 1, this.map.gameMap.GetLength(1)-1);
                        }
                    }
                }
                this.GameObjects.Add(new Explosion(spawnPos * 32 + 16, 16,
                               this.Sprites[5], this.Sprites[6], this.Sprites[8], this.Sprites[9], 3));
                this.GameObjects.Add(new EnemyTank(this, spawnPos * 32 + 16, 16, this.Sprites[2]));
            }
        }
    }
}