public class Horse : BaseGame
{
  public override bool[,] PossibleMove()
    {

        bool[,] r = new bool[6, 6];
        HorseMove(CurrentX - 1, CurrentY + 2, ref r);
        HorseMove(CurrentX + 1, CurrentY + 2, ref r);
        HorseMove(CurrentX + 2, CurrentY + 1, ref r);
        HorseMove(CurrentX + 2, CurrentY -1, ref r);

        HorseMove(CurrentX - 1, CurrentY - 2, ref r);
        HorseMove(CurrentX + 1, CurrentY - 2, ref r);
        HorseMove(CurrentX - 2, CurrentY + 1, ref r);
        HorseMove(CurrentX - 2, CurrentY - 1, ref r);

        return r;
    }
    public void HorseMove(int x, int y, ref bool[,] r)
    {
        BaseGame c;
        if(x >= 0 && x < 6 && y >= 0 && y < 6)
        {
            c = BoredBoard.Instance.Chessmans[x, y];
            if (c == null || c.tag == "Coin")
            {
                r[x, y] = true;
            }
        }
    }
}