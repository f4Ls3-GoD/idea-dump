using System.Collections;
using UnityEngine;

public class hodajcreepy : MonoBehaviour
{
    AudioSource sors;
    public AudioClip prvizvuk;
    public AudioClip drugizvuk;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("krenulo je");
        sors = GetComponent<AudioSource>();
        StartCoroutine(cekajmalo());
        Debug.Log("zavrsilo je");

    }

    IEnumerator cekajmalo()
    {
        yield return new WaitForSeconds(9);
        sors.PlayOneShot(prvizvuk);
        yield return new WaitForSeconds(1);
        sors.PlayOneShot(drugizvuk);
    }
   
}
