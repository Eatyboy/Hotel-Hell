using FMOD.Studio;
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
    public float closeRotationSpeed = 1.0f;

    private EventInstance talkingEventInstance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        talkingEventInstance = AudioManager.instance.CreateEventInstance(AudioManager.instance.talking);
        talkingEventInstance.stop(mode: STOP_MODE.IMMEDIATE);
    }

    public IEnumerator Open(string sinnerName, string sinnerDialogue, List<Sin> sins)
    {
        transform.rotation = Quaternion.identity;
        cardCanvasGroup.alpha = 1.0f;
        float elapsedTime = 0.0f;
        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / openDuration;
            t = Utils.ExpEaseOut(t);

            transform.position = Vector3.Lerp(startPosition.position, stayPosition.position, t);
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            yield return null;
        }
        transform.position = stayPosition.position;
        transform.localScale = Vector3.one;

        sinnerDialoguer.transform.parent.gameObject.SetActive(true);
        sinnerNameTMP.text = sinnerName;
        sinnerDialoguer.text = sinnerDialogue;
        StartCoroutine(Talk(talkingDurationMultiplier * sinnerDialogue.Length));
        StringBuilder sb = new();
        foreach (var sin in sins)
        {
            sb.Append("• ");
            sb.Append(sin.description);
            sb.Append('\n');
        }
        sinnerDescriptionTMP.text = sb.ToString();

        sinnerCardObject.SetActive(true);

        AudioManager.instance.PlayOneShot(AudioManager.instance.paper);
    }

    public IEnumerator Close()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < openDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / openDuration;
            t = Utils.ExpEaseOut(t);

            transform.position = Vector3.Lerp(stayPosition.position, exitPosition.position, t);
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            transform.rotation = Quaternion.AngleAxis(elapsedTime * closeRotationSpeed, Vector3.forward);
            cardCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, t);

            yield return null;
        }
        transform.position = exitPosition.position;
        transform.localScale = Vector3.zero;
        cardCanvasGroup.alpha = 0.0f;

        sinnerDialoguer.transform.parent.gameObject.SetActive(false);
        sinnerCardObject.SetActive(false);
    }

    private IEnumerator Talk(float duration)
    {
        talkingEventInstance.start();
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        talkingEventInstance.stop(STOP_MODE.IMMEDIATE);
    }
}
