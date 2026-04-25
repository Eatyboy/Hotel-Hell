using UnityEngine;

public class Satan : MonoBehaviour
{
    public GameObject[] satans;

    private void Awake()
    {
        if (satans.Length != 4)
        {
            Debug.LogError("There should be 4 satans");
        }

        SetSatanLevel(0);
    }

    public void SetSatanLevel(int level)
    {
        if (level < 0 || level > satans.Length)
        {
            Debug.LogError("Invalid satan level: " + level.ToString());
            return;
        }

        for (int i = 0; i < satans.Length; i++)
        {
            if (i == level) satans[i].SetActive(true);
            else satans[i].SetActive(false);
        }
    }
}
