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
    [SerializeField] private List<Transform> targets;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.5f;

    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomLimiter = 50f;


    // REFERENCES
    private Vector3 velocity;
    private Camera cam;


    // FUNCTIONS

    private void Start()
    {
        cam = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;

        Move();
        Zoom();
    }


    public void AddTarget(Transform newTarget)
    {
        targets.Add(newTarget);
    }


    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }


    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }


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
