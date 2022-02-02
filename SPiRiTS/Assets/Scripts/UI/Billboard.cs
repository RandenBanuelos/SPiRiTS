using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by: Randen Banuelos

/// <summary>
/// Keeps World Space UI facing towards an inputted Camera
/// </summary>
public class Billboard : MonoBehaviour
{
    // VARIABLES
    /// <summary>
    /// The default camera that the UI will always be facing towards
    /// </summary>
    [SerializeField] private Transform cam;


    // FUNCTIONS
    /// <summary>
    /// Rotate the UI towards the Camera after all other Updates have completed
    /// </summary>
    private void LateUpdate()
    {
        if (cam != null)
            transform.LookAt(transform.position + cam.forward);
    }

    /// <summary>
    /// Set the targeted Camera to a different Camera than the default
    /// </summary>
    /// <param name="mainCam">Camera now being targeted</param>
    public void SetCamera(Transform mainCam)
    {
        cam = mainCam;
    }
}
