using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BunnyBD : PausableObject
{
    public GameObject bd;
    public GameObject bunnyFrame;

    RabbitStats currentRabbitStats;

    List<Rabbit> bunnyList;
    int index = 0;
    bool inMenu = false;

    private void OnMouseDown()
    {
        if (bd.activeInHierarchy || IsPaused)
            return;
        ShowBD();
    }

    private void Start()
    {
        bd.SetActive(false);
        if (PauseManager.Instance != null)
            PauseManager.Instance.RegisterPausable(this);
    }

    public void ShowNextBunny(bool increment)
    {
        if (increment)
            index = (index + 1) % bunnyList.Count;
        else
            index = index - 1;
        if (index < 0)
            index = bunnyList.Count - 1;
        Debug.Log(index);


        currentRabbitStats = bunnyList[index].rabbitStats;

        bunnyFrame.transform.Find("LikedFrame").Find("LikedItem").GetComponent<Image>().sprite = currentRabbitStats.likedItem.itemIcon;
        bunnyFrame.transform.Find("DislikedFrame").Find("DislikedItem").GetComponent<Image>().sprite = currentRabbitStats.dislikedItem.itemIcon;
        bunnyFrame.transform.Find("BunnyName").GetComponent<TMP_Text>().text = currentRabbitStats.bunnyName;
        bunnyFrame.transform.Find("BunnySprite").GetComponent<Image>().sprite = NotificationManager.Instance.rabbitSprites[(int)currentRabbitStats.bunnyColor];
        bunnyFrame.transform.Find("Days").GetComponent<TMP_Text>().text = "Осталось: " + (currentRabbitStats.currentDays) + NotificationManager.Instance.GetDayString(currentRabbitStats.currentDays);

    }

    public void ShowBD()
    {
        bunnyList = RabbitManager.Instance.allRabbits;
        if (bunnyList.Count == 0)
            return;
        index = -1;

        bd.SetActive(true);

        ShowNextBunny(true);
    }
}
