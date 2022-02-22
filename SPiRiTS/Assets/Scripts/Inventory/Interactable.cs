using UnityEngine;

// Written by: Randen Banuelos
// Based on Brackeys' Inventory implementation in his Unity RPG series

/// <summary>
/// Base class for any interactible objects in the game (e.g. an ItemPickup)
/// </summary>
public class Interactable : MonoBehaviour
{
    /// <summary>
    /// The range at which the interactable detects players
    /// </summary>
    [SerializeField] private float radius = 3f;

    /// <summary>
    /// Stores what is considered a "player"
    /// </summary>
    [SerializeField] private LayerMask playerLayer;

    /// <summary>
    /// Stores the location of the interactable
    /// </summary>
    [SerializeField] private Transform interactionTransform;


    /// <summary>
    /// Bool value of whether or not the interactable has been interacted with yet
    /// </summary>
    private bool hasInteracted = false;


    public virtual void Interact(Transform player)
    {
        // This function is meant to be overwritten in subclasses
    }

    /// <summary>
    /// Handles interaction with players
    /// </summary>
    private void Update()
    {
        if (!hasInteracted)
        {
            // Search for players within radius
            Collider[] nearbyPlayers = Physics.OverlapSphere(interactionTransform.position, radius, playerLayer);
            // If a player comes within radius:
            if (nearbyPlayers.Length > 0)
            {
                // Disable interaction
                hasInteracted = true;
                // Interact and pass in the first player to come within range
                Interact(nearbyPlayers[0].transform);
            }
        }
    }

    /// <summary>
    /// Draws out hitboxes for interaction
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
