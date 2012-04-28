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

namespace RPG
{
    public class Tool
    {
        WorldTile type;
        Texture2D texture;
        bool isObstacle;
        int cost;

        public Tool(WorldTile type, Texture2D texture)
        {
            this.type = type;
            this.texture = texture;
            this.isObstacle = type.isObstacle();
            this.cost = type.GetCost();
        }

        public WorldTile getType()
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
