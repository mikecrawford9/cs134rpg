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

namespace AStarGame
{
    class Tile: IComparable<Tile>, IEquatable<Tile>
    {
        Texture2D pixel;
        Rectangle sq;
        Color curcolor;
        TileType type;
        int arrx;
        int arry;
        int cost;
        int totalcost;
        bool obstacle;
        Tool tool;
        Tile prev;


        public Tile(int arrx, int arry, int x, int y, int sidelength, Texture2D pixel)
        {
            this.arrx = arrx;
            this.arry = arry;
            curcolor = Color.White;
            this.pixel = pixel;
            sq = new Rectangle(x, y, sidelength, sidelength);
        }

        public Tile(int arrx, int arry, int x, int y, int sidelength, Texture2D pixel, Color color)
        {
            this.arrx = arrx;
            this.arry = arry;
            curcolor = Color.White;
            this.pixel = pixel;
            sq = new Rectangle(x, y, sidelength, sidelength);
            curcolor = color;
        }

        public Tile(int arrx, int arry, int x, int y, int sidelength, Tool tool)
        {
            this.arrx = arrx;
            this.arry = arry;
            curcolor = Color.White;
            this.pixel = tool.getTexture();
            sq = new Rectangle(x, y, sidelength, sidelength);
            this.type = tool.getType();
            this.obstacle = tool.isObstacleTile();
            this.cost = tool.getCost();
            this.totalcost = this.cost;
            this.tool = tool;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pixel, sq, curcolor);
        }

        public void highlight()
        {
            curcolor = Color.LightGray;
        }

        public void unhighlight()
        {
            curcolor = Color.White;
        }

        public void select()
        {
            curcolor = Color.SeaGreen;
        }

        public void unselect()
        {
            curcolor = Color.White;
        }

        public int getX()
        {
            return sq.X;
        }

        public int getY()
        {
            return sq.Y;
        }

        public int getLength()
        {
            return sq.Width;
        }

        public void setColor(Color color)
        {
            curcolor = color;
        }

        public Color getColor()
        {
            return curcolor;
        }

        public void setTexture(Texture2D texture)
        {
            this.pixel = texture;
        }

        public Texture2D getTexture()
        {
            return pixel;
        }

        public TileType getType()
        {
            return type;
        }

        public int getTileCost()
        {
            return cost;
        }

        public int getTotalCost()
        {
            return totalcost;
        }

        public void addToTotalCost(int add)
        {
            totalcost = totalcost + add;
        }

        public void resetCost()
        {
            totalcost = cost;
        }

        public void resetPrev()
        {
            prev = null;
        }

        public bool isObstacle()
        {
            return obstacle;
        }

        public void setX(int x)
        {
            sq.X = x;
        }

        public void setMapX(int x)
        {
            arrx = x;
        }

        public void setMapY(int y)
        {
            arry = y;
        }

        public void setY(int y)
        {
            sq.Y = y;
        }

        public int getMapX()
        {
            return arrx;
        }

        public int getMapY()
        {
            return arry;
        }

        public int CompareTo(Tile other)
        {
            return totalcost.CompareTo(other.totalcost);
        }

        public bool isAt(int mapx, int mapy)
        {
            if (arrx == mapx && arry == mapy)
                return true;
            else
                return false;
        }


        public void setPrevious(Tile tile)
        {
            this.prev = tile;
        }

        public Tile getPrevious()
        {
            return prev;
        }

        public int getCost()
        {
            return cost;
        }

        public void applyTool(Tile tool)
        {
            this.pixel = tool.getTexture();
            this.type = tool.getType();
            this.cost = tool.getTileCost();
        }

        public bool Equals(Tile other)
        {
            if (arrx == other.getMapX() && arry == other.getMapY())
                return true;
            else
                return false;
        }
    }
}
