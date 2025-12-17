using UnityEngine;

public class Bottle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.name != "Plane")
            GetComponent<Destructible>().DestructDestructible();
    }
}
