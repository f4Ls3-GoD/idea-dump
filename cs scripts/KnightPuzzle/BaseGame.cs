using UnityEngine;

public abstract class BaseGame : MonoBehaviour
{
    public int CurrentX { set; get; }
    public int CurrentY { set; get; }
    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[6,6];
    }
}