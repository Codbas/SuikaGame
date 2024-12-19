using System;
using System.Xml.Schema;
using Microsoft.Xna.Framework;

namespace SuikaGame;

public enum FruitType
{
    Cranberry,
    Blueberry,
    Strawberry,
    Apple,
    Orange,
    Mango,
    Grapefruit,
    Dragonfruit,
    Melon,
    Watermelon
}

public class Fruit
{
    public FruitType Type { get; private set; }
    public int X { get; set; }
    public int Y { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public int Radius { get; private set; }
    public int Diameter { get; private set; }
    public int Mass { get; private set; }
    public Color Color { get; private set; }

    public Fruit(FruitType type, int x, int y)
    {
        Type = type;
        X = x;
        Y = y;
        Radius = GetFruitRadius(type);
        Color = GetFruitColor(type);
        Mass = GetFruitMass(type);
        Diameter = Radius * 2;
        VelocityX = 0;
        VelocityY = 0;
    }

    public Fruit(int score, int x, int y)
    {
        Type = RandomNewFruit(score);
        X = x;
        Y = y;
        Radius = GetFruitRadius(Type);
        Color = GetFruitColor(Type);
        Mass = GetFruitMass(Type);
        Diameter = Radius * 2;
        VelocityX = 0;
        VelocityY = 0;
    }

    public Vector2 getCenter()
    {
        return new Vector2(X - Radius, Y + Radius);
    }

    public int getCenterX()
    {
        return X - Radius;
    }

    public int getCenterY()
    {
        return Y - Radius;
    }

    private FruitType RandomNewFruit(int score)
    {
        Random rand = new Random();
        int index = rand.Next(0, 100);
        
        // TODO: implement algorithm that increases bigger fruit frequency with score
        
        return index switch
        {
            // 28% Cranberry, 25% Blueberry, 20% Strawberry, 15% Apple, 5% Orange, 3% Mango,
            // 2% Grapefruit, 1% Dragonfruit, 1% Melon, 0% Watermelon
            < 28 => FruitType.Cranberry,
            < 53 => FruitType.Blueberry,
            < 73 => FruitType.Strawberry,
            < 88 => FruitType.Apple,
            < 93 => FruitType.Orange,
            < 97 => FruitType.Mango,
            < 98 => FruitType.Grapefruit,
            < 99 => FruitType.Dragonfruit,
            < 100 => FruitType.Melon,
            _ => FruitType.Cranberry
        };
    }
    
    public void NextFruit()
    {
        if (Type == FruitType.Watermelon)
        {
            return;
        }
        
        var values = Enum.GetValues(typeof(FruitType));
        int currentIndex = Array.IndexOf(values, Type);
        int nextIndex = currentIndex + 1;
        
        Type = (FruitType)values.GetValue(nextIndex);
        Radius = GetFruitRadius(Type);
        Color = GetFruitColor(Type);
        Diameter = Radius * 2;
    }
    
    public void PreviousFruit()
    {
        if (Type ==FruitType.Cranberry)
        {
            return;
        }
        
        var values = Enum.GetValues(typeof(FruitType));
        int currentIndex = Array.IndexOf(values, Type);
        int nextIndex = currentIndex - 1;
        
        Type = (FruitType)values.GetValue(nextIndex);
        Radius = GetFruitRadius(Type);
        Color = GetFruitColor(Type);
        Diameter = Radius * 2;
    }
    
    private Color GetFruitColor(FruitType type)
    {
        return type switch
        {
            FruitType.Cranberry => Color.IndianRed,
            FruitType.Blueberry => Color.Blue,
            FruitType.Strawberry => Color.DarkRed,
            FruitType.Apple => Color.Red,
            FruitType.Orange => Color.Orange,
            FruitType.Mango => Color.OrangeRed,
            FruitType.Grapefruit => Color.LightPink,
            FruitType.Dragonfruit => Color.Aquamarine,
            FruitType.Melon => Color.PaleGreen,
            FruitType.Watermelon => Color.ForestGreen,
            _ => Color.White
        };
    }

    private int GetFruitRadius(FruitType type)
    {

        return type switch
        {
            FruitType.Cranberry => 17,
            FruitType.Blueberry => 26,
            FruitType.Strawberry => 38,
            FruitType.Apple => 52,
            FruitType.Orange => 70,
            FruitType.Mango => 86,
            FruitType.Grapefruit => 108,
            FruitType.Dragonfruit => 130,
            FruitType.Melon => 152,
            FruitType.Watermelon => 170,
            _ => 15
        };
    }

    private int GetFruitMass(FruitType type)
    {
        return type switch
        {
            FruitType.Cranberry => 1,
            FruitType.Blueberry => 2,
            FruitType.Strawberry => 4,
            FruitType.Apple => 8,
            FruitType.Orange => 12,
            FruitType.Mango => 18,
            FruitType.Grapefruit => 25,
            FruitType.Dragonfruit => 35,
            FruitType.Melon => 50,
            FruitType.Watermelon => 75,
            _ => 5
        };
    }

}