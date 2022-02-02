using UnityEngine;
using UnityEngine.UI;

// Originally written by Al-Maliki from this Unity forums post:
// answers.unity.com/questions/1432377/scroll-rect-autoscroll-content.html

// Any modifications done by Randen Banuelos

/// <summary>
/// Automatically scrolls the credits bar up and down in the Credits submenu
/// </summary>
public class AutoScroll : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// Adjust the scrolling speed of the text on a slider
    /// </summary>
    [SerializeField, Range(0f, 5f)] private float scrollSpeed = .2f;

    /// <summary>
    /// Stopping point at the top of the credits
    /// </summary>
    [SerializeField] private float minScroll;

    /// <summary>
    /// Stopping point at the bottom of the credits
    /// </summary>
    [SerializeField] private float maxScroll;


    // REFERENCES
    /// <summary>
    /// Cache for the text field's ScrollRect component
    /// </summary>
    private RectTransform contentRectTransform;

    /// <summary>
    /// Used for switching the direction of the scroll
    /// </summary>
    private bool isMovingDown = true;


    // FUNCTIONS
    private void Start()
    {
        // Cache ScrollRect
        contentRectTransform = GetComponent<ScrollRect>().content;
    }

    private void Update()
    {
        // Check if the scrolling has hit either boundary
        bool atBottom = contentRectTransform.position.y > maxScroll;
        bool atTop = contentRectTransform.position.y < minScroll;

        // Switch scrolling direction if a boundary has been hit
        if (atBottom)
            isMovingDown = false;
        if (atTop)
            isMovingDown = true;

        Scroll();
    }

    /// <summary>
    /// Scroll the credits up/down with a decrement/increment of scrollSpeed
    /// </summary>
    private void Scroll()
    {
        // TODO: See if there's a way to "loop" the credits smoothly instead of just bouncing up and down
        Vector3 contentPosition = contentRectTransform.position;
        float newPositionY = 0f;

        // Either increase or decrease the credits text's position depending on direction
        if (isMovingDown)
        {
            newPositionY = contentPosition.y + scrollSpeed;
        }
        else
        {
            newPositionY = contentPosition.y - scrollSpeed;
        }

        // Set the new value for the text's position
        Vector3 newPosition = new Vector3(contentPosition.x, newPositionY, contentPosition.z);
        contentRectTransform.position = newPosition;
    }
}
