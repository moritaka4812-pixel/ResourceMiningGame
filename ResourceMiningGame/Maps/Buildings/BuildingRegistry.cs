using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Maps.Buildings
{
    public static class BuildingRegistry
    {
        public static Dictionary<BuildType, BuildingInfo> Data =
            new()
            {
                {
                    BuildType.Drill,
                    new BuildingInfo()
                    {
                        TexturePath = "Buildings/drill",
                        FrameCount = 9,
                        FrameTime = 0.2f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 1.0f
                    }
                }
            };
    }
}
