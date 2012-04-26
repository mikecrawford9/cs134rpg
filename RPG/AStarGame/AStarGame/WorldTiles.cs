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

namespace RPG
{
    public enum TileType { MONSTER = 1, PLAYER, GRASS, TREES, WALL, WATER, SWAMP, ROCKS, MOUNTAIN, SELECT, FENCE, TENT, NPC};

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

        [WorldTileAttribute(TileType.SELECT, "SELECT", true, 0)]
        SELECT,
        [WorldTileAttribute(TileType.GRASS, "Tiles/Grass", false, 1)] 
        GRASS,
        [WorldTileAttribute(TileType.TREES, "Tiles/Trees", false, 2)] 
        TREES,
        [WorldTileAttribute(TileType.SWAMP, "Tiles/Swamp", false, 4)] 
        SWAMP,
        [WorldTileAttribute(TileType.MOUNTAIN, "Tiles/Mountain", true, 0)] 
        MOUNTAIN,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroFront", true, 0)] 
        PLAYER,
        [WorldTileAttribute(TileType.WATER, "Tiles/Water", true, 6)] 
        WATER,
        [WorldTileAttribute(TileType.ROCKS, "Tiles/LavaRocks", false, 8)] 
        ROCKS,
        [WorldTileAttribute(TileType.WALL, "Tiles/Wall", true, 0)] 
        WALL,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Monster", true, 0)] 
        MONSTER,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceLeft", true, 0)] 
        FENCE_LEFT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceRight", true, 0)]
        FENCE_RIGHT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceTop", true, 0)]
        FENCE_TOP,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceBottom", true, 0)]
        FENCE_BOTTOM,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceAll", true, 0)]
        FENCE_ALL,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceUpperLeft", true, 0)]
        FENCE_UPPER_LEFT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceUpperRight", true, 0)]
        FENCE_UPPER_RIGHT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceLowerLeft", true, 0)]
        FENCE_LOWER_LEFT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceLowerRight", true, 0)]
        FENCE_LOWER_RIGHT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceNoBottom", true, 0)]
        FENCE_NO_BOTTOM,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceNoLeft", true, 0)]
        FENCE_NO_LEFT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceNoRight", true, 0)]
        FENCE_NO_RIGHT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceNoTop", true, 0)]
        FENCE_NO_TOP,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FencePole", true, 0)]
        FENCE_POLE,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceHorizontal", true, 0)]
        FENCE_HORIZONTAL,
        [WorldTileAttribute(TileType.FENCE, "Tiles/FenceVertcal", true, 0)]
        FENCE_VERTICAL,
        [WorldTileAttribute(TileType.FENCE, "Tiles/GrassStairs", true, 1)]
        GRASS_STAIRS,
        [WorldTileAttribute(TileType.FENCE, "Tiles/Tent", false, 0)]
        TENT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/TentPurple", false, 0)]
        TENT_PURPLE,
        [WorldTileAttribute(TileType.FENCE, "Tiles/TentBrown", false, 0)]
        TENT_BROWN,
        [WorldTileAttribute(TileType.FENCE, "Tiles/Castle", true, 1)]
        CASTLE,
        [WorldTileAttribute(TileType.FENCE, "Tiles/Black", true, 0)]
        BLACK,
        [WorldTileAttribute(TileType.FENCE, "Tiles/OldMan", true, 0)]
        OLD_MAN,
        [WorldTileAttribute(TileType.FENCE, "Tiles/ShopKeep", true, 0)]
        SHOP_KEEP,
        [WorldTileAttribute(TileType.FENCE, "Tiles/dirt", true, 0)]
        DIRT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/PillowDirt", true, 0)]
        PILLOW_DIRT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/RockWall", true, 0)]
        ROCK_WALL
    }
}
