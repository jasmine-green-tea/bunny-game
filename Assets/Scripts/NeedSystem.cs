using UnityEngine;
using UnityEngine.UI;

public class NeedSystem : MonoBehaviour
{
    [SerializeField] private const float needBarFillFullWidth = 3.2f;





    private Rabbit rabbit;

    [Header("��������� ������")]
    public int maxHunger = 100;
    public int currentHunger = 100;
    public int hungerDecreaseRate = 1; // �� ������� ����������� ����� � �������
    public float hungerDecreaseInterval = 0.3f; // �������� ���������� ������ � ��������

    [Header("��������� ����������")]
    public int maxMood = 100;
    public int currentMood = 100;
    public int moodDecreaseRate = 1; // �� ������� ����������� ���������� � �������
    public float moodDecreaseInterval = 0.3f; // �������� ���������� ���������� � ��������

    [Header("UI ��������")]
    public Color fullColor = Color.green;
    public Color mediumColor = Color.yellow;
    public Color lowColor = Color.red;
    public Color zeroColor = Color.black;
    public float fullRate = 1.0f;
    public float mediumRate = 0.6f;
    public float lowRate = 0.3f;
    public float zeroRate = 0.0f;
    public SpriteRenderer hungerBarFill;
    public SpriteRenderer moodBarFill;


    [Header("������ �������")]
    public bool isStarving = false;
    public bool isSad = false;
    private float hungerTimer;
    private float moodTimer;

    void Start()
    {
        rabbit = GetComponent<Rabbit>();

        currentHunger = maxHunger;
        currentMood = maxMood;
        UpdateUI();
    }

    void Update()
    {
        hungerTimer += Time.deltaTime;
        if (hungerTimer >= hungerDecreaseInterval)
        {
            DecreaseHunger(hungerDecreaseRate);
            hungerTimer = 0f;
        }

        moodTimer += Time.deltaTime;
        if (moodTimer >= moodDecreaseInterval)
        {
            DecreaseMood(moodDecreaseRate);
            moodTimer = 0f;
        }
    }

    // �����
    public void DecreaseHunger(int amount)
    {
        currentHunger -= amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        CheckStarvation();
        UpdateUI();
    }

    public void IncreaseHunger(int amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        if (currentHunger > 20)
            isStarving = false;

        CheckStarvation();
        UpdateUI();
    }
    
    private void CheckStarvation()
    {
        if (currentHunger <= 10)
        {
            isStarving = true;
            string name = rabbit != null ? rabbit.GetName() : gameObject.name;
            Debug.LogWarning($"{name} ��������! ��������� �������!");
            // �������� ���������� ������� � ������ ��������� ��������� �������
        }
    }
    // ����� ������

    // ����������
    public void DecreaseMood(int amount)
    {
        currentMood -= amount;
        currentMood = Mathf.Clamp(currentMood, 0, maxMood);

        CheckMood();
        UpdateUI();
    }

    public void IncreaseMood(int amount)
    {
        currentMood += amount;
        currentMood = Mathf.Clamp(currentMood, 0, maxMood);

        if (currentMood > 20)
            isSad = false;

        CheckMood();
        UpdateUI();
    }

    private void CheckMood()
    {
        if (currentMood <= 10)
        {
            isSad = true;
            string name = rabbit != null ? rabbit.GetName() : gameObject.name;
            Debug.LogWarning($"{name} �������! ��������� � ��������!");
        }
    }
    // ����� ����������

    // �������
    // ����� �������

    private void UpdateUI()
    {
        float hungerPercentage = (float)currentHunger / maxHunger;
        hungerBarFill.size = new Vector2(needBarFillFullWidth * hungerPercentage, hungerBarFill.size.y);

        float moodPercentage = (float)currentMood / maxMood;
        moodBarFill.size = new Vector2(needBarFillFullWidth * moodPercentage, moodBarFill.size.y);



        // hunger

        if (hungerPercentage > 0.6f)
        {
            float currentRate = (hungerPercentage - mediumRate) / (fullRate - mediumRate);
            Debug.Log(currentRate);
            hungerBarFill.color = mediumColor * (1 - currentRate) + fullColor * (currentRate);
        }
        else if (hungerPercentage > 0.3f)
        {
            float currentRate = (hungerPercentage - lowRate) / (mediumRate - lowRate);

            hungerBarFill.color = lowColor * (1 - currentRate) + mediumColor * (currentRate);
        }
        else
        {
            float currentRate = (hungerPercentage - zeroRate) / (lowRate - zeroRate);

            hungerBarFill.color = zeroColor * (1 - currentRate) + lowColor * (currentRate);
        }


        // mood
        if (moodPercentage > 0.6f)
        {
            float currentRate = (moodPercentage - mediumRate) / (fullRate - mediumRate);

            moodBarFill.color = mediumColor * (1 - currentRate) + fullColor * (currentRate);
        }
        else if (moodPercentage > 0.3f)
        {
            float currentRate = (moodPercentage - lowRate) / (mediumRate - lowRate);

            moodBarFill.color = lowColor * (1 - currentRate) + mediumColor * (currentRate);
        }
        else
        {
            float currentRate = (moodPercentage - zeroRate) / (lowRate - zeroRate);

            moodBarFill.color = zeroColor * (1 - currentRate) + lowColor * (currentRate);
        }
    }
}
