/*
Name: Michael Crawford
Class: CS134
Instructor: Dr. Teoh
Term: Spring 2012
Assignment: Project 2
*/
#region IMPORTS
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
using System.Collections;
#endregion

namespace RPG
{
    public enum ItemType {NULL, ARMOR, ROBE, CLOTHING, SWORD, MACE, STAFF, RECOVERY_POTION, STAT_POTION, QUEST};
    public enum ItemEffectType {AGL, ATK, DEF, MAG_ATK, HP, MP, REVIVE}
    public enum ItemTargetType {NULL, SELF, SINGLE, ALL }
    public enum ItemUsageType {NULL, TEMP, PERM, REGEN }
    public enum Status {NONE, DEAD}

    public class ItemEffect
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

    public class Item
    {
        public Item(ItemType type, ItemEffect[] effects,  int cost, String name, String description)
        {
            this.type = type;
            this.effects = effects;
            this.cost = cost;
            this.name = name;
            this.description = description;
        }
        public Item()
        {
            this.type = ItemType.NULL;
            this.effects = null;
            this.cost = -1;
            this.name = "";
            this.description = "";
        }
        public ItemType type;
        public ItemEffect[] effects;
        public int cost;
        public String description;
        public String name;

        public bool Equals(Item b)
        {
            if (b == null)
            {
                return false;
            }
            if (this.name == b.name)
            {
                return true;
            }
            return false;
        }

        #region DEFAULT
        public static Item BLANK = new Item();
        #endregion

        #region QUEST
        public static Item DUNGEON_KEY = new Item (ItemType.QUEST, null,  0, "Dungeon Key", "Key used to open Dragon's Cave.");
        public static Item DRAGON_SKULL = new Item(ItemType.QUEST, null,  0, "Dragon's Skull", "Skull of the Dragon King.");
        #endregion

