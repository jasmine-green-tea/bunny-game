using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedSystem : MonoBehaviour
{
    [SerializeField] private const float needBarFillFullWidth = 3.2f;

    private Rabbit rabbit;
    private bool paused = false;

    [Header("Настройки голода")]
    public float maxHunger = 100f;
    public float currentHunger = 100f;
    public float hungerDecreaseRate = 1f; // На сколько уменьшается голод в секунду
    public float hungerDecreaseInterval = 0.3f; // Интервал уменьшения голода в секундах

    [Header("Настройки настроения")]
    public float maxMood = 100f;
    public float currentMood = 100f;
    public float moodDecreaseRate = 1f; // На сколько уменьшается настроения в секунду
    public float moodDecreaseInterval = 0.3f; // Интервал уменьшения настроения в секундах

    [Header("Настройки чистоты")]
    public float maxHygiene = 100f;
    public float currentHygiene = 100f;
    public float hygieneDecreaseRate = 1f; // На сколько уменьшается чистоты в секунду
    public float hygieneDecreaseInterval = 0.3f; // Интервал уменьшения чистоты в секундах
    public int hygieneIncreaseRate = 1;
    public int maxDirtPiles = 5;

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

    public RabbitController rabbitController;

    private List<DirtPile> dirtPiles;
    

    public void Pause(bool paused)
    {
        this.paused = paused;
    }

    void Start()
    {
        rabbit = GetComponent<Rabbit>();
        dirtPiles = new List<DirtPile>();
        currentHunger = maxHunger;
        currentMood = maxMood;
        UpdateUI();
    }

    void Update()
    {
        DecreaseHunger(hungerDecreaseRate * Time.deltaTime);
        DecreaseMood(moodDecreaseRate * Time.deltaTime);
        DecreaseHygiene(hygieneDecreaseRate * Time.deltaTime);
    }

    // Голод
    public void DecreaseHunger(float amount)
    {
        if (paused)
            return;
        currentHunger -= amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        CheckStarvation();
        UpdateUI();
    }

    public void IncreaseHunger(int amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        rabbitController.isEating = true;
        //rabbitController.pausedForInteraction = true;

        CheckStarvation();
        UpdateUI();
    }
    
    private void CheckStarvation()
    {
        if (currentHunger <= 20)
        {
            isStarving = true;
            string name = rabbit != null ? rabbit.GetName() : gameObject.name;
            //Debug.LogWarning($"{name} голодает! Накормите кролика!");
            // Добавить визуальные эффекты и логику поведения голодного кролика
        }
        else
            isStarving = false;
    }
    // Конец голода

    // Настроение
    public void DecreaseMood(float amount)
    {
        if (paused)
            return;
        currentMood -= amount;
        currentMood = Mathf.Clamp(currentMood, 0, maxMood);

        CheckMood();
        UpdateUI();
    }

    public void IncreaseMood(int amount)
    {
        currentMood += amount;
        currentMood = Mathf.Clamp(currentMood, 0, maxMood);


        CheckMood();
        UpdateUI();
    }

    private void CheckMood()
    {
        if (currentMood <= 20)
        {
            isSad = true;
            string name = rabbit != null ? rabbit.GetName() : gameObject.name;
            //Debug.LogWarning($"{name} грустит! Поиграйте с кроликом!");
        }
        else
            isSad = false;
    }
    // Конец настроения

    // Чистота
    public void DecreaseHygiene(float amount)
    {
        if (paused)
            return;

        float newAmount = amount * (dirtPiles.Count / (float)maxDirtPiles);
        if (newAmount > 0)
        {

            currentHygiene -= newAmount;
            currentHygiene = Mathf.Clamp(currentHygiene, 0, maxHygiene);

            CheckHygiene();
            UpdateUI();
        }
        else
            IncreaseHygiene(amount);
        
    }

    public void IncreaseHygiene(float amount)
    {
        currentHygiene += amount;
        currentHygiene = Mathf.Clamp(currentHygiene, 0, maxHygiene);

        CheckHygiene();
        UpdateUI();
    }

    private void CheckHygiene()
    {
        if (currentHygiene <= 20)
        {
            isDirty = true;
            string name = rabbit != null ? rabbit.GetName() : gameObject.name;
            //Debug.LogWarning($"{name} не любит быть в грязи! Прибиритесь!");
        }
        else
            isDirty = false;

    }
    // Конец чистоты

    public bool GetIsSad()
    {
        return isSad || isStarving || isDirty;
    }

    public void UpdateUI()
    {
        float hungerPercentage = currentHunger / maxHunger;
        hungerBarFill.size = new Vector2(needBarFillFullWidth * hungerPercentage, hungerBarFill.size.y);

        float moodPercentage = currentMood / maxMood;
        moodBarFill.size = new Vector2(needBarFillFullWidth * moodPercentage, moodBarFill.size.y);

        float hygienePercentage = (float)currentHygiene / maxHygiene;
        hygieneBarFill.size = new Vector2(needBarFillFullWidth * hygienePercentage, hygieneBarFill.size.y);

        // Голод
        if (hungerPercentage > 0.6f)
        {
            float currentRate = (hungerPercentage - mediumRate) / (fullRate - mediumRate);
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

    public void AddDirtPile(DirtPile other)
    {

        if (!dirtPiles.Contains(other))
            dirtPiles.Add(other);


    }

    public void RemoveDirtPile(DirtPile other)
    {


        if (dirtPiles.Contains(other))
            dirtPiles.Remove(other);


    }
}
