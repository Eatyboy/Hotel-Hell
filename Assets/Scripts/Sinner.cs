using UnityEngine;
using UnityEngine.UI;

public class Sinner : MonoBehaviour
{
    public SinnerData data;
    public Image image;

    public Sinner(SinnerData data)
    {
        this.data = data;
        image.sprite = data.sprite;
    }
}
