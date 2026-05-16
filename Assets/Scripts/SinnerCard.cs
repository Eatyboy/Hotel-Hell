using Cysharp.Threading.Tasks;
using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;

public class SinnerCard : MonoBehaviour
{
    public static SinnerCard instance;

    public GameObject sinnerCardObject;
    public TextMeshProUGUI sinnerNameTMP;
    public TextMeshProUGUI sinnerDescriptionTMP;
    public TextMeshProUGUI sinnerDialoguer;
    public CanvasGroup cardCanvasGroup;

    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform stayPosition;
    [SerializeField] private Transform exitPosition;

    public float talkingDurationMultiplier = 0.1f;
    public float openDuration = 0.3f;
    public float closeDuration = 0.3f;
    public float closeRotationSpeed = 20.0f;

    private EventInstance talkingEventInstance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        talkingEventInstance = AudioManager.instance.CreateEventInstance(AudioManager.instance.talking);
        talkingEventInstance.stop(mode: STOP_MODE.IMMEDIATE);
    }

    public async UniTask Open(string sinnerName, string sinnerDialogue, List<Sin> sins)
    {
        sinnerCardObject.SetActive(true);

        sinnerDialoguer.transform.parent.gameObject.SetActive(true);
        sinnerNameTMP.text = sinnerName;
        sinnerDialoguer.text = sinnerDialogue;
        StringBuilder sb = new();
        foreach (var sin in sins)
        {
            sb.Append("• ");
            sb.Append(sin.description);
            sb.Append('\n');
        }
        sinnerDescriptionTMP.text = sb.ToString();

        sinnerCardObject.transform.rotation = Quaternion.identity;
        cardCanvasGroup.alpha = 1.0f;
        await Tweener.Group(openDuration, Easing.EaseOutExpo)
            .AddVector3(startPosition.position, stayPosition.position, x => sinnerCardObject.transform.position = x)
            .AddVector3(Vector3.one * 0.3f, Vector3.one, x => sinnerCardObject.transform.localScale = x)
            .Play();

        AudioManager.instance.PlayOneShot(AudioManager.instance.paper);

        await Talk(talkingDurationMultiplier * sinnerDialogue.Length);
    }

    public async UniTask Close()
    {
        await Tweener.Group(closeDuration, Easing.EaseOutExpo)
            .AddVector3(stayPosition.position, exitPosition.position, x => sinnerCardObject.transform.position = x)
            .AddVector3(Vector3.one, Vector3.zero, x => sinnerCardObject.transform.localScale = x)
            .AddFloat(1.0f, 0.0f, x => cardCanvasGroup.alpha = x)
            .Play();

        sinnerDialoguer.transform.parent.gameObject.SetActive(false);
        sinnerCardObject.SetActive(false);
    }

    private async UniTask Talk(float duration)
    {
        talkingEventInstance.start();
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        talkingEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
}
