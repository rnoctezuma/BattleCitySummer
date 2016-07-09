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
        }

        public void Update()
        {
            foreach (Box S in Boxes)
            {
                S.Update();
            }
            foreach (IGameObject S in GameObjects)
            {
                S.Update(this);
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
            
        }

        public void Init()
        {
            //create new map
            map = new Map();

            Boxes = new List<Box>();
            GameObjects = new List<IGameObject>();
            this.player = new PlayerTank(this, 96, 16);
            this.GameObjects.Add(player);

            this.GameObjects.Add(new EnemyTank(this, 16, 16));
            this.gameover = false;
        }
    }
}