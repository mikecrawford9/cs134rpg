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
    class Tool
    {
        TileType type;
        Texture2D texture;
        bool isObstacle;
        int cost;

        public Tool(TileType type, Texture2D texture, bool isObstacle, int cost)
        {
            this.type = type;
            this.texture = texture;
            this.isObstacle = isObstacle;
            this.cost = cost;
        }

        public TileType getType()
        {
            return type;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public bool isObstacleTile()
        {
            return isObstacle;
        }

        public int getCost()
        {
            return cost;
        }

    }
}
