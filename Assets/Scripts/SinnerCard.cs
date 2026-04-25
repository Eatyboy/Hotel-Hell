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

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void Open(string sinnerName, List<Sin> sins)
    {
        sinnerNameTMP.text = sinnerName;
        StringBuilder sb = new();
        foreach (var sin in sins)
        {
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
