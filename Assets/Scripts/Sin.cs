using UnityEngine;

[CreateAssetMenu(fileName = "New Sin", menuName = "Sin")]
public class Sin : ScriptableObject
{
    public int sinId;
    [TextArea] public string description = "REPLACE ME";
    public HellCircle hellCircle;
}
