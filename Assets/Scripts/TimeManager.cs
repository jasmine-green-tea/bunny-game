using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private int currentMonth = 1;
    private int currentDay = 1;

    [SerializeField]
    private float dayTimeSeconds = 100f;

    private float daySecondsCounter = 0f;

    private string currentDayStr;


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
        currentDayStr = GetMonthName() + " " + currentDay.ToString();
    }

    private void Update()
    {
        daySecondsCounter += Time.deltaTime;

        if (daySecondsCounter >= dayTimeSeconds)
        {
            // increase day, switch to new day, show progress
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


            daySecondsCounter = 0f;
        }
    }
}
