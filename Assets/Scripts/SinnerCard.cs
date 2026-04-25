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

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void Open(string sinnerName, string sinnerDialogue, List<Sin> sins)
    {
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

        sinnerCardObject.SetActive(true);
    }

    public void Close()
    {
        sinnerCardObject.SetActive(false);
    }
}
