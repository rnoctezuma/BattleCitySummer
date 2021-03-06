﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCitySummer
{
    public class Base : IGameObject
    {
        public Box box;
        public Texture2D Sprite { get; set; }
        public int health = 2;
        private int frameWidth = 16;
        private int frameHeight = 16;
        private Point currentFrame = new Point(0, 0);
        private Point spriteSize = new Point(4, 1);
        public bool destroy = false;

        public Base(MainGame F, double x, double y, double w, double h, Texture2D Sprite)
        {
            this.box = new Box(x, y, w, h, 0, 0, true);
            this.Sprite = Sprite;
            F.Boxes.Add(this.box);
        }
        public bool isDestroyed()
        {
            return destroy;
        }
        public void Destroy()
        {
            destroy = true;
            this.box.destroy = true;
        }
        public void Damage()
        {
            this.health--;
        }

        public void Update(MainGame mainGame, GameTime gameTime)
        {
            if (this.health < 0)
            {
                this.Destroy();
                mainGame.isGameover = true;
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
