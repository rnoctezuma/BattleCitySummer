using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleCitySummer
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;   //default graphics
        SpriteBatch spriteBatch; //graphics for sprites
        public MainGame game = new MainGame();
        public int widthWindow;
        public int heightWindow;

        public Game1() //Load <=> WinForms
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            widthWindow = Window.ClientBounds.Width;
            heightWindow = Window.ClientBounds.Height;
            this.game.Init();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            this.game.Update();
            BarrierCheck();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            this.game.Draw(graphics, spriteBatch);
            spriteBatch.End();
            //DRAW HERE
            base.Draw(gameTime);        
        }

        public void BarrierCheck()                  
        {
            if (this.game.player.playerBox.x < 0)
                this.game.player.playerBox.x = 0;
            if (this.game.player.playerBox.y < 0)
                this.game.player.playerBox.y = 0;
            if (this.game.player.playerBox.x > Window.ClientBounds.Width - this.game.player.playerBox.width)
                this.game.player.playerBox.x = Window.ClientBounds.Width - this.game.player.playerBox.width;
            if (this.game.player.playerBox.y > Window.ClientBounds.Height - this.game.player.playerBox.height)
                this.game.player.playerBox.y = Window.ClientBounds.Height - this.game.player.playerBox.height;
        }

    }
}
