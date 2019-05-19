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


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

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

            DialogTree<DialogBox> dT2NPC1 = new DialogTree<DialogBox>(new DialogBox(spriteBatch, Content, graphics, "Wow, that was pretty quick. Did you manage to get me that item?", new List<string> { "Hand over item.", "Keep this item." })); //Used when the running shoes have been acquired
            dT2NPC1.AddChild(new DialogBox(spriteBatch, Content, graphics, "Hmmmmmmm...", new List<string> { "Stare as the stranger deeply examines the item." }));
            dT2NPC1.AddChild(new DialogBox(spriteBatch, Content, graphics, "Really??? Nothing? How strange. I guess I better look around myself. Thanks anyway.", new List<string> { }));
            dT2NPC1[0].AddChild(new DialogBox(spriteBatch, Content, graphics, "It's just a pair of running shoes. And they don't fit me... how about this, you can have them. Think of it as thanks for helping me.", new List<string> { "Take the running shoes and equip them.", "Decline the offer by shaking your head side to side." }));
            dT2NPC1[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "They fit you quite nicely don't they? Oh yes, how could I forget. What might your name be child?", new List<string> { "..." }));
            dT2NPC1[0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Oh come on now, just take them. Here.", new List<string> { "Accept the running shoes hesitantly." }));
            dT2NPC1[0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Now that I think about it, you haven't spoken even an inkling yet. Might you be a mute?", new List<string> { "Nods head." }));
            dT2NPC1[0][0][1].AddChild(new DialogBox(spriteBatch, Content, graphics, "Alright, thanks for help. See ya around I guess.", new List<string> {  }));
            dT2NPC1[0][0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Hahahaha! This is might first time being sorry for someone. Well, the names Chris Burkingham. You... I'll call you Bud. How about it?", new List<string> { "Accept"}));
            dT2NPC1[0][0][0][0][0].AddChild(new DialogBox(spriteBatch, Content, graphics, "Alright Bud, I think it's about time that I go. See ya around!", new List<string> {  }));
            curLevel.setTile(0, 0, new Tile(new Rectangle(0 * 64, 0 * 64, 64, 64), "dialog", dT2NPC1));
            s = new StartScreen(spriteBatch, this.Content, graphics);



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
            else
            {
                player.Draw();
                curLevel.Draw(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
