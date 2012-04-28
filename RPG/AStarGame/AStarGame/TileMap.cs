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
        public String filename;

        int curxtilemin;
        int curytilemin;

        int center;

        ToolMap toolmap;
        Tile[][] map;
        Tile selectedtile;
        Rectangle[][] displaytiles;
        SpriteFont font;

        Map battlemap;

        Tile highlighted;

        Tile playertile;
        List<Tile> monstertiles;

        int playerx;
        int playery;

        public TileMap(int x, int y, int size, int xtiles, int ytiles, Texture2D pixel, ToolMap tools, SpriteFont font, String filename)
        {
            this.filename = filename;
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
            battlemap = Map.BATTLE_GENERIC;

            //int curtoolx = tools.Length-1;
            //int curtooly = tools[0].Length-1;

            pixelsperside = tilesidesize;
            center = (int)Math.Ceiling(size / 2.0);
            displaytiles = new Rectangle[size][];
            monstertiles = new List<Tile>();
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
        public void RemoveMonsterTile(int index)
        {
           monstertiles.RemoveAt(index);
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

                    for (int p = 0; p < monstertiles.Count; p++)
                    {
                        Tile monstertile = monstertiles[p];
                        if (monstertile != null && i == monstertile.getMapX() && j == monstertile.getMapY())
                            monstertile.Draw(spriteBatch, displaytiles[ia][ja]);
                    }
                }
        }

        public bool SaveMap(StreamWriter file)
        {
            bool success = false;

            file.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            file.Write("<MAP x=\"" + map.Length + "\" y=\"" + map[0].Length + "\" ");
            
            if (playertile != null)
                file.Write("playerx=\"" + playertile.getMapX() + "\" playery=\"" + playertile.getMapY() + "\" ");

            file.WriteLine(">");
            file.WriteLine("<MONSTERS>");
            for (int i = 0; i < monstertiles.Count; i++)
                file.WriteLine("<MONSTER type=\"" + monstertiles[i].getType() + "\" x=\"" + monstertiles[i].getMapX() + "\" y=\"" + monstertiles[i].getMapY() + "\" />");
            file.WriteLine("</MONSTERS>");
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

        public bool LoadMap(StreamReader file, String filename, ToolMap toolmap)
        {
            this.filename = filename;
            bool success = false;
            bool gotxandy = false;
            int x = 0;
            int y = 0;
            int playerx = -1;
            int playery = -1;

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
                                if(reader["playerx"] != null && reader["playery"] != null)
                                {
                                    playerx = Convert.ToInt32(reader["playerx"]);
                                    playery = Convert.ToInt32(reader["playery"]);
                                }

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
                            if (reader.Name == "MONSTER")
                            {
                                //file.WriteLine("<MONSTER type=\"" + monstertiles[i].getType() + "\" x=\"" +
                                //    monstertiles[i].getMapX() + "\" y=\"" + monstertiles[i].getMapY() + "\" />");

                                String type = reader["type"];
                                tilex = Convert.ToInt32(reader["x"]);
                                tiley = Convert.ToInt32(reader["y"]);

                                monstertiles.Add(new Tile(tilex, tiley, 0, 0, 0, toolmap.getTool(type)));
                            }
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

            if (playertile == null)
                playertile = toolmap.getPlayerTile();

            if (playerx > 0 && playery > 0)
                setPlayerLocation(map[playerx][playery]);

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
                                playertile = new Tile(tilex, tiley, cur.getX(), cur.getY(), cur.getLength(), selectedTool);
                            else
                            {
                                playerx = tilex;
                                playery = tiley;

                                playertile.setMapX(tilex);
                                playertile.setMapY(tiley);
                            }
                        }
                    }
                    else if (selectedType == WorldTile.MONSTER)
                    {
                        Tile cur = map[tilex][tiley];
                        if (cur.getType() != WorldTile.WALL)
                        {
                                monstertiles.Add(new Tile(tilex, tiley, cur.getX(), cur.getY(), cur.getLength(), selectedTool));
                        }
                    }
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
                    Game1.addToEventQueue(ce[i]);
                }

            for(int i = 0; i < monstertiles.Count; i++)
                if(cur.getMapX() == monstertiles[i].getMapX() && cur.getMapY() == monstertiles[i].getMapY())
                {
                    Event e = new Event();
                    e.setEventType(EventType.BATTLE_TILE);
                    e.addProperty("battlemap", battlemap.GetFilePath());
                    e.addProperty("enemytexture", monstertiles[i].getTexture().Name);
                    e.addProperty("index", Convert.ToString(i));
                    Game1.addToEventQueue(e);
                }
        }

        public void shiftDown(int numtiles, bool noclip)
        {
            if (!noclip && playertile != null)
                playertile.setTexture(toolmap.getTexture(WorldTile.HERO_FRONT));

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
            if (!noclip && playertile != null)
                playertile.setTexture(toolmap.getTexture(WorldTile.HERO_BACK));

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
            if (!noclip && playertile != null)
                playertile.setTexture(toolmap.getTexture(WorldTile.HERO_LEFT));

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
            if (!noclip && playertile != null)
                playertile.setTexture(toolmap.getTexture(WorldTile.HERO_RIGHT));

            int newcurxtilemin = curxtilemin + numtiles;
            if (newcurxtilemin <= (xtiles - size))
            {
                Console.WriteLine("isObstacle()=" + map[playertile.getMapX() + numtiles][playertile.getMapY()].isObstacle());

                if (noclip || (playertile != null && 
                    !map[playertile.getMapX() + numtiles][playertile.getMapY()].isObstacle()))
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

        /*public Tile getMonsterTile()
        {
            return monstertile;
        }*/

        public void setMonsterLocation(int index, Tile newloc)
        {
            Tile monstertile = monstertiles[index];
            if (monstertile != null)
            {
                monstertile.setMapX(newloc.getMapX());
                monstertile.setMapY(newloc.getMapY());

                if (playertile != null && monstertile.getMapX() == playertile.getMapX() && monstertile.getMapY() == playertile.getMapY())
                {
                    processEvents(map[playertile.getMapX()][playertile.getMapY()]);
                    /*Event e = new Event();
                    e.setEventType(EventType.BATTLE_TILE);
                    Game1.addToEventQueue();*/
                }
            }
            else
                Console.WriteLine("Monster is null!!");
        }

        public Tile[] getMonsters()
        {
            return monstertiles.ToArray();
        }

        public void setPlayerLocation(Tile newloc)
        {
            if (playertile == null)
                playertile = toolmap.getPlayerTile();

            playertile.setMapX(newloc.getMapX());
            playertile.setMapY(newloc.getMapY());
            playerx = playertile.getMapX();
            playery = playertile.getMapY();
            curxtilemin = Math.Max(Math.Min(playerx - 8,xtiles - 17), 0);
            curytilemin = Math.Max(Math.Min(playery - 8,ytiles - 17), 0);
        }

        public Rectangle getDisplayRectangleFor(Tile cur)
        {
            Rectangle ret = Rectangle.Empty;

            int curx = cur.getMapX();
            int cury = cur.getMapY();

            if (curx >= curxtilemin && curx <= (curxtilemin + size) && cury >= curytilemin && cury <= (curytilemin + size))
            {
                int dispx = curx - curxtilemin;
                int dispy = cury - curytilemin;

                ret = displaytiles[dispx][dispy];
            }

            return ret;
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
            /*Tile m = map[monsterx][monstery];
            if (monstertile != null)
            {
                monstertile.setMapX(monsterx);
                monstertile.setMapY(monstery);
            }*/
        }
    }
}
