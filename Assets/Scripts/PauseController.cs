using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseController : MonoBehaviour
{
    public static PauseController instance;

    public GameObject pause;
    public List<FloorText> floors = new List<FloorText>();
    public TextMeshProUGUI sinName;
    public TextMeshProUGUI sinDescription;
    public Image sinImage;
    private int[] eachSin = new int[] {0,0,0,0,0,0,0,0,0};

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        pause.SetActive(false);
        resetPause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        pause.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        pause.SetActive(false);
    }

    public void updateEachSin(int index)
    {
        eachSin[index]++;
    }

    public GameObject sinnerPrefab;      
    public RectTransform spawnArea;    

    public void SpawnImageAtRandom()
    {
        GameObject newImage = Instantiate(sinnerPrefab, spawnArea, false);

        float width = spawnArea.rect.width - 100;
        float height = spawnArea.rect.height - 100;
        float randomX = Random.Range(-width / 2, width / 2);
        float bottomY = -height / 2;


        newImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, bottomY);
    }

    public void OnFloorButtonPressed(int floorNumber)
    {
        if (sinName.enabled == false) {sinName.enabled = true;}
        if (sinDescription.enabled == false) {sinDescription.enabled = true;}
        if (sinImage.enabled == false) {sinImage.enabled = true;}

        int index = floorNumber -1; 

        sinName.text = floors[index].sinName;
        sinDescription.text = floors[index].sinDescription;
        sinImage.sprite = floors[index].hellSprite;

        ClearSpawnedSinners();
        for (int i = 0; i < eachSin[index]; i++)
        {
            SpawnImageAtRandom();
        }
    }

    void ClearSpawnedSinners()
    {
        foreach (Transform child in spawnArea)
        {
            Destroy(child.gameObject);
        }
    }

    public void resetPause()
    {
        sinName.enabled = false;
        sinDescription.enabled = false;
        sinImage.enabled = false;
        ClearSpawnedSinners();
    }

}
