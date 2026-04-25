using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PauseController : MonoBehaviour
{
    public List<FloorText> floors = new List<FloorText>();
    public TextMeshProUGUI sinName;
    public TextMeshProUGUI sinDescription;
    public Image sinImage;
    private int[] eachSin = new int[] {0,0,0,0,0,0,0,0,0};
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sinName.enabled = false;
        sinDescription.enabled = false;
        sinImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        float randomY = Random.Range(-height / 2, height / 2);

        newImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);
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


}
