using System.Collections.Generic;
using UnityEngine;

public enum HellCircle {
    None,
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

[System.Serializable]
public class SinnerData
{
    public string name;
    public Sprite sprite;
    public List<Sin> sins = new();
}
