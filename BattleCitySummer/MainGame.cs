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
        public List <IGameObject> GameObjects { get; set; }
        public List <Box> Boxes { get; set; }
        public PlayerTank player = null;
        public Map map;
        private Box barrier = new Box(288, 288, 285, 285, 0, 0, false);
        public bool gameover { get; set; }

        public void Clear()
        {
            this.GameObjects.Clear();
            this.Boxes.Clear();
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            foreach (IGameObject S in GameObjects)
            {
                S.Draw(graphics, spriteBatch, pixel);
            }
            
            foreach (Box S in Boxes)
            {
                S.Draw(graphics, spriteBatch, pixel);
            }
        }

        public void Update()
        {
            foreach (Box box in Boxes)
            {
                box.Update();
            }
            /*
            foreach (IGameObject gameObject in GameObjects)
            {
                gameObject.Update(this);
            }
            */

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
            for (int i = 0; i < GameObjects.Count; i++)
            {
                GameObjects[i].Update(this);
                if (GameObjects[i].GetType() == typeof(Bullet))
                {
                    if (!((Bullet)GameObjects[i]).bulletBox.Collides(barrier, ref tmp, ref tmp)) //если не внутри барьера
                    {
                        GameObjects[i].Destroy();
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
            //barrier
            this.Boxes.Add(new Box(288, 588, 295, 10, 0, 0, true));
            this.Boxes.Add(new Box(288, -12, 295, 10, 0, 0, true));
            this.Boxes.Add(new Box(588, 288, 10, 290, 0, 0, true));
            this.Boxes.Add(new Box(-12, 288, 10, 290, 0, 0, true));

           // this.Boxes.Add(barrier);

            //create new map


            this.player = new PlayerTank(this, 96, 96);
            this.GameObjects.Add(player);

            this.GameObjects.Add(new EnemyTank(this, 200, 200));
            this.gameover = false;
        }
    }
}