using System;
using System.Collections;
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
    [SerializeField] private Transform sinnerStayPoint;
    [SerializeField] private Transform sinnerExitPoint;
    [SerializeField] private Transform sinnerParent;
    [SerializeField] private Elevator elevator;

    [Header("Data")]
    public List<Sin> sinsList = new();
    public List<string> firstNamesList = new();
    public List<string> lastNamesList = new();
    public List<string> dialogueList = new();

    public List<Sprite> spritesList = new();
    public List<SinnerData> keySinnerList = new();
    private List<SinnerData> unusedKeySinners;

    [Header("Parameters")]
    public int minSins = 3;
    public int maxSins = 5;
    public float maxInspectionTimeSeconds = 30.0f;
    public int keyNPCRate = 9;
    public float sinnerEnterDuration = 0.5f;
    public float sinnerExitDuration = 0.5f;

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
        dialogueList = Importer.LoadDialogue();

        unusedKeySinners = new(keySinnerList);

        for (int i = 0; i < SINNER_QUEUE_LENGTH; ++i)
        {
            AddSinnerToQueue();
        }

        SinnerCard.instance.Close();

        StartCoroutine(NextSinner());

        AudioManager.instance.PlayMusic(AudioManager.instance.mainTheme);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (currentSinner != null)
        {
            inspectionTimeSecondsRemaining -= dt;

            if (inspectionTimeSecondsRemaining < 0.0f)
            {
                StartCoroutine(SendSinnerAway());
                Player.instance.LoseHP();
            }
        }
    }

    public void AddSinnerToQueue()
    {
        bool isKeySinner = sinnersProcessed % 5 == 2 && unusedKeySinners.Count > 0;
        SinnerData data = isKeySinner
            ? GetKeySinnerData()
            : GetRandomlyGeneratedSinnerData();
        sinnerQueue.Enqueue(data);
    }

    [ContextMenu("Next Sinner")]
    public IEnumerator NextSinner()
    {
        Assert.IsTrue(currentSinner == null);

        SinnerData data = sinnerQueue.Dequeue();
        if (sinnersProcessed == 1) data.sinnerName = "Joe Lina";

        Sinner sinner = Instantiate(sinnerPrefab, sinnerSpawnPoint.position, Quaternion.identity, sinnerParent);
        sinner.data = data;
        sinner.image.sprite = data.sprite;

        currentSinner = sinner;

        inspectionTimeSecondsRemaining = maxInspectionTimeSeconds;

        Color opaqueColor = new(
            currentSinner.image.color.r,
            currentSinner.image.color.g,
            currentSinner.image.color.b,
            1.0f
            );
        Color transparentColor = new(
            currentSinner.image.color.r,
            currentSinner.image.color.g,
            currentSinner.image.color.b,
            0.0f
            );

        currentSinner.image.color = opaqueColor;
        float elapsedTime = 0.0f;
        while (elapsedTime < sinnerEnterDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / sinnerEnterDuration;
            t = t == 1.0f ? 1.0f : 1.0f - Mathf.Pow(2.0f, -10.0f * t);

            currentSinner.transform.position = Vector3.Lerp(sinnerSpawnPoint.position, sinnerStayPoint.position, t);
            currentSinner.image.color = Color.Lerp(transparentColor, opaqueColor, t);

            yield return null;
        }
        currentSinner.image.color = opaqueColor;
        currentSinner.transform.position = sinnerStayPoint.position;

        SinnerCard.instance.Open(data.sinnerName, data.sinnerDialogue, data.sins);
    }

    public SinnerData GetRandomlyGeneratedSinnerData()
    {
        SinnerData data = new()
        {
            sinnerName = Utils.GetRandomItemInList(firstNamesList)
                + " "
                + Utils.GetRandomItemInList(lastNamesList),
            sprite = Utils.GetRandomItemInList(spritesList), 
            sinnerDialogue = Utils.GetRandomItemInList(dialogueList)
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

        int bobDaveIndex = 1;
        int index = UnityEngine.Random.Range(0, unusedKeySinners.Count);
        if (unusedKeySinners.Count == 8) index = bobDaveIndex;
        SinnerData data = unusedKeySinners[index];
        unusedKeySinners.RemoveAt(index);
        return data;
    }

    [ContextMenu("Send Current Sinner Away")]
    public IEnumerator SendSinnerAway()
    {
        SinnerCard.instance.Close();

        elevator.OpenElevator();
        yield return new WaitUntil(() => elevator.isOpened);

        Color opaqueColor = new(
            currentSinner.image.color.r,
            currentSinner.image.color.g,
            currentSinner.image.color.b,
            1.0f
            );
        Color transparentColor = new(
            currentSinner.image.color.r,
            currentSinner.image.color.g,
            currentSinner.image.color.b,
            0.0f
            );

        float elapsedTime = 0.0f;
        while (elapsedTime < sinnerExitDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / sinnerExitDuration;
            t = t == 1.0f ? 1.0f : 1.0f - Mathf.Pow(2.0f, -10.0f * t);

            currentSinner.transform.position = Vector3.Lerp(sinnerStayPoint.position, sinnerExitPoint.position, t);
            currentSinner.image.color = Color.Lerp(opaqueColor, transparentColor, t);

            yield return null;
        }
        currentSinner.transform.position = sinnerExitPoint.position;
        currentSinner.image.color = transparentColor;

        elevator.CloseElevator();
        yield return new WaitUntil(() => !elevator.isOpened);

        Destroy(currentSinner.gameObject);
        currentSinner = null;
        AddSinnerToQueue();
        sinnersProcessed++;
    }
}
