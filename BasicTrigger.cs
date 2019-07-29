using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrigger : MonoBehaviour
{
    public GameObject triggerd;

        private void OnTriggerEnter(Collider other)
        {
            
            triggerd.SetActive(true);
            
        }

}
