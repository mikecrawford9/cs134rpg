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
using System.Collections.Generic;

namespace RPG
{
    class ToolMap
    {
        Tile[][] tools;
        Dictionary<WorldTile, Tool> toolmap;
        Dictionary<String, Texture2D> texmap;
        Tool selected;
        Tile selectedtile;
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
        Rectangle savebutton;
        Rectangle loadbutton;
        Rectangle addeventbutton;

        WorldTile curtool;

        System.Windows.Forms.ComboBox combobox;

        public ToolMap(int x, int y, Texture2D pixel, Dictionary<String,Texture2D> texmap, WorldTile[] tiles, SpriteFont font, IntPtr handle)
        {
            combobox = new System.Windows.Forms.ComboBox();
            combobox.Location = new System.Drawing.Point(542, 50);
            combobox.Size = new System.Drawing.Size(200, 25);
            combobox.BackColor = System.Drawing.Color.Orange;
            combobox.ForeColor = System.Drawing.Color.Black;
            combobox.SelectedIndexChanged +=
            new System.EventHandler(ComboBox1_SelectedIndexChanged);

            System.Windows.Forms.Control.FromHandle(handle).Controls.Add(combobox);
            this.texmap = texmap;
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

            toolmap = new Dictionary<WorldTile, Tool>();
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] != WorldTile.SELECT)
                {
                    try
                    {
                        toolmap.Add(tiles[i], new Tool(tiles[i], texmap[tiles[i].GetTexture()]));
                    }
                    catch (KeyNotFoundException e)
                    { }
                }
            }
            
            for (int i = 0; i < tiles.Length; i++)
                combobox.Items.Add(tiles[i]);

            /*tools = new Tile[2][];
            for(int j = 0; j < col.Length; j++)
            {
                tools[j] = new Tile[4];
                for (int i = 0; i < 4; i++)
                {
                    tools[j][i] = new Tile(j, i, col[j] + 1, 1 + y + yinterval * i, 32, tooltextures[j][i]);
                }

            }*/
            selected = null;
            selectedtop = new Rectangle(-40,-40,36,2);
            selectedbottom = new Rectangle(-40,-40,36,2);
            selectedleft = new Rectangle(-40,-40,2,36);
            selectedright = new Rectangle(-40,-40,2,36);
            //updateSelected(selected.getX(), selected.getY(), selected.getLength(), selected.getLength());



            addeventbutton = new Rectangle(xmin, ymax, 122, 30);
            astarbutton = new Rectangle(xmin, ymax + 40, 122, 30);
            playbutton = new Rectangle(xmin, ymax + 90, 122, 30);
            savebutton = new Rectangle(xmin, ymax + 140, 122, 30);
            loadbutton = new Rectangle(xmin, ymax + 190, 122, 30);

            
        }

        private void ComboBox1_SelectedIndexChanged(object sender,
        System.EventArgs e)
        {
            System.Windows.Forms.ComboBox comboBox = (System.Windows.Forms.ComboBox)sender;
            comboBox.FindForm().ActiveControl = null;

        }

        public void Draw(SpriteBatch spriteBatch, GameState current)
        {
            /*for (int i = 0; i < 2; i++)
                for (int j = 0; j < 4; j++)
                    tools[i][j].Draw(spriteBatch, tools[i][j].sq);
            */
            spriteBatch.Draw(pixel, astarbutton, Color.LightGray);
            spriteBatch.Draw(pixel, playbutton, Color.Green);
            spriteBatch.Draw(pixel, savebutton, Color.Blue);
            spriteBatch.Draw(pixel, loadbutton, Color.Orange);
            if (selectedtile != null)
            {
                spriteBatch.Draw(pixel, addeventbutton, Color.LightGray);
                spriteBatch.DrawString(font, "Add Event", new Vector2(addeventbutton.X + 10, addeventbutton.Y - 5), Color.Black);
                spriteBatch.DrawString(font, "(" + selectedtile.getMapX() + "," + selectedtile.getMapY() + ")", new Vector2(addeventbutton.X + 10, addeventbutton.Y - 30), Color.Black);
            }
            spriteBatch.DrawString(font, "Edit", new Vector2(astarbutton.X + 20, astarbutton.Y - 6), Color.Black);
            spriteBatch.DrawString(font, "Play", new Vector2(playbutton.X + 20, playbutton.Y - 6), Color.Black);
            spriteBatch.DrawString(font, "Save", new Vector2(savebutton.X + 20, savebutton.Y - 6), Color.Black);
            spriteBatch.DrawString(font, "Load", new Vector2(loadbutton.X + 20, loadbutton.Y - 6), Color.Black);

            drawSelectHighlight(spriteBatch);
        }

        public GameState Update(GameState current)
        {
            GameState ret = current;
            MouseState mouseState = Mouse.GetState();
            int mousex = mouseState.X;
            int mousey = mouseState.Y;

            Object selectedobj = combobox.SelectedItem;
            if (selectedobj != null)
            {
                WorldTile select = (WorldTile)selectedobj;
                if (select != WorldTile.SELECT)
                {
                    Texture2D seltex = texmap[select.GetTexture()];
                    selected = new Tool(select, seltex);
                }
                else
                    selected = new Tool(select, null);
            }

            if (selectedtile != null && (mousex >= addeventbutton.X && mousex <= (addeventbutton.X + addeventbutton.Width)) &&
                (mousey >= addeventbutton.Y && mousey <= (addeventbutton.Y + addeventbutton.Height)) &&
                (mouseState.LeftButton == ButtonState.Pressed))
            {
                updateSelected(addeventbutton.X, addeventbutton.Y, addeventbutton.Width, addeventbutton.Height);
                ret = GameState.ADDEVENT;
            }

            if ((mousex >= astarbutton.X && mousex <= (astarbutton.X + astarbutton.Width)) &&
                (mousey >= astarbutton.Y && mousey <= (astarbutton.Y + astarbutton.Height)) && 
                (mouseState.LeftButton == ButtonState.Pressed))
            {
                updateSelected(astarbutton.X, astarbutton.Y, astarbutton.Width, astarbutton.Height);
                ret = GameState.EDIT;
                //ret = GameState.ASTAR;
            }

            if ((mousex >= playbutton.X && mousex <= (playbutton.X + playbutton.Width)) &&
                (mousey >= playbutton.Y && mousey <= (playbutton.Y + playbutton.Height)) &&
                (mouseState.LeftButton == ButtonState.Pressed))
            {
                updateSelected(playbutton.X, playbutton.Y, playbutton.Width, playbutton.Height);
                ret = GameState.RUNNING;
            }

            if ((mousex >= savebutton.X && mousex <= (savebutton.X + savebutton.Width)) &&
                (mousey >= savebutton.Y && mousey <= (savebutton.Y + savebutton.Height)) &&
                (mouseState.LeftButton == ButtonState.Pressed))
            {
                updateSelected(savebutton.X, savebutton.Y, savebutton.Width, savebutton.Height);
                ret = GameState.SAVEMAP;
            }

            if ((mousex >= loadbutton.X && mousex <= (loadbutton.X + loadbutton.Width)) &&
                (mousey >= loadbutton.Y && mousey <= (loadbutton.Y + loadbutton.Height)) &&
                (mouseState.LeftButton == ButtonState.Pressed))
            {
                updateSelected(loadbutton.X, loadbutton.Y, loadbutton.Width, loadbutton.Height);
                ret = GameState.LOADMAP;
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

        public void unSelect()
        {
            selectedbottom.X = selectedbottom.Y = selectedleft.X = 
                selectedleft.Y = selectedright.X = selectedright.Y = selectedtop.X = selectedtop.Y = 2000;
        }

        public void drawSelectHighlight(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, selectedtop, Color.Yellow);
            spriteBatch.Draw(pixel, selectedbottom, Color.Yellow);
            spriteBatch.Draw(pixel, selectedleft, Color.Yellow);
            spriteBatch.Draw(pixel, selectedright, Color.Yellow);
        }

        public Tool getSelected()
        {
            return selected;
        }

        public Tool getDefaultTool()
        {
           // return toolmap[TileType.GRASS];
            return toolmap[WorldTile.GRASS];
        }

        public Tool getTool(String type)
        {
            Tool ret = null;
            WorldTile cur = (WorldTile)Enum.Parse(typeof(WorldTile), type, true);
            if(cur != null)
                ret = this.toolmap[cur];

            return ret;
        }

        public void setSelectedTile(Tile selected)
        {
            this.selectedtile = selected;
        }

        public Tile getSelectedTile()
        {
            return this.selectedtile;
        }

        public Texture2D getTexture(WorldTile type)
        {
            return this.texmap[type.GetTexture()];
        }
    }
}
