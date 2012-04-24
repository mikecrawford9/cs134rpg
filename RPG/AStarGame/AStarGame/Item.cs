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
    public enum ItemType {ARMOR, ROBE, CLOTHING, SWORD, MACE, STAFF, RECOVERY_POTION, STAT_POTION, QUEST};
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
        public static ItemEffect[] GetEffects(this Item i)
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
        [ItemAttribute(ItemType.QUEST, new ItemEffect[] { }, 0, "Dungeon Key", "Key used to open Dragon's Cave.")]
        DUNGEON_KEY,
        [ItemAttribute(ItemType.QUEST, new ItemEffect[] { }, 0, "Dragon's Skull", "Skull of the Dragon King.")]
        DRAGON_SKULL,
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
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 21)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 1000, "Magic Armor", "Something magical about that old armor.")]
        ARMOR_2,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 25)) }, 2000, "Chain Mail", "Is this really iron, it seems more like paper clips.")]
        ARMOR_3,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)) }, 3600, "Armored Shell", "You can hide in it like a turtle.")]
        ARMOR_4,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 33)) }, 4400, "Plate Mail", "You can't eat off this kind of plate.")]
        ARMOR_5,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 36)) }, 4900, "Steel Armor", "Just try to move in this armor.")]
        ARMOR_6,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 43)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 9000, "Erebus Armor", "For something so dark, it pretty light.")]
        ARMOR_7,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)) }, 10000, "Dragon Armor", "No dragons were harmed in the making of this armor.")]
        ARMOR_8,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 47)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 13000, "Chronos Armor", "Only time can tell if this armor is any good.")]
        ARMOR_9,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 15000, "Spiked Armor", "Good thing the spikes are on the outside.")]
        ARMOR_10,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 17000, "Asura's Armor", "Find balance within yourself.")]
        ARMOR_11,
        [ItemAttribute(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 52)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)) }, 22000, "Lion's Armor", "Become the king of beasts.")]
        ARMOR_12,

        #endregion

        #region SWORDS
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 6))}, 30, "Machete", "Good for weeding.")]
        SWORD_1,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 8)) }, 120, "Short Sword", "Commonly mistaken for a large knife.")]
        SWORD_2,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 14)) }, 500, "Long Sword", "May be misinterpreted as compensation for something.")]
        SWORD_3,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 1600, "Hunter's Sword", "Used for more than hunting game.")]
        SWORD_4,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)) }, 3500, "Broad Sword", "Can cut down small trees in one swoop.")]
        SWORD_5,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 55)) }, 6000, "Elven Rapier", "Only elves can produce something of this much detail.")]
        SWORD_6,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 62)) }, 7500, "Battle Rapier", "Always be battle ready.")]
        SWORD_7,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 70)) }, 9000, "Claymore", "The sword only a giant can love.")]
        SWORD_8,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 90)) }, 12000, "Great Sword", "Whats so great about it, its oversided and weights a ton.")]
        SWORD_9,
        [ItemAttribute(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 100)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 15000, "Soul Blade", "It feels like my soul is attached to this sword.")]
        SWORD_10,
        #endregion

        #region MACE
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 4)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 2)) }, 80, "Mace", "A ball of metal on a stick, what else do you need.")]
        MACE_1,
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 9)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 5)) }, 500, "Spiked Mace", "Great for tendering meat, as well as your enemies.")]
        MACE_2,
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 18)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 8)) }, 2000, "Heavy Mace", "Not for the weak of arm.")]
        MACE_3,
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 28)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 4000, "Battle Mace", "Just make sure to only hit your enemies.")]
        MACE_4,
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 22)) }, 6500, "War Mace", "Only war would force someone to make something so dangerous.")]
        MACE_5,
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 27)) }, 8200, "Bladed Mace", "All the fun of beating your enemy senseless with the presision of a sword.")]
        MACE_6,
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 59)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 35)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 11000, "Blessed Mace", "When god support using a weapon, you can't say no.")]
        MACE_7,
        [ItemAttribute(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 70)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 20)) }, 14000, "Righteous Mace", "Using this mace just feels so right.")]
        MACE_8,
        #endregion

        #region STAFF
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 6)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 3)) }, 50, "Wooden Staff", "More useful as a walking stick...")]
        STAFF_1,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 5)) }, 250, "Magic Staff", "Where is the compartment where the magic flowers come out?")]
        STAFF_2,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 8)) }, 1000, "Blessed Ankh", "Blessed by the Egyptian Gods")]
        STAFF_3,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, -10)) }, 2000, "Soul Wand", "Converts health to damage.")]
        STAFF_4,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 3500, "Regen Staff", "Holding this staff make you feel rejuvenated.")]
        STAFF_5,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 55)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 6000, "Conjurer's Staff", "Helps focus your spells.")]
        STAFF_6,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 68)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 7500, "Zodiac Sign Wand", "I asked god for a sign, he gave me Cancer.")]
        STAFF_7,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 75)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 18)) }, 9500, "Runic Staff", "A staff with a mysterious message written on it.")]
        STAFF_8,
        [ItemAttribute(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 90)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 20)) }, 9500, "Zeus' Gold Staff", "Zeus is said to of made the color with his lightning.")]
        STAFF_9,
        #endregion

    }
}
