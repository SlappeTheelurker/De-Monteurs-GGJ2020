using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RobotOutputUIController : MonoBehaviour
{
    //editor variables
    [SerializeField] private Animator thisAnimator;
    [SerializeField] private TextMeshProUGUI ErrorTextBox;

    //function variables
    public void Trigger(string ErrorMessage)
    {
        ErrorTextBox.SetText(ErrorMessage);
        thisAnimator.SetTrigger("Print");
        printerSFX();
        
    }

    public void printerSFX(){
        AudioSource audio;
        audio = GetComponent<AudioSource>();
        audio.clip = Resources.Load("print2") as AudioClip;
        audio.Play();
    }
}
