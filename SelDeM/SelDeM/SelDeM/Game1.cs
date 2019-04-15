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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Level curLevel;
        static Player player;
        KeyboardState oldkb, kb;
        public static CameraHandler camHand;
        MouseState oldms, ms;

        DialogueTree test;

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
            curLevel = new Level(spriteBatch, this.Content.Load<Texture2D>("start"), spriteSize, GraphicsDevice.Viewport.Bounds, player, graphics, this.Content);
            camHand = new CameraHandler(GraphicsDevice,new Vector2(64,32),2,1,player.Speed);
            curLevel.setTile(0, 0, new Tile(new Rectangle(0*3, 0*3, 64, 64), "dialog"));

            test = new DialogueTree(spriteBatch, Content, graphics, "bgiuwqgbiuabfw");
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
            curLevel.Update(gameTime);
            player.Update(kb, oldkb, ms, oldms);
            camHand.Update(player.Rectangle);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camHand.Camera.get_transformation(GraphicsDevice));
            player.Draw();
            curLevel.Draw();
            test.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
