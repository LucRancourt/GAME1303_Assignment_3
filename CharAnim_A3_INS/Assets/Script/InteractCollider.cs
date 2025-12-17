using UnityEngine;

public class InteractCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Activatable button))
        {
            button.Activate();
        }
    }
}
