using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class SinnerManager : MonoBehaviour
{
    public static SinnerManager instance;

    [Header("References")]
    [SerializeField] private Sinner sinnerPrefab;
    [SerializeField] private Transform sinnerSpawnPoint;
    [SerializeField] private Canvas canvas;

    [Header("Data")]
    public List<Sin> sinsList = new();
    public List<string> namesList = new();
    public List<Sprite> spritesList = new();

    [Header("Parameters")]
    public int maxSins = 4;

    [Header("State")]
    public Sinner currentSinner = null;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    [ContextMenu("Next Sinner")]
    public void NextSinner()
    {
        Assert.IsTrue(currentSinner == null);

        SinnerData data = new()
        {
            name = Utils.GetRandomItemInList(namesList),
            sprite = Utils.GetRandomItemInList(spritesList)
        };
        for (int i = 0; i < maxSins; ++i)
        {
            Sin sin = Utils.GetRandomItemInList(sinsList);
            while (data.sins.Any(s => s.sinId == sin.sinId))
            {
                sin = Utils.GetRandomItemInList(sinsList);
            }
            data.sins.Add(sin);
        }

        Sinner sinner = Instantiate(sinnerPrefab, sinnerSpawnPoint.position, Quaternion.identity, canvas.transform);
        sinner.data = data;

        currentSinner = sinner;

        Debug.Log($"{data.name}:");
        foreach (var sin in data.sins)
        {
            Debug.Log($"{Enum.GetName(typeof(HellCircle), sin.hellCircle)}: {sin.description}");
        }
    }

    [ContextMenu("Send Current Sinner Away")]
    public void SendSinnerAway()
    {
        Destroy(currentSinner);
        currentSinner = null;
    }
}
