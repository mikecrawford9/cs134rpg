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
    public enum ItemType {ARMOR, ROBE, CLOTHING, MELEE_WEAPON, MAGIC_WEAPON, RECOVERY_POTION, STAT_POTION, QUEST};
    public enum ItemEffectType {AGL, ATK, DEF, MAG_ATK, HP, MP}
    public enum ItemTargetType { SELF, SINGLE, ALL }
    public enum ItemUsageType { TEMP, PERM, REGEN }

    class ItemEffect
    {
        public ItemEffect(ItemEffectType effect, ItemTargetType target, ItemUsageType usage, bool isEquipable, int value)
        {
            this.target = target;
            this.effect = effect;
            this.value = value;
            this.usage = usage;
            this.isEquipable = isEquipable;
        }
        public ItemTargetType target;
        public ItemEffectType effect;
        public int value;
        public ItemUsageType usage;
        public bool isEquipable;

    }


    class ItemAttribute : Attribute
    {

        internal ItemAttribute(ItemType type, ItemEffect[] effect, int cost, String name, String description)
        {
            this.type = type;
            this.effect = effect;
            this.cost = cost;
            this.name = name;
            this.description = description;
        }
        public ItemType type { get; private set; }
        public ItemEffect[] effect { get; private set; }
        public int cost { get; private set; }
        public String description {get; private set;}
        public String name { get; private set; }
    }


    public static class Items
    {
        private static ItemAttribute GetAttribute(Item i)
        {
            return (ItemAttribute)Attribute.GetCustomAttribute(ForValue(i), typeof(ItemAttribute));
        }
        private static MemberInfo ForValue(Item i)
        {
            return typeof(Item).GetField(Enum.GetName(typeof(Item), i));
        }
        public static int GetCost(this Item i)
        {
            return GetAttribute(i).cost;
        }
        public static ItemEffect[] GetEffect(this Item i)
        {
            return GetAttribute(i).effect;
        }
        public static ItemType GetItemType(this Item i)
        {
            return GetAttribute(i).type;
        }
        public static String GetDescription(this Item i)
        {
            return GetAttribute(i).description;
        }
        public static String GetName(this Item i)
        {
            return GetAttribute(i).name;
        }

       
    }

    public enum Item
    {
        #region QUEST

        #endregion

        #region RECOVERY POTION
        [ItemAttribute(ItemType.RECOVERY_POTION, new ItemEffect[] {(new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 100))}, 20, "Low Health Potion", "Heals 100 HP.")] 
        HP_POTION_100,
        [ItemAttribute(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 250)) }, 110, "Medium Health Potion", "Heals 250 HP.")]
        HP_POTION_250,
        [ItemAttribute(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 500)) }, 250, "High Health Potion", "Heals 500 HP")]
        HP_POTION_500,
        [ItemAttribute(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.MP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 100)) }, 20, "Low Mana Potion", "Heals 100 MP")]
        MP_POTION_100,
        [ItemAttribute(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.MP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 250)) }, 110, "Medium Mana Potion", "Heals 250 MP")]
        MP_POTION_250,
        [ItemAttribute(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 500)) }, 20, "High Mana Potion", "Heals 500 MP")]
        MP_POTION_500,
        #endregion

        #region STAT POTION
        #endregion

        #region CLOTHING
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 3)) }, 20, "Cotton Shirt", "Better than being naked...I guess...")]
        CLOTHING_1,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 7)) }, 50, "Travel Vest", "Light, fashionable armor.")]
        CLOTHING_2,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 16)) }, 400, "Fur Coat", "I hope its Faux.")]
        CLOTHING_3,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 18)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.PERM, true, 8)) }, 850, "Mage's Clothes", "We stole them from magi.")]
        CLOTHING_4,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 21)) }, 1100, "Full Metal Vest", "More metal than human.")]
        CLOTHING_5,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 22)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 1700, "Elven Mail", "Seems more metrosexual than asexual to me.")]
        CLOTHING_6,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 28)) }, 2800, "Festival Coat", "So many tassles...")]
        CLOTHING_7,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 25)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 10)) }, 2800, "Kimono", "The ultimate in Japanese only-wear.")]
        CLOTHING_8,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 37)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)) }, 4000, "Wild Coat", "Bring out your inner beast.")]
        CLOTHING_9,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42))}, 9800, "Storm Gear", "Something for that perfect storm.")]
        CLOTHING_10,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 43)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 5)) }, 10400, "Erinyes Tunic", "Might make you a bit vengeful.")]
        CLOTHING_11,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 47)) }, 14900, "Mythril Clothes", "Made from the best stuff on Earth.")]
        CLOTHING_12,
        #endregion 

        #region ROBE
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 4)) }, 25, "Cotton Robe", "You don't even need pants with them.")]
        ROBE_1,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 10)) }, 200, "Travel Robe", "Just enough protection to leave your house.")]
        ROBE_2,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 1400, "Silk Robe", "The best in comfort.")]
        ROBE_3,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 26)) }, 2400, "Jerkin", "Let's just say you will have a hard time putting them on.")]
        ROBE_4,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 29)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 4000, "Formal Wear", "So fancy.")]
        ROBE_5,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 39)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 7000, "Blessed Robe", "I heard its possessed by holy ghosts.")]
        ROBE_6,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42)) }, 8900, "Dragon Robe", "I can't believe a dragon could fit this.")]
        ROBE_7,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 39)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 9000, "Magical Cassock", "Only the holiest of men can wear something so good.")]
        ROBE_8,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 44)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 10)) }, 9900, "Ardagh Robe", "Sown by the most flamboyant dragon ever.")]
        ROBE_9,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 46)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 11400, "Aeolian Robe", "Fabric so light, it seems like air.")]
        ROBE_10,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 43)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 13500, "Oracle's Robe", "You'll feel insightful in this robe.")]
        ROBE_11,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)) }, 16000, "Feathered Robe", "Become as agile as a bird.")]
        ROBE_12,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 47)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 20)) }, 20000, "Iris Robe", "Almost as beautiful as Iris herself.")]
        ROBE_13,
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 48)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 30)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 30)) }, 36000, "Mysterious Robe", "No one knows where is came from.")]
        ROBE_14,
        #endregion 

        #region ARMOR
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_1,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_2,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_3,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_4,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_5,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_6,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_7,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_8,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_9,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_10,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_11,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.")]
        ARMOR_12,

        #endregion

        #region MELEE WEAPON
        #endregion

        #region MAGIC WEAPON
        #endregion


    }
}
