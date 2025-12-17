using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Destructible des))
        {
            des.DestructDestructible();
        }
    }
}
