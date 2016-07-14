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
        private int[] TargerPosition = new int[2];
        private int chooseTargetPosition = 0;
        public bool destroy = false;
        private double randTimer = 10;
        private double animation = 0;
        private Texture2D Sprite { get; set; }
        private int frameWidth = 16;
        private int frameHeight = 16;
        private Point currentFrame = new Point(0, 0);
        private Point spriteSize = new Point(8, 1);
        private Random rand = new Random();

        

        public EnemyTank(MainGame F, int x, int y, Texture2D Sprite)
        {
            this.box = new Box(x, y, 14, 14, 0, 0, false);
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

        public void Update(MainGame mainGame, GameTime gameTime)
        {
     /*       if (testSpawn == 1)
            {
                EnemySpawn(mainGame);
                testSpawn = 0;
            }
            */
            int[,] logicMap = GetLogicMap(mainGame.map.map);
            /*   
                    if (gameTime.TotalGameTime.TotalSeconds <= 20)
                    {
                        if (randTimer >= 10)
                        {
                            RandomMove();
                            randTimer = 0;

                        }
                    }
                    */
            //     if (gameTime.TotalGameTime.TotalSeconds > 20 && gameTime.TotalGameTime.TotalSeconds <= 50)
    /*        {
                SearchTargetPosition(mainGame);
                FindOptimalPath(logicMap);
                MoveToTarget();
            }
      */      

       //     if (gameTime.TotalGameTime.TotalSeconds > 50)
 /*           {
                chooseTargetPosition = 1;
                SearchTargetPosition(mainGame);
                FindOptimalPath(logicMap);
                MoveToTarget();
            }
            if (randTimer < 11)
            {
                randTimer += 0.1;
            }
            
*/
            double angle = Math.Atan2(this.box.vy, this.box.vx);      //V METODU POTOM

            if (angle > -0.3 && angle < 0.3)
                currentFrame.X = 3;
            if (angle > Math.PI / 2 - 0.3 && angle < Math.PI / 2 + 0.3)
                currentFrame.X = 2;
            if (angle > Math.PI - 0.3 && angle < Math.PI + 0.3)
                currentFrame.X = 1;
            if (angle > -Math.PI / 2 - 0.3 && angle < -Math.PI / 2 + 0.3)
                currentFrame.X = 0;

            if (Math.Abs(this.box.vx) > 0 || Math.Abs(this.box.vy) > 0)
                this.animation += 0.1;
            if (this.animation >= 2)
                this.animation = 0;
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
                            if (nx == TargerPosition[0] && ny == TargerPosition[1])
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
            int x = TargerPosition[0];
            int y = TargerPosition[1];
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

        public void SearchTargetPosition(MainGame mainGame)
        {
            if (chooseTargetPosition == 0)
            {
                TargerPosition[0] = (int)Math.Ceiling(mainGame.player.box.x / 32d);
                TargerPosition[1] = (int)Math.Ceiling(mainGame.player.box.y / 32d);
            }
            else
            {
                TargerPosition[0] = 9;
                TargerPosition[1] = 16;
            }
        }

        public void MoveToTarget()
        {
            double startX = (wave[wave.Count - 1].Key - 1) * 32 + 16;
            int startY = (wave[wave.Count - 1].Value - 1) * 32 + 16;
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
                    axis = true;
                if (angle > Math.PI / 2 - 0.3 && angle < Math.PI / 2 + 0.3)
                    axis = false;
                if (angle > Math.PI - 0.3 && angle < Math.PI + 0.3)
                    axis = true;
                if (angle > 3 * Math.PI / 4 - 0.3 && angle < 3 * Math.PI / 4 + 0.3)
                    axis = false;
                if (axis)
                {
                    if (Math.Abs(diffY) > 1)
                    {
                        this.box.vy = diffY / Math.Abs(diffY) * 0.6;
                        this.box.vx = 0;
                    }
                    else
                    {
                        this.box.vx = diffX / Math.Abs(diffX) * 0.6;
                        this.box.vy = 0;
                    }
                }
                else
                {
                    if (Math.Abs(diffX) > 1)
                    {
                        this.box.vx = diffX / Math.Abs(diffX) * 0.6;
                        this.box.vy = 0;
                    }
                    else
                    {
                        this.box.vy = diffY / Math.Abs(diffY) * 0.6;
                        this.box.vx = 0;
                    }
                }
            }
        }

        public void RandomMove()
        {
            int direction = rand.Next(0, 100);
            if (direction >= 0 && direction <= 24)
            {
                this.box.vx = -0.6;
                this.box.vy = 0;
            }
            if (direction > 24 && direction <= 49)
            {
                this.box.vx = 0.6;
                this.box.vy = 0;
            }
            if (direction > 49 && direction <= 74)
            {
                this.box.vy = -0.6;
                this.box.vx = 0;
            }
            if (direction > 74)
            {
                this.box.vy = 0.6;
                this.box.vx = 0;
            }
        }

        public void EnemySpawn(MainGame mainGame)
        {
            /*
            int spawnPos = rand.Next(0, 18);
            int leftRight = rand.Next(0, 2);
            int playerPosX = (int)Math.Floor(mainGame.player.box.x / 32d);
            int playerPosY = (int)Math.Floor(mainGame.player.box.y / 32d);
            if (playerPosY == 0)
            {
                if (playerPosX == spawnPos)
                {
                    if (playerPosX == 0)
                        spawnPos = rand.Next(1, 18);
                    else if (playerPosX == 17)
                        spawnPos = rand.Next(0, 17);
                    else if(playerPosX != 0 && playerPosX != 17)
                    {
                        if (leftRight == 0)
                            spawnPos = rand.Next(0, playerPosX);
                        else
                            spawnPos = rand.Next(playerPosX + 1, 18);
                    }
                }
            }
            */
            mainGame.GameObjects.Add(new EnemyTank(mainGame, 100, 200, mainGame.Sprites[2]));
        }


        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(((int)this.box.x - (int)this.box.width - 1), (int)this.box.y - (int)this.box.height - 1),
                new Rectangle((currentFrame.X * 2 + (int)animation) * frameWidth,
                    currentFrame.Y * frameHeight,
                    frameWidth, frameHeight),
                Color.White, 0, Vector2.Zero,
                2, SpriteEffects.None, 0);
        }
    }
}
