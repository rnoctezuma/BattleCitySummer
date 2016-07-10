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
        public Box bulletBox;
        public bool destroy = false;
        public IGameObject parent = null;
        public Bullet(MainGame F, double x, double y, double vx, double vy, IGameObject parent)
        {
            this.bulletBox = new Box(x, y, 2, 2, vx, vy, false);
            this.parent = parent;
            F.Boxes.Add(this.bulletBox);
        }
        public void Destroy()
        {
            destroy = true;
            this.bulletBox.destroy = true;
        }
        public bool isDestroyed()
        {
            return destroy;
        }
        public void Update(MainGame F)
        {
            /*
            Cop C = null;
            Player P = null;
            Wall W = null;
            Door D = null;
            Meth M = null;
            foreach (GameObject S in F.GameObjects)
            {
                if (S.GetType() == typeof(Cop) && S.GetType() != parent.GetType())
                {
                    C = (Cop)S;
                    if (this.box.colliders.Contains(C.box))
                    {
                        C.Damage(20);
                        Destroy();
                    }
                }
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

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            DrawRectangle(new Rectangle((int)this.bulletBox.x - (int)this.bulletBox.width, (int)this.bulletBox.y - (int)this.bulletBox.height,
                (int)this.bulletBox.width * 2, (int)this.bulletBox.height * 2), Color.Green, graphics, spriteBatch, pixel);
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
        {
            pixel.SetData(new[] { color });
            spriteBatch.Draw(pixel, coords, color);
        }
    }
}
