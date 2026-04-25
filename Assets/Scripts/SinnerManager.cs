using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SinnerManager : MonoBehaviour
{
    public List<WeightedItem<Sin>> sinsList = new();
    public List<WeightedItem<string>> namesList = new();
    public List<WeightedItem<Sprite>> spritesList = new();

    public int maxSins = 4;
    [SerializeField] private Sinner sinnerPrefab; 

    public Sinner GenerateSinner()
    {
        SinnerData data = new SinnerData();
        data.name = Utils.GetRandomWeightedItem(namesList);
        data.sprite = Utils.GetRandomWeightedItem(spritesList);
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

        return sinner;
    }
}
