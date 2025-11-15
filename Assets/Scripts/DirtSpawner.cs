using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSpawner : MonoBehaviour
{
    public GameObject dirtPrefab;
    public RectTransform spawnArea;
    public float spawnPeriodSeconds;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCoroutine()
    {

        while (true)
        {
            yield return new WaitForSeconds(spawnPeriodSeconds);
            Vector2 randomPosition = GetRandomPositionInRect();
            Instantiate(dirtPrefab, randomPosition, Quaternion.identity, transform);
        }

        yield return null;
    }

    private Vector2 GetRandomPositionInRect()
    {
        Vector3[] corners = new Vector3[4];
        spawnArea.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
}
