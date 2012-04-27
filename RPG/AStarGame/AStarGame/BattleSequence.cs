using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG
{
    class BattleSequence
    {
        Player[] players;
        Enemy[] enemies;
        List<String> combatLog;
        SpriteFont combatLogFont;
        List<Button> visibleButtons;
        List<BattleAction> currentActions;
        public BattleSequence(Player[] players, Enemy[] enemies, SpriteFont displayTextFont)
        {
            this.players = players;
            this.enemies = enemies;
            this.combatLog = new List<String>();
            this.combatLogFont = displayTextFont;
            this.currentActions = new List<BattleAction>();
            
        }

        public void Start()
        {
            combatLog.Add("Enemies have appeared.");
            bool continueCombat = true; 
            bool partyDead = false;
            bool enemiesDead= false;
            while (continueCombat)
            {
                foreach (Player p in players)
                {

                }
            }
        }

        public void Draw()
        {

        }


    }
    public enum BattleActionType {ATTACK, SPELL, ITEM, FLEE}

    class BattleAction
    {
        public Player user;
        public Player[] target;
        public BattleActionType type;
        public Spell spell; //null if not a spell
        public Item item; //null if not an item 

        
        public BattleAction(Player user, Player[] target, BattleActionType type, Spell spell, Item item)
        {
            this.user = user;
            this.target = target;
            this.type = type;
            this.spell = spell;
            this.item = item;
        }

        public void performAction()
        {
            switch (type)
            {
                case BattleActionType.ATTACK:
                    foreach(Player t in target)
                    {
                        user.UseSpell(t, spell);
                    }
                    break;
                case BattleActionType.ITEM:
                    break;
                case BattleActionType.SPELL:
                    break;
                case BattleActionType.FLEE:
                    break;
                default: 
                    break;
            }
        }
    }
}
