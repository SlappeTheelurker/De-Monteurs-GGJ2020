using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipSocket : MonoBehaviour
{
    public Chip socketedChip;
    public Transform snapPoint;
    public int chipOrientation; // -1 = none
  
    private float tolerance = 30f;

	[Header("Correct data")]
    public bool needChip;
    public Chip.ChipFormats format;
    public string type;
    public int correctOrientation; // 0-3 w/ 0=360
	
    #region audioshit
    private AudioSource audioSource;

	private AudioClip clickClip;
	

    #endregion 

    private void Start(){
        audioSource = gameObject.AddComponent<AudioSource>();;
    }

	private void OnTriggerEnter(Collider other)
    {
        Chip chip = other.GetComponent<Chip>();
        if (chip != null && socketedChip == null)
        {

			if (chip.claw != null)
            {
                if (!chip.claw.getUngrabAllowed()) { return; }
            }

            chip.claw?.Ungrab();
			
            //sfx
            clickClip = Resources.Load("click1") as AudioClip;
            Debug.Log(clickClip);
			audioSource.clip = clickClip;
			audioSource.Play();


			float rot = other.transform.rotation.eulerAngles.y;
            bool snap = false;
            if (Mathf.Abs(rot % 90) <= tolerance)
            {
                rot = Mathf.RoundToInt(rot / 90) * 90;
                snap = true;
            }

            if (Mathf.Abs(rot % 90) >= 90 - tolerance)
            {
                rot = Mathf.RoundToInt((rot + tolerance) / 90) * 90;
                snap = true;
            }
            if (snap)
            {
                chipOrientation = Mathf.FloorToInt(rot % 360 / 90);

                socketedChip = chip;
                socketedChip.transform.rotation = Quaternion.Euler(new Vector3(0, rot, 0));
                socketedChip.transform.position = snapPoint.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Chip chip = other.GetComponent<Chip>();
        if (chip != null && socketedChip != null)
        {
            socketedChip = null;
        }
    }
}
