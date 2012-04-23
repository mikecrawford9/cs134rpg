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
using System.IO;
using System.Xml;

namespace AStarGame
{
    class TileMap
    {
        public const int VIEW_HEIGHT = 512;
        public const int VIEW_WIDTH = 512;

        private int x;
        private int y;
        private int size;
        private int pixelsperside;
        private int totalmapsize;
        private int xtiles;
        private int ytiles;

        int curxtilemin;
        int curytilemin;

        int center;

        Tile[][] map;
        Rectangle[][] displaytiles;

        Tile highlighted;

        Tile playertile;
        Tile monstertile;

        int playerx;
        int playery;

        int monsterx;
        int monstery;

        public TileMap(int x, int y, int size, int xtiles, int ytiles, Texture2D pixel, Tool[][] tools)
        {
            this.size = size;
            this.xtiles = xtiles;
            this.ytiles = ytiles;
            double tss = VIEW_HEIGHT / size;
            int tilesidesize = (int)Math.Floor(tss);
            totalmapsize = tilesidesize * size;
            curxtilemin = 0;
            curytilemin = 0;

            int curtoolx = tools.Length-1;
            int curtooly = tools[0].Length-1;

            pixelsperside = tilesidesize;
            center = (int)Math.Ceiling(size / 2.0);
            displaytiles = new Rectangle[size][];
            for (int i = 0; i < size; i++)
            {
                displaytiles[i] = new Rectangle[size];
                for (int j = 0; j < size; j++)
                {
                    displaytiles[i][j] = new Rectangle(x + i*tilesidesize, y + j*tilesidesize, pixelsperside, pixelsperside);
                }
            }

            //Random randval = new Random();

            map = new Tile[xtiles][];

            for (int i = 0; i < xtiles; i++)
            {
                map[i] = new Tile[ytiles];
                for (int j = 0; j < ytiles; j++)
                {
                    //int currandx = randval.Next(0, 2);
                    //int currandy = randval.Next(0, 3);
                    map[i][j] = new Tile(i, j, (x + i * tilesidesize), (y + j * tilesidesize), tilesidesize, tools[0][0]);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i, ia, j, ja;
            for (ia = 0, i = curxtilemin; i < (size + curxtilemin) && i < xtiles && ia < size; i++, ia++)
                for (ja = 0, j = curytilemin; j < (size + curytilemin) && j < ytiles && ja < size; j++, ja++)
                    map[i][j].Draw(spriteBatch, displaytiles[ia][ja]);



            if (playertile != null)
                playertile.Draw(spriteBatch, displaytiles[center-1][center-1]);

            //if (monstertile != null)
            //    monstertile.Draw(spriteBatch);
        }

        public bool SaveMap(StreamWriter file)
        {
            bool success = false;
            file.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            file.WriteLine("<MAP x=\"" + map.Length + "\" y=\"" + map[0].Length + "\">");
            file.WriteLine("<TILES>");
            for(int i = 0; i < map.Length; i++)
                for (int j = 0; j < map[i].Length; j++)
                {
                    file.WriteLine("<TILE x=\"" + i + "\" y=\"" + j + "\" type=\"" + map[i][j].getType() + "\" />");
                }
            file.WriteLine("</TILES>");
            file.WriteLine("</MAP>");
            success = true;

            return success;
        }

        public bool LoadMap(StreamReader file, ToolMap toolmap)
        {
            bool success = false;
            bool initmap = false;
            bool gotxandy = false;
            int x = 0;
            int y = 0;
            using (XmlReader reader = XmlReader.Create(file))
            {
                while (reader.Read() && !gotxandy)
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "MAP")
                            {
                                x = Convert.ToInt32(reader["x"]);
                                y = Convert.ToInt32(reader["y"]);
                                gotxandy = true;
                            }
                            break;
                    }
                }
            }
            
            map = new Tile[x][];
            for(int i = 0; i < x; i++)
                map[i] = new Tile[y];
            
