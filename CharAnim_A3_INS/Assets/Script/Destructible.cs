using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVersion;
    public AudioClip sfx;

    private bool _canBeDestroyed = true;


    public void DestructDestructible()
    {
        if (!_canBeDestroyed) return;
        _canBeDestroyed = false;

        Instantiate(destroyedVersion, transform.position, transform.rotation).AddComponent<AudioSource>().PlayOneShot(sfx);
        Destroy(gameObject);
    }
}
