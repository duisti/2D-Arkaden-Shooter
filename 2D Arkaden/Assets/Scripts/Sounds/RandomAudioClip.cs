using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioClip : MonoBehaviour
{
    public List<AudioClip> AudioClips = new List<AudioClip>();
    // Start is called before the first frame update
    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (AudioClips.Count == 0)
        {
            return;
        }
        int randomInt = Random.Range(0, AudioClips.Count);
        source.clip = AudioClips[randomInt];
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
