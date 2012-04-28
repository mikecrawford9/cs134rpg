#region IMPORTS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion
namespace RPG
{
    public enum PlayerType { HUMAN, ENEMY }
    
    public class PlayerBase
    {
        #region CONSTRUCTOR
        public PlayerBase(PlayerType type, int HP_MAX, int MP_MAX,
            int INIT_ATK, int INIT_MAG_ATK, int INIT_DEF, int INIT_AGL, int INIT_HP_REGEN, int INIT_MP_REGEN,
            Item[] inventory, Item armor, Item weapon, ItemType[] equipableArmor, ItemType[] equipableWeapon,
            int HP_GROWTH, int MP_GROWTH, int ATK_GROWTH, int MAG_ATK_GROWTH, int DEF_GROWTH, int AGL_GROWTH,
            int[] expGrowth, Spell[] spellList, Sprite battleSprite)
        {
            #region INITIALIZATIONS
            this.playerType = type;
            this.baseHP = HP_MAX;
            this.baseMP= MP_MAX;
            this.baseATK = INIT_ATK;
            this.baseDEF = INIT_DEF;
            this.baseAGL = INIT_AGL;
            this.baseMAGATK = INIT_MAG_ATK;
            this.baseHPRegen = INIT_HP_REGEN;
            this.baseMPRegen = INIT_MP_REGEN;
            this.inventory = new Inventory(inventory);
            this.armor = armor;
            this.weapon = weapon;
            this.weaponSet = equipableWeapon;
            this.armorSet = equipableArmor;
            this.hpGrowth = HP_GROWTH;
            this.mpGrowth = MP_GROWTH;
            this.atkGrowth = ATK_GROWTH;
            this.magAtkGrowth = MAG_ATK_GROWTH;
            this.defGrowth = DEF_GROWTH;
            this.aglGrowth = AGL_GROWTH;
            this.expGrowth = expGrowth;
            this.spellList = spellList;
            #endregion

        }
        #region VARIABLES
        public readonly PlayerType playerType;
        public readonly int baseHP;
        public readonly int baseMP;
        public readonly int baseATK;
        public readonly int baseMAGATK;
        public readonly int baseDEF;
        public readonly int baseAGL;
        public readonly int baseHPRegen;
        public readonly int baseMPRegen;
        public readonly Inventory inventory;
        public readonly Item armor;
        public readonly ItemType[] armorSet;
        public readonly Item weapon;
        public readonly ItemType[] weaponSet;
        public readonly int hpGrowth;
        public readonly int mpGrowth;
        public readonly int atkGrowth;
        public readonly int magAtkGrowth;
        public readonly int defGrowth;
        public readonly int aglGrowth;
        public readonly int[] expGrowth;
        public readonly Spell[] spellList;
        #endregion

        #endregion

        #region ITEM COMPATIBILITY CHECKS
        public bool canEquip(Item i)
        {
            return canEquipArmor(i) || canEquipWeapon(i);
        }

        public bool canEquipArmor(Item i)
        {
            foreach (ItemType t in this.armorSet)
            {
                if (i.type == t)
                    return true;
            }
            return false;
        }

        public bool canEquipWeapon(Item i)
        {
            foreach (ItemType t in this.weaponSet)
            {
                if (i.type == t)
                    return true;
            }
            return false;
        }
        #endregion


        #region HUMAN PLAYER EXP GROWTH
        public static int[] WARRIOR_EXP_GROWTH = {0, 0, 30, 84, 176, 332, 582, 957, 1482, 2191, 3113, 4265, 5647, 7292, 9233, 11504};
        public static int[] CLERIC_EXP_GROWTH  = {0, 0, 45, 90, 180, 345, 572, 903, 1458, 2140, 3226, 4341, 5657, 7197, 8999, 11107};
        public static int[] MAGE_EXP_GROWTH    = {0, 0, 50, 96, 194, 350, 568, 895, 1418, 2150, 3102, 4292, 5760, 7419, 9424, 11770};
        #endregion

        
    }

    public class Player
    {
        public Player()
        {
        }

