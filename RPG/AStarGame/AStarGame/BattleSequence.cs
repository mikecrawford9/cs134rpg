﻿using System;
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
     public class BattleSequence
    {
         public static int PROJECTILE_TIME = 2000;
        public Party party;
        public Enemy[] enemies;
        public List<String> combatLog;
        public SpriteFont combatLogFont;
        public List<Button> visibleButtons;
        public Queue<BattleAction> currentActions;
        public bool continueCombat;
        public bool partyDead;
        public bool enemiesDead;
        public bool isWaiting;
        public int shotprojectileat;
        public String xRet, yRet, retMap;

        public bool drawprojectile;
        public Projectile currentprojectile;
        public Queue<Projectile> projectiles;

        public Rectangle[] playerec;
        public Rectangle[] enemyrec;

        public BattleStageType state;

        public AttackButton aButton;
        public ItemButton itemButton;
        public FleeButton fleeButton;
        public SpellButton spellButton;

        public BattleSequence(Party party, Enemy[] enemies, SpriteFont displayTextFont, TileMap battleMap, int xRet, int yRet, String retMap)
        {
            this.playerec = new Rectangle[] { new Rectangle(366, 207, 32, 32) };
            this.enemyrec = new Rectangle[] { new Rectangle(111, 207, 32, 32) };
            this.party = party;
            this.enemies = enemies;
            this.combatLog = new List<String>();
            this.combatLogFont = displayTextFont;
            this.currentActions = new Queue<BattleAction>();
            this.continueCombat = true; 
            this.partyDead = false;
            this.enemiesDead= false;
            this.isWaiting = false;
            this.xRet = Convert.ToString(xRet);
            this.yRet = Convert.ToString(yRet);
            this.retMap = retMap;
            this.projectiles = new Queue<Projectile>();
            state = BattleStageType.ACTION;
            
        }

        public void Start()
        {
            combatLog.Add("Enemies have appeared.");
            switch (state)
            {
                    case BattleStageType.ACTION:
                        foreach (Player p in party.partyMembers)
                        {
                            aButton = new AttackButton(Game1.buttonImage, Game1.buttonFont, "Attack", p, this, null);
                            spellButton = new SpellButton(Game1.buttonImage, Game1.buttonFont, "Spell", p, this, null);
                            itemButton = new ItemButton(Game1.buttonImage, Game1.buttonFont, "Use Item", p, this, null);
                            fleeButton = new FleeButton(Game1.buttonImage, Game1.buttonFont, "Flee", p, this, null);
                        }
                        break;
                    /*case BattleStageType.FIGHT:
                        foreach (BattleAction b in this.currentActions)
                        {
                            b.performAction();
                            currentActions.Remove(b);
                        }
                        this.state = BattleStageType.ACTION;
                        break;
                    case BattleStageType.FLEE:
                        Event e = new Event();
                        e.setEventType(EventType.MAP_TRANSITION);
                        e.addProperty("x", xRet);
                        e.addProperty("y", yRet);
                        e.addProperty("mapfile", retMap);
                        Game1.addToEventQueue(e);
                        this.continueCombat = false;
                        break;
                    case BattleStageType.LOSE:
                        this.continueCombat = false;
                        break;
                    case BattleStageType.WIN:
                        this.continueCombat = false;
                        break;
                     */
            }
        }

        public bool AttemptToFlee()
        {
            int test = (new Random()).Next(0, 2);
            switch (test)
            {
                case 0: return false;
                case 1: return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (drawprojectile)
            {
                currentprojectile.Draw(spriteBatch);
            }
            for (int i = 0; i < playerec.Length; i++)
            {
                spriteBatch.Draw(Game1.playerLeftFace, playerec[i], Color.AliceBlue);
            }
            for (int i = 0; i < enemyrec.Length; i++)
            {
                spriteBatch.Draw(Game1.enemy1RightFace, enemyrec[i], Color.AliceBlue);
            }
            
            switch (combatLog.ToArray().Length)
            {
                default:
                case 3:
                    spriteBatch.DrawString(Game1.buttonFont, combatLog[combatLog.ToArray().Length - 3], new Vector2(20, 300), Color.White);
                    spriteBatch.DrawString(Game1.buttonFont, combatLog[combatLog.ToArray().Length - 2], new Vector2(20, 320), Color.White);
                    spriteBatch.DrawString(Game1.buttonFont, combatLog[combatLog.ToArray().Length - 1], new Vector2(20, 340), Color.White);
                    break;
                case 2:
                    spriteBatch.DrawString(Game1.buttonFont, combatLog[combatLog.ToArray().Length - 2], new Vector2(20, 320), Color.White);
                    spriteBatch.DrawString(Game1.buttonFont, combatLog[combatLog.ToArray().Length - 1], new Vector2(20, 340), Color.White);
                    break;
                case 1:
                    spriteBatch.DrawString(Game1.buttonFont, combatLog[combatLog.ToArray().Length - 1], new Vector2(20, 340), Color.White);
                    break;
                case 0:
                    break;

            }
            switch (state)
            {
                case BattleStageType.ACTION:
                    aButton.Location(600, 100);
                    aButton.Draw(spriteBatch);
                    spellButton.Location(600, 140);
                    spellButton.Draw(spriteBatch);
                    itemButton.Location(600, 180);
                    itemButton.Draw(spriteBatch);
                    fleeButton.Location(600, 220);
                    fleeButton.Draw(spriteBatch);
                    break;
                case BattleStageType.FIGHT:
                    aButton.Draw(spriteBatch);
                    break;
                case BattleStageType.FLEE:
                    fleeButton.Draw(spriteBatch);
                    break;
                case BattleStageType.LOSE:
                    break;
                case BattleStageType.WIN:
                    break;
            }   
        }

        public void Update(GameTime gameTime)
        {
            if (!drawprojectile && projectiles.Count > 0)
            {
                currentprojectile = projectiles.Dequeue();
                shotprojectileat = (int)gameTime.TotalGameTime.TotalMilliseconds;
                drawprojectile = true;
            }

            if (drawprojectile)
            {
                currentprojectile.Update();
                int curtime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                if (curtime - shotprojectileat > PROJECTILE_TIME)
                    drawprojectile = false;
            }

            if (!drawprojectile)
            {
                switch (state)
                {
                    case BattleStageType.ACTION:
                        aButton.Update();
                        itemButton.Update();
                        fleeButton.Update();
                        spellButton.Update();
                        break;
                    case BattleStageType.FIGHT:
                        while (this.currentActions.Count > 0)
                        {
                            BattleAction a = this.currentActions.Dequeue();
                            a.performAction(gameTime);
                        }
                        //this.state = BattleStageType.ACTION;
                        break;
                    case BattleStageType.FLEE:
                        Console.WriteLine("FLEE STAGE?");
                        Event e = new Event();
                        e.setEventType(EventType.MAP_TRANSITION);
                        e.addProperty("x", xRet);
                        e.addProperty("y", yRet);
                        e.addProperty("mapfile", retMap);
                        Game1.addToEventQueue(e);
                        this.continueCombat = false;
                        break;
                    case BattleStageType.LOSE:
                        break;
                    case BattleStageType.WIN:
                        break;
                }
            }
            
        }




    }
    public enum BattleActionType {ATTACK, SPELL, ITEM, FLEE}

    public class BattleAction
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

        public void performAction(GameTime gameTime)
        {
            switch (type)
            {
                case BattleActionType.ATTACK:
                    foreach(Player t in target)
                    {
                        bool isenemy = (t.playerBase.playerType == PlayerType.ENEMY);
                        Console.WriteLine("Melee Attack");
                        Texture2D cur = user.UseSpell(t, spell);
                        Projectile proj = new Projectile();
                        if(isenemy)
                            proj.Initialize(cur, new Vector2(battleSequence.enemyrec[0].X, battleSequence.enemyrec[0].Y), false);
                        else
                            proj.Initialize(cur, new Vector2(battleSequence.playerec[0].X, battleSequence.playerec[0].Y), true);
                        battleSequence.projectiles.Enqueue(proj);
                    }
                    break;
                case BattleActionType.ITEM:
                    foreach (Player t in target)
                    {
                        Console.WriteLine("Item");
                        t.UseHealingItem(item);
                    }
                    break;
                case BattleActionType.SPELL:
                    foreach (Player t in target)
                    {
                        bool isenemy = (t.playerBase.playerType == PlayerType.ENEMY);
                        Texture2D cur = user.UseSpell(t, spell);
                        Projectile proj = new Projectile();
                        if (isenemy)
                            proj.Initialize(cur, new Vector2(battleSequence.enemyrec[0].X, battleSequence.enemyrec[0].Y), false);
                        else
                            proj.Initialize(cur, new Vector2(battleSequence.playerec[0].X, battleSequence.playerec[0].Y), true);
                        battleSequence.projectiles.Enqueue(proj);
                        //battleSequence.drawprojectile = true;
                    }
                    break;
                case BattleActionType.FLEE:
                    Console.Write("Fleeeing...");
                    if (battleSequence.AttemptToFlee())
                    {
                        Console.WriteLine("Success!");
                        //continueCombat = false;
                        battleSequence.state = BattleStageType.FLEE;
                    }
                    else
                    {
                        Console.WriteLine("Failure!");
                        battleSequence.state = BattleStageType.ACTION;
                    }
                    break;
                default: 
                    break;
            }
            if (battleSequence.state != BattleStageType.FLEE)
                battleSequence.state = BattleStageType.ACTION;
        }
    }
}
