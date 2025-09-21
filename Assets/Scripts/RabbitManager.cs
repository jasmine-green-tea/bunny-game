using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : MonoBehaviour
{
    public static RabbitManager Instance;

    [Header("Управление кроликами")]
    public List<Rabbit> allRabbits = new List<Rabbit>();
    public Rabbit currentlySelectedRabbit;

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
