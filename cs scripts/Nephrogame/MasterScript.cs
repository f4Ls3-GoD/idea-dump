using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;





public class MasterScript : MonoBehaviour
{
    
    [Header("Object with flashlight script")]
    public GameObject Flashlight;
    bool FlashlightEnabled = false;
    bool FlashlightExists = false;
    public bool GameRunning = true;


    [Header("Other stuff")]
    public Camera firstPersonCamera;
    float movementSpeed, runSpeed = 0;
    public GameObject AimDot;
    public GameObject InventorySystem;
    public GameObject HUD;


    private Inventory inventory;
    private DrugMeter_CS drugMeter;


    IActivate puzzleScript;
    bool puzzleActive = false;
    public string PuzzleActivationTag = "Interactable";
    public string EnvActivationTag = "InteractableEnv";
    public string InteractableEnvNonHighlight = "InteractableEnvNonHighlight";
    public string PickupTag = "Item";




    
    public KeyCode ExitPuzzleKey = KeyCode.Escape;




    
    GameObject selectedUnit;
    bool hoverOverActive;
    string hoverName;
    Shader StandardShader;
    public Shader HightlightShader;
    
    Renderer rend;


    public int HightlightDistance = 100;


    void Start()
    {
        
        StandardShader = Shader.Find("Standard");
        HightlightShader = Shader.Find("Legacy Shaders/Self-Illumin/Diffuse");
        selectedUnit = null;


        inventory = InventorySystem.GetComponent<Inventory>();
        drugMeter = HUD.GetComponent<DrugMeter_CS>();

    }


    
    void OnGUI()
    {
        if (hoverOverActive)
            GUI.Label(new Rect(Input.mousePosition.x - 100, Screen.height - Input.mousePosition.y, 100, 20), "" + hoverName);
    }


    
    void Update()
    {
        if (GameRunning)
        {
            if (puzzleActive == false)
            {
                InfoAndHighlightInteractibleObjProcessor();
            }


            
            if (Input.GetMouseButtonDown(0))
            {
                
                if (puzzleActive == false)
                {
                    ActivateInteractible();
                }
            }
            
            else if (Input.GetKeyDown(ExitPuzzleKey))
            {
                if (puzzleActive == true)
                {
                    DeactivatePuzzle();
                    ActivateMovementAndCameraAndMovement();
                }
            }


            
            else if (Input.GetKeyDown(KeyCode.E))
            {
                
                if (inventory.WhatIsTheEquipedItem() == Item.ItemName.DrugPositive || inventory.WhatIsTheEquipedItem() == Item.ItemName.DrugNegative)
                {
                    
                    drugMeter.AddDrugsValue(inventory.GetDrugStrenght());

                    
                    //inventory.DestroyEquipedItem();
                }

            }


            else if (Input.GetKeyDown(KeyCode.F))
            {
                
                if (inventory.CheckInventoryForItem(Item.ItemName.FlashlightWBattery))
                {
                    FlashlightExists = true;
                    inventory.DestroyItemFromInventory(Item.ItemName.FlashlightWBattery);
                    
                }




                if (FlashlightEnabled == false && FlashlightExists == true)
                {
                    FlashlightEnabled = true;
                    Flashlight.SetActive(true);
                    Flashlight.GetComponent<Goran_Flashlight_Toggle>().Activate();
                }
                else if (FlashlightEnabled == true && FlashlightExists == true)
                {
                    FlashlightEnabled = false;
                    Flashlight.GetComponent<Goran_Flashlight_Toggle>().Activate();
                    Flashlight.SetActive(false);

                }
            }
        }
       // else
       // {
            //if (Input.GetKeyDown(KeyCode.Escape))
           // {
            //   Application.Quit();
            //}
        //}
       
    }

    public void DisableFlashIfNeeded()
    {
        if (FlashlightEnabled == true && FlashlightExists == true)
        {
            FlashlightEnabled = false;
            Flashlight.GetComponent<Goran_Flashlight_Toggle>().Activate();
            Flashlight.SetActive(false);

        }
    }




    private void InfoAndHighlightInteractibleObjProcessor()
    {
        
        Ray ray2 = firstPersonCamera.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit2;




        if (Physics.Raycast(ray2, out hit2, HightlightDistance) && (hit2.transform.gameObject.tag == PuzzleActivationTag || hit2.transform.gameObject.tag == EnvActivationTag || hit2.transform.gameObject.tag == PickupTag))
        {
            if (selectedUnit == null)
            {
                ActivateHighlightShaderAndHoverInfo(hit2);
            }
            else
            {
                
                if (selectedUnit.name != hit2.transform.gameObject.name)
                {
                    if (selectedUnit.transform.GetComponent<Renderer>() != null)
                    {
                        selectedUnit.transform.GetComponent<Renderer>().material.shader = StandardShader;
                        ActivateHighlightShaderAndHoverInfo(hit2);
                    }


                }
            }
        }
        else
        {
            DeactiveHighlighting();
        }
        
    }


