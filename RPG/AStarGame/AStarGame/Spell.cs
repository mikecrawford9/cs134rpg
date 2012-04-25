using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarGame
{
    public enum SpellType {MAGIC, PHYSICAL}
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
        //public static Spell ATTACK = new Spell(SpellType.PHYSICAL, SpellElement.PHYSICAL, new SpellEffect[] {new SpellEffect(SpellEffectType.HP, SpellTargetType.SINGLE, 0)}, "Attack", 
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
