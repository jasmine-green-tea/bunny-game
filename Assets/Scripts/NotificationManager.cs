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


public class NotificationManager : MonoBehaviour
{


    private List<Notification> notifications = new List<Notification>();

    public static NotificationManager Instance;

    public GameObject notificationKnob;
    public GameObject applicationForm;
    public GameObject releaseForm;

    private Notification currentNotification;
    private RabbitStats currentRabbitStats;

    [SerializeField] private List<InventoryItem> inventoryItemsDB = new List<InventoryItem>();
    [SerializeField] private List<string> BunnyNameDB = new List<string>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddNotification(Rabbit rabbit, bool newRabbit)
    {
        Notification notification = new Notification(rabbit, newRabbit);

        if (newRabbit)
            notifications.Add(notification);
        else
            notifications.Insert(0, notification);

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
        currentNotification = notifications[0];
        // show the head of the list notification

        TimeManager.Instance.SetPaused(true);

        if (currentNotification.newRabbit)
        {
            // show an accept/decline form

            // generate stats for a rabbit
            RabbitStats newRabbitStats = new RabbitStats();
            newRabbitStats.hungerRate = UnityEngine.Random.Range(0.5f, 3f);
            newRabbitStats.moodRate = UnityEngine.Random.Range(0.5f, 3f);
            newRabbitStats.hygieneRate = UnityEngine.Random.Range(5 + 3f*0.5f,5 + 3f*3f);

            int liked_index = Random.Range(0, inventoryItemsDB.Count-1);
            Debug.Log(inventoryItemsDB.Count);

            newRabbitStats.likedItem = inventoryItemsDB[liked_index];
            int disliked_index = (liked_index + Random.Range(1, inventoryItemsDB.Count - 2)) % inventoryItemsDB.Count;
            newRabbitStats.dislikedItem = inventoryItemsDB[(liked_index + Random.Range(1, inventoryItemsDB.Count - 2)) % inventoryItemsDB.Count];
            newRabbitStats.bunnyName = BunnyNameDB[Random.Range(0, BunnyNameDB.Count)];




            currentRabbitStats = newRabbitStats;

            applicationForm.SetActive(true);

            applicationForm.transform.GetChild(0).Find("LikedFrame").Find("LikedItem").GetComponent<Image>().sprite = newRabbitStats.likedItem.itemIcon;
            applicationForm.transform.GetChild(0).Find("DislikedFrame").Find("DislikedItem").GetComponent<Image>().sprite = newRabbitStats.dislikedItem.itemIcon;
            applicationForm.transform.GetChild(0).Find("BunnyName").GetComponent<TMP_Text>().text = newRabbitStats.bunnyName;

            // fill data with rabbit stats
        }
        else
        {
            // show a release form
            releaseForm.SetActive(true);
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

        RabbitManager.Instance.UnregisterRabbit(currentNotification.rabbit);

        releaseForm.SetActive(false);


        notifications.Remove(currentNotification);

        if (notifications.Count == 0)
            notificationKnob.SetActive(false);
        TimeManager.Instance.SetPaused(false);


    }




    // Start is called before the first frame update
    void Start()
    {
        applicationForm.SetActive(false);
        releaseForm.SetActive(false);
        notificationKnob.SetActive(false);

        AddNotification(null, true);
        AddNotification(null, true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
