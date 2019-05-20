using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace SelDeM
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private const int spriteSize = 64;
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Level curLevel;
        static Player player;
        KeyboardState oldkb, kb;
        public static CameraHandler camHand;
        MouseState oldms, ms;
        StartScreen s;
        EndScreen e;
        public static Song song1;

        Rectangle[] npc;
        //Rectangle shoes, key;
        Texture2D[] npcT;
        //Texture2D shoesT, keyT;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            MediaPlayer.IsRepeating = true;
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();
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
            oldkb = Keyboard.GetState();
            oldms = Mouse.GetState();
            npcT = new Texture2D[3];
            npc = new Rectangle[3];
            npc[0] = new Rectangle(0*64, 0*64, 64, 64);
            npc[1] = new Rectangle(2*64, 0*64, 64, 64);
            npc[2] = new Rectangle(4*64, 0*64, 64, 64);
            //shoes = new Rectangle(500, 200, 50, 50);
            //key = new Rectangle(600, 200, 50, 50);

            IsMouseVisible = true;
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
            // TODO: use this.Content to load your game content here
            song1 = Content.Load<Song>("bensound-acousticbreeze");
            MediaPlayer.Play(song1);
            npcT[0] = Content.Load<Texture2D>("sprite_0");
            npcT[1] = Content.Load<Texture2D>("sprite_1");
            npcT[2] = Content.Load<Texture2D>("sprite_2");
            //shoesT = Content.Load<Texture2D>("Temporary/shoes");
            //keyT = Content.Load<Texture2D>("Temporary/key");

            player = new Player(spriteBatch, this.Content.Load<Texture2D>("Hero"),new Rectangle(64,64,spriteSize,spriteSize), 3f);
            camHand = new CameraHandler(GraphicsDevice, new Vector2(64, 32), 2, 1, player.Speed);
            curLevel = new Level(spriteBatch, this.Content.Load<Texture2D>("start"), spriteSize, GraphicsDevice.Viewport.Bounds, player, graphics, this.Content);

            //DialogTree<DialogBox> T = new DialogTree<DialogBox>(new DialogBox(spriteBatch, Content, graphics, "Hifdsafe!", new List<string> { "Greet back" }));
            //T.AddChild(new DialogBox(spriteBatch, Content, graphics, "Hello.", new List<string> { "Let him introduce himself" }));
            //T[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "My name is Pete Hamburg. I'm a high school student at Allen High School. I'm a junior. I have zero social skills, no friends, and my grades are below average, to say the least. I have been like this since middle school, and they always said that high school would be a new start for me, but that wasn't the case. My family is dysfunctional. My dad left us when I was in elementary school, and I haven't seen him since. I hardly remember what he looks like. My mother works two jobs to accommodate for us, one shift from the morning till five in the afternoon, and then she joins the night shift and gets home at around 2 in the morning every night. I have to take care of myself for the most part, at least I'm independent. Too independent it seems though, since I can't make friends and all of my love interests have ended miserably. What do you want me to do?", new List<string> { "Go to school", "Skip and eat breakfast", "Keep sleeping" }));
            //T[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Wow, school is as boring as usual. I wish someone would come and bully me... *Bully walks up to character and pushes character down*", new List<string> { "Fight the bully", "Get bullied" }));
            //T[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "MMM, breakfast", new List<string> { "Eat food", "Throw away food" }));
            //T[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Keep sleeping", new List<string> { "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ" }));
            //T[0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Punches, kicks, somersaults into a hind kick, and lastly fatal blow.", new List<string> { }));
            //T[0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Oh no, I'm getting bullied. Ow, ouch, oof, yikes.", new List<string> { }));
            //T[0][0][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "Stomach capacity = 100%", new List<string> { }));
            //T[0][0][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "Nevermind, I guess this doesn't look that great. Better throw it away!", new List<string> { }));
            //T[0][0][2].AddChild(new DialogBox(spriteBatch, Content, graphics, "Zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz\nzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz...", new List<string> { }));
            //curLevel.setTile(0, 0, new Tile(new Rectangle(0 * 64, 0 * 64, 64, 64), "dialog", T));

            DialogTree<DialogBox> dT1NPC1 = new DialogTree<DialogBox>(new DialogBox(spriteBatch, Content, graphics, "Oh? Well what do we have here...", new List<string> { "..." })); //Used when the running shoes have not been acquired
            dT1NPC1.AddChild(new DialogBox(spriteBatch, Content, graphics, "Not much of a talker are you. Well, no worries, you mind helping me for a sec?", new List<string> { "Nod head.", "Back down from helping this friendly stranger." }));
            dT1NPC1[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "You see that item all the way yonder there? I would greatly appreciate if you could get it for me. My legs aren't as useful as they used to be.", new List<string> { }));
            curLevel.setTile(0, 0, new Tile(npc[0], "dialog", dT1NPC1, npcT[0], spriteBatch));

            //DialogTree<DialogBox> dT2NPC1 = new DialogTree<DialogBox>(new DialogBox(spriteBatch, Content, graphics, "Wow, that was pretty quick. Did you manage to get me that item?", new List<string> { "Hand over item.", "Keep this item." })); //Used when the running shoes have been acquired
            //dT2NPC1.AddChild(new DialogBox(spriteBatch, Content, graphics, "Hmmmmmmm...", new List<string> { "Stare as the stranger deeply examines the item." }));
            //dT2NPC1.AddChild(new DialogBox(spriteBatch, Content, graphics, "Really??? Nothing? How strange. I guess I better look around myself. Thanks anyway.", new List<string> { }));
            //dT2NPC1[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "It's just a pair of running shoes. And they don't fit me... how about this, you can have them. Think of it as thanks for helping me.", new List<string> { "Take the running shoes and equip them.", "Decline the offer by shaking your head side to side." }));
            //dT2NPC1[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "They fit you quite nicely don't they? Oh yes, how could I forget. What might your name be child?", new List<string> { "..." }));
            //dT2NPC1[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Oh come on now, just take them. Here.", new List<string> { "Accept the running shoes hesitantly." }));
            //dT2NPC1[0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Now that I think about it, you haven't spoken even an inkling yet. Might you be a mute?", new List<string> { "Nods head." }));
            //dT2NPC1[0][0][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "Alright, thanks for help. See ya around I guess.", new List<string> {  }));
            //dT2NPC1[0][0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Hahahaha! This is might first time being sorry for someone. Well, the names Chris Burkingham. You... I'll call you Bud. How about it?", new List<string> { "Accept"}));
            //dT2NPC1[0][0][0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Alright Bud, I think it's about time that I go. See ya around!", new List<string> {  }));
            ////curLevel.setTile(0, 0, new Tile(new Rectangle(0 * 64, 0 * 64, 64, 64), "dialog", dT2NPC1));

            DialogTree<DialogBox> dT1NPC2 = new DialogTree<DialogBox>(new DialogBox(spriteBatch, Content, graphics, "Sigh...", new List<string> { "Look at the depressed student." }));
            dT1NPC2.AddChild(new DialogBox(spriteBatch, Content, graphics, "Hm? What are you looking at?", new List<string> { "..." }));
            dT1NPC2[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Are you mocking me?!?!?", new List<string> { "Shake head." }));
            dT1NPC2[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "What's your freaking name!", new List<string> { "Point to mouth and throat" }));
            dT1NPC2[0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "You can't talk? Man, my luck is the best isn't it. Just leave me alone.", new List<string> { "Pat his head.", "Stare.", "Leave" }));
            dT1NPC2[0][0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "DO YOU WANT ME TO KILL YOU?!?!?! If you're not leaving, then I'm leaving!!!", new List<string> {  }));
            dT1NPC2[0][0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "...", new List<string> { "Stare intensely.", "Look at the student worriedly" }));
            dT1NPC2[0][0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "You leave the student alone.", new List<string> { }));
            dT1NPC2[0][0][0][0][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "GET AWAY FROM ME!!!", new List<string> {  }));
            dT1NPC2[0][0][0][0][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "Sigh. Look, I'm fine, I just lost my keys, that's all. Mind leaving me alone?", new List<string> { "Try to get more information about the keys by showing him your keys.", "Leave respectfully." }));
            dT1NPC2[0][0][0][0][1][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "Those are your keys right? Mine is both silver and smaller.", new List<string> { }));
            dT1NPC2[0][0][0][0][1][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "You walk away.", new List<string> { }));
            curLevel.setTile(2, 0, new Tile(npc[1], "dialog", dT1NPC2, npcT[1], spriteBatch));

            //DialogTree<DialogBox> dT2NPC2 = new DialogTree<DialogBox>(new DialogBox(spriteBatch, Content, graphics, "What do you want?", new List<string> { "Hand over his keys.", "Leave." }));
            //dT2NPC2.AddChild(new DialogBox(spriteBatch, Content, graphics, "You found my keys? Heck yeah! Now I can get outta here! Thanks bro, you're the best! Want me to give you a ride?", new List<string> { "Shake head." }));
            //dT2NPC2.AddChild(new DialogBox(spriteBatch, Content, graphics, "", new List<string> { }));
            //dT2NPC2[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "No, huh. Well, I'm gonna get goin' then. Cya.", new List<string> { }));
            //curLevel.setTile(0, 0, new Tile(new Rectangle(0 * 64, 0 * 64, 64, 64), "dialog", dT2NPC2));

            DialogTree<DialogBox> dT1NPC3 = new DialogTree<DialogBox>(new DialogBox(spriteBatch, Content, graphics, "HAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHAHA!!!", new List<string> { "Look at the person on the roof with you.", "Leave." }));
            dT1NPC3.AddChild(new DialogBox(spriteBatch, Content, graphics, "Look here world! I can fly!!!", new List<string> {"Run up to the person and grab them.", "Watch silently" }));
            dT1NPC3.AddChild(new DialogBox(spriteBatch, Content, graphics, "You left.", new List<string> { }));
            dT1NPC3[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "OHHH!?!?", new List<string> { "Throw them down on the ground."}));
            dT1NPC3[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "...", new List<string> { "Watch as he abruptly stops and just stares into the sky." }));
            dT1NPC3[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Hey, what are you doing? That hurt!", new List<string> { "Intensely stare at the person with a strained look." }));
            dT1NPC3[0][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "You've been staring for some time. Do you think I'm crazy?", new List<string> { "No.", "Yes.", "Really crazy. Worthy enough to leave him be." }));
            dT1NPC3[0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Hahaha, boy, you truly are innocent. Were you thinking I was jumping off the roof? Too bad! I was just reminiscing my childhood. You don't need to mind this old man.", new List<string> {  }));
            dT1NPC3[0][1][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Weird, wouldn't normally seeing a seemingly suicidal man spell out his doom be a strange occurrence?", new List<string> { "Yes.", "No."}));
            dT1NPC3[0][1][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Brutally honest aren't you... Hey, what do you think the point of life is? I'm growing older as the days pass and all I can see to do is reminisce about the past.", new List<string> { }));
            dT1NPC3[0][1][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "You leave.", new List<string> { }));
            dT1NPC3[0][1][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Hm...so you are normal. Ok well, don't mind me, I'll still be kicking even after the world ends. HAHAHAHAHAHAHAHAHAHA!!!", new List<string> { }));
            dT1NPC3[0][1][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "I think I'll leave you to your own devices then.", new List<string> { }));
            curLevel.setTile(4, 0, new Tile(npc[2], "dialog", dT1NPC3, npcT[2], spriteBatch));

            s = new StartScreen(spriteBatch, this.Content, graphics);
            e = new EndScreen(spriteBatch, this.Content, graphics);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            kb = Keyboard.GetState();
            ms = Mouse.GetState();
            // TODO: Add your update logic here
            if (s.Showing)
                s.Update(gameTime, kb, oldkb);
            else
            {
                camHand.Update(player.Rectangle);
                curLevel.Update(gameTime, kb, oldkb);
                player.Update(kb, oldkb, ms, oldms);
                if (Tile.count == 3)
                    e.Showing = true;
            }
            oldkb = kb;
            oldms = ms;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camHand.Camera.get_transformation(GraphicsDevice));
            if (s.Showing)
                s.Draw();
            else if (e.Showing)
                e.Draw();
            else
            {
                player.Draw();
                curLevel.Draw(gameTime);
                curLevel.Tiles[0, 0].Draw();
                curLevel.Tiles[2, 0].Draw();
                curLevel.Tiles[4, 0].Draw();
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
