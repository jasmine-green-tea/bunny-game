using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Notification
{
    public Rabbit rabbit;
    public bool newRabbit;

    public Notification(Rabbit rabbit, bool newRabbit)
    {
        this.rabbit = rabbit;
        this.newRabbit = newRabbit;
    }
}


public class NotificationManager : PausableObject
{


    private List<Notification> notifications = new List<Notification>();

    public static NotificationManager Instance;

    public GameObject notificationKnob;
    public GameObject applicationForm;
    public GameObject releaseForm;

    public GameObject gameOverScreen;

    public Color positiveRep;
    public Color negativeRep;

    private Notification currentNotification;
    private RabbitStats currentRabbitStats;

    [SerializeField] private List<InventoryItem> inventoryItemsDB = new List<InventoryItem>();
    [SerializeField] private List<string> BunnyNameDB = new List<string>();

    public int totalRep = 20;
    int currentRepIncrement = 0;

    public TMP_Text repText;

    float badRepThresholdSeconds = 5;

    public Sprite[] rabbitSprites;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public string GetDayString(int days)
    {
        int d = days % 10;
        switch (d)
        {
            case 1:
                return " день";
            case 2:
            case 3:
            case 4:
                return " дня";
        }

        return " дней";
    }

    public void AddNotification(Rabbit rabbit, bool newRabbit)
    {
        Notification notification = new Notification(rabbit, newRabbit);

        if (newRabbit)
            notifications.Add(notification);
        else
        {
            notifications.Insert(0, notification);
            rabbit.SaveSadTime();
        }

        if (!notificationKnob.activeSelf)
            notificationKnob.SetActive(true);



    }

    public void PopNotification()
    {
        if (notifications.Count == 0)
        {
            // probably notify the player there are no notifications
            return;
        }

        PauseManager.Instance.PauseAll();
        currentNotification = notifications[0];
        // show the head of the list notification

        TimeManager.Instance.SetPaused(true);

        if (currentNotification.newRabbit)
        {
            // show an accept/decline form

            // generate stats for a rabbit
            RabbitStats newRabbitStats = new RabbitStats();
            newRabbitStats.hungerRate = UnityEngine.Random.Range(0.3f, 1.2f);
            newRabbitStats.moodRate = UnityEngine.Random.Range(0.3f, 1.2f);
            newRabbitStats.hygieneRate = UnityEngine.Random.Range(3 + 3f*0.3f,3 + 3f*1.2f);
            newRabbitStats.bunnyColor = (BunnyColor)UnityEngine.Random.Range((int)BunnyColor.Orange, (int)BunnyColor.MaxColors);
            newRabbitStats.daysLeft = UnityEngine.Random.Range(1, 6);
            if (newRabbitStats.daysLeft == 2)
                newRabbitStats.daysLeft = (UnityEngine.Random.Range(0, 2) == 1 ? 3 : 1);
            if (newRabbitStats.daysLeft == 4)
                newRabbitStats.daysLeft = (UnityEngine.Random.Range(0, 2) == 1 ? 5 : 3);
            newRabbitStats.currentDays = newRabbitStats.daysLeft;

            int liked_index = Random.Range(0, inventoryItemsDB.Count-1);

            newRabbitStats.likedItem = inventoryItemsDB[liked_index];
            int disliked_index = (liked_index + Random.Range(1, inventoryItemsDB.Count - 2)) % inventoryItemsDB.Count;
            newRabbitStats.dislikedItem = inventoryItemsDB[(liked_index + Random.Range(1, inventoryItemsDB.Count - 2)) % inventoryItemsDB.Count];
            newRabbitStats.bunnyName = BunnyNameDB[Random.Range(0, BunnyNameDB.Count)];




            currentRabbitStats = newRabbitStats;

            applicationForm.SetActive(true);

            applicationForm.transform.GetChild(0).Find("LikedFrame").Find("LikedItem").GetComponent<Image>().sprite = currentRabbitStats.likedItem.itemIcon;
            applicationForm.transform.GetChild(0).Find("DislikedFrame").Find("DislikedItem").GetComponent<Image>().sprite = currentRabbitStats.dislikedItem.itemIcon;
            applicationForm.transform.GetChild(0).Find("BunnyName").GetComponent<TMP_Text>().text = currentRabbitStats.bunnyName;
            applicationForm.transform.GetChild(0).Find("BunnySprite").GetComponent<Image>().sprite = rabbitSprites[(int)currentRabbitStats.bunnyColor];
            applicationForm.transform.GetChild(0).Find("Days").GetComponent<TMP_Text>().text = "На " + currentRabbitStats.daysLeft + GetDayString(currentRabbitStats.daysLeft);


            // fill data with rabbit stats
        }
        else
        {
            currentRabbitStats = currentNotification.rabbit.rabbitStats;

            // show a release form
            releaseForm.SetActive(true);

            releaseForm.transform.GetChild(0).Find("BunnyName").GetComponent<TMP_Text>().text = currentRabbitStats.bunnyName;
            releaseForm.transform.GetChild(0).Find("BunnySprite").GetComponent<Image>().sprite = rabbitSprites[(int)currentRabbitStats.bunnyColor];
            releaseForm.transform.GetChild(0).Find("Days").GetComponent<TMP_Text>().text = "Пробыл " + currentRabbitStats.daysLeft + GetDayString(currentRabbitStats.daysLeft) + "\nи уезжает домой";


            currentRepIncrement = 3;

            currentRepIncrement -= Mathf.FloorToInt(currentRabbitStats.sadTime / badRepThresholdSeconds);

            //releaseForm.transform.GetChild(2).GetComponent<Image>().color = (reputation < 0) ? negativeRep : positiveRep;
            releaseForm.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = "Репутация " + (currentRepIncrement > 0 ? "+" : (currentRepIncrement < 0 ? "-" : "")) + Mathf.Abs(currentRepIncrement);


            // fill data with money and reputation, probably rabbit stats
        }




    }

    public void OnAcceptBunny(bool accepted)
    {
        if (accepted)
        {
            RabbitManager.Instance.GenerateNewRabbit(currentRabbitStats);

        }
        else
        {
            // adjust reputation
        }


        applicationForm.SetActive(false);


        notifications.Remove(currentNotification);

        if (notifications.Count == 0)
            notificationKnob.SetActive(false);
        TimeManager.Instance.SetPaused(false);


    }

    public void OnReleaseBunny()
    {
        // add money
        // add reputation

        totalRep += currentRepIncrement;

        repText.text = "Репутация: " + totalRep;

        RabbitManager.Instance.UnregisterRabbit(currentNotification.rabbit);

        releaseForm.SetActive(false);


        notifications.Remove(currentNotification);

        if (notifications.Count == 0)
            notificationKnob.SetActive(false);
        TimeManager.Instance.SetPaused(false);


        if (totalRep < 0)
            gameOverScreen.SetActive(true);

    }




    // Start is called before the first frame update
    void Start()
    {
        applicationForm.SetActive(false);
        releaseForm.SetActive(false);
        notificationKnob.SetActive(false);
        gameOverScreen.SetActive(false);

        repText.text = "Репутация: " + totalRep;


        AddNotification(null, true);

        if (PauseManager.Instance != null)
            PauseManager.Instance.RegisterPausable(this);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
