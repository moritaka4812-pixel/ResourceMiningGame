

namespace ResourceMiningGame.Maps.Tiles
{
    public static class ResourceRegistry
    {
        public static Dictionary<ResourceType, TileAnimationInfo> Resources =
            new Dictionary<ResourceType, TileAnimationInfo>()
            {
            {
                ResourceType.None,
                null
            },
            {
                ResourceType.Copper,
                new TileAnimationInfo
                {
                    TexturePath = "Resource/copper",
                    FrameCount = 1,
                    FrameTime = 0f
                }
            }
            };
    }

}
