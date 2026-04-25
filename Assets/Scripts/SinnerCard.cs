using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
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

    public float talkingDurationMultiplier = 0.1f;

    private EventInstance talkingEventInstance;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        talkingEventInstance = AudioManager.instance.CreateEventInstance(AudioManager.instance.talking);
        talkingEventInstance.stop(mode: STOP_MODE.IMMEDIATE);
    }

    public void Open(string sinnerName, string sinnerDialogue, List<Sin> sins)
    {
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

    public void Close()
    {
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
