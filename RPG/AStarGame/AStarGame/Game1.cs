/*
Name: Michael Crawford
Class: CS134
Instructor: Dr. Teoh
Term: Spring 2012
Assignment: Project 2
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;


namespace RPG
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);
        public static Texture2D evindicator;
        const int MONSTER_MOVE_DELAY = 500;
        const int PLAYER_MOVE_DELAY = 100;
        Party party;
        Player p;
        BattleSequence bs;

        const bool STARTPLAY = true;
        const String FIRSTMAP = "world3.rpgmf";
        const int DEFAULT_X_TILES = 20;
        const int DEFAULT_Y_TILES = 20;

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        SpriteFont font;

       public static Texture2D buttonImage;
        public static SpriteFont buttonFont;

        TileMap map;
        ToolMap toolmap;
        Texture2D whitepixel;
        Texture2D astarwaypoint;
        Song cave, town, battle;
        Song currSong;

        WorldTile[] worldtiles;
        Dictionary<String, Texture2D> texmap;
        Dictionary<String, TileMap> maps;

        Tool[][] tools;

        List<Tile> astartiles;
        bool edited;
        bool inaddevent;
        bool inprogress;
        const bool DEBUG = false;

        int lastmonstermove;
        int lastplayermove;

        bool mapsaved = false;
        bool maploaded = false;

        public static GameState state;
        public static PlayState playstate;

        static Queue<Event> eventqueue;

        //System.Windows.Forms.ComboBox combobox;

        public static void addToEventQueue(Event e)
        {
            if (eventqueue != null)
                eventqueue.Enqueue(e);
        }

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
            
            if (STARTPLAY)
            {
                state = GameState.RUNNING;
                playstate = PlayState.WORLD;
            }
            else
            {
                state = GameState.EDIT;
            }
            eventqueue = new Queue<Event>();
            texmap = new Dictionary<String, Texture2D>();
            maps = new Dictionary<String, TileMap>();
            lastmonstermove = 0;
            lastplayermove = 0;
            inaddevent = false;
            buttonFont = Content.Load<SpriteFont>("buttonFont");
            buttonImage = Content.Load<Texture2D>("Tiles/buttonSmall");
            //PlayerBase war = p.getNewPlayer("WARRIOR");
          
            Player[] playerList = new Player[] { new Player(Player.WARRIOR, Sprite.WARRIOR, "Wally") };
            party = new Party(playerList);
            base.Initialize();
        }
        private void PlayMusic(Song song)
        {
            // Due to the way the MediaPlayer plays music,
            // we have to catch the exception. Music will play when the game is not tethered
            try
            {
                // Play the music
                MediaPlayer.Play(song);

                // Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch { }
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
            astartiles = new List<Tile>();
            astarwaypoint = Content.Load<Texture2D>("AStarWayPoint");
            edited = true;
            
            font = Content.Load<SpriteFont>("gameFont");

            evindicator = Content.Load<Texture2D>("AStarWayPoint");
            cave = Content.Load<Song>("cavemusic");
            town = Content.Load<Song>("Townmusic");
            battle = Content.Load<Song>("battlemusic");
            currSong = town;
            /*tools = new Tool[2][];
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
            */

            


            System.Collections.ArrayList tiles = new System.Collections.ArrayList();
            foreach (WorldTile t in Enum.GetValues(typeof(WorldTile)))
            {
                tiles.Add(t);
                Console.WriteLine(t.GetInformation());

                try
                {
                    Texture2D cur = Content.Load<Texture2D>(t.GetTexture());
                    if (cur != null && !texmap.ContainsKey(t.GetTexture()))
                        texmap.Add(t.GetTexture(), cur);
                }
                catch (Microsoft.Xna.Framework.Content.ContentLoadException e)
                {
                }
                
            }
            worldtiles = tiles.ToArray(typeof(WorldTile)) as WorldTile[];
            Array.Sort(worldtiles, delegate(WorldTile a, WorldTile b)
                                    {
                                        return a.ToString().CompareTo(b.ToString());
                                    });
            Console.WriteLine(worldtiles.Length);


            //map = new TileMap(10, 10, 17, 512, 512, whitepixel, tools[0][0]);
            toolmap = new ToolMap(578, 100, whitepixel, texmap, worldtiles, font, Window.Handle);
            if (!STARTPLAY)
            {
                toolmap.enable();
            }
            map = new TileMap(10, 10, 17, DEFAULT_X_TILES, DEFAULT_Y_TILES, whitepixel, toolmap, font, FIRSTMAP);
            if (STARTPLAY)
            {
                 FileStream fileStream = new FileStream(@FIRSTMAP, FileMode.Open);
                 StreamReader reader = new StreamReader(fileStream);
                 map.LoadMap(reader, FIRSTMAP, toolmap);
                 maps.Add(FIRSTMAP, map);
                 reader.Close();
                 fileStream.Close();
            }
            PlayMusic(Content.Load<Song>("Townmusic"));
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
                case GameState.MAINMENU:
                    break;
                case GameState.COMBAT:
                    break;
                case GameState.GAMEOVER:
                    break;
                case GameState.SHOP:
                    break;
                case GameState.INN:
                    break;
                case GameState.EDIT:
                    edited = true;
                    mapsaved = maploaded = false;
                    catchInput(gameTime, true);
                    map.resetPlayers();
                    map.refreshTiles();
                    map.Update(toolmap);
                    astartiles.Clear();
                    break;
                case GameState.ASTAR:
                    //start a*star code here...
                    map.unhighlight();
                    if (edited)
                    {
                        edited = false;
                        doMultiAStar();
                    }
                    map.refreshTiles();
                    break;
                case GameState.RUNNING:
                    //start game here...
                    switch(playstate)
                    {
                        case PlayState.WORLD:
                        map.unhighlight();
                        playGame(gameTime);
                        break;
                        case PlayState.BATTLE:
                            Console.WriteLine("PlayState is Battle!");
                            if (bs != null)
                            {
                                bs.Update();
                            }
                        break;
                    }

                    processEvents();
                    break;
                case GameState.SAVEMAP:
                    if (!mapsaved)
                    {
                        edited = false;
                        SaveMap();
                        mapsaved = true;
                        state = GameState.EDIT;
                    }
                    break;
                case GameState.LOADMAP:
                    if(!maploaded)
                    {
                        edited = true;
                        LoadMap();
                        maploaded = true;
                        state = GameState.EDIT;
                    }
                    break;
                case GameState.ADDEVENT:
                    if (!inaddevent)
                    {
                        inaddevent = true;
                        Tile seltile = toolmap.getSelectedTile();
                        if(seltile != null)
                            processAddEvent(seltile);
                        inaddevent = false;
                    }
                    state = GameState.EDIT;
                    break;

                default:
                    break;
            }
            
            base.Update(gameTime);
        }

        public void doMultiAStar()
        {
            Tile[] monsters = map.getMonsters();
            if (DEBUG)
                Console.WriteLine("Called doMultiAStar()");

            for (int i = 0; i < monsters.Length; i++)
            {
                Tile playertile = map.getPlayerTile();
                if (DEBUG)
                    Console.WriteLine("Player location is (" + playertile.getMapX() + "," + playertile.getMapY() + ")");

                Tile cur = processAStar(map.getPlayerTile(), monsters[i]);
                astartiles.Add(createAStarHighlight(cur));
                map.refreshTiles();
            }
            edited = false;
        }

        private void processAddEvent(Tile thetile)
        {
            Event e = new Event();
            Form1 form = new Form1(e);
            form.ShowDialog();

            /*
            String map = e.getProperty("mapfile");
            String x = e.getProperty("x");
            String y = e.getProperty("y");
            */
            if(e.getEventType() != EventType.CANCELED)
                thetile.addEvent(e);

            //Console.WriteLine("map=>" + map + "<, x=>" + x + "<, y=>" + y + "<");
        }

        private Tile createAStarHighlight(Tile best)
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
        }

        private Tile addToAStarHighlight(Tile toadd, Tile prev)
        {
            int x = toadd.getX();
            int y = toadd.getY();
            int mapx = toadd.getMapX();
            int mapy = toadd.getMapY();
            int len = toadd.getLength();

            if(DEBUG)
                Console.WriteLine("Adding tile at (" + mapx + "," + mapy + ") to bestpath! Cost is " + toadd.getTotalCost());

            Tile newadd = new Tile(mapx, mapy, x, y, len, new Tool(WorldTile.BLACK, astarwaypoint));
            newadd.setPrevious(prev);
            return newadd;
        }

        public void SaveMap()
        {
            System.Windows.Forms.SaveFileDialog openFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            openFileDialog1.Filter = "RPG Map Files (.rpgmf)|*.rpgmf";
            System.Windows.Forms.DialogResult userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();

                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    map.SaveMap(writer);
                }
                fileStream.Close();
            }
        }

        public void LoadMap()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            openFileDialog1.Filter = "RPG Map Files (.rpgmf)|*.rpgmf";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;


            System.Windows.Forms.DialogResult userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                System.IO.Stream fileStream = openFileDialog1.OpenFile();

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    map.LoadMap(reader, openFileDialog1.FileName, toolmap);
                }
                fileStream.Close();
            }
        }

        public void catchInput(GameTime gameTime, bool noclip)
        {
            if (!inprogress && (inprogress = true))
            {
                int currenttime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                if (currenttime - lastplayermove > PLAYER_MOVE_DELAY)
                {
                    //Console.WriteLine("noclip=" + noclip);
                    KeyboardState kb = Keyboard.GetState();
                    if (kb.IsKeyDown(Keys.LeftControl) && kb.IsKeyDown(Keys.D))
                    {
                        state = GameState.EDIT;
                        toolmap.selectEdit();
                        toolmap.enable();
                        inprogress = false;
                        return;
                    }
                    else if (kb.IsKeyDown(Keys.Up))
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
                    else if (kb.IsKeyDown(Keys.Right))
                    {
                        map.shiftRight(1, noclip);
                    }

                    lastplayermove = currenttime;
                }
                inprogress = false;
            }
        }

        private Tile processAStar(Tile player, Tile monster)
        {
            Tile ret = null;

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
            catchInput(gameTime, false);

            Tile[] monsters = map.getMonsters();
            Tile playertile = map.getPlayerTile();
            int currenttime = (int)gameTime.TotalGameTime.TotalMilliseconds;

            if ((currenttime - lastmonstermove) > MONSTER_MOVE_DELAY)
            {
                for (int i = 0; i < monsters.Length && playstate == PlayState.WORLD; i++)
                {
                    Tile astartile = null;
                    Tile best = processAStar(playertile, monsters[i]);
                    if (best != null)
                    {
                        astartile = best;
                        Tile next = getNextStep(best);
                        map.setMonsterLocation(i, next);
                        map.refreshTiles();
                        edited = true;
                    }
                    else
                        Console.WriteLine("Best is null!");
                }

                lastmonstermove = currenttime;
            }
        }

        private void processEvents()
        {
            while (eventqueue.Count > 0)
            {
                Event e = eventqueue.Dequeue();
                if (e.getEventType() == EventType.MAP_TRANSITION)
                {
                    String mapfile = e.getProperty("mapfile");
                    int x = Convert.ToInt32(e.getProperty("x"));
                    int y = Convert.ToInt32(e.getProperty("y"));

                    Console.WriteLine("Processing Map Transition Event for " + mapfile + " x=" + x + ",y=" + y);
                    
                    if(mapfile.Contains("dragon"))
                    {
                        if (!currSong.Equals(cave))
                        {
                            MediaPlayer.Stop();
                            PlayMusic(cave);
                            currSong = cave;
                        }
                    }
                    else
                    {
                        if (!currSong.Equals(town))
                        {
                            MediaPlayer.Stop();
                            PlayMusic(town);
                            currSong = town;
                        }
                    }
                    map = getMap(mapfile, x, y);
                    Console.WriteLine("Reached 2");
                    Game1.playstate = PlayState.WORLD;
                }
                else if (e.getEventType() == EventType.BATTLE_TILE)
                {
                    playstate = PlayState.BATTLE;
                    int x = map.getPlayerTile().getMapX();
                    int y = map.getPlayerTile().getMapY();
                    String file = map.filename;
                    map.RemoveMonsterTile(Convert.ToInt32(e.getProperty("index")));
                    map = getMap(e.getProperty("battlemap"), 8, 8);
                    


                    Player px = new Player();

                    bs = new BattleSequence(party, new Enemy[] {new Enemy(new Player(px.getNewPlayer("WARRIOR"), Sprite.ENEMY_1, "Ninja Pu", 7))}, Content.Load<SpriteFont>("gameFont"),map, x,y,file);
                    bs.Start();
                    //bs = null;
                    
                     
                    
                    PlayMusic(battle);
                    /*String mapfile = e.getProperty("mapfile");
                    int x = Convert.ToInt32(e.getProperty("x"));
                    int y = Convert.ToInt32(e.getProperty("y"));
                    */
                }
            }
        }

        private TileMap getMap(String mapfile, int playerx, int playery)
        {
            TileMap ret = null;
            if (maps.ContainsKey(mapfile))
            {
                ret = maps[mapfile];
                ret.setPlayerLocation(ret.getTileAt(playerx, playery));
            }
            else
            {
                FileStream fileStream = new FileStream(@mapfile, FileMode.Open);
                StreamReader reader = new StreamReader(fileStream);
                ret = new TileMap(10, 10, 17, DEFAULT_X_TILES, DEFAULT_Y_TILES, whitepixel, toolmap, font, mapfile);
                ret.LoadMap(reader, mapfile, toolmap);
                ret.setPlayerLocation(ret.getTileAt(playerx, playery));

                maps.Add(mapfile, ret);
                reader.Close();
                fileStream.Close();
            }
            return ret;
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
                    Console.WriteLine("Min retrieved node (" + cur.getMapX() + "," + cur.getMapY() + ") with " + cur.getTotalCost() + " from the open list");

                if (cur.getMapX() == goal.getMapX() && cur.getMapY() == goal.getMapY())
                {
                    if(DEBUG)
                        Console.WriteLine("Found goal! cur is (" + cur.getMapX() + "," + cur.getMapY() + ")");

                    return cur;
                    //return addToBestPath(bestpath, cur, last);
                }
                else
                {
                    int curx = cur.getMapX();
                    int cury = cur.getMapY();
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
            bool ret = false;
            if (DEBUG)
                Console.WriteLine("entered canAdd");

            if (toadd != null && !toadd.isObstacle() && !closed.Contains(toadd) && !open.Contains(toadd))
            {
                if(DEBUG)
                    Console.WriteLine("Can add (" + toadd.getMapX() + "," + toadd.getMapY() + ")");

                ret = true;
            }

            if (DEBUG)
                Console.WriteLine("exiting canAdd");

            return ret;
        }

        private void addTile(Tile toadd, Tile prev, List<Tile> open, Tile goal)
        {
            if (DEBUG)
            {
                Console.WriteLine("Adding node (" + toadd.getMapX() + "," + toadd.getMapY() + "), with cost " + toadd.getTotalCost() + " to open list, open count is = " + open.Count);
                if (prev != null)
                    Console.WriteLine("Previous is (" + prev.getMapX() + "," + prev.getMapY() + ")");
                else
                    Console.WriteLine("Previous is null!");
            }
            toadd.setPrevious(prev);
            int distance = getCumulativeCost(toadd);
            //int distance = 0;
            toadd.addToTotalCost(distance + Math.Abs(toadd.getMapX() - goal.getMapX()) + Math.Abs(toadd.getMapY() - goal.getMapY()));

            open.Add(toadd);
            if (DEBUG)
                Console.WriteLine("Node added.");
        }

        private int getCumulativeCost(Tile node)
        {
            int ret = 0;
            Tile cur = node;
            if (DEBUG)
            {
                while (cur != null)
                {
                    Console.Write("x");
                    ret += cur.getCost();
                    cur = cur.getPrevious();
                }
            }
            else
            {
                while (cur != null)
                {
                    ret += cur.getCost();
                    cur = cur.getPrevious();
                }
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

                /*if (map.getMonsterTile() == null)
                    spriteBatch.DrawString(font, "Add A Monster!", new Vector2(200, 330), Color.Red);
                */
                if (astartiles == null)
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
            if (state != GameState.RUNNING)
                toolmap.Draw(spriteBatch, state);
            else
            {
                if (PlayState.BATTLE == playstate)
                {
                    if (bs != null)
                    {
                        bs.Draw(spriteBatch);
                    }

                }

                    
            }

            if(state == GameState.ASTAR)
               drawAStarTiles(spriteBatch);

            //drawErrors(spriteBatch);
            spriteBatch.End();
            
            
            base.Draw(gameTime);
        }

        private void drawAStarTiles(SpriteBatch spriteBatch)
        {
            if (astartiles != null)
            {
                for (int i = 0; i < astartiles.Count; i++)
                {
                    Tile cur = astartiles[i];
                    while (cur != null)
                    {
                        Rectangle disp = map.getDisplayRectangleFor(cur);
                        if(disp != null)
                        {
                        cur.Draw(spriteBatch, disp);
                        cur = cur.getPrevious();
                        }
                    }
                }
            }
        }
    }
}
