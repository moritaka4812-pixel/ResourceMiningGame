using Point = Microsoft.Xna.Framework.Point;
using Craftory.Maps.Buildings.Miners;
using Craftory.Maps.Buildings.Conveyors;

namespace Craftory.Maps.Buildings
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
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Miner/Drill" }
                        },
                        Type = BuildType.Drill,
                        Width = 1,
                        Height = 1,
                        FrameCount = 9,
                        FrameTime = 0.2f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 0.25f,
                        Create = (pos, dir) => new Drill(pos)
                    }
                },
                {
                    BuildType.Conveyor,
                    new BuildingInfo
                    {
                        TexturePaths = new()
                        {
                            { BuildingDirection.None, "Buildings/Conveyor/ConveyorStraight"}
                        },
                        Type = BuildType.Conveyor,
                        Width = 1,
                        Height = 1,
                        FrameCount = 5,
                        FrameTime = 0.25f,
                        SizeInTiles = new Point(1,1),
                        WorkSpeed = 1.0f,
                        Create = (pos, dir) => new Conveyor(pos, dir)
                    }
                }
            };

        public static void LoadTextures()
        {
            foreach( var info in Data.Values)
            {
                foreach( var kv in info.TexturePaths)
                {
                    var dir = kv.Key;
                    var path = kv.Value;

                    info.CachedTextures[dir] = ContentLoader.LoadTexture(path);
                }
            }
        }
    }
}
