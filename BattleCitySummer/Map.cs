using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class Map
    {      
        const int WALL = 9999;
        const int N = 16;
        public int[,] map = new int[N, N];
        List<KeyValuePair<int, int>> wave = new List<KeyValuePair<int, int>>();
        public void Update()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    map[i, j] = -1;
                }
            }
            map[3, 4] = WALL;
            map[4, 4] = WALL;
            map[10, 10] = WALL;
            map[4, 1] = WALL;

            for (int i = 0; i < N; i++)
            {
                map[i, 0] = WALL;
                map[0, i] = WALL;
                map[i, N - 1] = WALL;
                map[N - 1, i] = WALL;
            }
                       
           List<KeyValuePair<int, int>> Oldwave = new List<KeyValuePair<int, int>>();
            Oldwave.Add(new KeyValuePair<int, int>(1, 1));

            int nstep = 0;
            map[1, 1] = nstep;
            int[] dx = { 0, 1, 0, -1 };
            int[] dy = { -1, 0, 1, 0 };
            while (Oldwave.Count > 0)
            {
                ++nstep;
                wave.Clear();
                foreach (KeyValuePair<int, int> waveElement in Oldwave)
                {
                    for (int d = 0; d < 4; d++)
                    {
                        int nx = waveElement.Key + dx[d];
                        int ny = waveElement.Value + dy[d];

                        if (map[nx, ny] == -1)
                        {
                            wave.Add(new KeyValuePair<int, int>(nx, ny));
                            map[nx, ny] = nstep;
                            if (nx == N - 2 && ny == N - 2)
                            {
                                goto done;
                            }
                        }
                    }
                }
                Oldwave.Clear();
                Oldwave.AddRange(wave);             
            }
        done:
            
            int x = N - 2;
            int y = N - 2;
            wave.Clear();
            wave.Add(new KeyValuePair<int, int>(x, y));
            while (map[x,y] !=0)
            {
                for (int d = 0; d < 4; d++)
                {
                    int nx = x + dx[d];
                    int ny = y + dy[d];
                    if (map[x, y] - 1 == map[nx, ny])
                    {
                        x = nx;
                        y = ny;
                        wave.Add(new KeyValuePair<int, int>(x, y));
                        break;
                    }
                }
            }          
        }


        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (map[i, j] == WALL)
                    {
                        DrawRectangle(new Rectangle(i * 512 / 16, j * 512 / 16, 32, 32), Color.Black, graphics, spriteBatch, texture);
                    }                
                }
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (map[i, j] == -1)
                    {
                        DrawRectangle(new Rectangle(i * 512 / 16, j * 512 / 16, 32, 32), Color.Green, graphics, spriteBatch, texture);
                    }
                }
            }


            foreach (KeyValuePair<int, int> waveElement in wave)
            {
                DrawRectangle(new Rectangle(waveElement.Key * 512 / 16, waveElement.Value * 512 / 16, 32, 32), Color.Red, graphics, spriteBatch, texture);
            }
        }

        public void DrawRectangle(Rectangle coords, Color color, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, Texture2D texture)
        {
            texture.SetData(new[] { color });
            spriteBatch.Draw(texture, coords, color);
        }
    }

}
