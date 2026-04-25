using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SinnerManager : MonoBehaviour
{
    public static SinnerManager instance;

    public List<WeightedItem<Sin>> sinsList = new();
    public List<WeightedItem<string>> namesList = new();
    public List<WeightedItem<Sprite>> spritesList = new();

    public int maxSins = 4;
    [SerializeField] private Sinner sinnerPrefab;

    public Sinner currentSinner = null;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void NextSinner()
    {
        SinnerData data = new()
        {
            name = Utils.GetRandomWeightedItem(namesList),
            sprite = Utils.GetRandomWeightedItem(spritesList)
        };
        for (int i = 0; i < maxSins; ++i)
        {
            Sin sin = Utils.GetRandomWeightedItem(sinsList);
            while (data.sins.Any(s => s.sinId == sin.sinId))
            {
                sin = Utils.GetRandomWeightedItem(sinsList);
            }
            data.sins.Add(sin);
        }

        Sinner sinner = Instantiate(sinnerPrefab);
        sinner.data = data;

        currentSinner = sinner;

        Debug.Log($"{data.name}:");
        foreach (var sin in data.sins)
        {
            Debug.Log($"{Enum.GetName(typeof(HellCircle), sin.hellCircle)}: {sin.description}");
        }
    }
}
