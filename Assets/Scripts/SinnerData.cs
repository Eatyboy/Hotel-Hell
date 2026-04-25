using System.Collections.Generic;
using UnityEngine;

public enum HellCircle {
    Limbo,
    Lust,
    Gluttony,
    Greed,
    Anger,
    Heresy,
    Violence,
    Fraud,
    Treachery,
}

public class Sin : ScriptableObject
{
    string description = "REPLACE ME";
    HellCircle hellCircle;
}

[System.Serializable]
public class SinnerData
{
    public string name;
    public Sprite sprite;
    public List<Sin> sins;
}
