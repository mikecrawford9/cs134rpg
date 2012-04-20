/*
Name: Michael Crawford
Class: CS134
Instructor: Dr. Teoh
Term: Spring 2012
Assignment: Project 2
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AStarGame
{
    class ToolMap
    {
        Tile[][] tools;
        Tile selected;
        Texture2D pixel;
        SpriteFont font;
        Rectangle selectedtop;
        Rectangle selectedbottom;
        Rectangle selectedleft;
        Rectangle selectedright;
        int xmin;
        int xmax;
        int ymin;
        int ymax;
        double xcolwidth;
        double yrowheight;

        Rectangle astarbutton;
        Rectangle playbutton;

        public ToolMap(int x, int y, Texture2D pixel, Tool[][] tooltextures, SpriteFont font)
        {
            this.pixel = pixel;
            this.font = font;
            /*Color[] colors = new Color[]{Color.White, Color.Red, Color.Goldenrod, 
                Color.Green, Color.Blue, Color.Chocolate, Color.Orange, Color.DarkGray, 
            Color.Purple, Color.Black};
            */
            xmin = x;
            ymin = y;

            int[] col = {x,x+90};
            xmax = col[1] + 32;
            int yinterval = 44;
            ymax = y + 3 * yinterval + 32;

            xcolwidth = (xmax - xmin) / 2;
            yrowheight = (ymax - ymin) / 4;

            tools = new Tile[2][];
            for(int j = 0; j < col.Length; j++)
            {
                tools[j] = new Tile[4];
                for (int i = 0; i < 4; i++)
                {
                    tools[j][i] = new Tile(j, i, col[j] + 1, 1 + y + yinterval * i, 32, tooltextures[j][i]);
                }

            }
            selected = tools[0][0];
            selectedtop = new Rectangle(-40,-40,36,2);
            selectedbottom = new Rectangle(-40,-40,36,2);
            selectedleft = new Rectangle(-40,-40,2,36);
            selectedright = new Rectangle(-40,-40,2,36);
            updateSelected(selected.getX(), selected.getY(), selected.getLength(), selected.getLength());

            astarbutton = new Rectangle(xmin, ymax + 40, 122, 30);
            playbutton = new Rectangle(xmin, ymax + 90, 122, 30);
            
        }

        public void Draw(SpriteBatch spriteBatch, GameState current)
        {
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                    tools[i][j].Draw(spriteBatch, tools[i][j].sq);

            spriteBatch.Draw(pixel, astarbutton, Color.LightGray);
            spriteBatch.Draw(pixel, playbutton, Color.Green);
            spriteBatch.DrawString(font, "A*Star", new Vector2(astarbutton.X + 10, astarbutton.Y - 5), Color.Black);
            spriteBatch.DrawString(font, "Play", new Vector2(playbutton.X + 20, playbutton.Y - 6), Color.Black);

            drawSelectHighlight(spriteBatch);
        }

        public GameState Update(GameState current)
        {
            GameState ret = current;
            MouseState mouseState = Mouse.GetState();
            int mousex = mouseState.X;
            int mousey = mouseState.Y;

            if ((mousex >= xmin && mousex <= xmax) && (mousey >= ymin && mousey <= ymax))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    ret = GameState.EDIT;
                    int xinbox = mousex - xmin;
                    int yinbox = mousey - ymin;

                    int col = (int)Math.Floor(xinbox / xcolwidth);
                    int row = (int)Math.Floor(yinbox / yrowheight);

                    if (col >= 0 && col <= 1 && row >= 0 && row <= 3)
                    {
                        Tile cur = tools[col][row];

                        if (cur != selected || current != GameState.EDIT)
                        {
                            selected = cur;
                            updateSelected(cur.getX(), cur.getY(), cur.getLength(), cur.getLength());
                        }
                    }
                }
            }

            if ((mousex >= astarbutton.X && mousex <= (astarbutton.X + astarbutton.Width)) &&
                (mousey >= astarbutton.Y && mousey <= (astarbutton.Y + astarbutton.Height)) && 
                (mouseState.LeftButton == ButtonState.Pressed))
            {
                updateSelected(astarbutton.X, astarbutton.Y, astarbutton.Width, astarbutton.Height);
                ret = GameState.ASTAR;
            }

            if ((mousex >= playbutton.X && mousex <= (playbutton.X + playbutton.Width)) &&
                (mousey >= playbutton.Y && mousey <= (playbutton.Y + playbutton.Height)) &&
                (mouseState.LeftButton == ButtonState.Pressed))
            {
                updateSelected(playbutton.X, playbutton.Y, playbutton.Width, playbutton.Height);
                ret = GameState.RUNNING;
            }

            if (ret != current)
                Console.WriteLine("Game state set to " + ret);

            return ret;
        }

        public void updateSelected(int x, int y, int width, int height)
        {
            selectedtop.X = x - 2;
            selectedtop.Y = y - 2;
            selectedtop.Width = width + 4;

            selectedbottom.X = x - 2;
            selectedbottom.Y = y + height;
            selectedbottom.Width = width + 4;

            selectedleft.X = x - 2;
            selectedleft.Y = y - 2;
            selectedleft.Height = height + 4;

            selectedright.X = x + width;
            selectedright.Y = y - 2;
            selectedright.Height = height + 4;
        }

        public void drawSelectHighlight(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, selectedtop, Color.Yellow);
            spriteBatch.Draw(pixel, selectedbottom, Color.Yellow);
            spriteBatch.Draw(pixel, selectedleft, Color.Yellow);
            spriteBatch.Draw(pixel, selectedright, Color.Yellow);
        }

        public Tile getSelected()
        {
            return selected;
        }
    }
}
