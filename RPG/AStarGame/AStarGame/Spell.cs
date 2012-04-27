using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public enum SpellType {MAGIC, SUPPORT, PHYSICAL}
    public enum SpellTargetType {SINGLE, ALL, SELF}
    public enum SpellElement{PHYSICAL, HOLY, FIRE, ICE, LIGHTNING}
    public enum SpellEffectType {AGL, ATK, DEF, MAG_ATK, HP, MP, REVIVE}

    public class Spell
    {
        public Spell (SpellType type, SpellElement element, SpellEffect[] effects, String name, String description)
        {
            this.type = type;
            this.element = element;
            this.effects = effects;
        }
        public SpellType type;
        public SpellElement element;
        public SpellEffect[] effects;

        #region DEFAULT
        public static Spell ATTACK = new Spell(SpellType.PHYSICAL, SpellElement.PHYSICAL, new SpellEffect[] { new SpellEffect(SpellEffectType.HP, SpellTargetType.SINGLE, -1) }, "Attack", "A melee attack");
        public static Spell FIRE = new Spell(SpellType.MAGIC, SpellElement.FIRE, new SpellEffect[] { new SpellEffect(SpellEffectType.HP, SpellTargetType.SINGLE, -10), new SpellEffect(SpellEffectType.MP, SpellTargetType.SELF, -10) }, "Fire", "Blast a fireball at an enemy.");
        public static Spell HEAL = new Spell(SpellType.SUPPORT, SpellElement.HOLY, new SpellEffect[] { new SpellEffect(SpellEffectType.HP, SpellTargetType.SINGLE, 10), new SpellEffect(SpellEffectType.MP, SpellTargetType.SELF, -10) }, "Heal", "Heals a single ally."); 
        #endregion
    }
    public class SpellEffect
    {
        public SpellEffect(SpellEffectType type, SpellTargetType target, int value)
        {
            this.type = type;
            this.target = target;
            this.value = value;
        }
        public SpellEffectType type;
        public SpellTargetType target;
        public int value;

    }

    


}
