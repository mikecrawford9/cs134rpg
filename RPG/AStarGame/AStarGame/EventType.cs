using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
    public enum EventType { MAP_TRANSITION=1, BATTLE, BATTLE_TILE, WAITFORNPC, NPCQUEST, MESSAGE, QUESTRETURN, PICKUPITEM, NPCMERCH, NPCHEAL, CANCELED,END_BATTLE_FLEE, END_BATTLE_VICTORY, END_BATTLE_LOSS};
}