        public PlayerBase getNewPlayer(String type)
        {
            if(type == "WARRIOR")
            return new PlayerBase(PlayerType.HUMAN, 33, 18, 11, 0, 8, 6, 0, 0, 
            new Item[] { }, Item.ARMOR_1, Item.SWORD_1, new ItemType[] { ItemType.ARMOR, ItemType.CLOTHING }, 
            new ItemType[] { ItemType.SWORD, ItemType.MACE }, 7, 2, 4, 0, 1, 3, PlayerBase.WARRIOR_EXP_GROWTH, 
            new Spell[] { Spell.ATTACK }, Sprite.WARRIOR);
            else if(type == "CLERIC")
            return new PlayerBase(PlayerType.HUMAN, 29, 23, 6, 6, 5, 7, 0, 0, new Item[] { }, Item.CLOTHING_1, Item.MACE_1, new ItemType[] { ItemType.CLOTHING, ItemType.ROBE }, new ItemType[] { ItemType.STAFF, ItemType.MACE }, 7, 3, 2, 2, 1, 3, PlayerBase.CLERIC_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.HEAL }, Sprite.CLERIC);
            else if(type == "MAGE")
            return new PlayerBase(PlayerType.HUMAN, 28, 24, 0, 11, 4, 11, 0, 0, new Item[] { }, Item.ROBE_1, Item.STAFF_1, new ItemType[] { ItemType.ROBE }, new ItemType[] { ItemType.STAFF }, 6, 3, 0, 4, 1, 4, PlayerBase.MAGE_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.FIRE }, Sprite.MAGE);

