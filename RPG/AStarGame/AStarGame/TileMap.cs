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
    class TileMap
    {
        public const int MAP_HEIGHT = 512;
        public const int MAP_WIDTH = 512;

        private int x;
        private int y;
        private int size;
        private int pixelsperside;
        Tile[][] map;

        Tile highlighted;

        Tile playertile;
        Tile monstertile;

        int playerx;
        int playery;

        int monsterx;
        int monstery;

        public TileMap(int x, int y, int size, Texture2D pixel, Tool initial)
        {
            this.size = size;
            double tss = MAP_HEIGHT / size;
            int tilesidesize = (int)Math.Floor(tss);
            pixelsperside = tilesidesize;

            map = new Tile[size][];

            for (int i = 0; i < size; i++)
            {
                map[i] = new Tile[size];
                for (int j = 0; j < size; j++)
                {
                    map[i][j] = new Tile(i, j, (x + i * tilesidesize), (y + j * tilesidesize), tilesidesize, initial);
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    map[i][j].Draw(spriteBatch);

            if (playertile != null)
                playertile.Draw(spriteBatch);

            if (monstertile != null)
                monstertile.Draw(spriteBatch);
        }

        public void Update(Tile selectedTool)
        {
            MouseState mouseState = Mouse.GetState();
            int mousex = mouseState.X;
            int mousey = mouseState.Y;

            //if mouse is within the map, do mouse over...
            if ((mousex > 10 && mousex < MAP_WIDTH + 10) &&
                (mousey > 10 && mousey < MAP_HEIGHT + 10))
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

                                playertile.setX(cur.getX());
                                playertile.setY(cur.getY());
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

                                monstertile.setX(cur.getX());
                                monstertile.setY(cur.getY());
                                monstertile.setMapX(tilex);
                                monstertile.setMapY(tiley);
                            }
                        }
                    }
                    else
                    {
                        map[tilex][tiley].applyTool(selectedTool);
                    }

                }
                    if (highlighted != null)
                        highlighted.unhighlight();

                    
                    highlighted = map[tilex][tiley];
                    highlighted.highlight();
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
                monstertile.setX(newloc.getX());
                monstertile.setY(newloc.getY());
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
                playertile.setX(newloc.getX());
                playertile.setY(newloc.getY());
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
                playertile.setX(p.getX());
                playertile.setY(p.getY());
                playertile.setMapX(playerx);
                playertile.setMapY(playery);
            }
            Tile m = map[monsterx][monstery];
            if (monstertile != null)
            {
                monstertile.setX(m.getX());
                monstertile.setY(m.getY());
                monstertile.setMapX(monsterx);
                monstertile.setMapY(monstery);
            }
        }
    }
}
