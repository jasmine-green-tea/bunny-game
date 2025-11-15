using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    public static TimeManager Instance;

    private int currentMonth = 1;
    private int currentDay = 1;

    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private Button bellButton;
    [SerializeField]
    private TMP_Text timeText;
    [SerializeField]
    private TMP_Text countdownText;

    [SerializeField]
    private float dayTimeSeconds = 100f;
    [SerializeField]
    private int dayHourStart = 8;

    [SerializeField]
    private int maxRabbitsPerDay = 2;

    private int counter = 0;
    private int currentTimestampIndex = -1;

    private List<int> timestamps = new List<int>();

    private string currentDayStr;

    private Coroutine timerCoroutine;

    private bool paused = false;

    private string GetMonthName()
    {
        switch (currentMonth)
        {
            case 1:
                return "JAN";
            case 2:
                return "FEB";
            case 3:
                return "MAR";
            case 4:
                return "APR";
            case 5:
                return "MAY";
            case 6:
                return "JUN";
            case 7:
                return "JUL";
            case 8:
                return "AUG";
            case 9:
                return "SEP";
            case 10:
                return "OCT";
            case 11:
                return "NOV";
            case 12:
                return "DEC";
        }
        return "NaM";
    }

    private int GetMaxMonthDays()
    {
        switch (currentMonth)
        {
            case 1:
                return 31;
            case 2:
                return 28;
            case 3:
                return 31;
            case 4:
                return 30;
            case 5:
                return 31;
            case 6:
                return 30;
            case 7:
                return 31;
            case 8:
                return 31;
            case 9:
                return 30;
            case 10:
                return 31;
            case 11:
                return 30;
            case 12:
                return 31;
        }
        return -1;
    }

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        currentDayStr = GetMonthName() + " " + currentDay.ToString();
        ReleaseDay();

    }

    private void Update()
    {

    }

    public void ReleaseDay()
    {

        int currentDayRabbits = UnityEngine.Random.Range(maxRabbitsPerDay / 2, maxRabbitsPerDay);

        if (currentDayRabbits > 0)
        {
            if (timestamps.Count > 0)
                timestamps.Clear();
            timestamps = new List<int>();
            for (int i = 0; i < currentDayRabbits; i++)
            {
                timestamps.Add((i+1) * ((int)dayTimeSeconds / (currentDayRabbits + 1)));
                //Debug.Log("added timestamp: " + (i + 1) * ((int)dayTimeSeconds / (currentDayRabbits + 1)));
            }

            currentTimestampIndex = 0;
            counter = 0;
        }
       

        ui.SetActive(false);
        bellButton.interactable = true;
        RabbitManager.Instance.SetRabbitsPause(false);
        countdownText.gameObject.SetActive(true);

        timerCoroutine = StartCoroutine(DayTimerCoroutine());
    }

    public void SetPaused(bool paused)
    {
        this.paused = paused;
    }

    private IEnumerator DayTimerCoroutine()
    {
        for (int i = 0; i < dayTimeSeconds / 60; i++)
        {
            for (int j = 0; j < 60; j++)
            {
                timeText.text = (dayHourStart + i).ToString() + ":" + (j > 9 ? j.ToString() : "0" + j.ToString());
                yield return new WaitForSeconds(1);
                while (paused) 
                {
                    yield return new WaitWhile(() => paused);
                }
                ;
                counter++;
                if (currentTimestampIndex != -1)
                {
                    string countdownString = "Новый кролик через: " + (timestamps[currentTimestampIndex] - counter).ToString();
                    countdownText.text = countdownString;

                    if (counter == timestamps[currentTimestampIndex])
                    {
                        currentTimestampIndex++;
                        if (currentTimestampIndex == timestamps.Count)
                        {
                            currentTimestampIndex = -1;
                            countdownText.gameObject.SetActive(false);
                        }

                        NotificationManager.Instance.AddNotification(null, true);

                    }
                }
            }

        }

        // set a flag to all bunnies to prevent them from losing stats
        RabbitManager.Instance.SetRabbitsPause(true);

        Debug.Log(currentDayStr);
        ui.SetActive(true);
        bellButton.interactable = false;


        if (currentDay + 1 > GetMaxMonthDays())
        {
            currentDay = 1;
            currentMonth = (currentMonth + 1) % 13;
            if (currentMonth == 0)
                currentMonth = 1;
        }
        else
            currentDay++;

        currentDayStr = GetMonthName() + " " + currentDay.ToString();


        yield return null;
    }
}
