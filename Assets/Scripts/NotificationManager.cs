using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


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

        if (currentNotification.newRabbit)
        {
            // show an accept/decline form

            // generate stats for a rabbit
            RabbitStats newRabbitStats = new RabbitStats();
            newRabbitStats.hungerRate = UnityEngine.Random.Range(0.5f, 3f);
            newRabbitStats.moodRate = UnityEngine.Random.Range(0.5f, 3f);
            newRabbitStats.hygieneRate = UnityEngine.Random.Range(0.5f, 3f);
            // fill favourite/hated lists

            currentRabbitStats = newRabbitStats;

            applicationForm.SetActive(true);
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
