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
    private List<SinnerData> unusedKeySinners;

    [Header("Parameters")]
    public int minSins = 3;
    public int maxSins = 5;
    public float maxInspectionTimeSeconds = 30.0f;
    public int keyNPCRate = 9;

    [Header("State")]
    public Sinner currentSinner = null;
    public Queue<SinnerData> sinnerQueue = new();
    public int sinnersProcessed = 0;
    public float inspectionTimeSecondsRemaining = 0.0f;

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

        unusedKeySinners = new(keySinnerList);

        for (int i = 0; i < SINNER_QUEUE_LENGTH; ++i)
        {
            AddSinnerToQueue();
        }

        SinnerCard.instance.Close();

        NextSinner();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (currentSinner != null)
        {
            inspectionTimeSecondsRemaining -= dt;

            if (inspectionTimeSecondsRemaining < 0.0f)
            {
                SendSinnerAway();
                Player.instance.LoseHP();
            }
        }
    }

    public void AddSinnerToQueue()
    {
        bool isKeySinner = sinnersProcessed % 9 == 2 && unusedKeySinners.Count > 0;
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
        if (sinnersProcessed == 1) data.sinnerName = "Joe Lina";

        Sinner sinner = Instantiate(sinnerPrefab, sinnerSpawnPoint.position, Quaternion.identity, canvas.transform);
        sinner.data = data;
        sinner.image.sprite = data.sprite;

        currentSinner = sinner;

        SinnerCard.instance.Open(data.sinnerName, data.sins);

        inspectionTimeSecondsRemaining = maxInspectionTimeSeconds;
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
        int sinCount = UnityEngine.Random.Range(minSins, maxSins + 1);
        for (int i = 0; i < sinCount; ++i)
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
        if (unusedKeySinners.Count == 0)
        {
            Debug.LogError("Tried to get key sinner when none were remaining");
            return null;
        }

        int index = UnityEngine.Random.Range(0, unusedKeySinners.Count);
        SinnerData data = unusedKeySinners[index];
        unusedKeySinners.RemoveAt(index);
        return data;
    }

    [ContextMenu("Send Current Sinner Away")]
    public void SendSinnerAway()
    {
        SinnerCard.instance.Close();
        Destroy(currentSinner.gameObject);
        currentSinner = null;
        AddSinnerToQueue();
        sinnersProcessed++;

        NextSinner();
    }
}
