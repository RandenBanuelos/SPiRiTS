using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos
// Based on Brackeys' implementation in his Multiple Target Camera tutorial

/// <summary>
/// Keeps all players in frame as best as possible
/// </summary>
[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{
    //VARIABLES
    /// <summary>
    /// Stores the list of all targets in the game
    /// </summary>
    [SerializeField] private List<Transform> targets;

    /// <summary>
    /// The offset for the camera's position away from the center of all targets
    /// </summary>
    [SerializeField] private Vector3 offset;

    /// <summary>
    /// The rate at which the camera will move at
    /// </summary>
    [SerializeField] private float smoothTime = 0.5f;


    /// <summary>
    /// The minimum zoom for the Math.Lerp calculation for the camera zoom level
    /// </summary>
    [SerializeField] private float minZoom = 40f;

    /// <summary>
    /// The maximum zoom for the Math.Lerp calculation for the camera zoom level
    /// </summary>
    [SerializeField] private float maxZoom = 10f;

    /// <summary>
    /// Divisor for the the interpolation value for the Math.Lerp calculation for the camera zoom level; Divides the greatest distance between all targets
    /// </summary>
    [SerializeField] private float zoomLimiter = 50f;


    // REFERENCES
    /// <summary>
    /// The max speed the camera will move at
    /// </summary>
    private Vector3 velocity;

    /// <summary>
    /// References the game's camera
    /// </summary>
    private Camera cam;


    // FUNCTIONS
    /// <summary>
    /// Gets the camera component from camera GameObject
    /// </summary>
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    /// <summary>
    /// If there aren't any targets, skip
    /// Else, call move and zoom functions
    /// </summary>
    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();
    }

    /// <summary>
    /// Adds a new camera target to the list of active targets
    /// </summary>
    public void AddTarget(Transform newTarget)
    {
        targets.Add(newTarget);
    }

    /// <summary>
    /// Changes the camera's zoom level 
    /// </summary>
    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    /// <summary>
    /// Moves the camera location
    /// </summary>
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    /// <summary>
    /// Returns the center location of all targets in the game
    /// </summary>
    private Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    /// <summary>
    /// Finds the greatest distance value between all targets
    /// </summary>
    private float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }
}
