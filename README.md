# idea-dump
Just about everything

My code in the MasterScript.cs file:

//Aim dot in the middle of the screen
public GameObject AimDot;

//Highlight interactible object, show its name
GameObject selectedUnit;
    bool hoverOverActive;
    string hoverName;
    Shader StandardShader;
    public Shader HightlightShader
    Renderer rend;
    public int HightlightDistance = 100;
    
void Start()
    {
        StandardShader = Shader.Find("Standard");
        HightlightShader = Shader.Find("Legacy Shaders/Self-Illumin/Diffuse");
        selectedUnit = null;
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
                }
            }
            selectedUnit = null;
        }
        hoverOverActive = false;
    }
}
