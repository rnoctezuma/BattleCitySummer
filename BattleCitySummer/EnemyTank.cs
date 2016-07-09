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
        public Box enemyBox { get; set; }
        public int health { get; set; }
        public List<KeyValuePair<int, int>> wave = new List<KeyValuePair<int, int>>();  //note: wave normalize (wave.Key --, wave.Value --)
        public int[] playerPosition = new int[2];

        public EnemyTank(MainGame F, int x, int y)
        {
            this.enemyBox = new Box(x, y, 16, 16, 0, 0, true);
            health = 100;
            // shoot = false;
            // pos = 0;
            F.Boxes.Add(this.enemyBox);
        }

        public void Damage(int x)
        {
            if (health > 0)
            {
                health -= x;
            }
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
                }
            }
            return generatedMap;
        }

        public void FindOptimalPath(int[,] logicMap)
        {
            List<KeyValuePair<int, int>> Oldwave = new List<KeyValuePair<int, int>>();
            Oldwave.Add(new KeyValuePair<int, int>((int)Math.Ceiling(enemyBox.x / 32d), (int)Math.Ceiling(enemyBox.y / 32d)));

            int nstep = 0;
            logicMap[(int)Math.Ceiling(enemyBox.x / 32d), (int)Math.Ceiling(enemyBox.y / 32d)] = nstep;
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

            playerPosition[0] = (int)Math.Ceiling(player.playerBox.x / 32d);
            playerPosition[1] = (int)Math.Ceiling(player.playerBox.y / 32d);

        }

        public void MoveToPlayer()
        {
            int startX = wave[wave.Count - 1].Key - 1;
            int startY = wave[wave.Count - 1].Value - 1;
            if (wave.Count != 1)
            {

                /*
               if (wave[wave.Count - 2].Value - 1 < startY)
                    this.enemyBox.y -= 0.3;
                else if (wave[wave.Count - 2].Value - 1 > startY)
                    this.enemyBox.y += 0.3; 
                else if (wave[wave.Count - 2].Key - 1 < startX)
                    this.enemyBox.x -= 0.3;
                else if (wave[wave.Count - 2].Value - 1 > startX)
                    this.enemyBox.x += 0.3;        */
                if (wave[wave.Count - 2].Value - 1 < startY)
                {
                    this.enemyBox.x = wave[wave.Count - 2].Value;
                    this.enemyBox.y = wave[wave.Count - 2].Key;
                }
                else if (wave[wave.Count - 2].Value - 1 > startY)
                {
                    this.enemyBox.x = wave[wave.Count - 2].Value;
                    this.enemyBox.y = wave[wave.Count - 2].Key;
                }
                else if (wave[wave.Count - 2].Key - 1 < startX)
                {
                    this.enemyBox.x = wave[wave.Count - 2].Value;
                    this.enemyBox.y = wave[wave.Count - 2].Key;
                }
                else if (wave[wave.Count - 2].Value - 1 > startX)
                {
                    this.enemyBox.x = wave[wave.Count - 2].Value;
                    this.enemyBox.y = wave[wave.Count - 2].Key;
                }
            }
        }

        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {/*
            foreach (KeyValuePair<int, int> waveElement in wave)
            {
                DrawRectangle(new Rectangle((waveElement.Key - 1) * 512 / 16, (waveElement.Value - 1) * 512 / 16, 32, 32), Color.Green, graphics, spriteBatch, texture);
            }
            */

            DrawRectangle(new Rectangle((int)this.enemyBox.x - (int)this.enemyBox.width, (int)this.enemyBox.y - (int)this.enemyBox.height,
                (int)this.enemyBox.width * 2, (int)this.enemyBox.height * 2), Color.Red, graphics, spriteBatch, texture);
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {
            texture.SetData(new[] { color });
            spriteBatch.Draw(texture, coords, color);
        }
    }
}