            return null;
        }
        public PlayerBase getNewEnemy(String type)
        {
            if (type == "WARRIOR")
                return new PlayerBase(PlayerType.ENEMY, 33, 18, 11, 0, 8, 6, 0, 0,
                new Item[] { }, Item.ARMOR_1, Item.SWORD_1, new ItemType[] { ItemType.ARMOR, ItemType.CLOTHING },
                new ItemType[] { ItemType.SWORD, ItemType.MACE }, 7, 2, 4, 0, 1, 3, PlayerBase.WARRIOR_EXP_GROWTH,
                new Spell[] { Spell.ATTACK }, Sprite.WARRIOR);
            else if (type == "CLERIC")
                return new PlayerBase(PlayerType.ENEMY, 29, 23, 6, 6, 5, 7, 0, 0, new Item[] { }, Item.CLOTHING_1, Item.MACE_1, new ItemType[] { ItemType.CLOTHING, ItemType.ROBE }, new ItemType[] { ItemType.STAFF, ItemType.MACE }, 7, 3, 2, 2, 1, 3, PlayerBase.CLERIC_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.HEAL }, Sprite.CLERIC);
            else if (type == "MAGE")
                return new PlayerBase(PlayerType.ENEMY, 28, 24, 0, 11, 4, 11, 0, 0, new Item[] { }, Item.ROBE_1, Item.STAFF_1, new ItemType[] { ItemType.ROBE }, new ItemType[] { ItemType.STAFF }, 6, 3, 0, 4, 1, 4, PlayerBase.MAGE_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.FIRE }, Sprite.MAGE);

            return null;
        }
        

        public Player(PlayerBase pb, Sprite cs, String name = "Player", int level = 1, int currentExp = 0)
        {
            #region INITIALIZATIONS
            this.name = name;
            this.playerBase = pb;
            this.level = level;
            this.armor = pb.armor;
            this.weapon = pb.weapon;
            this.inventory = pb.inventory;
            this.hpLoss = 0;
            this.mpLoss = 0;
            this.isDead = false;
            this.currentExp = currentExp;
            this.currMaxMP = this.GetMAXMP();
            this.currMaxHP = this.GetMAXHP();
            this.currATK = this.GetCurrentAGL();
            this.currDEF = this.GetCurrentDEF();
            this.currAGL = this.GetCurrentAGL();
            this.currMAGATK = this.GetCurrentMAGATK();
            this.currHPREGEN = this.GetCurrentHealthRegen();
            this.currMPREGEN = this.GetCurrentManaRegen();
            this.sprite = cs;
            #endregion
        }

        #region VARIABLES
        public readonly String name;
        public readonly Sprite sprite;
        public PlayerBase playerBase;
        public int level;
        public int hpLoss;
        public int mpLoss;
        public Item armor;
        public Item weapon;
        public Inventory inventory;
        public bool isDead;
        public int currentExp;
        public int currMaxMP;
        public int currMaxHP;
        public int currATK;
        public int currDEF;
        public int currAGL;
        public int currMAGATK;
        public int currHPREGEN;
        public int currMPREGEN;
        #endregion
        #region HUMAN PLAYER


        public static PlayerBase WARRIOR = new PlayerBase(PlayerType.HUMAN, 33, 18, 11, 0, 8, 6, 0, 0,
            new Item[] { }, Item.ARMOR_1, Item.SWORD_1, new ItemType[] { ItemType.ARMOR, ItemType.CLOTHING },
            new ItemType[] { ItemType.SWORD, ItemType.MACE }, 7, 2, 4, 0, 1, 3, new int[] { 1, 2, 3 },
            new Spell[] { Spell.ATTACK }, Sprite.WARRIOR);

        public static PlayerBase CLERIC = new PlayerBase(PlayerType.HUMAN, 29, 23, 6, 6, 5, 7, 0, 0, new Item[] { }, Item.CLOTHING_1, Item.MACE_1, new ItemType[] { ItemType.CLOTHING, ItemType.ROBE }, new ItemType[] { ItemType.STAFF, ItemType.MACE }, 7, 3, 2, 2, 1, 3, PlayerBase.CLERIC_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.HEAL }, Sprite.CLERIC);
        public static PlayerBase MAGE = new PlayerBase(PlayerType.HUMAN, 28, 24, 0, 11, 4, 11, 0, 0, new Item[] { }, Item.ROBE_1, Item.STAFF_1, new ItemType[] { ItemType.ROBE }, new ItemType[] { ItemType.STAFF }, 6, 3, 0, 4, 1, 4, PlayerBase.MAGE_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.FIRE }, Sprite.MAGE);

        #endregion

        #region ENEMY PLAYER

        public static PlayerBase ENEMY1 = new PlayerBase(PlayerType.HUMAN, 33, 18, 11, 0, 8, 6, 0, 0, new Item[] { }, Item.ARMOR_1, Item.SWORD_1, new ItemType[] { ItemType.ARMOR, ItemType.CLOTHING }, new ItemType[] { ItemType.SWORD, ItemType.MACE }, 7, 2, 4, 0, 1, 3, PlayerBase.WARRIOR_EXP_GROWTH, new Spell[] { Spell.ATTACK }, Sprite.ENEMY_1);
        public static PlayerBase ENEMY2 = new PlayerBase(PlayerType.HUMAN, 29, 23, 6, 6, 5, 7, 0, 0, new Item[] { }, Item.CLOTHING_1, Item.MACE_1, new ItemType[] { ItemType.CLOTHING, ItemType.ROBE }, new ItemType[] { ItemType.STAFF, ItemType.MACE }, 7, 3, 2, 2, 1, 3, PlayerBase.CLERIC_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.HEAL }, Sprite.ENEMY_2);
        public static PlayerBase ENEMY3 = new PlayerBase(PlayerType.HUMAN, 28, 24, 0, 11, 4, 11, 0, 0, new Item[] { }, Item.ROBE_1, Item.STAFF_1, new ItemType[] { ItemType.ROBE }, new ItemType[] { ItemType.STAFF }, 6, 3, 0, 4, 1, 4, PlayerBase.MAGE_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.FIRE }, Sprite.ENEMY_3);
        public static PlayerBase ENEMYDRAGON = new PlayerBase(PlayerType.HUMAN, 35, 25, 12, 12, 5, 12, 10, 10, new Item[] { }, Item.ARMOR_3, Item.MACE_4, new ItemType[] { ItemType.ARMOR, ItemType.CLOTHING, ItemType.ROBE }, new ItemType[] { ItemType.SWORD, ItemType.MACE, ItemType.STAFF }, 10, 10, 10, 10, 10, 5, PlayerBase.MAGE_EXP_GROWTH, new Spell[] { Spell.ATTACK, Spell.FIRE, Spell.HEAL }, Sprite.WARRIOR);

        #endregion


        #region STAT CALCULATORS
        public bool UpdateLevel()
        {
            int newLevel = 0;
            foreach (int i in playerBase.expGrowth)
            {
                if (i < currentExp)
                {
                    newLevel = i;
                }
                else
                {
                    break;
                }
            }
            if (level < newLevel)
            {
                level = newLevel;
                return true;
            }
            return false;
        }

        public void CalculateCurrentStats()
        {
            this.UpdateLevel();
            this.currMaxMP = this.GetMAXMP();
            this.currMaxHP = this.GetMAXHP();
            this.currATK = this.GetCurrentAGL();
            this.currDEF = this.GetCurrentDEF();
            this.currAGL = this.GetCurrentAGL();
            this.currMAGATK = this.GetCurrentMAGATK();
            this.currHPREGEN = this.GetCurrentHealthRegen();
            this.currMPREGEN = this.GetCurrentManaRegen();
        }

        public int GetCurrentATK()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.ATK)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects)
            {
                if (i.effect == ItemEffectType.ATK)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseATK + (playerBase.atkGrowth * level) + bonus;
        }

        public int GetCurrentMAGATK()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.MAG_ATK)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects)
            {
                if (i.effect == ItemEffectType.MAG_ATK)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseMAGATK + (playerBase.magAtkGrowth * level) + bonus;
        }

        public int GetCurrentDEF()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.DEF)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects)
            {
                if (i.effect == ItemEffectType.DEF)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseDEF + (playerBase.defGrowth * level) + bonus;
        }
        public int GetCurrentAGL()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.AGL)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects)
            {
                if (i.effect == ItemEffectType.AGL)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseAGL + (playerBase.aglGrowth * level) + bonus;
        }
        public int GetMAXHP()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.HP && i.usage == ItemUsageType.PERM)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects )
            {
                if (i.effect == ItemEffectType.HP && i.usage == ItemUsageType.PERM)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseHP + (playerBase.hpGrowth * level) + bonus;
        }
        public int GetMAXMP()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.MP && i.usage == ItemUsageType.PERM)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects)
            {
                if (i.effect == ItemEffectType.MP && i.usage == ItemUsageType.PERM)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseMP + (playerBase.mpGrowth * level) + bonus;
        }

        public int GetCurrentHealthRegen()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.HP && i.usage == ItemUsageType.REGEN)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects)
            {
                if (i.effect == ItemEffectType.HP && i.usage == ItemUsageType.REGEN)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseHPRegen + bonus;
        }

        public int GetCurrentManaRegen()
        {
            int bonus = 0;
            foreach (ItemEffect i in playerBase.weapon.effects)
            {
                if (i.effect == ItemEffectType.MP && i.usage == ItemUsageType.REGEN)
                {
                    bonus += i.value;
                }
            }
            foreach (ItemEffect i in playerBase.armor.effects )
            {
                if (i.effect == ItemEffectType.MP && i.usage == ItemUsageType.REGEN)
                {
                    bonus += i.value;
                }
            }
            return playerBase.baseMPRegen + bonus;
        }

        public int GetCurrentHealth()
        {
            return GetMAXHP() - hpLoss;
        }
        public int GetCurrentMana()
        {
            return GetMAXMP() - mpLoss;
        }
        public void DealDamage(int amount)
        {
            this.hpLoss += amount;
            if(hpLoss >= this.GetMAXHP())
            {
                hpLoss = this.GetMAXHP();
                this.isDead = true;
            }

        }
        public void RecoverDamage(int amount)
        {
            this.hpLoss -= amount;
            if (hpLoss < 0)
            {
                hpLoss = 0;
            }

        }

        #endregion

        #region ITEM EFFECTS
        public bool UseHealingItem(Item healingItem)
        {
            bool effective = false;
            if (healingItem.type == ItemType.RECOVERY_POTION)
            {
                foreach (ItemEffect i in healingItem.effects)
                {
                    if (i.effect == ItemEffectType.HP && !isDead)
                    {
                        hpLoss -= i.value;
                        if (hpLoss < 0)
                            hpLoss = 0;
                        effective = true;
                    }
                    else if (i.effect == ItemEffectType.MP && !isDead)
                    {
                        mpLoss -= i.value;
                        if (mpLoss < 0)
                            mpLoss = 0;
                        effective = true;
                        
                    }
                    else if (i.effect == ItemEffectType.REVIVE)
                    {
                        if (isDead)
                        {
                            isDead = false;
                            effective = true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }
            
            return effective;

        }
        public Item equipArmor(Item i)
        {
            Item output = i;
            if (playerBase.canEquipArmor(i))
            {
                output = armor;
                armor = i;

            }
            this.CalculateCurrentStats();
            return output;
        }
        public Item equipWeapon(Item i)
        {
            Item output = i;
            if (playerBase.canEquipWeapon(i))
            {
                output = armor;
                weapon = i;

            }
            this.CalculateCurrentStats();
            return output;
        }
        #endregion

        #region SKILL EFFECTS

        public void ModifyStatSpell(SpellEffectType stat, int baseAmount)
        {
            switch (stat)
            {
                case SpellEffectType.AGL:
                    break;
                case SpellEffectType.ATK:
                    break;
                case SpellEffectType.DEF:
                    break;
                case SpellEffectType.HP:
                    this.hpLoss -= baseAmount;
                    break;
                case SpellEffectType.MAG_ATK:
                    break;
                case SpellEffectType.MP:
                    this.mpLoss -= baseAmount;
                    break;
                case SpellEffectType.REVIVE:
                    break;
            }
        }
        public void UseSpell(Player target, Spell spell)
        {
            switch (spell.type)
            {
                case SpellType.MAGIC:
                    foreach (SpellEffect se in spell.effects)
                    {
                        switch (se.target)
                        {
                            case SpellTargetType.ALL:
                                break; //not supported
                            case SpellTargetType.SELF:
                                this.ModifyStatSpell(se.type, se.value);
                                break;
                            case SpellTargetType.SINGLE:
                                int damage = se.value + this.GetCurrentMAGATK();
                                target.ModifyStatSpell(se.type, damage);
                                break;
                            default: break;
                        }
                    }
                    break;
                case SpellType.PHYSICAL:
                    foreach (SpellEffect se in spell.effects)
                    {
                        switch (se.target)
                        {
                            case SpellTargetType.ALL:
                                break; //not supported
                            case SpellTargetType.SELF:
                                this.ModifyStatSpell(se.type, se.value);
                                break;
                            case SpellTargetType.SINGLE:
                                int damage = se.value + this.GetCurrentATK();
                                target.ModifyStatSpell(se.type, damage);
                                break;
                            default: break;
                        }
                    }
                    break;
                case SpellType.SUPPORT:
                    foreach (SpellEffect se in spell.effects)
                    {
                        switch (se.target)
                        {
                            case SpellTargetType.ALL:
                                break; //not supported
                            case SpellTargetType.SELF:
                                this.ModifyStatSpell(se.type, se.value);
                                break;
                            case SpellTargetType.SINGLE:
                                int damage = se.value + this.GetCurrentMAGATK();
                                target.ModifyStatSpell(se.type, damage);
                                break;
                            default: break;
                        }
                    }
                    break;
                default: break;
            }
        }
        #endregion
    }

    public class Enemy
    {
        public Enemy(Player p, int expGiveBase = 20, int goldGiveBase = 20)
        {
            this.player = p;
            this.expGiveBase = expGiveBase;
            this.goldGiveBase = goldGiveBase;
        }

        public Player player;
        public int expGiveBase;
        public int goldGiveBase;

        public int GetExp()
        {
            return expGiveBase + expGiveBase * (this.player.level - 1) * (this.player.level - 1);
        }
        public int GetGold()
        {
            return goldGiveBase + goldGiveBase * (this.player.level) * (this.player.level);
        }

    }

    

    
}
