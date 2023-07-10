internal class TilingDirection
{
    public static readonly TilingDirection LeftRight = new TilingDirection("lr");
    public static readonly TilingDirection TopDown = new TilingDirection("td");

    private readonly string direction;

    private TilingDirection(string direction)
    {
        this.direction = direction;
    }

    public string Value { get => direction; }

    public override bool Equals(object obj)
    {
        if (obj is TilingDirection other)
        {
            return direction.Equals(other.direction);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return direction.GetHashCode();
    }

    public override string ToString()
    {
        return $"TilingDirection: {direction}";
    }
}