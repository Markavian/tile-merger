internal class TilingDirection
{
    public static TilingDirection LeftRight = new TilingDirection("lr");
    public static TilingDirection TopDown = new TilingDirection("td");

    private string direction;

    TilingDirection(string direction)
    {
        this.direction = direction;
    }
}