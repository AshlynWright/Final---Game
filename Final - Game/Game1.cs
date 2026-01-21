// XNA / MonoGame-style version using Microsoft.Xna.Framework
// This is a simple two-screen game: Animal चयन -> Gender चयन
// Works in MonoGame DesktopGL project

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final___Game
{
    public enum ScreenState
    {
        AnimalSelect,
        MainGame
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ScreenState currentScreen = ScreenState.AnimalSelect;
        private string selectedAnimal = null;

        // Textures 
        private Texture2D txCow;
        private Texture2D txPig;
        private Texture2D txSheep;

        // Rectangles for click areas
        private Rectangle rctCow;
        private Rectangle rctPig;
        private Rectangle rctSheep;

        private MouseState previousMouse;

        // Main animal texture
        private Texture2D txSelectedAnimal;
        private Rectangle rctAnimalCenter;

        // Buttons
        private Texture2D txButton;
        private Rectangle[] leftButtons = new Rectangle[3];
        private Rectangle[] rightButtons = new Rectangle[3];

        // Health bars
        private Texture2D txWhitePixel;
        private float health = 100f;
        private float happiness = 75f;
        private float hunger = 50f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 400;
            _graphics.ApplyChanges();

            if (currentScreen == ScreenState.AnimalSelect)
            { 
                Window.Title = "Select Your Animal";
                // Animal positions
                rctCow = new Rectangle(50, 80, 120, 120);
                rctPig = new Rectangle(220, 80, 120, 120);
                rctSheep = new Rectangle(390, 80, 120, 120);
            }
            else if (currentScreen == ScreenState.MainGame)
            {
                Window.Title = $"You selected a {selectedAnimal}!";

                // Center animal
                rctAnimalCenter = new Rectangle(240, 140, 120, 120);

                // Left buttons
                for (int i = 0; i < 3; i++)
                {
                    leftButtons[i] = new Rectangle(40, 120 + i * 90, 120, 60);
                }

                // Right buttons
                for (int i = 0; i < 3; i++)
                {
                    rightButtons[i] = new Rectangle(440, 120 + i * 90, 120, 60);
                }
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load images from Content pipeline
            txCow = Content.Load<Texture2D>("cow_normal");
            txPig = Content.Load<Texture2D>("pig_normal");
            txSheep = Content.Load<Texture2D>("sheep_normal");
            // 1x1 white pixel for bars
            txWhitePixel = new Texture2D(GraphicsDevice, 1, 1);
            txWhitePixel.SetData(new[] { Color.White });

            // Simple button texture (make a gray rectangle image)
            //txButton = Content.Load<Texture2D>("button");
        }

        protected override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();

            // Detect left-click (pressed this frame)
            bool clicked = mouse.LeftButton == ButtonState.Pressed &&
                           previousMouse.LeftButton == ButtonState.Released;

            if (clicked)
            {
                Point pos = mouse.Position;

                if (currentScreen == ScreenState.AnimalSelect)
                {
                    if (rctCow.Contains(pos)) SelectAnimal("Cow");
                    else if (rctPig.Contains(pos)) SelectAnimal("Pig");
                    else if (rctSheep.Contains(pos)) SelectAnimal("Sheep");
                }
                else if (currentScreen == ScreenState.MainGame)
                {
                    // Left buttons
                    for (int i = 0; i < 3; i++)
                    {
                        if (leftButtons[i].Contains(pos))
                        {
                            OnLeftButtonPressed(i);
                            return;
                        }
                    }

                    // Right buttons
                    for (int i = 0; i < 3; i++)
                    {
                        if (rightButtons[i].Contains(pos))
                        {
                            OnRightButtonPressed(i);
                            return;
                        }
                    }
                }
            }

            previousMouse = mouse;
            base.Update(gameTime);
        }

        private void SelectAnimal(string animal)
        {
            selectedAnimal = animal;

            if (animal == "Cow")
                txSelectedAnimal = txCow;
            else if (animal == "Pig")
                txSelectedAnimal = txPig;
            else if (animal == "Sheep")
                txSelectedAnimal = txSheep;

            currentScreen = ScreenState.MainGame;
        }

        private void MainGame(string animal)
        {
            if (currentScreen == ScreenState.MainGame)
            {
                // Draw top bars
                DrawBar(50, 20, 150, 20, health, "Health");
                DrawBar(230, 20, 150, 20, happiness, "Happiness");
                DrawBar(410, 20, 150, 20, hunger, "Hunger");

                // Draw center animal
                _spriteBatch.Draw(txSelectedAnimal, rctAnimalCenter, Color.White);

                // Draw left buttons
                for (int i = 0; i < 3; i++)
                    _spriteBatch.Draw(txButton, leftButtons[i], Color.LightGray);

                // Draw right buttons
                for (int i = 0; i < 3; i++)
                    _spriteBatch.Draw(txButton, rightButtons[i], Color.LightGray);
            }
        }
        private void DrawBar(int x, int y, int width, int height, float value, string label)
        {
            // Background
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x, y, width, height), Color.DarkGray);

            // Filled part
            int filledWidth = (int)(width * (value / 100f));
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x, y, filledWidth, height), Color.Green);

            // You can later add SpriteFont text here for labels
        }

        private void OnLeftButtonPressed(int index)
        {
            // Example effects
            if (index == 0) health = Math.Min(100, health + 10);      // Heal
            if (index == 1) happiness = Math.Min(100, happiness + 10); // Play
            if (index == 2) hunger = Math.Max(0, hunger - 10);        // Feed
        }

        private void OnRightButtonPressed(int index)
        {
            if (index == 0) health = Math.Max(0, health - 10);
            if (index == 1) happiness = Math.Max(0, happiness - 10);
            if (index == 2) hunger = Math.Min(100, hunger + 10);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (currentScreen == ScreenState.AnimalSelect)
            {
                _spriteBatch.Draw(txCow, rctCow, Color.White);
                _spriteBatch.Draw(txPig, rctPig, Color.White);
                _spriteBatch.Draw(txSheep, rctSheep, Color.White);
            }
            else if (currentScreen == ScreenState.MainGame)
            {

            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
