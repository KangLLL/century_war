public enum BuildingObstacleDirection
{
    None = 0,
    Top = 1,
    Left = 2,
    Bottom = 4,
    Right = 8,
    RightTop = Right | Top,
    RightLeft = Right | Left,
    RightBottom = Right | Bottom,
    RightTopLeft = Right | Top | Left,
    RightBottomLeft = Right | Bottom | Left,
    All = Right | Top | Left | Bottom,
    TopLeft = Top | Left,
    TopBottom = Top | Bottom,
    TopLeftBottom = Top | Left | Bottom,
    TopRightBottom = Top | Right | Bottom,
    LeftBottom = Left | Bottom
}
