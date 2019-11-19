using System.Collections.Generic;
using UnityEngine;


public class BoredBoard : MonoBehaviour
{
    public static BoredBoard Instance { set; get; }
    private bool[,] allowedMoves { set; get; }
    public BaseGame[,] Chessmans { set; get; }
    private BaseGame selectedChessman;
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;
    public int score = 0;
    private int selectionX = -1;
    private int selectionY = -1;
    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;
    private Material previousMat;
    public Material selectedMat;
    private Quaternion orientation = Quaternion.Euler(0, 90, 0);
    public TextAsset Level;
    public string previouslevelName;
    public string nextlevelName;
    char[,] levelData;
    int rows;
    int cols;
    public int invalidTile;
    int CoinCount;
    GameObject[] mdk;
    AudioSource sors;
    public AudioClip move;
    public AudioClip coinpickup;
    public AudioClip gameover;



    private void Start()
    {
        sors = GetComponent<AudioSource>();
        previouslevelName = Level.name;
        Instance = this;
        SpawnAllChessman();
        ParseLevel();
        CreateLevel();
    }
    private void Update()
    {
        nextlevelName = Level.name;
        UpdateSelection();
        DrawChessboard();
        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    MoveChessman(selectionX, selectionY);
                }
            }
        }
        if(previouslevelName != nextlevelName)
        {
            mdk = GameObject.FindGameObjectsWithTag("Horse");
            for (int i = 0; i < mdk.Length; i++)
            {
                Destroy(mdk[i].gameObject);
            }
            mdk = GameObject.FindGameObjectsWithTag("Wall");
            for (int i = 0; i < mdk.Length; i++)
            {
                Destroy(mdk[i].gameObject);
            }
            mdk = GameObject.FindGameObjectsWithTag("Coin");
            for (int i = 0; i < mdk.Length; i++)
            {
                Destroy(mdk[i].gameObject);
            }
            
            Hotswap();
        }
    }
    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null)
        {
            return;
        }
        allowedMoves = Chessmans[x, y].PossibleMove();
        selectedChessman = Chessmans[x, y];
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;
        Highlight.Instance.HighlightAllowedMoves(allowedMoves);
    }
    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            BaseGame c = Chessmans[x, y];
            if (c != null && c.tag == "Coin")
            {
                
                Destroy(c.gameObject);
                score++;
                checkscore();
                sors.PlayOneShot(coinpickup);
                
            }
            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
            sors.PlayOneShot(move);

        }
        
        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;
        Highlight.Instance.HideHighlights();
        selectedChessman = null;
        
    }
    private void checkscore()
    {
        if (score == CoinCount)
        {
            sors.PlayOneShot(gameover);
        }
    }
    private void UpdateSelection()
    {
        if (!Camera.main)
            return;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }
    private void DrawChessboard()
    {
        Vector3 widthLine = Vector3.right * 6;
        Vector3 heightLine = Vector3.forward * 6;
        for (int i = 0; i <= 6; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 6; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                           Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(Vector3.forward * (selectionY + 1) + Vector3.right * selectionX,
                           Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }
    private void SpawnChessman(int index, int x, int y)
    {
        GameObject go = Instantiate(chessmanPrefabs[index], GetTileCenter(x, y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<BaseGame>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }
    private void SpawnAllChessman()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new BaseGame[6, 6];
    }
    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }
    void ParseLevel()
    {

        string[] lines = Level.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        char[] nums = lines[0].ToCharArray(); 
        rows = lines.Length;
        cols = nums.Length;
        levelData = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            string st = lines[i];
            nums = st.ToCharArray();
            for (int j = 0; j < cols; j++)
            {
                char val;
                val = nums[j];
                levelData[i, j] = val;
            }
        }
    }
    void CreateLevel()
    {
        for (int i = 0; i < rows; i++)
        {
            
            for (int j = 0; j < cols; j++)
            {
                char val = levelData[i, j];
                if (val == 'K')
                    {
                        SpawnChessman(0, i, j);
                    }
                    else
                    {
                        if (val == '#')
                        {
                            SpawnChessman(1, i, j);
                        }
                        else if (val == 'C')
                        {
                            SpawnChessman(2, i, j);
                            CoinCount++;
                        }
                    }
            }
            
        }
    }
    void Hotswap()
    {
        score = 0;
        CoinCount = 0;
        previouslevelName = nextlevelName;
        SpawnAllChessman();
        ParseLevel();
        CreateLevel();
    }
}