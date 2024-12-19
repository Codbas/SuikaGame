using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuikaGame;


public class Game1 : Game
{
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private FruitManager fruitManager;
    private Fruit currentFruit;
    private Fruit nextFruit;
    
    private int score = 0;
    private const float GRAVITY = 30f;
    private const int DROP_VELOCITY_Y = 400;
    private const float MAX_VELOCITY = 4000f;
    
    private const int WINDOW_WIDTH = 1600;
    private const int WINDOW_HEIGHT = 900;
    private const int PLAY_AREA_WIDTH = 800;
    private const int PLAY_AREA_HEIGHT = 900;
    private const int LEFT_WALL = (WINDOW_WIDTH - PLAY_AREA_WIDTH) / 2;
    private const int RIGHT_WALL = (WINDOW_WIDTH + PLAY_AREA_WIDTH) / 2;
    private const int NEXT_FRUIT_X = 1500;
    private const int NEXT_FRUIT_Y = 100;
    
    private Texture2D playAreaBgTexture;
    private Texture2D fruitTexture;
    
    private bool mouseLClickComplete = true;
    private int mousePosX; // mouse X position
    private int lastMousePosX = 0;
    private float timeSinceDrop = 0f;
    private bool dropped = false;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = false;
        _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;  
        _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
    }

    protected override void Initialize()
    {
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges(); 
        
        currentFruit = new Fruit(FruitType.Cranberry, 0, 0);
        nextFruit = new Fruit(score, NEXT_FRUIT_X, NEXT_FRUIT_Y);

        fruitManager = new FruitManager();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        fruitTexture = Content.Load<Texture2D>("fruit");

        playAreaBgTexture = new Texture2D(GraphicsDevice, 1, 1);
        playAreaBgTexture.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        mousePosX = Mouse.GetState().X;

        // Update current fruit
        currentFruit.X = calcCurrentFruitX();
        handleMouseClick();

        // Update dropped fruit
        applyFruitPhysics(deltaTime);
        
        lastMousePosX = mousePosX;
        
        // Calculate and log FPS
        //float fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        //Console.WriteLine($"FPS: {fps:0.00}");

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(playAreaBgTexture, new Rectangle(LEFT_WALL, 0, PLAY_AREA_WIDTH, PLAY_AREA_HEIGHT), Color.White);
        
        _spriteBatch.Draw(fruitTexture, new Rectangle(nextFruit.getCenterX(), nextFruit.getCenterY() ,nextFruit.Diameter, nextFruit.Diameter), nextFruit.Color);
        _spriteBatch.Draw(fruitTexture, new Rectangle(currentFruit.X, currentFruit.Y ,currentFruit.Diameter, currentFruit.Diameter), currentFruit.Color);

        foreach (Fruit fruit in fruitManager.Fruits)
        {
            _spriteBatch.Draw(fruitTexture, new Rectangle(fruit.X, fruit.Y ,fruit.Diameter, fruit.Diameter), fruit.Color);
        }
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void handleMouseClick()
    {
        if (mouseLClickComplete && Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            currentFruit.VelocityY = DROP_VELOCITY_Y + currentFruit.Mass;
            fruitManager.AddFruit(currentFruit);

            if (mousePosX - nextFruit.Radius <= LEFT_WALL)
            {
                nextFruit.X = LEFT_WALL;
            }
            else if (mousePosX + nextFruit.Diameter >= RIGHT_WALL)
            {
                nextFruit.X = RIGHT_WALL - nextFruit.Diameter;
            }
            else
            {
                nextFruit.X = mousePosX - nextFruit.Radius;
            }
            
            nextFruit.Y = currentFruit.Y;
            currentFruit = nextFruit;
            nextFruit = new Fruit(score, NEXT_FRUIT_X, NEXT_FRUIT_Y);
            
            mouseLClickComplete = false;
        }
        
        if (!mouseLClickComplete && Mouse.GetState().LeftButton == ButtonState.Released)
        {
            mouseLClickComplete = true;
        }
    }

    private int calcCurrentFruitX()
    {
        // Mouse hasn't moved since last update
        if (lastMousePosX == mousePosX)
        {
            return currentFruit.X;
        }
        
        // Mouse outside left bounds
        if (mousePosX < LEFT_WALL + currentFruit.Radius)
        {
            return LEFT_WALL;
        } 
        
        // Mouse outside right bounds
        if (mousePosX > RIGHT_WALL - currentFruit.Radius)
        {   
            return RIGHT_WALL - currentFruit.Diameter;
        }
        
        // Mouse moved and is inside bounds
        return mousePosX - currentFruit.Radius;
    }

    private void applyFruitPhysics(float deltaTime)
    {
        foreach (Fruit fruit in fruitManager.Fruits)
        {
            fruit.VelocityY += GRAVITY;
            
            fruit.Y += (int)((fruit.VelocityY) * deltaTime);

            if (fruit.VelocityY > MAX_VELOCITY)
            {
                fruit.VelocityY = MAX_VELOCITY;
            }
            
            if (fruit.Y + fruit.Diameter > WINDOW_HEIGHT)
            {
                fruit.Y = WINDOW_HEIGHT - fruit.Diameter;
                fruit.VelocityY = 0;
            }
        }
    }
    
}