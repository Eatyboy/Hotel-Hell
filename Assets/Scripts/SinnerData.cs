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

[CreateAssetMenu(fileName = "New Sin", menuName = "Sin")]
public class Sin : ScriptableObject
{
    public int sinId;
    public string description = "REPLACE ME";
    public HellCircle hellCircle;
}

[System.Serializable]
public class SinnerData
{
    public string name;
    public Sprite sprite;
    public List<Sin> sins;
}
