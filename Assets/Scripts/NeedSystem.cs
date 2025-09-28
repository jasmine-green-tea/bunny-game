using UnityEngine;
using UnityEngine.UI;

public class NeedSystem : MonoBehaviour
{
    [SerializeField] private const float needBarFillFullWidth = 3.2f;

    private Rabbit rabbit;

    [Header("Настройки голода")]
    public int maxHunger = 100;
    public int currentHunger = 100;
    public int hungerDecreaseRate = 1; // На сколько уменьшается голод в секунду
    public float hungerDecreaseInterval = 0.3f; // Интервал уменьшения голода в секундах

    [Header("Настройки настроения")]
    public int maxMood = 100;
    public int currentMood = 100;
    public int moodDecreaseRate = 1; // На сколько уменьшается настроения в секунду
    public float moodDecreaseInterval = 0.3f; // Интервал уменьшения настроения в секундах

    [Header("Настройки чистоты")]
    public int maxHygiene = 100;
    public int currentHygiene = 100;
    public int hygieneDecreaseRate = 1; // На сколько уменьшается чистоты в секунду
    public float hygieneDecreaseInterval = 0.3f; // Интервал уменьшения чистоты в секундах

    [Header("UI элементы")]
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
    public SpriteRenderer hygieneBarFill;


    [Header("Статус кролика")]
    public bool isStarving = false;
    public bool isSad = false;
    public bool isDirty = false;
    private float hungerTimer;
    private float moodTimer;
    private float hygieneTimer;

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

        hygieneTimer += Time.deltaTime;
        if (hygieneTimer >= hygieneDecreaseInterval)
        {
            DecreaseHygiene(hygieneDecreaseRate);
            hygieneTimer = 0f;
        }
    }

    // Голод
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
            Debug.LogWarning($"{name} голодает! Накормите кролика!");
            // Добавить визуальные эффекты и логику поведения голодного кролика
        }
    }
    // Конец голода

    // Настроение
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
            Debug.LogWarning($"{name} грустит! Поиграйте с кроликом!");
        }
    }
    // Конец настроения

    // Чистота
    public void DecreaseHygiene(int amount)
    {
        currentHygiene -= amount;
        currentHygiene = Mathf.Clamp(currentHygiene, 0, maxHygiene);

        CheckHygiene();
        UpdateUI();
    }

    public void IncreaseHygiene(int amount)
    {
        currentHygiene += amount;
        currentHygiene = Mathf.Clamp(currentHygiene, 0, maxHygiene);

        if (currentHygiene > 20)
            isDirty = false;

        CheckHygiene();
        UpdateUI();
    }

    private void CheckHygiene()
    {
        if (currentHygiene <= 10)
        {
            isSad = true;
            string name = rabbit != null ? rabbit.GetName() : gameObject.name;
            Debug.LogWarning($"{name} не любит быть в грязи! Прибиритесь!");
        }
    }
    // Конец чистоты

    private void UpdateUI()
    {
        float hungerPercentage = (float)currentHunger / maxHunger;
        hungerBarFill.size = new Vector2(needBarFillFullWidth * hungerPercentage, hungerBarFill.size.y);

        float moodPercentage = (float)currentMood / maxMood;
        moodBarFill.size = new Vector2(needBarFillFullWidth * moodPercentage, moodBarFill.size.y);

        float hygienePercentage = (float)currentHygiene / maxHygiene;
        hygieneBarFill.size = new Vector2(needBarFillFullWidth * hygienePercentage, hygieneBarFill.size.y);

        // Голод
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


        // Настроение
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

        // Чистота
        if (hygienePercentage > 0.6f)
        {
            float currentRate = (hygienePercentage - mediumRate) / (fullRate - mediumRate);

            hygieneBarFill.color = mediumColor * (1 - currentRate) + fullColor * (currentRate);
        }
        else if (hygienePercentage > 0.3f)
        {
            float currentRate = (hygienePercentage - lowRate) / (mediumRate - lowRate);

            hygieneBarFill.color = lowColor * (1 - currentRate) + mediumColor * (currentRate);
        }
        else
        {
            float currentRate = (hygienePercentage - zeroRate) / (lowRate - zeroRate);

            hygieneBarFill.color = zeroColor * (1 - currentRate) + lowColor * (currentRate);
        }
    }
}
