using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RPG
{
    public enum BattleStageType { ACTION, FIGHT, WIN, LOSE, FLEE }
    class BattleSequence
    {
        Party party;
        Enemy[] enemies;
        List<String> combatLog;
        SpriteFont combatLogFont;
        List<Button> visibleButtons;
        List<BattleAction> currentActions;
        bool continueCombat; 
        bool partyDead;
        bool enemiesDead;
        bool isWaiting;
        String xRet, yRet, retMap;
        BattleStageType state;

        AttackButton aButton;
        AttackButton itemButton;
        AttackButton fleeButton;

        public BattleSequence(Party party, Enemy[] enemies, SpriteFont displayTextFont, TileMap battleMap, int xRet, int yRet, String retMap)
        {
            this.party = party;
            this.enemies = enemies;
            this.combatLog = new List<String>();
            this.combatLogFont = displayTextFont;
            this.currentActions = new List<BattleAction>();
            this.continueCombat = true; 
            this.partyDead = false;
            this.enemiesDead= false;
            this.isWaiting = false;
            this.xRet = Convert.ToString(xRet);
            this.yRet = Convert.ToString(yRet);
            this.retMap = retMap;
            state = BattleStageType.ACTION;
            
        }

        public void Start()
        {
            combatLog.Add("Enemies have appeared.");
            while (continueCombat)
            {
                switch (state)
                {
                    case BattleStageType.ACTION:

                        foreach (Player p in party.partyMembers)
                        {
                            Event e = new Event();

                            e.setEventType(EventType.MAP_TRANSITION);
                            e.addProperty("x", xRet);
                            e.addProperty("y", yRet);
                            e.addProperty("mapfile", retMap);
                            List<Event> actionList = new List<Event>();
                            actionList.Add(e);
                            aButton = new AttackButton(Game1.buttonImage, Game1.buttonFont, "Attack", p, actionList);
                            itemButton = new AttackButton(Game1.buttonImage, Game1.buttonFont, "Use Item", p, null);
                            fleeButton = new AttackButton(Game1.buttonImage, Game1.buttonFont, "Flee", p, null);

                        }
                        break;
                    case BattleStageType.FIGHT:
                        break;
                    case BattleStageType.FLEE:
                        break;
                    case BattleStageType.LOSE:
                        break;
                    case BattleStageType.WIN:
                        break;
                        
                }
                this.continueCombat = false;
            }
        }

        public void AttemptToFlee()
        {
            switch ((new Random()).Next(0, 1))
            {
                case 0: break;
                case 1: this.continueCombat = false; break;
                default: break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.enemy1RightFace, new Rectangle(111, 207, 32, 32), Color.AliceBlue);
            spriteBatch.Draw(Game1.playerLeftFace, new Rectangle(366, 207, 32, 32), Color.AliceBlue);
            switch (state)
            {
                case BattleStageType.ACTION:
                    aButton.Location(600, 100);
                    aButton.Draw(spriteBatch);
                    itemButton.Location(600, 140);
                    itemButton.Draw(spriteBatch);
                    fleeButton.Location(600, 180);
                    fleeButton.Draw(spriteBatch);
                    break;
                case BattleStageType.FIGHT:
                    break;
                case BattleStageType.FLEE:
                    break;
                case BattleStageType.LOSE:
                    break;
                case BattleStageType.WIN:
                    break;
            }   
        }

        public void Update()
        {
            switch (state)
            {
                case BattleStageType.ACTION:
                    aButton.Update();
                    itemButton.Update();
                    fleeButton.Update();
                    break;
                case BattleStageType.FIGHT:
                    break;
                case BattleStageType.FLEE:
                    break;
                case BattleStageType.LOSE:
                    break;
                case BattleStageType.WIN:
                    break;
            }  
            
        }




    }
    public enum BattleActionType {ATTACK, SPELL, ITEM, FLEE}

    class BattleAction
    {
        public Player user;
        public Player[] target;
        public BattleActionType type;
        public BattleSequence battleSequence;
        public Spell spell; //null if not a spell
        public Item item; //null if not an item 

        
        public BattleAction(BattleSequence bs, Player user, Player[] target, BattleActionType type, Spell spell, Item item)
        {
            this.user = user;
            this.target = target;
            this.type = type;
            this.spell = spell;
            this.item = item;
            this.battleSequence = bs;
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
                    foreach (Player t in target)
                    {
                        t.UseHealingItem(item);
                    }
                    break;
                case BattleActionType.SPELL:
                    foreach (Player t in target)
                    {
                        user.UseSpell(t, spell);
                    }
                    break;
                case BattleActionType.FLEE:
                    
                    break;
                default: 
                    break;
            }
        }
    }
}
