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

    public void GenerateNewRabbit()
    {

        GameObject rabbit = Instantiate(rabbitPrefab, transform);
        rabbit.SetActive(true);
       
        Rabbit newRabbit = rabbit.GetComponent<Rabbit>();

        // Random generation of rabbit stats
        string newName = "Кролик " + UnityEngine.Random.Range(0, 100).ToString();
        newRabbit.SetName(newName);
        rabbit.name = newName;
        
        newRabbit.SetNeedSystemMultipliers(UnityEngine.Random.Range(0.5f, 3f), UnityEngine.Random.Range(0.5f, 3f), UnityEngine.Random.Range(0.5f, 3f));
        newRabbit.SetPausedStatus(false);

        RegisterRabbit(newRabbit);

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
