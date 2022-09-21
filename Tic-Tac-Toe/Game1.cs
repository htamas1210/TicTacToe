using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Tic_Tac_Toe {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RectangleF[,] rectArray;
        private int[] CircleXPostion;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            //Setting Resolution
            this._graphics.PreferredBackBufferWidth = 800;
            this._graphics.PreferredBackBufferHeight = 600;
            this._graphics.ApplyChanges();

            //For field
            rectArray = new RectangleF[3,3];

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            
            DrawTicTacToeFieldRect(rectArray, Color.White);
            for (int i = 0; i < rectArray.GetLength(0); i++) {
                for (int j = 0; j < rectArray.GetLength(1); j++) {
                    var mouse = Mouse.GetState();
                    if (rectArray[i, j].Contains(new Point(mouse.X, mouse.Y))) {
                        DrawSingleRect(rectArray, Color.Red, i, j);
                    }
                }
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawTicTacToeField() {
            //drawing the field
            //Vertical Lines
            _spriteBatch.DrawLine(250, 0, 250, _graphics.PreferredBackBufferHeight, Color.White, 10, 0);
            _spriteBatch.DrawLine(550, 0, 550, _graphics.PreferredBackBufferHeight, Color.White, 10, 0);
            //Horizontal Lines
            _spriteBatch.DrawLine(0, 200, _graphics.PreferredBackBufferWidth, 200, Color.White, 10, 0);
            _spriteBatch.DrawLine(0, 400, _graphics.PreferredBackBufferWidth, 400, Color.White, 10, 0);
        }

        private void DrawTicTacToeFieldRect(RectangleF[,] rectArray, Color color) {
            //do a base rectangle, pass in a 2d rect array to get rect.contains
            //loop through the array to draw each rect
            const int size = 200;
            for (int i = 0; i < rectArray.GetLength(0); i++) {         
                for (int j = 0; j < rectArray.GetLength(1); j++) {
                    rectArray[i, j] = new RectangleF(size * i + 100, size*j, size, size); //+100 is offset
                    _spriteBatch.DrawRectangle(rectArray[i, j], color, 10, 0);
                }
            }
        }

        private void DrawSingleRect(RectangleF[,] rectArray, Color color, int x, int y) {
            _spriteBatch.DrawRectangle(rectArray[x, y], color, 10, 0);
        }
    }
}