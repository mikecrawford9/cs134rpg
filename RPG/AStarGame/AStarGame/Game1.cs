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
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AStarGame
{



    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        const int MONSTER_MOVE_DELAY = 1000;
        const int PLAYER_MOVE_DELAY = 100;

        const int NUM_X_TILES = 512;
        const int NUM_Y_TILES = 512;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        TileMap map;
        ToolMap toolmap;
        Texture2D whitepixel;
        Texture2D astarwaypoint;

        Tool[][] tools;

        Tile astartile;
        bool edited;
        const bool DEBUG = false;

        int lastmonstermove;
        int lastplayermove;

        GameState state;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Make the mouse appear  
            IsMouseVisible = true;

            // Set back buffer resolution  
            graphics.PreferredBackBufferWidth = 768;
            graphics.PreferredBackBufferHeight = 532;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            state = GameState.EDIT;
            lastmonstermove = 0;
            lastplayermove = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            whitepixel  = new Texture2D(GraphicsDevice, 1, 1);
            whitepixel.SetData(new Color[] { Color.White });

            astarwaypoint = Content.Load<Texture2D>("AStarWayPoint");
            edited = true;
            
            font = Content.Load<SpriteFont>("gameFont");

            tools = new Tool[2][];
            tools[0] = new Tool[4];
            tools[1] = new Tool[4];

            tools[0][0] = new Tool(TileType.GRASS, Content.Load<Texture2D>("Tiles/Grass"), false, 1);
            tools[0][1] = new Tool(TileType.TREES, Content.Load<Texture2D>("Tiles/Trees"), false, 2);
            tools[0][2] = new Tool(TileType.SWAMP, Content.Load<Texture2D>("Tiles/Swamp"), false, 4);
            tools[0][3] = new Tool(TileType.PLAYER, Content.Load<Texture2D>("Tiles/Player"), false, 0);
            tools[1][0] = new Tool(TileType.WATER, Content.Load<Texture2D>("Tiles/Water"), false, 6);
            tools[1][1] = new Tool(TileType.ROCKS, Content.Load<Texture2D>("Tiles/LavaRocks"), false, 8);
            tools[1][2] = new Tool(TileType.WALL, Content.Load<Texture2D>("Tiles/Wall"), true, 0);
            tools[1][3] = new Tool(TileType.MONSTER, Content.Load<Texture2D>("Tiles/Monster"), false, 0);
            
            //Testing Enumerator access
            /* 
            foreach (WorldTile t in Enum.GetValues(typeof(WorldTile)))
            {
                MessageBox(new IntPtr(0), t.GetInformation() , t.ToString(), 0);
            }
             */

            //map = new TileMap(10, 10, 17, 512, 512, whitepixel, tools[0][0]);
            map = new TileMap(10, 10, 17, NUM_X_TILES, NUM_Y_TILES, whitepixel, tools);
            toolmap = new ToolMap(578, 100, whitepixel, tools, font);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            state = toolmap.Update(state);
            switch(state)
            {
                case GameState.EDIT:
                    edited = true;
                    catchInput(gameTime, false);
                    map.resetPlayers();
                    map.refreshTiles();
                    map.Update(toolmap.getSelected());
                    astartile = null;
                    break;
                /*case GameState.ASTAR:
                    //start a*star code here...
                    map.unhighlight();
                    if (edited)
                    {
                        Tile best = processAStar();
                        if(best != null)
                            astartile = createAStarHighlight(best);

                        edited = false;
                    }
                    map.refreshTiles();
                    break;
                */
                case GameState.RUNNING:
                    //start game here...
                    map.unhighlight();
                    playGame(gameTime);
                    break;


                default:
                    break;
            }

            base.Update(gameTime);
        }

        /*private Tile createAStarHighlight(Tile best)
        {
            Tile start = addToAStarHighlight(best, null);
            Tile cur = best;
            Tile curhl = start;
            while (cur != null)
            {
                Tile next = cur.getPrevious();
                if (next != null)
                {
                    curhl = addToAStarHighlight(next, curhl);
                }
                cur = next;
            }
            return curhl;
        }*/

        /*private Tile addToAStarHighlight(Tile toadd, Tile prev)
        {
            int x = toadd.getX();
            int y = toadd.getY();
            int mapx = toadd.getMapX();
            int mapy = toadd.getMapY();
            int len = toadd.getLength();

            if(DEBUG)
            Console.WriteLine("Adding tile at (" + mapx + "," + mapy + ") to bestpath! Cost is " + toadd.getTotalCost());

            Tile newadd = new Tile(mapx, mapy, x, y, len, astarwaypoint, Color.White);
            newadd.setPrevious(prev);
            return newadd;
        }*/

        public void catchInput(GameTime gameTime, bool noclip)
        {
            int currenttime = (int)gameTime.TotalGameTime.TotalMilliseconds;
            if (currenttime - lastplayermove > PLAYER_MOVE_DELAY)
            {
                KeyboardState kb = Keyboard.GetState();
                if (kb.IsKeyDown(Keys.Up))
                {
                    map.shiftUp(1, noclip);
                }
                else if (kb.IsKeyDown(Keys.Down))
                {
                    map.shiftDown(1, noclip);
                }
                else if (kb.IsKeyDown(Keys.Left))
                {
                    map.shiftLeft(1, noclip);
                }
                else if (kb.IsKeyDown(Keys.Right))
                {
                    map.shiftRight(1, noclip);
                }

                lastplayermove = currenttime;
            }
        }

        private Tile processAStar()
        {
            Tile ret = null;
            Tile player = map.getPlayerTile();
            Tile monster = map.getMonsterTile();

            if (player != null && monster != null)
            {
                if(state == GameState.ASTAR)
                    map.resetPlayers();

                Tile start = map.getTileAt(monster.getMapX(), monster.getMapY());
                Tile goal = map.getTileAt(player.getMapX(), player.getMapY());

                ret = iterateAStar(start, goal);
            }
            return ret;
        }

        private void playGame(GameTime gameTime)
        {
            Tile playertile = map.getPlayerTile();
            Tile monstertile = map.getMonsterTile();

            int currenttime = (int)gameTime.TotalGameTime.TotalMilliseconds;
            if ((currenttime - lastmonstermove) > MONSTER_MOVE_DELAY)
            {
                    Tile best = processAStar();
                    if (best != null)
                    {
                        astartile = best;
                        Tile next = getNextStep(best);
                        map.setMonsterLocation(next);
                        map.refreshTiles();
                        edited = true;
                    }
                    else
                        Console.WriteLine("Best is null!");

                lastmonstermove = currenttime;
            }

            if((currenttime - lastplayermove) > PLAYER_MOVE_DELAY)
            {
                Tile player = map.getPlayerTile();
                KeyboardState kb = Keyboard.GetState();
                if (kb.IsKeyDown(Keys.Up))
                {
                    Tile up = map.getTileAt(player.getMapX(), player.getMapY() - 1);
                    if (up != null && up.getType() != TileType.WALL)
                    {
                        map.setPlayerLocation(up);
                        lastplayermove = currenttime;
                    }
                }
                else if (kb.IsKeyDown(Keys.Down))
                {
                    Tile down = map.getTileAt(player.getMapX(), player.getMapY() + 1);
                    if (down != null && down.getType() != TileType.WALL)
                    {
                        map.setPlayerLocation(down);
                        lastplayermove = currenttime;
                    }
                }
                else if (kb.IsKeyDown(Keys.Left))
                {
                    Tile left = map.getTileAt(player.getMapX() - 1, player.getMapY());
                    if (left != null && left.getType() != TileType.WALL)
                    {
                        map.setPlayerLocation(left);
                        lastplayermove = currenttime;
                    }
                }
                else if (kb.IsKeyDown(Keys.Right))
                {
                    Tile right = map.getTileAt(player.getMapX() + 1, player.getMapY());
                    if (right != null && right.getType() != TileType.WALL)
                    {
                        map.setPlayerLocation(right);
                        lastplayermove = currenttime;
                    }
                }
            }
            if (playertile != null && monstertile != null)
            {
                if (playertile.getMapX() == monstertile.getMapX() && playertile.getMapY() == monstertile.getMapY())
                {
                    state = GameState.GAMEOVER;
                    map.resetPlayers();
                    Console.WriteLine("Game state set to " + state);
                }
            }
        }

        private Tile getNextStep(Tile best)
        {
            Tile cur = best;
            while (cur.getPrevious() != null)
            {
                if (cur.getPrevious().getPrevious() == null)
                {
                    return cur;
                }
                cur = cur.getPrevious();
            }
            return cur;
        }

        private Tile iterateAStar(Tile start, Tile goal)
        {
            if(DEBUG)
                Console.WriteLine("Called iterateAStar!, goal is at (" + goal.getMapX() + "," + goal.getMapY() + ")");

            Tile ret = null;
            List<Tile> bestpath = new List<Tile>();
            List<Tile> closed = new List<Tile>();
            start.addToTotalCost(Math.Abs(start.getMapX() - goal.getMapX()) + Math.Abs(start.getMapY() - goal.getMapY()));
            List<Tile> open = new List<Tile>();
            open.Add(start);
            while (open.Count > 0)
            {
                //get the lowest cost node...
                Tile cur = open.Min();
                if(DEBUG)
                    Console.WriteLine("Min retrieved a node with " + cur.getTotalCost() + " from the open list");

                if (cur.getMapX() == goal.getMapX() && cur.getMapY() == goal.getMapY())
                {
                    if(DEBUG)
                        Console.WriteLine("Found goal! cur is (" + cur.getMapX() + "," + cur.getMapY() + ")");

                    return cur;
                    //return addToBestPath(bestpath, cur, last);
                }
                else
                {
                    //Tile newlast = addToBestPath(bestpath, cur, last);
                    int curx = cur.getMapX();
                    int cury = cur.getMapY();
                    //last = newlast;
                    open.Remove(cur);
                    closed.Add(cur);
                    Tile up = map.getTileAt(curx, cury - 1);
                    Tile down = map.getTileAt(curx, cury + 1);
                    Tile left = map.getTileAt(curx - 1, cury);
                    Tile right = map.getTileAt(curx + 1, cury);

                    if (canAdd(up, open, closed))
                        addTile(up, cur, open, goal);

                    if (canAdd(down, open, closed))
                        addTile(down, cur, open, goal);

                    if (canAdd(left, open, closed))
                        addTile(left, cur, open, goal);

                    if (canAdd(right, open, closed))
                        addTile(right, cur, open, goal);
                }

            }
            if(DEBUG)
                Console.WriteLine("Failed to find a path!");

            return ret;
        }

        private bool canAdd(Tile toadd, List<Tile> open, List<Tile> closed)
        {
            if (toadd != null && toadd.getType() != TileType.WALL && !closed.Contains(toadd) && !open.Contains(toadd))
                return true;
            else
                return false;
        }

        private void addTile(Tile toadd, Tile prev, List<Tile> open, Tile goal)
        {
            toadd.setPrevious(prev);
            int distance = getCumulativeCost(toadd);
            toadd.addToTotalCost(distance + Math.Abs(toadd.getMapX() - goal.getMapX()) + Math.Abs(toadd.getMapY() - goal.getMapY()));
                if(DEBUG)
                Console.WriteLine("Adding node with cost " + toadd.getTotalCost() + " to open list");
            open.Add(toadd);
        }

        private int getCumulativeCost(Tile node)
        {
            int ret = 0;
            Tile cur = node;
            while (cur != null)
            {
                ret += cur.getCost();
                cur = cur.getPrevious();
            }
            return ret;
        }

        /*private Tile addToBestPath(List<Tile> bestpath, Tile toadd, Tile prev)
        {
            int x = toadd.getX();
            int y = toadd.getY();
            int mapx = toadd.getMapX();
            int mapy = toadd.getMapY();
            int len = toadd.getLength();

            if(DEBUG)
            Console.WriteLine("Adding tile at (" + mapx + "," + mapy + ") to bestpath! Cost is " + toadd.getTotalCost());

            Tile newadd = new Tile(mapx, mapy, x, y, len, astarwaypoint, Color.White);
            newadd.setPrevious(prev);
            bestpath.Add(newadd);
            return newadd;
        }*/

        protected void drawErrors(SpriteBatch spriteBatch)
        {
            if (state == GameState.ASTAR || state == GameState.RUNNING)
            {
                if (map.getPlayerTile() == null)
                    spriteBatch.DrawString(font, "Add A Player!", new Vector2(200, 300), Color.Red);

                if (map.getMonsterTile() == null)
                    spriteBatch.DrawString(font, "Add A Monster!", new Vector2(200, 330), Color.Red);

                if (astartile == null)
                    spriteBatch.DrawString(font, "No Path To Player!", new Vector2(200, 360), Color.Red);
            }
            else if (state == GameState.GAMEOVER)
            {
                    spriteBatch.DrawString(font, "GAME OVER", new Vector2(200, 360), Color.Red);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            map.Draw(spriteBatch);
            toolmap.Draw(spriteBatch, state);

            //if(state == GameState.ASTAR)
           //     drawAStarTiles(spriteBatch);

            drawErrors(spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        /*private void drawAStarTiles(SpriteBatch spriteBatch)
        {
            if (astartile != null)
            {
                Tile cur = astartile;
                while (cur != null)
                {
                    cur.Draw(spriteBatch);
                    cur = cur.getPrevious();
                }
            }
        }*/
    }
}
