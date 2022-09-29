using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Lab6
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private KeyboardState _keyboardState;

        Texture2D ballTexture;
        Texture2D charTexture;
        Vector2 charPosition = new Vector2(0,250);
        Vector2[] ballPosition = new Vector2 [4];
        int[] ballColor = new int[6];
        Random r = new Random();
        bool personHit;

        int direction = 0;
        int speed = 4;
        int frame;
        int totalFrame;
        int framePerSec;
        float timePerFrame;
        float totalElapsed;

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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            charTexture = Content.Load<Texture2D>("Char01");
            frame = 0;
            totalFrame = 4;
            framePerSec = 12;
            timePerFrame = (float)1 / framePerSec;
            totalElapsed = 0;

            ballTexture = Content.Load<Texture2D>("ball");


            for (int i = 0; i < 4; i++) {
                ballPosition[i].Y = r.Next(0, _graphics.GraphicsDevice.Viewport.Height - ballTexture.Height);
                ballPosition[i].X = r.Next(0, _graphics.GraphicsDevice.Viewport.Width - ballTexture.Width/6);
                ballColor[i] = r.Next(6);
            }
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            GraphicsDevice device = _graphics.GraphicsDevice; 
           
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState _keyboardState = Keyboard.GetState();
            if (_keyboardState.IsKeyDown(Keys.Down))
            {
                direction = 0;
                charPosition.Y = charPosition.Y + speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (_keyboardState.IsKeyDown(Keys.Up))
            {
                direction = 3;
                charPosition.Y = charPosition.Y - speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (_keyboardState.IsKeyDown(Keys.Left))
            {
                direction = 1;
                charPosition.X = charPosition.X - speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (_keyboardState.IsKeyDown(Keys.Right))
            {
                direction = 2;
                charPosition.X = charPosition.X + speed;
                UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            Rectangle charRectangle = new Rectangle((int)charPosition.X, (int)charPosition.Y, 32, 48);
            for (int i = 0; i < 4; i++)
            {
                Rectangle blockRectangle = new Rectangle((int)ballPosition[i].X, (int)ballPosition[i].Y, 24, 24);

                if (charRectangle.Intersects(blockRectangle) == true)
                {
                    personHit = true;

                    ballPosition[i].Y = r.Next(0, _graphics.GraphicsDevice.Viewport.Height - ballTexture.Height);
                    ballPosition[i].X = r.Next(0, _graphics.GraphicsDevice.Viewport.Width - ballTexture.Width);
                    ballColor[i] = r.Next(6);
                    break;
                }
                else if (charRectangle.Intersects(blockRectangle) == false)
                {
                    personHit = false;
                }

            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice device = _graphics.GraphicsDevice;
            if (personHit == true) 
            { 
                device.Clear(Color.Red); 
            }
            else 
            { 
                device.Clear(Color.CornflowerBlue);
            }
            _spriteBatch.Begin();

            for (int i = 0; i < 4; i++) {
                _spriteBatch.Draw(ballTexture, ballPosition[i], new Rectangle(24 * ballColor[i], 0, 24, 24), Color.White);
            }
            _spriteBatch.Draw(charTexture, charPosition, new Rectangle(32*frame, 48 * direction, 32, 48), Color.White); 
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        void UpdateFrame(float elapsed)
        {
            totalElapsed += elapsed;
            if (totalElapsed > timePerFrame)
            {
                frame = (frame + 1) % totalFrame;
                totalElapsed -= timePerFrame;
            }
        }
    }
}
