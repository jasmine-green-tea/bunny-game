using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RabbitManager : MonoBehaviour
{
    public static RabbitManager Instance;

    [SerializeField]
    private GameObject rabbitPrefab;

    [Header("Управление кроликами")]
    public List<Rabbit> allRabbits = new List<Rabbit>();
    public Rabbit currentlySelectedRabbit;

    public int maxRabbits = 10;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    

    // регистрация нового кролика
    public void RegisterRabbit(Rabbit rabbit)
    {
        if (!allRabbits.Contains(rabbit))
            allRabbits.Add(rabbit);
    }

    // Удаления кролика из списка
    public void UnregisterRabbit(Rabbit rabbit)
    {
        if (allRabbits.Contains(rabbit))
            allRabbits.Remove(rabbit);
    }

    // Сброс выделения у всех кроликов кроме указанного
    public void DeselectAllRabbitsExcept(Rabbit exception = null)
    {
        foreach (Rabbit rabbit in allRabbits)
        {
            if (rabbit != exception)
                rabbit.Deselect();
        }

        currentlySelectedRabbit = exception;
    }

    public void SetRabbitsPause(bool paused)
    {
        foreach (Rabbit rabbit in allRabbits)
        {
            rabbit.SetPausedStatus(paused);
        }
    }

    public void GenerateNewRabbit(RabbitStats rabbitStats)
    {

        GameObject rabbit = Instantiate(rabbitPrefab, Vector2.zero, Quaternion.identity, transform);
        rabbit.SetActive(true);
       
        Rabbit newRabbit = rabbit.GetComponent<Rabbit>();

        newRabbit.rabbitStats = rabbitStats;
        newRabbit.SetControllerColor();

        // Random generation of rabbit stats
        newRabbit.SetName(rabbitStats.bunnyName);
        rabbit.name = rabbitStats.bunnyName;
        
        

        newRabbit.SetNeedSystemMultipliers(rabbitStats.hungerRate, rabbitStats.moodRate, rabbitStats.hygieneRate);
        //newRabbit.SetNeedSystemMultipliers(UnityEngine.Random.Range(0.5f, 3f), UnityEngine.Random.Range(0.5f, 3f), UnityEngine.Random.Range(0.5f, 3f));
        newRabbit.SetPausedStatus(false);

        RegisterRabbit(newRabbit);

    }

    void Start()
    {
        //GenerateNewRabbit();
        //GenerateNewRabbit();
    }

    void Update()
    {
        
    }
}
