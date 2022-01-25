using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Collision_detection_with_rectangles
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        KeyboardState keyboardState;
        MouseState mouseState;

        SoundEffect coinPickup;
        SoundEffectInstance coinPickupInstance;
        Texture2D cherryTexture;
        Rectangle cherryRect;
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacUpTexture;
        Texture2D pacDownTexture;
        Texture2D pacSleepTexture;
        Texture2D currentPacTexture; 
        Rectangle pacRect;
        Texture2D exitTexture;
        Rectangle exitRect;
        Texture2D barrierTexture;
        List<Rectangle> barriers;
        Texture2D coinTexture;
        List<Rectangle> coins;
        int pacSpeed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            

            base.Initialize();
            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);
            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(0, 250, 350, 75));
            barriers.Add(new Rectangle(450, 250, 350, 75));
            exitRect = new Rectangle(700, 380, 100, 100);
            coins = new List<Rectangle>();
            coins.Add(new Rectangle(400, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 340, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(400, 300, coinTexture.Width, coinTexture.Height));
            cherryRect = new Rectangle(375, 275, 50, 50);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            pacDownTexture = Content.Load<Texture2D>("pacDown");
            pacUpTexture = Content.Load<Texture2D>("pacUp");
            pacRightTexture = Content.Load<Texture2D>("pacRight");
            pacLeftTexture = Content.Load<Texture2D>("pacLeft");
            pacSleepTexture = Content.Load<Texture2D>("pacSleep"); 
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
            exitTexture = Content.Load<Texture2D>("hobbit_door");
            coinTexture = Content.Load<Texture2D>("coin");
            cherryTexture = Content.Load<Texture2D>("Cherry");
            coinPickup = Content.Load<SoundEffect>("Coin Sound");
            coinPickupInstance = coinPickup.CreateInstance();
            currentPacTexture = pacSleepTexture;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.Right) != true)
                currentPacTexture = pacSleepTexture;

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Right;
                    }
                if (pacRect.Left <= 0)
                {
                    pacRect.X += pacRect.Width;
                }
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Left - pacRect.Width;
                    }
                if (pacRect.Right >= _graphics.PreferredBackBufferWidth)
                {
                    pacRect.X -= pacRect.Width;
                }
            }
            
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Bottom;
                    }
                if (pacRect.Top <= 0)
                {
                    pacRect.Y += pacRect.Height;
                }

            }   
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Top - pacRect.Height;
                    }
                if (pacRect.Bottom >= _graphics.PreferredBackBufferHeight)
                {
                    pacRect.Y -= pacRect.Height;
                }
            }
            

            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;
                    coinPickupInstance.Play();
                }
            }

            if (exitRect.Contains(pacRect))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    if (exitRect.Contains(mouseState.X, mouseState.Y))
                        Exit();
            }


            if (pacRect.Intersects(cherryRect))
            { 
                pacSpeed = 6;
                cherryRect = new Rectangle(0, 0, 0, 0);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);
            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);
            _spriteBatch.Draw(cherryTexture, cherryRect, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
