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
    public enum TileType { MONSTER, PLAYER, NPC, GRASS, TREES, WALL, WATER, SWAMP, ROCKS, MOUNTAIN, SELECT, FENCE, TENT };
    public enum MapType { WORLD, BATTLE }

    class MapAttribute : Attribute
    {
        internal MapAttribute(MapType type, String filePath, int initX, int initY)
        {
            this.mapType = type;
            this.filePath = filePath;
            this.x = initX;
            this.y = initY;
        }
        public MapType mapType { get; private set; }
        public String filePath { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
    }
    public static class Maps
    {
        private static MapAttribute GetAttribute(Map p)
        {
            return (MapAttribute)Attribute.GetCustomAttribute(ForValue(p), typeof(MapAttribute));
        }
        private static MemberInfo ForValue(Map p)
        {
            return typeof(Map).GetField(Enum.GetName(typeof(Map), p));
        }
        private static MapType GetMapType(this Map p)
        {
            return GetAttribute(p).mapType;
        }
        private static String GetFilePath(this Map p)
        {
            return GetAttribute(p).filePath;
        }
        private static int GetX(this Map p)
        {
            return GetAttribute(p).x;
        }
        private static int GetY(this Map p)
        {
            return GetAttribute(p).y;
        }
    }

    public enum Map
    {
        #region BATTLE MAPS
        [MapAttribute(MapType.BATTLE, "battlegrass", 8, 8)]
        BATTLE_GRASS,
        [MapAttribute(MapType.BATTLE, "battlerock", 8, 8)]
        BATTLE_ROCK,
        [MapAttribute(MapType.BATTLE, "battlecave", 8, 8)]
        BATTLE_CAVE,
        [MapAttribute(MapType.BATTLE, "battleswamp", 8, 8)]
        BATTLE_SWAMP,
        [MapAttribute(MapType.BATTLE, "battlegeneric", 8, 8)]
        BATTLE_GENERIC,
        #endregion
        #region OVERWORLD MAPS
        [MapAttribute(MapType.WORLD, "world2", 8, 8)]
        OVERWORLD,
        [MapAttribute(MapType.WORLD, "inn", 8, 8)]
        INN,
        [MapAttribute(MapType.WORLD, "shop", 8, 8)]
        SHOP,
        [MapAttribute(MapType.WORLD, "oldmanhouse", 8, 8)]
        OLD_MAN_HOUSE,
        [MapAttribute(MapType.WORLD, "town", 8, 8)]
        TOWN,
        #endregion
        #region DUNGEON
        [MapAttribute(MapType.WORLD, "dragoncave", 8, 8)]
        DRAGON_CAVE,
        [MapAttribute(MapType.WORLD, "dragoncave2", 8, 8)]
        DRAGON_CAVE_2,
        #endregion

    }


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
        #region SPECIAL MAP TILES
        [WorldTileAttribute(TileType.SELECT, "SELECT", true, 0)]
        SELECT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroFront", true, 0)]
        PLAYER,
        #endregion

        #region MAPTILES
        
        #region WORLDMAPTILES
        [WorldTileAttribute(TileType.GRASS, "Tiles/Grass", false, 1)] 
        GRASS,
        [WorldTileAttribute(TileType.TREES, "Tiles/Trees", false, 2)] 
        TREES,
        [WorldTileAttribute(TileType.SWAMP, "Tiles/Swamp", false, 4)] 
        SWAMP,
        [WorldTileAttribute(TileType.MOUNTAIN, "Tiles/Mountain", true, 0)] 
        MOUNTAIN,  
        [WorldTileAttribute(TileType.WATER, "Tiles/Water", true, 6)] 
        WATER,
        [WorldTileAttribute(TileType.ROCKS, "Tiles/LavaRocks", false, 8)] 
        ROCKS,
        [WorldTileAttribute(TileType.WALL, "Tiles/Wall", true, 0)] 
        WALL,
        [WorldTileAttribute(TileType.FENCE, "Tiles/Black", true, 0)]
        BLACK,
        [WorldTileAttribute(TileType.FENCE, "Tiles/Dirt", false, 0)]
        DIRT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/PillowDirt", false, 0)]
        PILLOW_DIRT,
        [WorldTileAttribute(TileType.FENCE, "Tiles/RockWall", true, 0)]
        ROCK_WALL,
        #endregion

        #region FENCES
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
        #endregion

        [WorldTileAttribute(TileType.FENCE, "Tiles/GrassStairs", false, 1)]
        GRASS_STAIRS,

        #region BUILDINGS 
        [WorldTileAttribute(TileType.TENT, "Tiles/Tent", false, 0)]
        TENT,
        [WorldTileAttribute(TileType.TENT, "Tiles/TentPurple", false, 0)]
        TENT_PURPLE,
        [WorldTileAttribute(TileType.TENT, "Tiles/TentBrown", false, 0)]
        TENT_BROWN,
        [WorldTileAttribute(TileType.FENCE, "Tiles/Castle", false, 0)]
        CASTLE,
        #endregion
        #endregion

        #region NPC
        [WorldTileAttribute(TileType.FENCE, "Tiles/OldMan", true, 0)]
        OLD_MAN,
        [WorldTileAttribute(TileType.FENCE, "Tiles/ShopKeep", true, 0)]
        SHOP_KEEP,
        #endregion

        #region ENEMY
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Monster", true, 0)]
        MONSTER,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy1LeftFace", true, 0)]
        ENEMY_1_LEFT_FACE,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy1RightFace", true, 0)]
        ENEMY_1_RIGHT_FACE,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy1LeftFaceHit", true, 0)]
        ENEMY_1_LEFT_FACE_HIT,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy1RightFaceHit", true, 0)]
        ENEMY_1_RIGHT_FACE_HIT,

        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy2LeftFace", true, 0)]
        ENEMY_2_LEFT_FACE,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy2RightFace", true, 0)]
        ENEMY_2_RIGHT_FACE,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy2LeftFaceHit", true, 0)]
        ENEMY_2_LEFT_FACE_HIT,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy2RightFaceHit", true, 0)]
        ENEMY_2_RIGHT_FACE_HIT,

        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy3LeftFace", true, 0)]
        ENEMY_3_LEFT_FACE,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy3RightFace", true, 0)]
        ENEMY_3_RIGHT_FACE,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy3LeftFaceHit", true, 0)]
        ENEMY_3_LEFT_FACE_HIT,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/Enemy3RightFaceHit", true, 0)]
        ENEMY_3_RIGHT_FACE_HIT,

        [WorldTileAttribute(TileType.MONSTER, "Tiles/EnemyDragonDark1FrontFace", true, 0)]
        ENEMY_DRAGON_DARK_1_FRONT_FACE,
        [WorldTileAttribute(TileType.MONSTER, "Tiles/EnemyDragonDarkDefeated1FrontFace", true, 0)]
        ENEMY_DRAGON_DARK_1_DEFEATED_FRONT_FACE,
        #endregion

        #region PLAYER
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroFront", true, 0)]
        HERO_FRONT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroBack", true, 0)]
        HERO_BACK,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroLeftFace", true, 0)]
        HERO_LEFT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroRightFace", true, 0)]
        HERO_RIGHT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroFrontHit", true, 0)]
        HERO_FRONT_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroBackHit", true, 0)]
        HERO_BACK_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroLeftFaceHit", true, 0)]
        HERO_LEFT_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HeroRightFaceHit", true, 0)]
        HERO_RIGHT_HIT,

        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroFront", true, 0)]
        DARK_HERO_FRONT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroBack", true, 0)]
        DARK_HERO_BACK,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroLeftFace", true, 0)]
        DARK_HERO_LEFT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroRightFace", true, 0)]
        DARK_HERO_RIGHT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroFrontHit", true, 0)]
        DARK_HERO_FRONT_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroBackHit", true, 0)]
        DARK_HERO_BACK_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroLeftFaceHit", true, 0)]
        DARK_HERO_LEFT_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/DarkHeroRightFaceHit", true, 0)]
        DARK_HERO_RIGHT_HIT,

        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroFront", true, 0)]
        LIGHT_HERO_FRONT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroBack", true, 0)]
        LIGHT_HERO_BACK,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroLeftFace", true, 0)]
        LIGHT_HERO_LEFT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroRightFace", true, 0)]
        LIGHT_HERO_RIGHT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroFrontHit", true, 0)]
        LIGHT_HERO_FRONT_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroBackHit", true, 0)]
        LIGHT_HERO_BACK_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroLeftFaceHit", true, 0)]
        LIGHT_HERO_LEFT_HIT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/LightHeroRightFaceHit", true, 0)]
        LIGHT_HERO_RIGHT_HIT,
        #endregion

        #region SPELLS
        [WorldTileAttribute(TileType.PLAYER, "Tiles/FireBall", true, 0)]
        FIREBALL,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/HealBall", true, 0)]
        HEAL,
        #endregion

        #region WEAPONS - MACE
        [WorldTileAttribute(TileType.PLAYER, "Tiles/MaceAttack1", true, 0)]
        MACE_ATTACK_1,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/MaceAttack2", true, 0)]
        MACE_ATTACK_2,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/MaceAttack3", true, 0)]
        MACE_ATTACK_3,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/MaceAttack4", true, 0)]
        MACE_ATTACK_4,
        #endregion

        #region WEAPONS - SWORD
        [WorldTileAttribute(TileType.PLAYER, "Tiles/SwordAttack1", true, 0)]
        SWORD_ATTACK_1,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/SwordAttack2", true, 0)]
        SWORD_ATTACK_2,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/SwordAttack3", true, 0)]
        SWORD_ATTACK_3,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/SwordAttack4", true, 0)]
        SWORD_ATTACK_4,
        #endregion

        #region WEAPONS - STAFF
        [WorldTileAttribute(TileType.PLAYER, "Tiles/StaffAttack1", true, 0)]
        STAFF_ATTACK_1,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/StaffAttack2", true, 0)]
        STAFF_ATTACK_2,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/StaffAttack3", true, 0)]
        STAFF_ATTACK_3,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/StaffAttack4", true, 0)]
        STAFF_ATTACK_4,
        #endregion

        #region TEXT BOX
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBox", true, 0)]
        TEXTBOX_CENTER,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxBottom", true, 0)]
        TEXTBOX_BOTTOM,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxLeft", true, 0)]
        TEXTBOX_LEFT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxLowerLeft", true, 0)]
        TEXTBOX_LOWER_LEFT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxLowerRight", true, 0)]
        TEXTBOX_LOWER_RIGHT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxRight", true, 0)]
        TEXTBOX_RIGHT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxTop", true, 0)]
        TEXTBOX_TOP,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxUpperLeft", true, 0)]
        TEXTBOX_UPPER_LEFT,
        [WorldTileAttribute(TileType.PLAYER, "Tiles/TextBoxUpperRight", true, 0)]
        TEXTBOX_UPPER_RIGHT,
        #endregion

    }
}
