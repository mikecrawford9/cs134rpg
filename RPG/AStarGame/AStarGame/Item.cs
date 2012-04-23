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
    public enum ItemUsageType { TEMP, PERM }

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
        #region POTIONS
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

        #region CLOTHING
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 3)) }, 20, "Cotton Shirt", "Better than being naked...I guess...")]
        CLOTHING_1,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 7)) }, 50, "Travel Vest", "Light, fashionable armor.")]
        CLOTHING_2,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 21)) }, 1100, "Full Metal Vest", "More metal than human!")]
        CLOTHING_3,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 22)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 1700, "Elven Mail", "Looks more metrosexual than asexual to me.")]
        CLOTHING_4,
        [ItemAttribute(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 21)) }, 1100, "Full Metal Vest", "More metal than human!")]
        CLOTHING_3,
        #endregion 

        #region ROBE
        [ItemAttribute(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 3)) }, 20, "Cotton Shirt", "Better than being naked...I guess...")]
        ROBE_1,
        #endregion 

        #region ARMOR
        #endregion
    }
}
