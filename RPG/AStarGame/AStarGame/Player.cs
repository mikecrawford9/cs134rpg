using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarGame
{
    public enum PlayerType { HUMAN, ENEMY, NPC }
    public class Player
    {
        public Player(PlayerType type, String name, int HP_MAX, int MP_MAX,
            int INIT_ATK, int INIT_DEF, int INIT_AGL, int INIT_HP_REGEN, int INIT_MP_REGEN,
            Item[] inventory, Item armor, Item weapon)
        {
            this.playerType = type;
            this.name = name;
            this.healthPointMax = HP_MAX;
            this.currentHealthPoints = HP_MAX;
            this.manaPointMax = MP_MAX;
            this.currentManaPoints = MP_MAX;
            this.baseATK = INIT_ATK;
            this.baseDEF = INIT_DEF;
            this.baseAGL = INIT_AGL;
            this.baseHPRegen = INIT_HP_REGEN;
            this.baseMPRegen = INIT_MP_REGEN;
            this.inventory = inventory;
            this.armor = armor;
            this.weapon = weapon;

        }
        public PlayerType playerType;
        public String name;
        public int healthPointMax;
        public int currentHealthPoints;
        public int manaPointMax;
        public int currentManaPoints;
        public int baseATK;
        public int baseDEF;
        public int baseAGL;
        public int baseHPRegen;
        public int baseMPRegen;
        public Item[] inventory;
        public Item armor;
        public Item weapon;

    }

    
}
