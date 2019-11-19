using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locks : MonoBehaviour, IActivate
{
    public GameObject invetoryRef;
    public GameObject key;
    public GameObject Cilinder;
    public GameObject Lock;
    

    public enum UnlockableType { Key, Dynamic, Open, Close }

    public UnlockableType unlockableType;
    private Inventory Inventory;
    private IDynamicPuzzle script;
    

    public bool open = false;                   // Door state 
    AudioSource lockSound;
    public AudioClip lockClosed;
    public AudioClip lockOpening;
    BoxCollider kolajder;

    [Tooltip("Choose number if you want use diffrent axis for rotation(x=0,y=1,z=2)")]
    //public int axisChooser = 1;

    bool KeyDoorOpened = false;

    public void Start()
    {
        
        kolajder = GetComponent<BoxCollider>();
        lockSound = GetComponent<AudioSource>();
        Cilinder.GetComponent<Transform>();

        if (invetoryRef != null)
            Inventory = invetoryRef.GetComponent<Inventory>();

    }
    public void ChangeLockState()
    {
        open = !open;
        lockSound.clip = lockOpening;
        lockSound.Play();
        StartCoroutine(cekaj());
        

    }



    public void Activate()    //Aron MasterScript
    {
        //Debug.Log("Door activated");
        if (true)
        {
            if (unlockableType == UnlockableType.Dynamic)
            {
                Component[] c = key.GetComponents<MonoBehaviour>();
                script = (IDynamicPuzzle)c[0];
                if (script.IsUnlocked())
                {
                    ChangeLockState();
                }
            }
            else if (unlockableType == UnlockableType.Key)
            {
                if ((Inventory.WhatIsTheEquipedItem() == key.GetComponent<Item>().itemName) || (KeyDoorOpened == true))
                {
                    KeyDoorOpened = true;
                    ChangeLockState();
                    Inventory.DestroyItemFromInventory(key.GetComponent<Item>().itemName);
                }
                else
                {
                    lockSound.clip = lockClosed;
                    lockSound.Play();
                }
            }
            else if (unlockableType == UnlockableType.Open)
            {
                ChangeLockState();
            }
            else if (unlockableType == UnlockableType.Close)
            {
                lockSound.clip = lockClosed;
                lockSound.Play();
            }
        }


    }
    public void Deactivate()
    {
        // throw new System.NotImplementedException();
    }
    public IEnumerator cekaj()
    {
        yield return new WaitForSeconds(2);
        //Lock.SetActive(false);
        kolajder.enabled = !kolajder.enabled;
        Lock.transform.position = new Vector3(100, 100, 100);
        
    }
    public bool IsDoorOpen()
    {
        return open;
    }
}


