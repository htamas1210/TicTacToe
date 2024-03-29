﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System;
using System.Collections.Generic;

namespace Tic_Tac_Toe {
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D Circle, X;
        private Texture2D CircleWonTexture, XWonTexture;
        private RectangleF[,] rectArray;
        private int[,] CircleXPostion;
        private List<string> Circle4PosList; //for storing the 4 latest positions
        private List<string> X4PosList;  
        private int lineThickness = 5;
        private int playerWon = 0; //1 is circle 2 is x
        private bool isCircleNext = false; //basically start player here

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            //Setting Resolution
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            //For field
            rectArray = new RectangleF[3,3];
            CircleXPostion = new int[3, 3]; //0 is free space, 1 is circle, 2 is X

            Circle4PosList = new List<string>();
            X4PosList = new List<string>();

            base.Initialize();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Circle = Content.Load<Texture2D>("Textures/circle");
            X = Content.Load<Texture2D>("Textures/X");
            CircleWonTexture = Content.Load<Texture2D>("Textures/Circle win text");
            XWonTexture = Content.Load<Texture2D>("Textures/Cross win text");

        }

        protected override void Update(GameTime gameTime) {
            if (playerWon == 0) {
                var mouse = Mouse.GetState();
                for (int i = 0; i < rectArray.GetLength(0); i++) {
                    for (int j = 0; j < rectArray.GetLength(1); j++) {
                        mouse = Mouse.GetState();
                        if (isCircleNext) {
                            if (rectArray[j, i].Contains(new Point(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && CircleXPostion[j, i] == 0) {
                                CircleXPostion[j, i] = 1; //eltároljuk a kör pozicióját ||| CHANGED FROM [i,j]
                                isCircleNext = false;

                                printGameStateArray();

                                //Put list stuff here
                                Circle4PosList.Add(j+";"+i);
                                if(Circle4PosList.Count == 4) {
                                    string[] index = Circle4PosList[0].Split(';');
                                    CircleXPostion[Convert.ToInt32(index[0]), Convert.ToInt32(index[1])] = 0; //set indexed field to empty
                                    Circle4PosList.RemoveAt(0);
                                }
                                //printing list
                                printList(Circle4PosList);

                                //System.Threading.Thread.Sleep(250);
                            }
                        } else {
                            if (rectArray[j, i].Contains(new Point(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && CircleXPostion[j, i] == 0) {
                                CircleXPostion[j, i] = 2; //eltaroljuk az x poziciojat
                                isCircleNext = true;
                                printGameStateArray();

                                //Put list stuff here
                                X4PosList.Add(j+";"+i);
                                if (X4PosList.Count == 4) {
                                    string[] index = X4PosList[0].Split(';');//(j;i)
                                    CircleXPostion[Convert.ToInt32(index[0]), Convert.ToInt32(index[1])] = 0; //set indexed field to empty (j;i)
                                    X4PosList.RemoveAt(0);
                                }
                                //printing list
                                printList(X4PosList);

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

            var mouse = Mouse.GetState();
            _spriteBatch.Begin();

            if(playerWon == 1) {
                _spriteBatch.Draw(CircleWonTexture, new Rectangle(10, 0, 50, 600), Color.White);
                _spriteBatch.Draw(CircleWonTexture, new Rectangle(_graphics.PreferredBackBufferWidth - 75, 0, 50, 600), Color.White);
            }else if(playerWon == 2) {
                _spriteBatch.Draw(XWonTexture, new Rectangle(10, 0, 50, 600), Color.White);
                _spriteBatch.Draw(XWonTexture, new Rectangle(_graphics.PreferredBackBufferWidth - 75, 0, 50, 600), Color.White);
            }
            
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

            //Testing one texture draw
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
                    System.Console.Write(CircleXPostion[j,i] +"(j:"+j+", i:"+i + ") | ");
                }
                System.Console.WriteLine();
            }
            System.Console.WriteLine("\n");
        }

        private void printList(List<string> list) {
            foreach(var elem in list) {
                System.Console.WriteLine(elem);
            }
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
                        //System.Console.WriteLine(count);
                    }
                }
                if (count == 3) {
                    playerWon = 2;
                    return;
                }
            }


            //diagonal
            //circle
            int count2 = 0;
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                
                //System.Console.WriteLine("ii: " + i + "" + i);
                if (CircleXPostion[i, i] == 1) {
                    count2++;
                    //System.Console.WriteLine(count2 + "  kor");
                }
                
                if (count2 == 3) {
                    playerWon = 1;               
                    return;
                }
            }

            //x
            int count3 = 0;
            for (int i = 0; i < CircleXPostion.GetLength(0); i++) {
                
                //System.Console.WriteLine("ii: " + i + "" + i);
                if (CircleXPostion[i, i] == 2) {
                    count3++;
                    //System.Console.WriteLine(count3 + "  X");
                }

                if (count3 == 3) {
                    playerWon = 2;
                    return;
                }
            }

            //reverse diagonal
            //circle
            if (CircleXPostion[0,2] == 1 && CircleXPostion[1,1] == 1 && CircleXPostion[2,0] == 1) {
                playerWon = 1;
                return;
            }else if(CircleXPostion[0, 2] == 2 && CircleXPostion[1, 1] == 2 && CircleXPostion[2, 0] == 2) {
                playerWon = 2;
                return;
            }

        }
    }
}