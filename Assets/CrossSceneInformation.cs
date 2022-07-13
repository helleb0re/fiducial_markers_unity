public static class CrossSceneInformation
{
    public static string MarkerName { get; set; } = SkinPlate.AprilTag16h5.ToString();
    public static float FocusLength { get; set; } = 19;
    public static float SensorSizeX { get; set; } = 54.12f;
    public static float SensorSizeY { get; set; } = 25.59f;

    public static StateOfGame StateGame { get; set; }

    public static MyScene ActiveScene { get; set; } = MyScene.Test1;

    public static string ActiveResolution { get; set; } = Resolution.Res_1920x1080.ToString();
}
