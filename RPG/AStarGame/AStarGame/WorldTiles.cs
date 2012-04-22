/*
Name: Michael Crawford
Class: CS134
Instructor: Dr. Teoh
Term: Spring 2012
Assignment: Project 2
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AStarGame
{
    public enum TileType { MONSTER = 1, PLAYER, GRASS, TREES, WALL, WATER, SWAMP, ROCKS};

    class WorldTileAttribute : Attribute
    {
        internal WorldTileAttribute(TileType type, Texture2D texture, bool isObstacle, byte cost)
        {
            this.tileType = type;
            this.texture = texture;
            this.isObstacle = isObstacle;
            this.cost = cost;
        }
        public TileType tileType { get; private set; }
        public Texture2D texture { get; private set; }
        public bool isObstacle { get; private set; }
        public byte cost { get; private set; }
    }

    public static class WorldTiles
    {
        private static WorldTileAttribute GetAttribute(WorldTile p)
        {
            return (WorldTileAttribute)Attribute.GetCustomAttribute(ForValue(p), typeof(WorldTileAttribute));
        }

        private static MemberInfo ForValue(WorldTile p)
        {
            return typeof(WorldTile).GetField(Enum.GetName(typeof(WorldTile), p));
        }


        public static TileType GetTileType(this WorldTile t)
        {
            return GetAttribute(t).tileType;
        }
        public static Texture2D GetTexture(this WorldTile t)
        {
            return GetAttribute(t).texture;
        }
        public static bool isObstacle(this WorldTile t)
        {
            return GetAttribute(t).isObstacle;
        }
        public static byte GetCost(this WorldTile t)
        {
            return GetAttribute(t).cost;
        }
    }

    public enum WorldTile
    {

    }
}
