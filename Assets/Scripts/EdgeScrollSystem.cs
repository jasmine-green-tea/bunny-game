using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeScrollSystem : PausableObject
{
    Vector3[] corners = new Vector3[4];

    public RectTransform area;
    public Transform cameraTransform;
    public int edgeScrollSize;
    public float cameraSpeed = 2f;

    public float xConstraint = 4.2f;
    public float yConstraint = 11f;


    float minX;
    float maxX;
    float minY;
    float maxY;

    float cameraHeight;
    float cameraWidth;

    private void Start()
    {
        area.GetWorldCorners(corners);

        minX = corners[0].x;
        maxX = corners[2].x;
        minY = corners[0].y;
        maxY = corners[2].y;

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;

        if (PauseManager.Instance != null)
            PauseManager.Instance.RegisterPausable(this);
    }

    protected override void UpdatePausable(float deltaTime)
    {

        Vector2 inputDir = Vector2.zero;

        int edgeScrollSize = 20;


        if (Input.mousePosition.x < edgeScrollSize) inputDir.x = -1f;
        if (Input.mousePosition.y < edgeScrollSize) inputDir.y = -1f;
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) inputDir.x = 1f;
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) inputDir.y = 1f;

        //if ((cameraTransform.position.x + cameraWidth / 2 + edgeScrollSize > maxX) || (cameraTransform.position.x - cameraWidth/2 - edgeScrollSize < minX))
        //{
        //    Debug.Log("X");

        //    inputDir.x = 0;
        //}

        //if ((cameraTransform.position.y + cameraHeight / 2 + edgeScrollSize > maxY) || (cameraTransform.position.y - cameraHeight / 2 - edgeScrollSize < minY))
        //{
        //    Debug.Log("Y");

        //    inputDir.y = 0;
        //}

        

        cameraTransform.Translate(inputDir * deltaTime * cameraSpeed);
        float newX = cameraTransform.position.x;
        float newY = cameraTransform.position.y;
        if (cameraTransform.position.x > xConstraint)
            newX = xConstraint;
        if (cameraTransform.position.x < -xConstraint)
            newX = -xConstraint;
        if (cameraTransform.position.y > yConstraint)
            newY = yConstraint;
        if (cameraTransform.position.y < -yConstraint)
            newY = -yConstraint;
        cameraTransform.position = new Vector2(newX, newY);

    }
}
