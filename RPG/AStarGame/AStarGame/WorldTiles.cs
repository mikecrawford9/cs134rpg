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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AStarGame
{
    public enum TileType { MONSTER = 1, PLAYER, GRASS, TREES, WALL, WATER, SWAMP, ROCKS, MOUNTAIN};

    class WorldTileAttribute : Attribute
    {
        internal WorldTileAttribute(TileType type, String texturePath, bool isObstacle, byte cost)
        {
            this.tileType = type;
            this.texture = texturePath;
            this.isObstacle = isObstacle;
            this.cost = cost;
            this.info = "WorldTile [TileType: \"" + type + "\", Texture Path: \""
                + texturePath + "\", Is Obstacle: \"" +isObstacle + "\", Terrain Cost: \"" + cost + "\"]";
        }
        public TileType tileType { get; private set; }
        public String texture { get; private set; }
        public bool isObstacle { get; private set; }
        public byte cost { get; private set; }
        public String info { get; private set; }
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
        public static String GetTexture(this WorldTile t)
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
        
        public static String GetInformation(this WorldTile t)
        {
            return GetAttribute(t).info;
        }

        public static String toString(this WorldTile t)
        {
            return t.GetType().ToString();
        }
    }

    public enum WorldTile
    {
        [WorldTileAttribute(TileType.GRASS, "Tiles/Grass", false, 1)] GRASS,
        [WorldTileAttribute(TileType.TREES, "Tiles/Trees", false, 2)] TREES,
        [WorldTileAttribute(TileType.SWAMP, "Tiles/Swamp", false, 4)] SWAMP,
        [WorldTileAttribute(TileType.MOUNTAIN, "Tiles/MountainRange", false, 0)] MOUNTAIN,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/Player", false, 0)] PLAYER,
        [WorldTileAttribute(TileType.WATER, "Tiles/Water", false, 6)] WATER,
        [WorldTileAttribute(TileType.ROCKS, "Tiles/LavaRocks", false, 8)] ROCKS,
        [WorldTileAttribute(TileType.WALL, "Tiles/Wall", false, 0)] WALL,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Monster", false, 0)] MONSTER,

    }
}
