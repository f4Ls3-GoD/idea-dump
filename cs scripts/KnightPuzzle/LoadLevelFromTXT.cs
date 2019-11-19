using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System.Linq;

public class LoadLevelFromTXT : MonoBehaviour
{
    public string[,] input;
    public static List<string> textArray;
    private bool display;
    private TextMesh textcomp;
    BoredBoard index;
    public int[] rowsToReadFrom;
    
    //public List<string> TextAssetToList(TextAsset text)
    //{
    //    return new List<string>(text.text.Split('\n'));
    //}


    //void Start()
    //{
    //    textcomp = GetComponent<TextMesh>();
    //    Debug.Log(text);


    //    //readTextFile();
    //}


    //void Update()
    //{

    //}


    //public void readTextFile()
    //{
    //    textArray = text.text.Split ('\n').ToList();
    //    for (int i = 0; i< rowsToReadFrom.Length;i++)
    //    {


    //        if (rowsToReadFrom[0] < 0 || rowsToReadFrom.Length ==0)
    //        {

    //            textcomp.text = text.ToString();

    //        } else
    //        {

    //            textcomp.text += textArray[rowsToReadFrom[i]] + "\n";

    //        }

    //    }
    //}

}
