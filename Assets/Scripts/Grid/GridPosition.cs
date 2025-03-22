using System;
public struct GridPosition : IEquatable<GridPosition>
{
    public int x;
    public int z;

    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
    public override string ToString()
    {
        return "x: " + x + " z: " + z;
    }
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }

    public bool Equals(GridPosition other)
    {
        return x == other.x && z == other.z;
    }
    public override bool Equals(object obj)
    {
        if (obj is GridPosition)
        {
            GridPosition gridPosition = (GridPosition)obj;
            return x == gridPosition.x && z == gridPosition.z;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(x, z);
    }
}
