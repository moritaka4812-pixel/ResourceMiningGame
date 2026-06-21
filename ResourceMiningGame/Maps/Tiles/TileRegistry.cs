
using ResourceMiningGame.Maps.Tiles;

namespace ResourceMiningGame.Maps.Tiles
{
    public static class TileRegistry
    {
        public static Dictionary<TileType, TileAnimationInfo> Terrain =
            new Dictionary<TileType, TileAnimationInfo>()
            {
            {
                TileType.Ground,
                new TileAnimationInfo
                {
                    TexturePath = "TileUI/ground",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            },
            {
                TileType.Road,
                new TileAnimationInfo
                {
                    TexturePath = "TileUI/road",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            },
            {
                TileType.stone,
                new TileAnimationInfo
                {
                    TexturePath = "TileUI/stone",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            },
            {
                    TileType.blockedStone,
                    new TileAnimationInfo()
                    {
                        TexturePath = "TileUI/stone",
                        FrameCount = 1,
                        FrameTime = 0f
                    }
            }
            };
    }

}
