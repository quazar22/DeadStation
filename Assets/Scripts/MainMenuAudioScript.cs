using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioScript : MonoBehaviour
{
    public AudioClip clip;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source.clip = clip;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
