using Point = Microsoft.Xna.Framework.Point;

namespace ResourceMiningGame.Maps.Buildings
{
    public static class BuildingRegistry
    {
        public static Dictionary<BuildType, BuildingInfo> Data =
            new()
            {
                {
                    BuildType.Test,
                    new BuildingInfo()
                    {
                        TexturePath = "Buildings/Test",
                        Type = BuildType.Test,
                        Width = 2,
                        Height = 2,
                        FrameCount = 8,
                        FrameTime = 0.2f,
                        SizeInTiles = new Point(2,2),
                        WorkSpeed = 10000
                    }
                },
                {
                    BuildType.Drill,
                    new BuildingInfo()
                    {
                        TexturePath = "Buildings/drill",
                        Type = BuildType.Drill,
                        Width = 1,
                        Height = 1,
                        FrameCount = 9,
                        FrameTime = 0.2f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 1.0f
                    }
                }
            };
    }
}
