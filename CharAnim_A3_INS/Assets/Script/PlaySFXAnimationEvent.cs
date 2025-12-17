using UnityEngine;

public class PlaySFXAnimationEvent : MonoBehaviour
{
    public AudioList[] clipDictionary;

    public AudioSource audioSource;


    public void PlaySFX(string name)
    {
        if (name != "Attack" && name != "Jump")
            if (audioSource.isPlaying) return;

        
        foreach (AudioList list in clipDictionary)
        {
            if (list.IsAMatch(name))
            {
                if (list.Clips.Length == 0)
                {
                    Debug.LogError("No Clip exists!");
                    return;
                }

                AudioClip clip = list.Clips[Random.Range(0, list.Clips.Length - 1)];

                audioSource.clip = clip;

                audioSource.Play();

                return;
            }
        }
    }
}

[System.Serializable]
public class AudioList
{
    public string Name;
    public AudioClip[] Clips;

    public bool IsAMatch(string name) { return name == Name; }
}