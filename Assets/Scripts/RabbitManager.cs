using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : MonoBehaviour
{
    public static RabbitManager Instance;

    [Header("���������� ���������")]
    public List<Rabbit> allRabbits = new List<Rabbit>();
    public Rabbit currentlySelectedRabbit;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ����������� ������ �������
    public void RegisterRabbit(Rabbit rabbit)
    {
        if (!allRabbits.Contains(rabbit))
            allRabbits.Add(rabbit);
    }

    // �������� ������� �� ������
    public void UnregisterRabbit(Rabbit rabbit)
    {
        if (allRabbits.Contains(rabbit))
            allRabbits.Remove(rabbit);
    }

    // ����� ��������� � ���� �������� ����� ����������
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
