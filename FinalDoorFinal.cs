using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorFinal : MonoBehaviour, IActivate
{
    AudioSource doorSound;
    public AudioClip doorClosed;
    public AudioClip doorOpening;
    public GameObject RedKeyLock;
    public GameObject BlueKeyLock;
    public GameObject YellowKeyLock;

    private Locks RedKeyLockScript;
    private Locks BlueKeyLockScript;
    private Locks YellowKeyLockScript;

    public bool FinalDoorOpened = false;


    [Header("Needed for final event")]
    public GameObject FinalEventAnimation;
    public GameObject PlayerReference;
    private MasterScript ms;



    void Start()
    {
        doorSound = GetComponent<AudioSource>();
        RedKeyLockScript = RedKeyLock.GetComponent<Locks>();
        BlueKeyLockScript = BlueKeyLock.GetComponent<Locks>();
        YellowKeyLockScript = YellowKeyLock.GetComponent<Locks>();
        ms = PlayerReference.GetComponent<MasterScript>();
    }


    void Update()
    {
        if (FinalDoorOpened == false)
        {
            if (RedKeyLockScript.IsDoorOpen() == true && BlueKeyLockScript.IsDoorOpen() == true && YellowKeyLockScript.IsDoorOpen() == true)
            {
                doorSound.PlayOneShot(doorOpening);
                FinalDoorOpened = true;


                FinalEventAnimation.SetActive(true);
                ms.DisableFlashIfNeeded();
                ms.GameRunning = false;
                ms.DeactiveHighlighting();
            }
            
        }

    }

    public void Activate()
    {

    }

    public void Deactivate()
    {

    }
}