    private void ActivateHighlightShaderAndHoverInfo(RaycastHit hit2)
    {
        hoverOverActive = true;
        hoverName = hit2.transform.name;
        selectedUnit = hit2.transform.gameObject;
        if (selectedUnit.transform.GetComponent<Renderer>() != null)
        {
            selectedUnit.transform.GetComponent<Renderer>().material.shader = HightlightShader;
        }


        var gameObjects = selectedUnit.GetComponentsInChildren<Renderer>();


        foreach (var item in gameObjects)
        {
            if (item.GetComponent<Renderer>() != null)
            {
                foreach (var shad in item.GetComponent<Renderer>().materials)
                {
                    shad.shader = HightlightShader;
                }


               
            }
        }

    }


    private void DeactivatePuzzle()
    {
        if (puzzleScript != null)
        {
            puzzleScript.Deactivate();
            puzzleScript = null;
            puzzleActive = false;
        }
    }


    private void ActivateInteractible()
    {
        RaycastHit hit;
        
        Ray ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, HightlightDistance))
        {




            
            if (hit.transform.parent != null && (hit.transform.parent.tag == PuzzleActivationTag || hit.transform.parent.tag == EnvActivationTag || hit.transform.parent.tag == InteractableEnvNonHighlight | hit.transform.gameObject.tag == PickupTag))
            {
                


                Component[] c = hit.transform.parent.GetComponents<MonoBehaviour>();
                SetScriptFromComponent(c);


                
                if (hit.transform.parent != null && hit.transform.parent.tag == EnvActivationTag || hit.transform.parent.tag == InteractableEnvNonHighlight)
                {
                    
                    TryActivateScript(puzzleScript);
                    return;
                }


                
                if (hit.transform.gameObject.tag == PickupTag)
                {
                    hit.transform.gameObject.GetComponent<Pickup>().PickMeUp();
                    return;
                }


                
                if (TryActivateScript(puzzleScript))
                {

                    DeactiveHighlighting();
                    puzzleActive = true;
                    DeactivateMainCameraAndMovementAndInventory();
                }
            }
           
            else if (hit.transform.gameObject != null && (hit.transform.gameObject.tag == PuzzleActivationTag || hit.transform.gameObject.tag == EnvActivationTag || hit.transform.gameObject.tag == PickupTag || hit.transform.gameObject.tag == InteractableEnvNonHighlight))
            {
                


                Component[] c = hit.transform.gameObject.GetComponents<MonoBehaviour>();
                
                SetScriptFromComponent(c);


                
                if (hit.transform.gameObject != null && (hit.transform.gameObject.tag == EnvActivationTag || hit.transform.gameObject.tag == InteractableEnvNonHighlight))
                {
                    ;
                    TryActivateScript(puzzleScript);
                    return;
                }
                
                if (hit.transform.gameObject.tag == PickupTag)
                {
                    hit.transform.gameObject.GetComponent<Pickup>().PickMeUp();
                    return;
                }


                
                if (TryActivateScript(puzzleScript))
                {
                    
                    DeactiveHighlighting();
                    puzzleActive = true;
                    DeactivateMainCameraAndMovementAndInventory();
                }










            }


            else
            {
                
            }
        }
    }


    private void DeactivateMainCameraAndMovementAndInventory()
    {
        firstPersonCamera.enabled = false;
        AimDot.SetActive(false);




        this.GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(false);
        this.GetComponent<FirstPersonController>().enabled = false;
        firstPersonCamera.GetComponent<AudioListener>().enabled = false;
        inventory.enabled = false;


    }


    private void ActivateMovementAndCameraAndMovement()
    {
        firstPersonCamera.enabled = true;
        GetComponent<FirstPersonController>().m_MouseLook.SetCursorLock(true);
        GetComponent<FirstPersonController>().enabled = true;
        firstPersonCamera.GetComponent<AudioListener>().enabled = true;


        AimDot.SetActive(true);


        inventory.enabled = true;


    }


    private bool TryActivateScript(IActivate puzzleScript)
    {
        
        if (puzzleScript != null)
        {
            
            puzzleScript.Activate();


            return true;
        }
        return false;
    }


    private void SetScriptFromComponent(Component[] c)
    {
        
        if (c.Length > 0 && c[0] != null)
        {
            
            puzzleScript = (IActivate)c[0];
        }


    }


    public void DeactiveHighlighting()
    {
        if (selectedUnit != null)
        {
            if (selectedUnit.transform.GetComponent<Renderer>() != null)
            {
                selectedUnit.transform.GetComponent<Renderer>().material.shader = StandardShader;

            }
            var gameObjects = selectedUnit.GetComponentsInChildren<Renderer>();


            foreach (var item in gameObjects)
            {
                if (item.GetComponent<Renderer>() != null)
                {
                    foreach (var shad in item.GetComponent<Renderer>().materials)
                    {
                        shad.shader = StandardShader;
                    }
                    //item.GetComponent<Renderer>().material.shader = StandardShader;
                }
            }
            selectedUnit = null;
        }
        hoverOverActive = false;
    }
}


// Interface mandatory for puzzle scripts
public interface IActivate
{
    void Activate();
    void Deactivate();
}


public interface IDynamicPuzzle
{
    bool IsUnlocked();
}