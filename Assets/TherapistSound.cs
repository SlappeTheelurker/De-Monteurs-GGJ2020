using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TherapistSound : MonoBehaviour
{
    void Start()
    {
        Invoke("hmmmSFX", 2f);
    }
 
    void hmmmSFX()
    {
        float randomTime = Random.Range(5, 10 );
        int randnum = Random.Range(1,13);

        
        AudioSource audio;
        audio = GetComponent<AudioSource>();
        try{
            audio.clip = Resources.Load("hmm" + randnum) as AudioClip;
        } catch{

        }
        audio.Play();
        Invoke("hmmmSFX", randomTime);
 
    }
}
