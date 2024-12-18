using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuikaGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D texture;
    private bool mouseOnScreen = false;
    private Vector2 mousePos;
    private Vector2 lastPos;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = false; // Allow dynamic frame rates

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges(); 
        
        lastPos = new Vector2(0, 0);

        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        texture = Content.Load<Texture2D>("img");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
        mousePos = new Vector2(mouseState.X, mouseState.Y);

        if (lastPos != mousePos && mousePos.X < 0)
        {
            mousePos.X = 0;
        }
        else if (mousePos.X > (_graphics.PreferredBackBufferWidth - 100))
        {
            mousePos.X = _graphics.PreferredBackBufferWidth - 100;
        }
        
        if (lastPos != mousePos) {
            Console.WriteLine($"{mousePos.X}, {mousePos.Y}");
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            Console.WriteLine("Space");
        }
        
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            Console.WriteLine("Mouse Button Pressed");
        }
        
        // TODO: Add your update logic here

        lastPos = mousePos;
        
        // Calculate and log FPS
        //float fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        //Console.WriteLine($"FPS: {fps:0.00}");

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        //_spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Begin();

        
        _spriteBatch.Draw(texture, new Rectangle((int)mousePos.X, 0,100, 100), Color.White);
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}