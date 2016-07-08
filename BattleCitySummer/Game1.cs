using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleCitySummer
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;   //default graphics
        SpriteBatch spriteBatch; //graphics for sprites
        //  public MainGame game = new MainGame();                where's right locate?
     //   public Texture2D texture;

        public Game1() //Load <=> WinForms
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);     
            //   this.game.Init();
            ScreenManager.Instance.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            ScreenManager.Instance.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
       //     this.game.Update();
        //    BarrierCheck();

            ScreenManager.Instance.Update(gameTime);
            //UPDATE HERE
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            ScreenManager.Instance.Draw(spriteBatch, graphics);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
