using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    // answers.unity.com/questions/1432377/scroll-rect-autoscroll-content.html
    // Al-Maliki

    [SerializeField, Range(0f, 5f)] private float scrollSpeed = .2f;
    [SerializeField] private float minScroll;
    [SerializeField] private float maxScroll;
    private RectTransform contentRectTransform;
    private bool isMovingDown = true;


    private void Start()
    {
        contentRectTransform = GetComponent<ScrollRect>().content;
    }


    private void Update()
    {
        bool atBottom = contentRectTransform.position.y > maxScroll;
        bool atTop = contentRectTransform.position.y < minScroll;

        if (atBottom)
            isMovingDown = false;
        if (atTop)
            isMovingDown = true;

        Scroll();
    }


    private void Scroll()
    {
        Vector3 contentPosition = contentRectTransform.position;
        float newPositionY = 0f;

        if (isMovingDown)
        {
            newPositionY = contentPosition.y + scrollSpeed;
        }
        else
        {
            newPositionY = contentPosition.y - scrollSpeed;
        }

        Vector3 newPosition = new Vector3(contentPosition.x, newPositionY, contentPosition.z);
        contentRectTransform.position = newPosition;
    }
}
