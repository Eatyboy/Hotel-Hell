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
    public List<string> firstNamesList = new();
    public List<string> lastNamesList = new();
    public List<Sprite> spritesList = new();
    public List<SinnerData> keySinnerList = new();

    [Header("Parameters")]
    public int maxSins = 4;

    [Header("State")]
    public Sinner currentSinner = null;
    public Queue<SinnerData> sinnerQueue = new();

    private const int SINNER_QUEUE_LENGTH = 9;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        var (firstNames, lastNames) = Importer.LoadNames();
        firstNamesList = firstNames;
        lastNamesList = lastNames;
        sinsList = Importer.LoadSins();

        for (int i = 0; i < SINNER_QUEUE_LENGTH; ++i)
        {
            AddSinnerToQueue(i == 1 || i == 6);
        }

        SinnerCard.instance.Close();

        NextSinner();
    }

    public void AddSinnerToQueue(bool isKeySinner = false)
    {
        SinnerData data = isKeySinner
            ? GetKeySinnerData()
            : GetRandomlyGeneratedSinnerData();
        sinnerQueue.Enqueue(data);
    }

    [ContextMenu("Next Sinner")]
    public void NextSinner()
    {
        Assert.IsTrue(currentSinner == null);

        SinnerData data = sinnerQueue.Dequeue(); 

        Sinner sinner = Instantiate(sinnerPrefab, sinnerSpawnPoint.position, Quaternion.identity, canvas.transform);
        sinner.data = data;
        sinner.image.sprite = data.sprite;

        currentSinner = sinner;

        SinnerCard.instance.Open(data.sinnerName, data.sins);
    }

    public SinnerData GetRandomlyGeneratedSinnerData()
    {
        SinnerData data = new()
        {
            sinnerName = Utils.GetRandomItemInList(firstNamesList)
                + " "
                + Utils.GetRandomItemInList(lastNamesList),
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
        return data;
    }

    public SinnerData GetKeySinnerData()
    {
        return Utils.GetRandomItemInList(keySinnerList);
    }

    [ContextMenu("Send Current Sinner Away")]
    public void SendSinnerAway()
    {
        SinnerCard.instance.Close();
        Destroy(currentSinner.gameObject);
        currentSinner = null;
        AddSinnerToQueue(false);

        NextSinner();
    }
}
