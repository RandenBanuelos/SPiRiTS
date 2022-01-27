using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform interactionTransform;

    private bool hasInteracted = false;


    public virtual void Interact(Transform player)
    {
        // This function is meant to be overwritten in subclasses
    }


    private void Update()
    {
        if (!hasInteracted)
        {
            Collider[] nearbyPlayers = Physics.OverlapSphere(interactionTransform.position, radius, playerLayer);
            if (nearbyPlayers.Length > 0)
            {
                hasInteracted = true;
                Interact(nearbyPlayers[0].transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
