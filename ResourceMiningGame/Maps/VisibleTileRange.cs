using Point = Microsoft.Xna.Framework.Point;

public struct VisibleTileRange //IMap GetVisibleRangeの返す構造体
{
    public int StartX;
    public int EndX;
    public int StartY;
    public int EndY;

    public bool Contains(Point tilePos)
    {
        return tilePos.X >= StartX && tilePos.X < EndX &&
                tilePos.Y >= StartY && tilePos.Y < EndY;
    }

    public bool Contains(int x, int y)
    {
        return x >= StartX && x < EndX &&
                y >= StartY && y < EndY;
    }
}