            file.BaseStream.Position = 0;
            using (XmlReader reader = XmlReader.Create(file))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "TILE")
                            {
                                int tilex = Convert.ToInt32(reader["x"]);
                                int tiley = Convert.ToInt32(reader["y"]);

                                String type = reader["type"];
                                Tool tiletool = toolmap.getTool(type);
                                map[tilex][tiley] = new Tile(tilex, tiley, tilex, tiley, 0, tiletool);
                            }
                            break;
                    }
                }
            }

            success = true;

            return success;
        }

        public void Update(Tile selectedTool)
        {
            MouseState mouseState = Mouse.GetState();
            int mousex = mouseState.X;
            int mousey = mouseState.Y;

            //if mouse is within the map, do mouse over...
            if ((mousex > 10 && mousex < totalmapsize + 10) &&
                (mousey > 10 && mousey < totalmapsize + 10))
            {
                double tempx = (mousex - 10) / pixelsperside;
                int tilex = (int)Math.Floor(tempx);

                double tempy = (mousey - 10) / pixelsperside;
                int tiley = (int)Math.Floor(tempy);

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    TileType selectedType = selectedTool.getType();
                    if (selectedType == TileType.PLAYER)
                    {
                        Tile cur = map[tilex][tiley];
                        if (cur.getType() != TileType.WALL)
                        {
                            if (playertile == null)
                                playertile = new Tile(tilex, tiley, cur.getX(), cur.getY(), cur.getLength(), selectedTool.getTexture(), selectedTool.getColor());
                            else
                            {
                                playerx = tilex;
                                playery = tiley;

                                //playertile.setX(cur.getX());
                                //playertile.setY(cur.getY());
                                playertile.setMapX(tilex);
                                playertile.setMapY(tiley);
                            }
                        }
                    }
                    else if (selectedType == TileType.MONSTER)
                    {
                        Tile cur = map[tilex][tiley];
                        if (cur.getType() != TileType.WALL)
                        {
                            if (monstertile == null)
                                monstertile = new Tile(tilex, tiley, cur.getX(), cur.getY(), cur.getLength(), selectedTool.getTexture(), selectedTool.getColor());
                            else
                            {
                                monsterx = tilex;
                                monstery = tiley;

                                //monstertile.setX(cur.getX());
                                //monstertile.setY(cur.getY());
                                monstertile.setMapX(tilex);
                                monstertile.setMapY(tiley);
                            }
                        }
                    }
                    else
                    {
                        map[curxtilemin + tilex][curytilemin + tiley].applyTool(selectedTool);
                    }

                }
                    if (highlighted != null)
                        highlighted.unhighlight();

                    
                    highlighted = map[curxtilemin + tilex][curytilemin + tiley];
                    highlighted.highlight();
            }

        }

        public void shiftDown(int numtiles, bool noclip)
        {
            int newcurytilemin = curytilemin + numtiles;
            if (newcurytilemin < (ytiles-size))
            {
                curytilemin = newcurytilemin;
            }
        }

        public void shiftUp(int numtiles, bool noclip)
        {
            int newcurytilemin = curytilemin - numtiles;
            if (newcurytilemin >= 0)
                curytilemin = newcurytilemin;
        }

        public void shiftLeft(int numtiles, bool noclip)
        {
            int newcurxtilemin = curxtilemin - numtiles;
            if (newcurxtilemin >= 0)
                curxtilemin = newcurxtilemin;
        }

        public void shiftRight(int numtiles, bool noclip)
        {
            int newcurxtilemin = curxtilemin + numtiles;
            if (newcurxtilemin < (xtiles-size))
                curxtilemin = newcurxtilemin;
        }

        public Tile getTileAt(int x, int y)
        {
            if (x < map.Length && x >= 0)
            {
                if (y < map[x].Length && y >= 0)
                {
                    return map[x][y];
                }
                else
                    return null;
            }
            else
                return null;
        }

        public Tile getPlayerTile()
        {
            return playertile;
        }

        public Tile getMonsterTile()
        {
            return monstertile;
        }

        public void setMonsterLocation(Tile newloc)
        {
            if (monstertile != null)
            {
                monstertile.setMapX(newloc.getMapX());
                monstertile.setMapY(newloc.getMapY());
                //monstertile.setX(newloc.getX());
                //monstertile.setY(newloc.getY());
            }
            else
                Console.WriteLine("Monster is null!!");
        }

        public void setPlayerLocation(Tile newloc)
        {
            if (playertile != null)
            {
                playertile.setMapX(newloc.getMapX());
                playertile.setMapY(newloc.getMapY());
                //playertile.setX(newloc.getX());
                //playertile.setY(newloc.getY());
            }
            else
                Console.WriteLine("Player is null!!");
        }
        
        public void unhighlight()
        {
            if (highlighted != null)
                highlighted.unhighlight();
        }

        public void refreshTiles()
        {
            for(int i = 0; i < map.Length; i++)
                for (int j = 0; j < map.Length; j++)
                {
                    map[i][j].resetCost();
                    map[i][j].resetPrev();
                }
        }

        public void resetPlayers()
        {
            Tile p = map[playerx][playery];
            if (playertile != null)
            {
                //playertile.setX(p.getX());
                //playertile.setY(p.getY());
                playertile.setMapX(playerx);
                playertile.setMapY(playery);
            }
            Tile m = map[monsterx][monstery];
            if (monstertile != null)
            {
                //monstertile.setX(m.getX());
                //monstertile.setY(m.getY());
                monstertile.setMapX(monsterx);
                monstertile.setMapY(monstery);
            }
        }
    }
}
