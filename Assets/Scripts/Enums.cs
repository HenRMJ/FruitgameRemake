using System;

[Serializable]
public enum Fruit
{
    Cherry, 
    Strawberry, 
    Grapes, 
    Pomegranate,
    Apple, 
    Orange, 
    Coconut, 
    Banana,
    Pineapple,
    Watermelon,
    Pumpkin
}

[Serializable]
public enum GameMode
{
    Quick,
    Classic,
    Endless
}

[Serializable]
public enum BatteryMode
{
    Extreme = 15,
    On = 30,
    Low = 60,
    Off = -1
}