        #region RECOVERY POTION
        public static Item HP_POTION_100 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] {new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 100)}, 20, "Low Health Potion", "Heals 100 HP." );    
        public static Item HP_POTION_250 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] { new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 250) }, 110, "Medium Health Potion", "Heals 250 HP.");
        public static Item HP_POTION_500 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 500)) }, 250, "High Health Potion", "Heals 500 HP");
        public static Item MP_POTION_100 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.MP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 100)) }, 20, "Low Mana Potion", "Heals 100 MP");
        public static Item MP_POTION_250 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.MP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 250)) }, 110, "Medium Mana Potion", "Heals 250 MP");
        public static Item MP_POTION_500 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] { (new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 500)) }, 20, "High Mana Potion", "Heals 500 MP");
        public static Item REVIVE_100 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] {new ItemEffect(ItemEffectType.REVIVE, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, (int) Status.NONE), new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 100)}, 1000, "Low Revive", "Revives one player to 100 HP.");
        public static Item REVIVE_250 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] {new ItemEffect(ItemEffectType.REVIVE, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, (int) Status.NONE), new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 250)}, 2500, "Low Revive", "Revives one player to 250 HP.");
        public static Item REVIVE_500 = new Item(ItemType.RECOVERY_POTION, new ItemEffect[] {new ItemEffect(ItemEffectType.REVIVE, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, (int) Status.NONE), new ItemEffect(ItemEffectType.HP, ItemTargetType.SINGLE, ItemUsageType.TEMP, false, 500)}, 5000, "Low Revive", "Revives one player to 500 HP.");
        #endregion

        #region STAT POTION
        #endregion

        #region CLOTHING
        public static Item CLOTHING_1 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 3)) }, 20, "Cotton Shirt", "Better than being naked...I guess...");
        public static Item CLOTHING_2 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 7)) }, 50, "Travel Vest", "Light, fashionable armor.");
        public static Item CLOTHING_3 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 16)) }, 400, "Fur Coat", "I hope its Faux.");
        public static Item CLOTHING_4 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 18)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.PERM, true, 8)) }, 850, "Mage's Clothes", "We stole them from magi.");
        public static Item CLOTHING_5 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 21)) }, 1100, "Full Metal Vest", "More metal than human.");
        public static Item CLOTHING_6 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 22)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 1700, "Elven Mail", "Seems more metrosexual than asexual to me.");
        public static Item CLOTHING_7 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 28)) }, 2800, "Festival Coat", "So many tassles...");
        public static Item CLOTHING_8 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 25)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 10)) }, 2800, "Kimono", "The ultimate in Japanese only-wear.");
        public static Item CLOTHING_9 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 37)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)) }, 4000, "Wild Coat", "Bring out your inner beast.");
        public static Item CLOTHING_10 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42))}, 9800, "Storm Gear", "Something for that perfect storm.");
        public static Item CLOTHING_11 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 43)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 5)) }, 10400, "Erinyes Tunic", "Might make you a bit vengeful.");
        public static Item CLOTHING_12 = new Item(ItemType.CLOTHING, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 47)) }, 14900, "Mythril Clothes", "Made from the best stuff on Earth.");
        #endregion 

        #region ROBE
        public static Item ROBE_1 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 4)) }, 25, "Cotton Robe", "You don't even need pants with them.");
        public static Item ROBE_2 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 10)) }, 200, "Travel Robe", "Just enough protection to leave your house.");
        public static Item ROBE_3 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 1400, "Silk Robe", "The best in comfort.");
        public static Item ROBE_4 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 26)) }, 2400, "Jerkin", "Let's just say you will have a hard time putting them on.");
        public static Item ROBE_5 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 29)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 4000, "Formal Wear", "So fancy.");
        public static Item ROBE_6 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 39)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 7000, "Blessed Robe", "I heard its possessed by holy ghosts.");
        public static Item ROBE_7 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42)) }, 8900, "Dragon Robe", "I can't believe a dragon could fit this.");
        public static Item ROBE_8 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 39)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 9000, "Magical Cassock", "Only the holiest of men can wear something so good.");
        
        public static Item ROBE_9 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 44)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 10)) }, 9900, "Ardagh Robe", "Sown by the most flamboyant dragon ever.");
        public static Item ROBE_10 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 46)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 11400, "Aeolian Robe", "Fabric so light, it seems like air.");
        public static Item ROBE_11 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 43)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 13500, "Oracle's Robe", "You'll feel insightful in this robe.");
        public static Item ROBE_12 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)), (new ItemEffect(ItemEffectType.AGL, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)) }, 16000, "Feathered Robe", "Become as agile as a bird.");
        public static Item ROBE_13 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 47)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 20)) }, 20000, "Iris Robe", "Almost as beautiful as Iris herself.");
        public static Item ROBE_14 = new Item(ItemType.ROBE, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 48)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 30)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 30)) }, 36000, "Mysterious Robe", "No one knows where is came from.");
        #endregion

        #region ARMOR
        public static Item ARMOR_1 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)) }, 240, "Leather Armor", "Smells worse than you think.");
        public static Item ARMOR_2 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 21)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 1000, "Magic Armor", "Something magical about that old armor.");
        public static Item ARMOR_3 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 25)) }, 2000, "Chain Mail", "Is this really iron, it seems more like paper clips.");
        public static Item ARMOR_4 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)) }, 3600, "Armored Shell", "You can hide in it like a turtle.");
        public static Item ARMOR_5 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 33)) }, 4400, "Plate Mail", "You can't eat off this kind of plate.");
        public static Item ARMOR_6 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 36)) }, 4900, "Steel Armor", "Just try to move in this armor.");
        public static Item ARMOR_7 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 43)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 9000, "Erebus Armor", "For something so dark, it pretty light.");
        public static Item ARMOR_8 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)) }, 10000, "Dragon Armor", "No dragons were harmed in the making of this armor.");
        public static Item ARMOR_9 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 47)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 13000, "Chronos Armor", "Only time can tell if this armor is any good.");
        public static Item ARMOR_10 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 15000, "Spiked Armor", "Good thing the spikes are on the outside.");
        public static Item ARMOR_11 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 42)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 17000, "Asura's Armor", "Find balance within yourself.");
        public static Item ARMOR_12 = new Item(ItemType.ARMOR, new ItemEffect[] { (new ItemEffect(ItemEffectType.DEF, ItemTargetType.SELF, ItemUsageType.PERM, true, 52)), (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)) }, 22000, "Lion's Armor", "Become the king of beasts.");
        #endregion

        #region SWORDS
        public static Item SWORD_1 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 6))}, 30, "Machete", "Good for weeding.");
        public static Item SWORD_2 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 8)) }, 120, "Short Sword", "Commonly mistaken for a large knife.");
        public static Item SWORD_3 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 14)) }, 500, "Long Sword", "May be misinterpreted as compensation for something.");
        public static Item SWORD_4 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)) }, 1600, "Hunter's Sword", "Used for more than hunting game.");
        public static Item SWORD_5 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)) }, 3500, "Broad Sword", "Can cut down small trees in one swoop.");
        public static Item SWORD_6 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 55)) }, 6000, "Elven Rapier", "Only elves can produce something of this much detail.");
        public static Item SWORD_7 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 62)) }, 7500, "Battle Rapier", "Always be battle ready.");
        public static Item SWORD_8 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 70)) }, 9000, "Claymore", "The sword only a giant can love.");
        public static Item SWORD_9 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 90)) }, 12000, "Great Sword", "Whats so great about it, its oversided and weights a ton.");
        public static Item SWORD_10 = new Item(ItemType.SWORD, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 100)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 15000, "Soul Blade", "It feels like my soul is attached to this sword.");
        #endregion

        #region MACE
        public static Item MACE_1 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 4)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 2)) }, 80, "Mace", "A ball of metal on a stick, what else do you need.");
        public static Item MACE_2 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 9)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 5)) }, 500, "Spiked Mace", "Great for tendering meat, as well as your enemies.");
        public static Item MACE_3 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 18)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 8)) }, 2000, "Heavy Mace", "Not for the weak of arm.");
        public static Item MACE_4 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 28)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 15)) }, 4000, "Battle Mace", "Just make sure to only hit your enemies.");
        public static Item MACE_5 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 22)) }, 6500, "War Mace", "Only war would force someone to make something so dangerous.");
        public static Item MACE_6 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 27)) }, 8200, "Bladed Mace", "All the fun of beating your enemy senseless with the presision of a sword.");
        public static Item MACE_7 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 59)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 35)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 11000, "Blessed Mace", "When god support using a weapon, you can't say no.");
        public static Item MACE_8 = new Item(ItemType.MACE, new ItemEffect[] { (new ItemEffect(ItemEffectType.ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 70)), (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 45)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 20)) }, 14000, "Righteous Mace", "Using this mace just feels so right.");
        #endregion

        #region STAFF
        public static Item STAFF_1 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 6)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 3)) }, 50, "Wooden Staff", "More useful as a walking stick...");
        public static Item STAFF_2 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 12)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 5)) }, 250, "Magic Staff", "Where is the compartment where the magic flowers come out?");
        public static Item STAFF_3 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 20)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 8)) }, 1000, "Blessed Ankh", "Blessed by the Egyptian Gods");
        public static Item STAFF_4 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 40)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, -10)) }, 2000, "Soul Wand", "Converts health to damage.");
        public static Item STAFF_5 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 30)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)), (new ItemEffect(ItemEffectType.HP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 10)) }, 3500, "Regen Staff", "Holding this staff make you feel rejuvenated.");
        public static Item STAFF_6 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 55)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 6000, "Conjurer's Staff", "Helps focus your spells.");
        public static Item STAFF_7 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 68)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 15)) }, 7500, "Zodiac Sign Wand", "I asked god for a sign, he gave me Cancer.");
        public static Item STAFF_8 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 75)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 18)) }, 9500, "Runic Staff", "A staff with a mysterious message written on it.");
        public static Item STAFF_9 = new Item(ItemType.STAFF, new ItemEffect[] { (new ItemEffect(ItemEffectType.MAG_ATK, ItemTargetType.SELF, ItemUsageType.PERM, true, 90)), (new ItemEffect(ItemEffectType.MP, ItemTargetType.SELF, ItemUsageType.REGEN, true, 20)) }, 9500, "Zeus' Gold Staff", "Zeus is said to of made the color with his lightning.");
        #endregion

        #region  ACCESS STORAGE BY ITEM TYPE
        public static Item[] QUEST_ITEMS = {DUNGEON_KEY, DRAGON_SKULL};
        public static Item[] HP_RECOVERY_POTIONS = {HP_POTION_100, HP_POTION_250, HP_POTION_500};
        public static Item[] MP_RECOVERY_POTIONS = {MP_POTION_100, MP_POTION_250, MP_POTION_500};
        public static Item[] REVIVE_POTIONS = {REVIVE_100,REVIVE_250, REVIVE_500};
        public static Item[] CLOTHING = {CLOTHING_1, CLOTHING_2, CLOTHING_3, CLOTHING_4, CLOTHING_5, CLOTHING_6, CLOTHING_7, CLOTHING_8, CLOTHING_9, CLOTHING_10, CLOTHING_11, CLOTHING_12};
        public static Item[] ROBES = {ROBE_1, ROBE_2, ROBE_3, ROBE_4, ROBE_5, ROBE_6, ROBE_7, ROBE_8, ROBE_9, ROBE_10, ROBE_11, ROBE_12, ROBE_13, ROBE_14};
        public static Item[] ARMOR = {ARMOR_1, ARMOR_2, ARMOR_3, ARMOR_4, ARMOR_5, ARMOR_6, ARMOR_7, ARMOR_8, ARMOR_9, ARMOR_10, ARMOR_11, ARMOR_12};
        public static Item[] SWORDS = {SWORD_1, SWORD_2, SWORD_3, SWORD_4, SWORD_5, SWORD_6, SWORD_7, SWORD_8, SWORD_9, SWORD_10};
        public static Item[] MACES = {MACE_1, MACE_2, MACE_3, MACE_4, MACE_5, MACE_6, MACE_7, MACE_8};
        public static Item[] STAFF = {STAFF_1, STAFF_2, STAFF_3, STAFF_4, STAFF_4, STAFF_5, STAFF_6, STAFF_7, STAFF_8, STAFF_9};
        #endregion

        #region ACCESS STORAGE BY SHOP
        public static Item[][] SHOP_ITEMS = {HP_RECOVERY_POTIONS, MP_RECOVERY_POTIONS, REVIVE_POTIONS, CLOTHING, ROBES, ARMOR, SWORDS, MACES, STAFF};
        #endregion
        
        #region ACCESSORS
        public static Item[] GetShopInventory(int minPrice, int maxPrice)
        {
            ArrayList output = new ArrayList();
            foreach (Item[] i in SHOP_ITEMS)
            {
                foreach (Item j in i)
                {
                    if(j.cost >= minPrice && j.cost <= maxPrice)
                    {
                        output.Add(j);
                    }
                }
            }
            return output.ToArray(typeof(Item)) as Item[];
        }

        public static Item[] GetItemListByType(ItemType type)
        {
            ArrayList output = new ArrayList();
            foreach (Item[] i in SHOP_ITEMS)
            {
                foreach (Item j in i)
                {
                    if(j.type == type)
                    {
                        output.Add(j);
                    }
                }
            }
            return output.ToArray(typeof(Item)) as Item[];
        }
        #endregion
    }

    public class Inventory
    {   
        public readonly int INVENTORY_MAX_SIZE = 20;
        public Item[] inventory;
        public Inventory(Item[] inventory, int maxSize = 20)
        {
            this.INVENTORY_MAX_SIZE = maxSize;
            inventory = new Item[this.INVENTORY_MAX_SIZE];
            for (int i = 0; i < inventory.Length || i < INVENTORY_MAX_SIZE; i++)
            {
                this.inventory[i] = inventory[i];
            }
            for (int i = inventory.Length; i < INVENTORY_MAX_SIZE; i++)
            {
                this.inventory[i] = Item.BLANK;
            }
        }

        public Item GetItem(int index)
        {
            if(index >= INVENTORY_MAX_SIZE || index < 0)
            {
                return null;
            }
            return inventory[index];
        }

        public bool AddItem(Item item)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                if(inventory[i] == Item.BLANK)
                {
                   inventory[i] = item;
                    return true;
                }
            }
            
            return false;
        }

        public bool RemoveItem(Item item)
        {
            if (Search(item))
            {
                for (int i = 0;  i < INVENTORY_MAX_SIZE; i++)
                {
                    if (inventory[i].Equals(item))
                    {
                        inventory[i] = Item.BLANK;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Search(Item item)
        {

            foreach (Item i in this.inventory)
            {
                if(i.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
