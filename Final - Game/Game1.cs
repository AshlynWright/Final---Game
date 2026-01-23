// XNA / MonoGame-style version using Microsoft.Xna.Framework
// This is a simple two-screen game: Animal चयन -> Gender चयन
// Works in MonoGame DesktopGL project

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        private Texture2D txFeed;
        private Texture2D txClean;
        private Texture2D txPlay;
        private Texture2D txWalk;
        private Texture2D txPet;
        private Texture2D txBrush;

        // Bar icons
        private Texture2D txHealthIcon;
        private Texture2D txEnergyIcon;
        private Texture2D txHungerIcon;
        private Texture2D txLoveIcon;

        // Left side actions
        private Rectangle feedButton;
        private Rectangle cleanButton;
        private Rectangle playButton;

        // Right side actions
        private Rectangle walkButton;
        private Rectangle petButton;
        private Rectangle brushButton;

        private SpriteFont uiFont;

        // Health bars
        private Texture2D txWhitePixel;
        private float health = 100f;
        private float energy = 50f;
        private float hunger = 50f;
        private float love = 20f;

        // Drain rates (points per second)
        private float hungerDrainRate = 2f;      // gets hungry
        private float energyDrainRate = 1f;      // gets bored
        private float loveDrainRate = 0.5f;      // feels neglected
        private float healthDrainRate = 0.3f;    // slowly declines

        // Pet naming
        private string petName = "";
        private bool enteringName = true;

        private SpriteFont nameFont;

        // Keyboard input tracking
        private KeyboardState previousKeyboard;

        private int feedStreak = 0;
        // Play combo tracking

        private int playStreak = 0;
        private Random rng = new Random();

        // Play-mood textures (up to 5 each)
        private Texture2D[] cowPlayTextures = new Texture2D[5];
        private Texture2D[] pigPlayTextures = new Texture2D[5];
        private Texture2D[] sheepPlayTextures = new Texture2D[5];

        private Texture2D txCowAngry;
        private Texture2D txPigAngry;
        private Texture2D txSheepAngry;
        private Texture2D txCowAnnoyed;
        private Texture2D txPigAnnoyed;
        private Texture2D txSheepAnnoyed;
        private Texture2D txCowCrying;
        private Texture2D txPigCrying;
        private Texture2D txSheepCrying;
        private Texture2D txCowKisses;
        private Texture2D txPigKisses;
        private Texture2D txSheepKisses;
        private Texture2D txCowLove;
        private Texture2D txPigLove;
        private Texture2D txSheepLove;
        private Texture2D txCowMad;
        private Texture2D txPigMad;
        private Texture2D txSheepMad;
        private Texture2D txCowSad;
        private Texture2D txPigSad;
        private Texture2D txSheepSad;
        private Texture2D txCowSick;
        private Texture2D txPigSick;
        private Texture2D txSheepSick;
        private Texture2D txCowSilly;
        private Texture2D txPigSilly;
        private Texture2D txSheepSilly;
        private Texture2D txCowSinging;
        private Texture2D txPigSinging;
        private Texture2D txSheepSinging;
        private Texture2D txCowSurprised;
        private Texture2D txPigSurprised;
        private Texture2D txSheepSurprised;

        SoundEffect seCow;
        SoundEffect sePig;
        SoundEffect seSheep;
        SoundEffect seCowSick;
        SoundEffect sePigSick;
        SoundEffect seSheepSick;


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

            Window.Title = "Select Your Animal";

            // Animal select positions
            rctCow = new Rectangle(50, 80, 120, 120);
            rctPig = new Rectangle(220, 80, 120, 120);
            rctSheep = new Rectangle(390, 80, 120, 120);

            // MainGame center animal
            rctAnimalCenter = new Rectangle(200, 180, 200, 200);

            // Left buttons (64x64)
            feedButton = new Rectangle(30, 120, 64, 64);
            cleanButton = new Rectangle(30, 210, 64, 64);
            playButton = new Rectangle(30, 300, 64, 64);

            // Right buttons (64x64)
            walkButton = new Rectangle(506, 120, 64, 64);
            petButton = new Rectangle(506, 210, 64, 64);
            brushButton = new Rectangle(506, 300, 64, 64);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load images for Animal

            //Normal
            txCow = Content.Load<Texture2D>("cow_normal");
            txPig = Content.Load<Texture2D>("pig_normal");
            txSheep = Content.Load<Texture2D>("sheep_normal");

            txCowAngry = Content.Load<Texture2D>("cow_angry");
            txPigAngry = Content.Load<Texture2D>("pig_angry");
            txSheepAngry = Content.Load<Texture2D>("sheep_angry");
            txCowAnnoyed = Content.Load<Texture2D>("cow_annoyed");
            txPigAnnoyed = Content.Load<Texture2D>("pig_annoyed");
            txSheepAnnoyed = Content.Load<Texture2D>("sheep_annoyed");
            txCowCrying = Content.Load<Texture2D>("cow_crying");
            txPigCrying = Content.Load<Texture2D>("pig_crying");
            txSheepCrying = Content.Load<Texture2D>("sheep_crying");
            txCowKisses = Content.Load<Texture2D>("cow_kisses");
            txPigKisses = Content.Load<Texture2D>("pig_kisses");
            txSheepKisses = Content.Load<Texture2D>("sheep_kisses");
            txCowLove = Content.Load<Texture2D>("cow_love");
            txPigLove = Content.Load<Texture2D>("pig_love");
            txSheepLove = Content.Load<Texture2D>("sheep_love");
            txCowMad = Content.Load<Texture2D>("cow_mad");
            txPigMad = Content.Load<Texture2D>("pig_mad");
            txSheepMad = Content.Load<Texture2D>("sheep_mad");
            txCowSad = Content.Load<Texture2D>("cow_sad");
            txPigSad = Content.Load<Texture2D>("pig_sad");
            txSheepSad = Content.Load<Texture2D>("sheep_sad");
            txCowSick = Content.Load<Texture2D>("cow_sick");
            txPigSick = Content.Load<Texture2D>("pig_sick");
            txSheepSick = Content.Load<Texture2D>("sheep_sick");
            txCowSilly = Content.Load<Texture2D>("cow_silly");
            txPigSilly = Content.Load<Texture2D>("pig_silly");
            txSheepSilly = Content.Load<Texture2D>("sheep_silly");
            txCowSinging = Content.Load<Texture2D>("cow_singing");
            txPigSinging = Content.Load<Texture2D>("pig_singing");
            txSheepSinging = Content.Load<Texture2D>("sheep_singing");
            txCowSurprised = Content.Load<Texture2D>("cow_surprised");
            txPigSurprised = Content.Load<Texture2D>("pig_surprised");
            txSheepSurprised = Content.Load<Texture2D>("sheep_surprised");

            // 1x1 white pixel for bars
            txWhitePixel = new Texture2D(GraphicsDevice, 1, 1);
            txWhitePixel.SetData(new[] { Color.White });

            // Interaction Buttons
            txFeed = Content.Load<Texture2D>("feed");
            txClean = Content.Load<Texture2D>("clean");
            txPlay = Content.Load<Texture2D>("play");
            txWalk = Content.Load<Texture2D>("walk");
            txPet = Content.Load<Texture2D>("pet");
            txBrush = Content.Load<Texture2D>("brush");

            txHealthIcon = Content.Load<Texture2D>("icon_health");
            txEnergyIcon = Content.Load<Texture2D>("icon_energy");
            txHungerIcon = Content.Load<Texture2D>("icon_hunger");
            txLoveIcon = Content.Load<Texture2D>("icon_love");

            uiFont = Content.Load<SpriteFont>("UIFont");
            nameFont = Content.Load<SpriteFont>("NameFont");

            // Cow play moods
            cowPlayTextures[0] = Content.Load<Texture2D>($"cow_annoyed");
            cowPlayTextures[1] = Content.Load<Texture2D>($"cow_angry");
            cowPlayTextures[2] = Content.Load<Texture2D>($"cow_surprised");
            cowPlayTextures[3] = Content.Load<Texture2D>($"cow_kisses");
            cowPlayTextures[4] = Content.Load<Texture2D>($"cow_singing");


            // Pig play moods
            pigPlayTextures[0] = Content.Load<Texture2D>($"pig_annoyed");
            pigPlayTextures[1] = Content.Load<Texture2D>($"pig_angry");
            pigPlayTextures[2] = Content.Load<Texture2D>($"pig_surprised");
            pigPlayTextures[3] = Content.Load<Texture2D>($"pig_kisses");
            pigPlayTextures[4] = Content.Load<Texture2D>($"pig_singing");

            // Sheep play moods
            sheepPlayTextures[0] = Content.Load<Texture2D>($"sheep_annoyed");
            sheepPlayTextures[1] = Content.Load<Texture2D>($"sheep_angry");
            sheepPlayTextures[2] = Content.Load<Texture2D>($"sheep_surprised");
            sheepPlayTextures[3] = Content.Load<Texture2D>($"sheep_kisses");
            sheepPlayTextures[4] = Content.Load<Texture2D>($"sheep_singing");

            seCow = Content.Load<SoundEffect>("cow_sound");
            sePig = Content.Load<SoundEffect>("pig_sound");
            seSheep = Content.Load<SoundEffect>("sheep_sound");
            seCowSick = Content.Load<SoundEffect>("cow_sick_sound");
            sePigSick = Content.Load<SoundEffect>("pig_sick_sound");
            seSheepSick = Content.Load<SoundEffect>("sheep_sick_sound");
        }

        protected override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();

            // Edge-triggered click: only true on the frame the button is first pressed
            bool clicked =
                mouse.LeftButton == ButtonState.Pressed &&
                previousMouse.LeftButton == ButtonState.Released;

            if (clicked)
            {
                Point pos = mouse.Position;

                if (currentScreen == ScreenState.AnimalSelect)
                {
                    if (!enteringName)
                    {
                        if (rctCow.Contains(pos))
                        {
                            SelectAnimal("Cow");
                            seCow.Play();
                        }
                        else if (rctPig.Contains(pos))
                        {
                            SelectAnimal("Pig");
                            sePig.Play();
                        }
                        else if (rctSheep.Contains(pos))
                        {
                            SelectAnimal("Sheep");
                            seSheep.Play();
                        }

                    }
                }
                else if (currentScreen == ScreenState.MainGame)
                {
                    // Left buttons
                    if (feedButton.Contains(pos))
                    {
                        OnLeftButtonPressed("Feed");
                    }
                    else if (cleanButton.Contains(pos))
                    {
                        OnLeftButtonPressed("Clean");
                    }
                    else if (playButton.Contains(pos))
                    {
                        OnLeftButtonPressed("Play");
                    }

                    // Right buttons
                    if (walkButton.Contains(pos))
                    {
                        OnRightButtonPressed("Walk");
                    }
                    else if (petButton.Contains(pos))
                    {
                        OnRightButtonPressed("Pet");
                    }
                    else if (brushButton.Contains(pos))
                    {
                        OnRightButtonPressed("Brush");
                    }
                }
            }

            if (currentScreen == ScreenState.MainGame)
            {
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Drain stats over time
                hunger -= hungerDrainRate * dt;
                energy -= energyDrainRate * dt;
                love -= loveDrainRate * dt;

                // Health depends on other stats
                if (hunger <= 10f || energy <= 10f || love <= 10f)
                    health -= 5f * dt;   // fast health loss
                else
                    health -= 0.3f * dt; // slow health loss

                // Clamp all stats to 0–100
                hunger = MathHelper.Clamp(hunger, 0f, 100f);
                energy = MathHelper.Clamp(energy, 0f, 100f);
                love = MathHelper.Clamp(love, 0f, 100f);
                health = MathHelper.Clamp(health, 0f, 100f);
            }

            var keyboard = Keyboard.GetState();

            if (currentScreen == ScreenState.AnimalSelect && enteringName)
            {
                foreach (Keys key in keyboard.GetPressedKeys())
                {
                    if (!previousKeyboard.IsKeyDown(key))
                    {
                        // A–Z
                        if (key >= Keys.A && key <= Keys.Z && petName.Length < 12)
                        {
                            petName += key.ToString();
                        }

                        // Space
                        if (key == Keys.Space && petName.Length < 12)
                        {
                            petName += " ";
                        }

                        // Backspace
                        if (key == Keys.Back && petName.Length > 0)
                        {
                            petName = petName.Substring(0, petName.Length - 1);
                        }

                        // Enter finishes naming
                        if (key == Keys.Enter && petName.Length > 0)
                        {
                            enteringName = false;
                        }
                    }
                }
            }

            previousKeyboard = keyboard;

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

        }

        private void DrawBar(int x, int y, int width, int height, float value)
        {
            // Clamp value to 0–100
            value = MathHelper.Clamp(value, 0f, 100f);

            // Choose color based on value
            Color barColor;
            if (value >= 66f)
                barColor = Color.LimeGreen;
            else if (value >= 33f)
                barColor = Color.Gold;
            else
                barColor = Color.IndianRed;

            // Background
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x, y, width, height), Color.DarkGray);

            // Filled part
            int filledWidth = (int)(width * (value / 100f));
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x, y, filledWidth, height), barColor);

            // Border (optional but looks nice)
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x - 1, y - 1, width + 2, 1), Color.Black);
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x - 1, y + height, width + 2, 1), Color.Black);
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x - 1, y, 1, height), Color.Black);
            _spriteBatch.Draw(txWhitePixel, new Rectangle(x + width, y, 1, height), Color.Black);

            // Numeric text: "75 / 100"
            string text = $"{(int)value} / 100";
            Vector2 textSize = uiFont.MeasureString(text);

            Vector2 textPos = new Vector2(x + (width - textSize.X) / 2f, y + (height - textSize.Y) / 2f);

            _spriteBatch.DrawString(uiFont, text, textPos, Color.Black);
        }

        private void OnLeftButtonPressed(string button)
        {
            switch (button)
            {
                case "Feed":
                    playStreak = 0;
                    hunger = Math.Max(0, hunger + 15);
                    energy = Math.Min(100, energy + 10);
                    feedStreak += 1;


                    if (feedStreak >= 3)
                    {
                        SwitchToSickTexture();
                    }

                    break;
                case "Clean":

                    feedStreak = 0;
                    playStreak = 0;
                    love = Math.Min(100, love + 10);
                    health = Math.Min(100, health + 10);
                    SwitchToNormalTexture();
                    break;

                case "Play":

                    feedStreak = 0;
                    energy = Math.Min(100, energy - 20);
                    hunger = Math.Min(100, hunger - 15);
                    playStreak++;

                    if (playStreak >= 1)
                    {
                        SwitchToRandomPlayTexture();
                    }

                    break;
            }
        }

        private void OnRightButtonPressed(string button)
        {

            feedStreak = 0;
            playStreak = 0;

            switch (button)
            {
                case "Walk":
                    health = Math.Min(100, health + 5);
                    energy = Math.Min(100, energy - 15);
                    hunger = Math.Min(100, hunger - 10);
                    break;

                case "Pet":
                    energy = Math.Min(100, energy + 5);
                    love = Math.Min(100, love + 10);
                    break;

                case "Brush":
                    love = Math.Min(100, love + 5);
                    break;
            }
        }
        private void SwitchToNormalTexture()
        {
            if (selectedAnimal == "Cow")
            {
                txSelectedAnimal = txCow;
                seCow.Play();
            }
            else if (selectedAnimal == "Pig")
            {
                txSelectedAnimal = txPig;
                sePig.Play();
            }
            else if (selectedAnimal == "Sheep")
            {
                txSelectedAnimal = txSheep;
                seSheep.Play();
            }
        }

        private void SwitchToSickTexture()
        {
            if (selectedAnimal == "Cow")
            {
                txSelectedAnimal = txCowSick;
                seCowSick.Play();
            }
            else if (selectedAnimal == "Pig")
            {
                txSelectedAnimal = txPigSick;
                sePigSick.Play();
            }
            else if (selectedAnimal == "Sheep")
            {
                txSelectedAnimal = txSheepSick;
                seSheepSick.Play();
            }
        }

        private void SwitchToRandomPlayTexture()
        {
            int index = rng.Next(0, 5); // 0–4

            if (selectedAnimal == "Cow")
                txSelectedAnimal = cowPlayTextures[index];
            else if (selectedAnimal == "Pig")
                txSelectedAnimal = pigPlayTextures[index];
            else if (selectedAnimal == "Sheep")
                txSelectedAnimal = sheepPlayTextures[index];
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (currentScreen == ScreenState.AnimalSelect)
            {
                int animalsLeftX = rctCow.X;
                int animalsRightX = rctSheep.Right;
                int animalsWidth = animalsRightX - animalsLeftX;

                int nameBoxWidth = 300;
                int nameBoxHeight = 30;

                // Center name box under animals
                int nameBoxX = animalsLeftX + (animalsWidth - nameBoxWidth) / 2;
                int nameBoxY = rctCow.Bottom + 60;

                _spriteBatch.Draw(txCow, rctCow, Color.White);
                _spriteBatch.Draw(txPig, rctPig, Color.White);
                _spriteBatch.Draw(txSheep, rctSheep, Color.White);

                // Prompt text
                string prompt = "Name your animal:";
                Vector2 promptSize = uiFont.MeasureString(prompt);

                Vector2 promptPos = new Vector2(
                    animalsLeftX + (animalsWidth - promptSize.X) / 2f,
                    nameBoxY - promptSize.Y - 6
                );

                _spriteBatch.DrawString(uiFont, prompt, promptPos, Color.Black);

                // Name entry box
                _spriteBatch.Draw(txWhitePixel, new Rectangle(nameBoxX, nameBoxY, nameBoxWidth, nameBoxHeight), Color.White);

                // Border
                _spriteBatch.Draw(txWhitePixel, new Rectangle(nameBoxX - 1, nameBoxY - 1, nameBoxWidth + 2, 1), Color.Black);
                _spriteBatch.Draw(txWhitePixel, new Rectangle(nameBoxX - 1, nameBoxY + nameBoxHeight, nameBoxWidth + 2, 1), Color.Black);
                _spriteBatch.Draw(txWhitePixel, new Rectangle(nameBoxX - 1, nameBoxY, 1, nameBoxHeight), Color.Black);
                _spriteBatch.Draw(txWhitePixel, new Rectangle(nameBoxX + nameBoxWidth, nameBoxY, 1, nameBoxHeight), Color.Black);

                // Typed name + cursor
                string displayName = petName + (enteringName ? "|" : "");
                Vector2 textSize = uiFont.MeasureString(displayName);

                Vector2 textPos = new Vector2(
                    nameBoxX + 8, nameBoxY + (nameBoxHeight - textSize.Y) / 2f
                );

                _spriteBatch.DrawString(uiFont, displayName, textPos, Color.Black);

                // Hint
                if (enteringName)
                {
                    string hint = "Press Enter when done";
                    Vector2 hintSize = uiFont.MeasureString(hint);

                    Vector2 hintPos = new Vector2(animalsLeftX + (animalsWidth - hintSize.X) / 2f, nameBoxY + nameBoxHeight + 8);

                    _spriteBatch.DrawString(uiFont, hint, hintPos, Color.Black);
                }
            }

            else if (currentScreen == ScreenState.MainGame)
            {
                int barWidth = 180;
                int barHeight = 18;

                // Row 1
                DrawBar(60, 20, barWidth, barHeight, health);     // Health
                DrawBar(330, 20, barWidth, barHeight, energy);  // Energy

                // Row 2
                DrawBar(60, 45, barWidth, barHeight, hunger);     // Hunger
                DrawBar(330, 45, barWidth, barHeight, love);       // Love

                // Row 1 icons
                _spriteBatch.Draw(txHealthIcon, new Rectangle(25, 18, 24, 24), Color.White);
                _spriteBatch.Draw(txEnergyIcon, new Rectangle(295, 18, 24, 24), Color.White);

                // Row 2 icons
                _spriteBatch.Draw(txHungerIcon, new Rectangle(25, 43, 24, 24), Color.White);
                _spriteBatch.Draw(txLoveIcon, new Rectangle(295, 43, 24, 24), Color.White);


                // Draw center animal
                _spriteBatch.Draw(txSelectedAnimal, rctAnimalCenter, Color.White);

                _spriteBatch.Draw(txFeed, feedButton, Color.White);
                _spriteBatch.Draw(txClean, cleanButton, Color.White);
                _spriteBatch.Draw(txPlay, playButton, Color.White);

                _spriteBatch.Draw(txWalk, walkButton, Color.White);
                _spriteBatch.Draw(txPet, petButton, Color.White);
                _spriteBatch.Draw(txBrush, brushButton, Color.White);

                // Draw pet name above the animal (big + white)
                if (!string.IsNullOrEmpty(petName))
                {
                    Vector2 nameSize = nameFont.MeasureString(petName);

                    Vector2 namePos = new Vector2(
                        rctAnimalCenter.X + (rctAnimalCenter.Width - nameSize.X) / 2f,
                        rctAnimalCenter.Y - nameSize.Y - 30
                    );

                    // Soft shadow for contrast
                    _spriteBatch.DrawString(nameFont, petName, namePos + new Vector2(2, 2), Color.Black * 0.6f);

                    // Main white text
                    _spriteBatch.DrawString(nameFont, petName, namePos, Color.White);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
