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
        public bool gameover { get; set; }

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
                        else if (!Boxes[i].Static && Boxes[j].Static || !Boxes[i].Static && !Boxes[j].Static) //тоже самое но для j,i
                        {
                            Boxes[i].Collide(Boxes[j], dx, dy);
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

                if (GameObjects[i].GetType() == typeof(EnemyTank))   //обнуление скорости вражеского танка при столкновении с игроком
                {
                    enemyTank = (EnemyTank)GameObjects[i];
                    if (enemyTank.box.Collides(player.box, ref tmp, ref tmp))
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

            // this.Boxes.Add(barrier);

            //create new map


            this.player = new PlayerTank(this, 96, 96, Sprites[0]);
            this.GameObjects.Add(player);

           // this.GameObjects.Add(new EnemyTank(this, 100, 200, Sprites[2]));
            this.gameover = false;
        }

        public void LoadMap()
        {
            for (int i = 0; i < map.map.GetLength(0); i++)
            {
                for (int j = 0; j < map.map.GetLength(1); j++)
                {
                    if (map.map[i, j] == 1)
                    {
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 8, j * 32 + 8, 8, 8, Sprites[3]));
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 24, j * 32 + 8, 8, 8, Sprites[3]));
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 8, j * 32 + 24, 8, 8, Sprites[3]));
                        this.GameObjects.Add(new BrickWall(this, i * 32 + 24, j * 32 + 24, 8, 8, Sprites[3]));
                    }
                    if (map.map[i, j] == 9)
                    {
                        this.GameObjects.Add(new Base(this, i * 32 + 16, j * 32 + 16, 16, 16, Sprites[4]));
                    }
                    if (map.map[i,j] == 2)
                    {
                        this.GameObjects.Add(new IronWall(this, i * 32 + 16, j * 32 + 16, 16, 16, Sprites[7]));
                    }
                }
            }
            
        }
    }
}