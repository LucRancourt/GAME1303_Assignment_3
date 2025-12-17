using UnityEngine;

public class Activatable : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    public float duration;
    float elapsedTime;

    private bool _activated = false;
    

    public void Activate()
    {
        if (_activated) return;

        GetComponent<AudioSource>().Play();
        _activated = true;
    }

    private void Update()
    {
        if (!_activated) return;

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration;
        t = Mathf.Clamp01(t);

        transform.position = Vector3.Lerp(startPos.position, endPos.position, t);
    }
}
