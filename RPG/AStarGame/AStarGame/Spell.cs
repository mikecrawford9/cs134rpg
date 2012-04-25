using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public enum SpellType {MAGIC, PHYSICAL}
    public enum SpellTargetType {SINGLE, ALL, SELF}
    public enum SpellElement{PHYSICAL, HOLY, FIRE, ICE, LIGHTNING}
    public enum SpellEffect {AGL, ATK, DEF, MAG_ATK, HP, MP, REVIVE}

    public class Spell (SpellType type, SpellTargetType target, SpellElement element, SpellEffect[] effects)
    {
        
    }
    public 
}
