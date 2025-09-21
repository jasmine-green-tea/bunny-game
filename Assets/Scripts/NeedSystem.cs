using UnityEngine;
using UnityEngine.UI;

public class NeedSystem : MonoBehaviour
{
    private Rabbit rabbit;

    [Header("Настройки голода")]
    public int maxHunger = 100;
    public int currentHunger = 100;
    public int hungerDecreaseRate = 1; // На сколько уменьшается голод в секунду
    public float hungerDecreaseInterval = 0.3f; // Интервал уменьшения голода в секундах

    [Header("UI элементы")]
    public Slider hungerSlider;
    public Image hungerFillImage;
    public Color fullColor = Color.green;
    public Color hungryColor = Color.yellow;
    public Color starvingColor = Color.red;
    public Transform sliderValue;
    public SpriteRenderer level;

    [Header("Статус кролика")]
    public bool isStarving = false;
    private float timer;

    void Start()
    {
        rabbit = GetComponent<Rabbit>();

        currentHunger = maxHunger;
        UpdateUI();

        if (hungerSlider == null)
            hungerSlider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= hungerDecreaseInterval)
        {
            DecreaseHunger(hungerDecreaseRate);
            timer = 0f;
        }
    }

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

    private void UpdateUI()
    {
        if (hungerSlider != null)
        {
            hungerSlider.maxValue = maxHunger;
            hungerSlider.value = currentHunger;
        }

        float hungerPercentage = (float)currentHunger / maxHunger;
        sliderValue.localScale = new Vector3(hungerPercentage, sliderValue.localScale.y, sliderValue.localScale.z);


        if (hungerPercentage > 0.6f)
            level.color = fullColor;
        else if (hungerPercentage > 0.3f)
            level.color = hungryColor;
        else
            level.color = starvingColor;

        // Изменяем цвет в зависимости от уровня голода
    //    if (hungerFillImage != null)
    //    {
    //        float hungerPercentage = (float)currentHunger / maxHunger;

    //        if (hungerPercentage > 0.6f)
    //            hungerFillImage.color = fullColor;
    //        else if (hungerPercentage > 0.3f)
    //            hungerFillImage.color = hungryColor;
    //        else
    //            hungerFillImage.color = starvingColor;
    //    }
    }

    public float GetHungerPercentage()
    {
        return (float)currentHunger / maxHunger;
    }
}
