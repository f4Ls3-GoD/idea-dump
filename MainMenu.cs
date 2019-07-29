using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{   
    
    public GameObject MAINMENU;
    public GameObject mainmenu;
    public GameObject game;
    public AudioClip startzvuk;
    AudioSource zvuk;


    void Start()
    {
        zvuk = GetComponent<AudioSource>();
    }
    

    public void StartGame()
    {
            
        zvuk.PlayOneShot(startzvuk);
        mainmenu.SetActive(false);
        StartCoroutine(WaitPlz());
    }
 
    public void QuitGame()
    {
         Application.Quit();
    }

    IEnumerator WaitPlz()
    {
        yield return new WaitForSeconds(3.5f);
        MAINMENU.SetActive(false);
        game.SetActive(true);
    }

}
