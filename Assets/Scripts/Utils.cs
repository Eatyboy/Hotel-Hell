using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[System.Serializable]
public class WeightedItem<T>
{
    public T item;
    public float weight = 1;
}

public class Utils
{
    public static T GetRandomItemInList<T>(List<T> itemList)
    {
        return itemList[Random.Range(0, itemList.Count)];
    }

    public static T GetRandomWeightedItem<T>(List<WeightedItem<T>> itemList)
    {
        if (itemList == null || itemList.Count == 0)
        {
            Debug.LogError("Item list is null or empty");
        }

        float totalWeight = 0;
        foreach (var item in itemList)
        {
            totalWeight += item.weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var item in itemList)
        {
            cumulativeWeight += item.weight;
            if (randomValue <= cumulativeWeight)
            {
                return item.item;
            }
        }

        return itemList[0].item; 
    }

    public static float ExpEaseOut(float t) =>
        t == 1.0f ? 1.0f : 1.0f - Mathf.Pow(2.0f, -10.0f * t);
}
