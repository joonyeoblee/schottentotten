using System;
using UnityEngine;
public enum ECardColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Brown
}
[Serializable]
public class Card 
{
    public int CardNumber;
    public ECardColor Color;
    
}
