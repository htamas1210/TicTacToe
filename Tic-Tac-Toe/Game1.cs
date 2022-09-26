using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Tic_Tac_Toe {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D Circle, X;
        private RectangleF[,] rectArray;
        private int[,] CircleXPostion;
        private int lineThickness = 5;
        private int playerWon = 0; //1 is circle 2 is x
        private bool isCircleNext = false;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            //Setting Resolution
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            //For field
            rectArray = new RectangleF[3,3];
            CircleXPostion = new int[3, 3]; //0 is free space, 1 is circle, 2 is X

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Circle = Content.Load<Texture2D>("Textures/circle");
            X = Content.Load<Texture2D>("Textures/X");

        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            

            // TODO: Add your update logic here
            if (playerWon == 0) {
                var mouse = Mouse.GetState();
                for (int i = 0; i < rectArray.GetLength(0); i++) {
                    for (int j = 0; j < rectArray.GetLength(1); j++) {
                        mouse = Mouse.GetState();
                        if (isCircleNext) {
                            if (rectArray[i, j].Contains(new Point(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && CircleXPostion[i, j] == 0) {
                                CircleXPostion[i, j] = 1; //eltároljuk a kör pozicióját ||| CHANGED FROM [i,j]
                                isCircleNext = false;
                                printGameStateArray();
                                //System.Threading.Thread.Sleep(250);
                            }
                        } else {
                            if (rectArray[i, j].Contains(new Point(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && CircleXPostion[i, j] == 0) {
                                CircleXPostion[i, j] = 2; //eltaroljuk az x poziciojat
                                isCircleNext = true;
                                printGameStateArray();
                                //System.Threading.Thread.Sleep(250);
                            }
                        }
                    }
                }
            }

            CheckGameCondition();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CadetBlue);

            // TODO: Add your drawing code here
            var mouse = Mouse.GetState();
            _spriteBatch.Begin();
            
            DrawTicTacToeFieldRect(rectArray, Color.LightGray);

            //checking if mouse is in a rect
            for (int i = 0; i < rectArray.GetLength(0); i++) {
                for (int j = 0; j < rectArray.GetLength(1); j++) {
                    mouse = Mouse.GetState();
                    if (isCircleNext) { //Circle player highlight
                        if (rectArray[i, j].Contains(new Point(mouse.X, mouse.Y))) {
                            DrawSingleRect(rectArray, Color.Red, i, j);
                        }
                    } else { //X player highlight
                        if (rectArray[i, j].Contains(new Point(mouse.X, mouse.Y))) {
                            DrawSingleRect(rectArray, Color.Green, i, j);
                        }
                    }
                }
            }

            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                for (int j = 0; j < CircleXPostion.GetLength(1); j++) {
                    mouse = Mouse.GetState();
                    if (CircleXPostion[i,j] == 1) { //Circle player highlight
                        _spriteBatch.Draw(Circle, new Rectangle(200 * i + 100, 200*j, 200, 200), Color.White);
                    } else if (CircleXPostion[i, j] == 2) { //X player highlight
                        _spriteBatch.Draw(X, new Rectangle(200 * i + 100, 200*j, 200, 200), Color.White);
                    }
                }
            }

            //loop through circle pos arr to draw the texture
            /*_spriteBatch.Draw(Circle, new Rectangle(100, 0, 200, 200), Color.White);
            _spriteBatch.Draw(X, new Rectangle(300, 0, 200, 200), Color.White);*/

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawTicTacToeField() {
            //drawing the field
            //Vertical Lines
            _spriteBatch.DrawLine(250, 0, 250, _graphics.PreferredBackBufferHeight, Color.White, lineThickness, 0);
            _spriteBatch.DrawLine(550, 0, 550, _graphics.PreferredBackBufferHeight, Color.White, lineThickness, 0);
            //Horizontal Lines
            _spriteBatch.DrawLine(0, 200, _graphics.PreferredBackBufferWidth, 200, Color.White, lineThickness, 0);
            _spriteBatch.DrawLine(0, 400, _graphics.PreferredBackBufferWidth, 400, Color.White, lineThickness, 0);
        }

        private void DrawTicTacToeFieldRect(RectangleF[,] rectArray, Color color) {
            //do a base rectangle, pass in a 2d rect array to get rect.contains
            //loop through the array to draw each rect
            const int size = 200;
            for (int i = 0; i < rectArray.GetLength(0); i++) {         
                for (int j = 0; j < rectArray.GetLength(1); j++) {
                    rectArray[i, j] = new RectangleF(size * i + 100, size*j, size, size); //+100 is offset
                    _spriteBatch.DrawRectangle(rectArray[i, j], color, lineThickness, 0);
                }
            }
        }

        private void DrawSingleRect(RectangleF[,] rectArray, Color color, int indexX, int indexY) {
            _spriteBatch.DrawRectangle(rectArray[indexX, indexY], color, lineThickness, 0);
        }

        private void printGameStateArray() {
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                for (int j = 0; j < CircleXPostion.GetLength(1); j++) {
                    System.Console.Write(CircleXPostion[i,j] + " | ");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine("\n\n");
        }

        private void CheckGameCondition() {
            //vertical
            //circle
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                int count = 0;
                for (int j = 0; j < CircleXPostion.GetLength(1); j++) {
                    if(CircleXPostion[i, j] == 1) {
                        count++;
                    }
                }
                if(count == 3) {
                    playerWon = 1;
                    return;
                }
            }

            //x
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                int count = 0;
                for (int j = 0; j < CircleXPostion.GetLength(1); j++) {
                    if (CircleXPostion[i, j] == 2) {
                        count++;
                    }
                }
                if (count == 3) {
                    playerWon = 2;
                    return;
                }
            }

            //horizontal
            //circle
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                int count = 0;
                for (int j = 0; j < CircleXPostion.GetLength(1); j++) {
                    if (CircleXPostion[j, i] == 1) {
                        count++;
                    }
                }
                if (count == 3) {
                    playerWon = 1;
                    return;
                }
            }

            //x
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                int count = 0;
                for (int j = 0; j < CircleXPostion.GetLength(1); j++) {
                    if (CircleXPostion[j, i] == 2) {
                        count++;
                    }
                }
                if (count == 3) {
                    playerWon = 2;
                    return;
                }
            }


            //diagonal
            //circle
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                int count = 0;
                if (CircleXPostion[i, i] == 1) {
                    count++;
                }
                
                if (count == 3) {
                    playerWon = 1;
                    
                    return;
                }
            }

            //x
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                int count = 0;
                if (CircleXPostion[i, i] == 2) {
                    count++;
                }

                if (count == 3) {
                    playerWon = 2;
                    return;
                }
            }

            //reverse diagonal
        }
    }
}