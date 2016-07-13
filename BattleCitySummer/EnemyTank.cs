using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class EnemyTank : IGameObject
    {
        public Box box { get; set; }
        public int health { get; set; }
        public List<KeyValuePair<int, int>> wave = new List<KeyValuePair<int, int>>();  //note: wave normalize (wave.Key --, wave.Value --)
        public int[] playerPosition = new int[2];
        public bool destroy = false;
        public Texture2D Sprite { get; set; }
        private int frameWidth = 16;
        private int frameHeight = 16;
        private Point currentFrame = new Point(0, 0);
        private Point spriteSize = new Point(4, 1);

        public EnemyTank(MainGame F, int x, int y, Texture2D Sprite)
        {
            this.box = new Box(x, y, 16, 16, 0, 0, true);
            health = 1;
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

        public void Damage()
        {
            this.Destroy();
        }

        public void Update(MainGame F)
        {
            int[,] logicMap = GetLogicMap(F.map.map);
            SearchPlayersPosition(F);
            FindOptimalPath(logicMap);
            MoveToPlayer();
        }



        public int[,] GetLogicMap(int[,] map)
        {
            int[,] generatedMap = new int[map.GetLength(0) + 2, map.GetLength(1) + 2];

            for (int i = 0; i < generatedMap.GetLength(0); i++)   //creating barriers for A*
            {
                generatedMap[i, 0] = 9999;
                generatedMap[0, i] = 9999;
                generatedMap[i, generatedMap.GetLength(0) - 1] = 9999;
                generatedMap[generatedMap.GetLength(0) - 1, i] = 9999;
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 0)
                    {
                        generatedMap[i + 1, j + 1] = -1;
                    }
                    else
                    {
                        generatedMap[i + 1, j + 1] = 9999;
                    }
                }
            }
            return generatedMap;
        }

        public void FindOptimalPath(int[,] logicMap)
        {
            List<KeyValuePair<int, int>> Oldwave = new List<KeyValuePair<int, int>>();
            Oldwave.Add(new KeyValuePair<int, int>((int)Math.Ceiling(box.x / 32d), (int)Math.Ceiling(box.y / 32d)));

            int nstep = 0;
            logicMap[(int)Math.Ceiling(box.x / 32d), (int)Math.Ceiling(box.y / 32d)] = nstep;
            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };
            while (Oldwave.Count > 0)
            {
                nstep++;
                wave.Clear();
                foreach (KeyValuePair<int, int> waveElement in Oldwave)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        int nx = waveElement.Key + dx[d];
                        int ny = waveElement.Value + dy[d];

                        if (logicMap[nx, ny] == -1)
                        {
                            wave.Add(new KeyValuePair<int, int>(nx, ny));
                            logicMap[nx, ny] = nstep;
                            if (nx == playerPosition[0] && ny == playerPosition[1])
                            {
                                goto done;
                            }
                        }
                    }
                }
                Oldwave.Clear();
                Oldwave.AddRange(wave);
            }
        done:;
            int x = playerPosition[0];
            int y = playerPosition[1];
            wave.Clear();
            wave.Add(new KeyValuePair<int, int>(x, y));
            while (logicMap[x, y] != 0)
            {
                for (int d = 0; d < 4; d++)
                {
                    int nx = x + dx[d];
                    int ny = y + dy[d];
                    if (logicMap[x, y] - 1 == logicMap[nx, ny])
                    {
                        x = nx;
                        y = ny;
                        wave.Add(new KeyValuePair<int, int>(x, y));
                        break;
                    }
                }
            }
        }

        public void SearchPlayersPosition(MainGame F)
        {
            PlayerTank player = F.player;

            playerPosition[0] = (int)Math.Ceiling(player.box.x / 32d);
            playerPosition[1] = (int)Math.Ceiling(player.box.y / 32d);

        }

        public void MoveToPlayer()
        {
            double startX = (wave[wave.Count - 1].Key - 1)*32 + 16;
            int startY = (wave[wave.Count - 1].Value - 1)*32 + 16;
            double nextX = 0;
            double nextY = 0;
            bool axis = false; //false - x, true - y
            if (wave.Count != 1)
            {
                double diffX = (wave[wave.Count - 2].Key - 1) * 32 + 16 - this.box.x;
                double diffY = (wave[wave.Count - 2].Value - 1) * 32 + 16 - this.box.y;
                nextX = (wave[wave.Count - 2].Key - 1) * 32 + 16;
                nextY = (wave[wave.Count - 2].Value - 1) * 32 + 16;

                double distX = nextX - startX;
                double distY = nextY - startY;
                double angle = Math.Atan2(distY, distX);
                if (angle > -0.3 && angle < 0.3)
                {
                    axis = true;
                    currentFrame.X = 3;
                }
                if (angle > Math.PI / 2 - 0.3 && angle < Math.PI / 2 + 0.3)
                {
                    axis = false;
                    currentFrame.X = 2;
                }
                if (angle > Math.PI - 0.3 && angle < Math.PI + 0.3)
                {
                    axis = true;
                    currentFrame.X = 1;
                }
                if (angle > 3 * Math.PI / 4 - 0.3 && angle < 3 * Math.PI / 4 + 0.3)
                {
                    axis = false;
                    currentFrame.X = 0;   //????????? ne robit, need to fix
                }
                if (axis)
                {
                    if (Math.Abs(diffY) > 1)
                    {
                        this.box.vy = diffY / Math.Abs(diffY) * 0.3;
                        this.box.vx = 0;
                    }
                    else
                    {
                        this.box.vx = diffX / Math.Abs(diffX) * 0.3;
                        this.box.vy = 0;
                    }
                }
                else
                {
                    if (Math.Abs(diffX)>1)
                    {
                        this.box.vx = diffX / Math.Abs(diffX) * 0.3;
                        this.box.vy = 0;
                    }
                    else
                    {
                        this.box.vy = diffY / Math.Abs(diffY) * 0.3;
                        this.box.vx = 0;
                    }
                }
            }
        }

         public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D pixel)
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
