using System;
using UnityEngine;
public enum ECardColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Brown,
    None
}
[Serializable]
public class Card 
{
    public int CardNumber;
    public ECardColor Color;

    public Card(int cardNumber, ECardColor color)
    {
        CardNumber = cardNumber;
        Color = color;
    }

    public Card()
    {
        CardNumber = 0;
        Color = ECardColor.None; 
    }
}
