using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSpawner : PausableObject
{
    public GameObject dirtPrefab;
    public RectTransform spawnArea;
    public float spawnPeriodSeconds;

    public Collider2D[] colliders;

    private float _currentTimer;

    // Start is called before the first frame update
    private void Start()
    {
        _currentTimer = 0f;

        if (PauseManager.Instance != null)
            PauseManager.Instance.RegisterPausable(this);
    }

    // Update is called once per frame
    protected override void UpdatePausable(float deltaTime)
    {
        if (IsPaused) return;

        _currentTimer += deltaTime;

        if (_currentTimer >= spawnPeriodSeconds)
        {
            _currentTimer = 0f;
            Vector2 randomPosition = GetRandomPositionInRect();
            Instantiate(dirtPrefab, randomPosition, Quaternion.identity, transform);
        }
    }

    private Vector2 GetRandomPositionInRect()
    {
        Vector3[] corners = new Vector3[4];
        spawnArea.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        Vector2 returnVector = Vector2.zero;

        while (true)
        {
            returnVector.x = Random.Range(minX, maxX);
            returnVector.y = Random.Range(minY, maxY);


            Collider2D overlap = Physics2D.OverlapPoint(returnVector);

            if (overlap == null)
            {
                return returnVector;
            }
        }
    }

    private void OnDestroy()
    {
        if (PauseManager.Instance != null)
            PauseManager.Instance.UnregisterPausable(this);
    }
}
