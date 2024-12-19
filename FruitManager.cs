using System;
using System.Collections.Generic;

namespace SuikaGame;

public class FruitManager
{
    public List<Fruit> Fruits { get;  private set; } = [];

    public void AddFruit(Fruit fruit)
    {
        Fruits.Add(fruit);
    }

    public void RemoveFruit(Fruit fruit)
    {
        Fruits.Remove(fruit);
    }
    
   
}