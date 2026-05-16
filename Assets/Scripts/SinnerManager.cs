using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

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

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerProgress;

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
    public bool isPlayerProcessingSinner = false;

    private const int SINNER_QUEUE_LENGTH = 9;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        ScreenFader.FadeIn(GameManager.instance.transitionDuration / 2.0f).Forget();
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
            if (i == 3)
            {
                AddSinnerToQueue(true);
            }
            else AddSinnerToQueue(false);
        }

        NextSinner().Forget();

        AudioManager.instance.PlayMusic(AudioManager.instance.mainTheme);
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        if (isPlayerProcessingSinner && currentSinner != null)
        {
            inspectionTimeSecondsRemaining -= dt;

            timerText.text = Mathf.FloorToInt(inspectionTimeSecondsRemaining).ToString();
            timerProgress.fillAmount = inspectionTimeSecondsRemaining / maxInspectionTimeSeconds;

            if (inspectionTimeSecondsRemaining < 0.0f)
            {
                SendSinnerAway().Forget();
                Player.instance.LoseHP();
            }
        }
    }

    public void AddSinnerToQueue(bool forceKeySinner = false, bool forceBobDave = false)
    {
        bool isKeySinner = forceKeySinner || (sinnersProcessed % 5 == 2 && unusedKeySinners.Count > 0);
        SinnerData data = isKeySinner
            ? GetKeySinnerData()
            : GetRandomlyGeneratedSinnerData(forceBobDave);
        sinnerQueue.Enqueue(data);
    }

    [ContextMenu("Next Sinner")]
    public async UniTask NextSinner()
    {
        Assert.IsTrue(currentSinner == null);

        SinnerData data = sinnerQueue.Dequeue();
        if (sinnersProcessed == 1) data.sinnerName = "Joe Lina";

        Sinner sinner = Instantiate(sinnerPrefab, sinnerSpawnPoint.position, Quaternion.identity, sinnerParent);
        sinner.data = data;
        sinner.image.sprite = data.sprite;

        currentSinner = sinner;

        inspectionTimeSecondsRemaining = maxInspectionTimeSeconds;

        await Tweener.Group(sinnerEnterDuration, Easing.EaseOutExpo)
            .AddVector3(sinnerSpawnPoint.position, sinnerStayPoint.position, x => currentSinner.transform.position = x)
            .AddColor(Utils.WithAlpha(currentSinner.image.color, 0.0f), Utils.WithAlpha(currentSinner.image.color, 1.0f), x => currentSinner.image.color = x)
            .Play();

        await SinnerCard.instance.Open(data.sinnerName, data.sinnerDialogue, data.sins);

        isPlayerProcessingSinner = true;
    }

    public SinnerData GetRandomlyGeneratedSinnerData(bool forceBobDave = false)
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
        if (unusedKeySinners.Count == 9) index = bobDaveIndex;
        SinnerData data = unusedKeySinners[index];
        unusedKeySinners.RemoveAt(index);
        return data;
    }

    [ContextMenu("Send Current Sinner Away")]
    public async UniTask SendSinnerAway()
    {
        isPlayerProcessingSinner = false;

        await SinnerCard.instance.Close();

        elevator.OpenElevator();
        await UniTask.WaitUntil(() => elevator.isOpened);

        if (maxInspectionTimeSeconds > 10)  maxInspectionTimeSeconds--;

        await Tweener.Group(sinnerExitDuration, Easing.EaseOutExpo)
            .AddVector3(sinnerStayPoint.position, sinnerExitPoint.position, x => currentSinner.transform.position = x)
            .AddColor(Utils.WithAlpha(currentSinner.image.color, 1.0f), Utils.WithAlpha(currentSinner.image.color, 0.0f), x => currentSinner.image.color = x)
            .Play();

        elevator.CloseElevator();
        await UniTask.WaitUntil(() => !elevator.isOpened);

        Destroy(currentSinner.gameObject);
        currentSinner = null;
        AddSinnerToQueue();
        sinnersProcessed++;
    }
}
