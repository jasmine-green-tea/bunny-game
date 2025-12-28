using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using UnityEngine.UI;

public class TimeManager : PausableObject
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
    private TMP_Text dateText;

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {

        currentDayStr = "Äåíü " + currentDay.ToString();

        if (PauseManager.Instance != null)
            PauseManager.Instance.RegisterPausable(this);



            ReleaseDay();

    }

    protected override void UpdatePausable(float deltaTime)
    {

    }

    public void ReleaseDay()
    {
        dateText.text = currentDayStr;
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
        RabbitManager.Instance.DecreaseDays();
        countdownText.gameObject.SetActive(true);

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

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
                // Update time display
                timeText.text = (dayHourStart + i).ToString() + ":" + (j > 9 ? j.ToString() : "0" + j.ToString());

                // Wait for one second, respecting pause state
                float timer = 0f;
                while (timer < 1f)
                {
                    if (!IsPaused)
                    {
                        timer += Time.deltaTime;
                    }
                    yield return null;
                }

                // Increment counter if not paused
                if (!IsPaused)
                {
                    counter++;

                    // Check for rabbit spawn timestamps
                    if (currentTimestampIndex != -1)
                    {
                        string countdownString = "Ñëåäóþùèé êðîëèê ÷åðåç: " + (timestamps[currentTimestampIndex] - counter).ToString();
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
        }

        // End of day - set a flag to all bunnies to prevent them from losing stats
        RabbitManager.Instance.SetRabbitsPause(true);

        ui.SetActive(true);
        bellButton.interactable = false;

        // Advance to next day

        currentDay++;

        currentDayStr = "Äåíü " + currentDay.ToString();
    }

    protected override void OnPaused()
    {

        // Optional: Update UI to show pause state
        // You could change the time display color or add "PAUSED" text
        if (countdownText.gameObject.activeSelf)
        {
            countdownText.text += " [Ïàóçà]";
        }
    }

    protected override void OnResumed()
    {

        // Optional: Remove pause indicator from UI
        if (countdownText.gameObject.activeSelf && countdownText.text.Contains("[Ïàóçà]"))
        {
            string currentText = countdownText.text;
            countdownText.text = currentText.Replace(" [Ïàóçà]", "");
        }
    }

    private string GetMonthName()
    {
        switch (currentMonth)
        {
            case 1: return "ßÍÂÀÐß";
            case 2: return "ÔÅÂÐÀËß";
            case 3: return "ÌÀÐÒÀ";
            case 4: return "ÀÏÐÅËß";
            case 5: return "ÌÀß";
            case 6: return "ÈÞÍß";
            case 7: return "ÈÞËß";
            case 8: return "ÀÂÃÓÑÒÀ";
            case 9: return "ÑÅÍÒßÁÐß";
            case 10: return "ÎÊÒßÁÐß";
            case 11: return "ÍÎßÁÐß";
            case 12: return "ÄÅÊÀÁÐß";
        }
        return "NaM";
    }

    private int GetMaxMonthDays()
    {
        switch (currentMonth)
        {
            case 1: return 31;
            case 2: return 28;
            case 3: return 31;
            case 4: return 30;
            case 5: return 31;
            case 6: return 30;
            case 7: return 31;
            case 8: return 31;
            case 9: return 30;
            case 10: return 31;
            case 11: return 30;
            case 12: return 31;
        }
        return -1;
    }

    // Clean up coroutine
    private void OnDestroy()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.UnregisterPausable(this);
        }
    }
}
