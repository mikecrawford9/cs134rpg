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

namespace RPG
{
    class TileMap
    {
        public const int VIEW_HEIGHT = 512;
        public const int VIEW_WIDTH = 512;

        private int size;
        private int pixelsperside;
        private int totalmapsize;
        private int xtiles;
        private int ytiles;

        int curxtilemin;
        int curytilemin;

        int center;

        ToolMap toolmap;
        Tile[][] map;
        Tile selectedtile;
        Rectangle[][] displaytiles;
        SpriteFont font;

        Tile highlighted;

        Tile playertile;
        Tile monstertile;

        int playerx;
        int playery;

        int monsterx;
        int monstery;

        public TileMap(int x, int y, int size, int xtiles, int ytiles, Texture2D pixel, ToolMap tools, SpriteFont font)
        {
            this.toolmap = tools;
            this.size = size;
            this.xtiles = xtiles;
            this.ytiles = ytiles;
            this.font = font;
            double tss = VIEW_HEIGHT / size;
            int tilesidesize = (int)Math.Floor(tss);
            totalmapsize = tilesidesize * size;
            curxtilemin = 0;
            curytilemin = 0;

            //int curtoolx = tools.Length-1;
            //int curtooly = tools[0].Length-1;

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
                    map[i][j] = new Tile(i, j, (x + i * tilesidesize), (y + j * tilesidesize), tilesidesize, toolmap.getDefaultTool());
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i, ia, j, ja;
            for (ia = 0, i = curxtilemin; i < (size + curxtilemin) && i < xtiles && ia < size; i++, ia++)
                for (ja = 0, j = curytilemin; j < (size + curytilemin) && j < ytiles && ja < size; j++, ja++)
                {
                    map[i][j].Draw(spriteBatch, displaytiles[ia][ja]);
                    if(playertile != null && i == playertile.getMapX() && j == playertile.getMapY())
                        playertile.Draw(spriteBatch, displaytiles[ia][ja]);

                }
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
                    file.Write("<TILE x=\"" + i + "\" y=\"" + j + "\" type=\"" + map[i][j].getType() + "\" ");
                    Event[] ev = map[i][j].getEvents();
                    if (ev.Length == 0)
                        file.WriteLine("/>");
                    else
                    {
                        file.WriteLine(">");
                        for (int x = 0; x < ev.Length; x++)
                        {
                            file.WriteLine("<EVENT type=\"" + ev[x].getEventType() + "\">");
                            file.WriteLine("<DATA>");
                            String[] propkeys = ev[x].getKeys();
                            for (int r = 0; r < propkeys.Length; r++)
                            {
                                file.WriteLine("<" + propkeys[r] + ">" + ev[x].getProperty(propkeys[r]) + "</" + propkeys[r] + ">");
                            }
                            file.WriteLine("</DATA>");
                            file.WriteLine("</EVENT>");

                        }
                            

                        file.WriteLine("</TILE>");
                    }
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

            xtiles = x;
            ytiles = y;

            map = new Tile[x][];
            for(int i = 0; i < x; i++)
                map[i] = new Tile[y];
            
            file.BaseStream.Position = 0;
            int tilex = 0;
            int tiley = 0;
            bool inEvent = false;
            Event current = new Event();
            using (XmlReader reader = XmlReader.Create(file))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == "TILE")
                            {
                                tilex = Convert.ToInt32(reader["x"]);
                                tiley = Convert.ToInt32(reader["y"]);

                                String type = reader["type"];
                                Tool tiletool = toolmap.getTool(type);
                                map[tilex][tiley] = new Tile(tilex, tiley, tilex, tiley, 0, tiletool);
                            }
                            else if (reader.Name == "EVENT")
                            {
                                String eventtype = reader["type"];
                                current.setEventType((EventType)Enum.Parse(typeof(EventType), eventtype));
                                inEvent = true;
                            }
                            else if (reader.Name == "DATA")
                            {
                                if (inEvent)
                                {
                                    XmlReader tempreader = reader.ReadSubtree();
                                    String propname = "";
                                    while(tempreader.Read())
                                        switch (tempreader.NodeType)
                                        {
                                            case XmlNodeType.Element:
                                                propname = tempreader.Name;
                                                break;
                                            case XmlNodeType.Text:
                                                current.addProperty(propname, tempreader.Value);
                                                break;
                                        }
                                }
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name == "EVENT")
                            {
                                Console.WriteLine("Adding event!");
                                map[tilex][tiley].addEvent(current);
                                inEvent = false;
                                current = new Event();
                            }
                            break;
                    }
                }
            }

            success = true;

            return success;
        }

        public void Update(ToolMap toolmap)
        {
            Tool selectedTool = toolmap.getSelected();
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

                if (mouseState.LeftButton == ButtonState.Pressed && selectedTool != null)
                {
                    WorldTile selectedType = selectedTool.getType();
                    if (selectedType == WorldTile.PLAYER)
                    {
                        Tile cur = map[tilex][tiley];
                        if (cur.getType() != WorldTile.WALL)
                        {
                            if (playertile == null)
                                playertile = new Tile(tilex, tiley, cur.getX(), cur.getY(), cur.getLength(), selectedTool.getTexture(), Color.White);
                            else
                            {
                                playerx = tilex;
                                playery = tiley;

                                playertile.setMapX(tilex);
                                playertile.setMapY(tiley);
                            }
                        }
                    }
                    /*else if (selectedType == WorldTile.MONSTER)
                    {
                        Tile cur = map[tilex][tiley];
                        if (cur.getType() != WorldTile.WALL)
                        {
                            if (monstertile == null)
                                monstertile = new Tile(tilex, tiley, cur.getX(), cur.getY(), cur.getLength(), selectedTool.getTexture(), Color.White);
                            else
                            {
                                monsterx = tilex;
                                monstery = tiley;

                                monstertile.setMapX(tilex);
                                monstertile.setMapY(tiley);
                            }
                        }
                    }*/
                    else
                    {
                        if (selectedTool.getType() == WorldTile.SELECT)
                        {
                            //if (selectedtile != null && selectedtile != map[curxtilemin + tilex][curytilemin + tiley])
                            //    selectedtile.setColor(Color.White);

                            toolmap.setSelectedTile(map[curxtilemin + tilex][curytilemin + tiley]);
                            //selectedtile.setColor(Color.DarkGray);
                        }
                        else
                        {
                            map[curxtilemin + tilex][curytilemin + tiley].applyTool(selectedTool, toolmap);
                        }
                    }

                }
                    if (highlighted != null)
                        highlighted.unhighlight();

                    
                    highlighted = map[curxtilemin + tilex][curytilemin + tiley];
                    highlighted.highlight();
            }

        }

        public void processEvents(Tile cur)
        {
            Console.WriteLine("Processing Events!");
            Event[] ce = cur.getEvents();

            Console.WriteLine("Found " + ce.Length + " events!");
                for(int i = 0; i < ce.Length; i++)
                {
                    Event e = ce[i];
                    if(e.getEventType() == EventType.MAP_TRANSITION)
                    {
                        String mapfile = e.getProperty("mapfile");
                        int x = Convert.ToInt32(e.getProperty("x"));
                        int y = Convert.ToInt32(e.getProperty("y"));

                        Console.WriteLine("Processing Map Transition Event for " + mapfile + " x=" + x + ",y=" + y);

                        FileStream fileStream = new FileStream(@mapfile, FileMode.Open);
                        StreamReader reader = new StreamReader(fileStream);
                        LoadMap(reader, toolmap);
                        setPlayerLocation(map[x][y]);

                        reader.Close();
                        fileStream.Close();
                    }
                }
        }

        public void shiftDown(int numtiles, bool noclip)
        {
            int newcurytilemin = curytilemin + numtiles;
            if (newcurytilemin <= (ytiles-size))
            {
                if (noclip || (playertile != null && !map[playertile.getMapX()][playertile.getMapY() + numtiles].isObstacle()))
                {
                    if (!noclip && playertile != null)
                    {
                        setPlayerLocation(map[playertile.getMapX()][playertile.getMapY() + numtiles]);
                        processEvents(map[playertile.getMapX()][playertile.getMapY()]);
                    }
                    else
                    {
                        curytilemin = newcurytilemin;
                    }
                }
            }
            else if (!noclip && playertile != null &&
                ((playertile.getMapY() + numtiles) < ytiles) &&
                !map[playertile.getMapX()][playertile.getMapY() + numtiles].isObstacle())
            {
                setPlayerLocation(map[playertile.getMapX()][playertile.getMapY() + numtiles]);
                processEvents(map[playertile.getMapX()][playertile.getMapY()]);
            }
        }

        public void shiftUp(int numtiles, bool noclip)
        {
            int newcurytilemin = curytilemin - numtiles;
            if (newcurytilemin >= 0)
            {
                if (noclip || (playertile != null 
                    && !map[playertile.getMapX()][playertile.getMapY() - numtiles].isObstacle()))
                {
                    if (!noclip && playertile != null)
                    {
                        setPlayerLocation(map[playertile.getMapX()][playertile.getMapY() - numtiles]);
                        processEvents(map[playertile.getMapX()][playertile.getMapY()]);
                    }
                    else
                    {
                        curytilemin = newcurytilemin;
                    }
                }
            }
            else if (!noclip 
                && playertile != null 
                && ((playertile.getMapY() - numtiles) >= 0)
                && !map[playertile.getMapX()][playertile.getMapY() - numtiles].isObstacle())
            {
                setPlayerLocation(map[playertile.getMapX()][playertile.getMapY() - numtiles]);
                processEvents(map[playertile.getMapX()][playertile.getMapY()]);
            }
        }

        public void shiftLeft(int numtiles, bool noclip)
        {
            int newcurxtilemin = curxtilemin - numtiles;
            if (newcurxtilemin >= 0)
            {
                if (noclip || (playertile != null && !map[playertile.getMapX() - numtiles][playertile.getMapY()].isObstacle()))
                {
                    if (!noclip && playertile != null)
                    {
                        setPlayerLocation(map[playertile.getMapX() - numtiles][playertile.getMapY()]);
                        processEvents(map[playertile.getMapX()][playertile.getMapY()]);
                    }
                    else
                    {
                        curxtilemin = newcurxtilemin;
                    }
                }
            }
            else if (!noclip 
                && playertile != null 
                && ((playertile.getMapX() - numtiles) >= 0)
                && !map[playertile.getMapX() - numtiles][playertile.getMapY()].isObstacle())
            {
                setPlayerLocation(map[playertile.getMapX() - numtiles][playertile.getMapY()]);
                processEvents(map[playertile.getMapX()][playertile.getMapY()]);
            }
        }

        public void shiftRight(int numtiles, bool noclip)
        {
            int newcurxtilemin = curxtilemin + numtiles;
            if (newcurxtilemin <= (xtiles - size))
            {
                if (noclip || (playertile != null && !map[playertile.getMapX() + numtiles][playertile.getMapY()].isObstacle()))
                {
                    if (!noclip && playertile != null)
                    {
                        setPlayerLocation(map[playertile.getMapX() + numtiles][playertile.getMapY()]);
                        processEvents(map[playertile.getMapX()][playertile.getMapY()]);
                    }
                    else
                    {
                        curxtilemin = newcurxtilemin;
                    }
                }
            }
            else if (!noclip && playertile != null 
                && ((playertile.getMapX() + numtiles) < xtiles)
                && !map[playertile.getMapX() + numtiles][playertile.getMapY()].isObstacle())
            {
                setPlayerLocation(map[playertile.getMapX() + numtiles][playertile.getMapY()]);
                processEvents(map[playertile.getMapX()][playertile.getMapY()]);
            }
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
                playerx = playertile.getMapX();
                playery = playertile.getMapY();
                curxtilemin = Math.Max(Math.Min(playerx - 8,xtiles - 17), 0);
                curytilemin = Math.Max(Math.Min(playery - 8,ytiles - 17), 0);
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

        public Tile getSelectedTile()
        {
            return selectedtile;
        }

        public void resetPlayers()
        {
            Tile p = map[playerx][playery];
            if (playertile != null)
            {
                playertile.setMapX(playerx);
                playertile.setMapY(playery);
            }
            Tile m = map[monsterx][monstery];
            if (monstertile != null)
            {
                monstertile.setMapX(monsterx);
                monstertile.setMapY(monstery);
            }
        }
    }
}
